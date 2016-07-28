
#Const SINGLE_OPERATION = True
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports envs = osi.root.envs

' Maintain a very basic send / receive lock to ensure only one single direction operation is ongoing.
' Receive data from a certain set of remote hosts.
<type_attribute()>
Public Class delegator
    Public ReadOnly p As powerpoint
    Public ReadOnly id As String
#If SINGLE_OPERATION Then
    Private ReadOnly send_lock As ref(Of event_comb_lock)
    Private ReadOnly receive_lock As ref(Of event_comb_lock)
#End If
    Private ReadOnly sources As const_array(Of IPEndPoint)
    Private ReadOnly c As UdpClient

    Shared Sub New()
        type_attribute.of(Of delegator).set(transmitter.[New]().
            with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New(ByVal sources() As IPEndPoint, ByVal c As UdpClient)
        Me.New(sources, c, Nothing)
    End Sub

    Public Sub New(ByVal sources() As IPEndPoint, ByVal c As UdpClient, ByVal p As powerpoint)
        assert(Not c Is Nothing)
#If SINGLE_OPERATION Then
        send_lock = New ref(Of event_comb_lock)()
        receive_lock = New ref(Of event_comb_lock)()
#End If
        Me.c = c
        Me.sources = sources
        Me.id = c.identity()
        Me.p = p
        If Not p Is Nothing Then
            If envs.udp_trace Then
                raise_error("new conneciton ",
                            id,
                            " has been generated to the powerpoint ",
                            p.identity)
            End If
        End If

        Try
            ' max_packet_size is usually larger than MTU (1500 for ethernet), so we allow datagrams to be fragmented.
            Me.c.DontFragment() = False
        Catch ex As Exception
            raise_error(error_type.exclamation,
                        "Cannot set DontFragment flag, ex ",
                        ex.Message(),
                        ", this usually means configurations of ipv4 / ipv6 in the system are not correct.")
        End Try
        ' We do not want icmp reset connect signal to make any trouble.
        Me.c.disable_icmp_reset()
    End Sub

    Public Function active() As Boolean
        Return c.active()
    End Function

    Public Function alive() As Boolean
        Return c.alive()
    End Function

    Public Function fixed_sources() As Boolean
        Return Not sources.null_or_empty()
    End Function

    Private Function unlock_send(ByVal buff() As Byte,
                                 ByVal len As UInt32,
                                 ByVal target As IPEndPoint,
                                 ByVal sent As pointer(Of UInt32)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If target Is Nothing OrElse Not c.active() Then
                                      ec = c.send(buff, len, sent)
                                  Else
                                      ec = c.send(buff, len, target, sent)
                                  End If
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal len As UInt32,
                         Optional ByVal target As IPEndPoint = Nothing,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
#If SINGLE_OPERATION Then
        Return send_lock.locked(Function() As event_comb
                                    Return unlock_send(buff, len, target, sent)
                                End Function)
#Else
        Return unlock_send(buff, len, target, sent)
#End If
    End Function

    Private Shared Function match_endpoint(ByVal received_from As IPEndPoint, ByVal expected As IPEndPoint) As Boolean
        assert(Not received_from Is Nothing)
        If expected Is Nothing Then
            Return False
        End If
        If received_from.Equals(expected) Then
            Return True
        Else
            Dim rp As UInt16 = uint16_0
            Dim ep As UInt16 = uint16_0
            rp = received_from.Port()
            ep = expected.Port()
            If rp = ep OrElse ep = uint16_0 Then
                ' If port has not been set, treat them as equal.
                Dim ea As IPAddress = Nothing
                ea = expected.Address()
                If ea.Equals(IPAddress.Any) OrElse ea.Equals(IPAddress.IPv6Any) Then
                    Return True
                End If

                Dim ra As IPAddress = Nothing
                ra = received_from.Address()
                If ra.Equals(ea) Then
                    Return True
                Else
                    Dim rb() As Byte = Nothing
                    Dim eb() As Byte = Nothing
                    rb = ra.GetAddressBytes()
                    eb = ea.GetAddressBytes()
                    If array_size(rb) = array_size(eb) Then
                        assert(Not isemptyarray(rb))
                        For i As UInt32 = uint32_0 To array_size(rb) - uint32_1
                            ' Make it simpler, if one byte is 255, treat it as broadcast mask.
                            If rb(i) <> eb(i) AndAlso eb(i) <> max_uint8 Then
                                Return False
                            End If
                        Next
                        Return True
                    Else
                        Return False
                    End If
                End If
            Else
                Return False
            End If
        End If
    End Function

    Private Function unlock_receive(ByVal result As pointer(Of Byte()),
                                    ByVal source As pointer(Of IPEndPoint)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If fixed_sources() Then
                                      source.renew()
                                  End If
                                  ec = c.receive(result, source)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If fixed_sources() Then
                                          assert(Not +source Is Nothing)
                                          For i As UInt32 = uint32_0 To sources.size() - uint32_1
                                              If match_endpoint((+source), sources(i)) Then
                                                  Return goto_end()
                                              End If
                                          Next
                                          ' Drop datagrams from unknown sources.
                                          Return goto_begin()
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function receive(ByVal result As pointer(Of Byte()),
                            Optional ByVal source As pointer(Of IPEndPoint) = Nothing) As event_comb
#If SINGLE_OPERATION Then
        Return receive_lock.locked(Function() As event_comb
                                       Return unlock_receive(result, source)
                                   End Function)
#Else
        Return unlock_receive(result, source)
#End If
    End Function

    Public Function buffered_bytes() As Int32
        Return c.buffered_bytes()
    End Function

    Public Sub shutdown()
        c.shutdown()
    End Sub
End Class
