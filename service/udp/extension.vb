
Imports System.Runtime.CompilerServices
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _extension
    Private ReadOnly act As invoker(Of Func(Of Boolean))

    Sub New()
        act = New invoker(Of Func(Of Boolean))(GetType(UdpClient),
                                               BindingFlags.Instance Or
                                               BindingFlags.NonPublic Or
                                               BindingFlags.InvokeMethod,
                                               "get_Active")
        assert(act.valid())
        assert(act.post_binding())
    End Sub

    <Extension()> Public Function active(ByVal u As UdpClient) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return act(u)()
        End If
    End Function

    <Extension()> Public Function set_icmp_reset(ByVal u As UdpClient, ByVal v As Boolean) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return u.Client().set_iocontrol(IOControlCodeExt.SIO_UDP_CONNRESET, v, Nothing) <> npos
        End If
    End Function

    <Extension()> Public Function disable_icmp_reset(ByVal u As UdpClient) As Boolean
        Return set_icmp_reset(u, False)
    End Function

    ' <0, error
    ' =0, no buffered data
    ' >0, has data to read
    <Extension()> Public Function buffered_bytes(ByVal u As UdpClient) As Int32
        If u Is Nothing Then
            Return npos
        Else
            Return u.Client().buffered_bytes()
        End If
    End Function

    <Extension()> Public Function identity(ByVal u As UdpClient) As String
        If u Is Nothing Then
            Return "null-udp-client"
        Else
            Return u.Client().identity()
        End If
    End Function

    <Extension()> Public Function handle_id(ByVal u As UdpClient) As Int64
        If u Is Nothing Then
            Return npos
        Else
            Return u.Client().handle_id()
        End If
    End Function

    <Extension()> Public Function set_keepalive(ByVal c As UdpClient,
                                                Optional ByVal enable As Boolean = enable_socket_keepalive,
                                                Optional ByVal first_keepalive_ms As UInt32 = socket_first_keepalive_ms,
                                                Optional ByVal interval_ms As UInt32 = socket_keepalive_interval_ms) _
                                               As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().set_keepalive(enable, first_keepalive_ms, interval_ms)
        End If
    End Function

    <Extension()> Public Function enable_keepalive(ByVal c As UdpClient,
                                                   Optional ByVal first_keepalive_ms As UInt32 = _
                                                       socket_first_keepalive_ms,
                                                   Optional ByVal interval_ms As UInt32 = _
                                                       socket_keepalive_interval_ms) _
                                                  As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().enable_keepalive(first_keepalive_ms, interval_ms)
        End If
    End Function

    <Extension()> Public Function send_buff_size(ByVal u As UdpClient) As UInt32
        If u Is Nothing OrElse
           u.Client() Is Nothing Then
            Return failure_send_buff_size
        Else
            Return u.Client().send_buff_size()
        End If
    End Function

    <Extension()> Public Function receive_buff_size(ByVal u As UdpClient) As UInt32
        If u Is Nothing OrElse u.Client() Is Nothing Then
            Return failure_receive_buff_size
        Else
            Return u.Client().receive_buff_size()
        End If
    End Function

    <Extension()> Public Function set_receive_buffer_size(ByVal u As UdpClient, ByVal size As UInt32) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return u.Client().set_receive_buffer_size(size)
        End If
    End Function

    <Extension()> Public Function set_send_buffer_size(ByVal u As UdpClient, ByVal size As UInt32) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return u.Client().set_send_buffer_size(size)
        End If
    End Function

    <Extension()> Public Function alive(ByVal u As UdpClient) As Boolean
        Return True
    End Function

    <Extension()> Public Sub shutdown(ByVal u As UdpClient)
        On Error Resume Next
        If Not u Is Nothing Then
            u.Client().Shutdown(SocketShutdown.Both)
            u.Client().Close()
            u.Close()
        End If
    End Sub

    <Extension()> Public Function match_endpoint(ByVal received_from As IPEndPoint,
                                                 ByVal expected As IPEndPoint) As Boolean
        If received_from Is Nothing OrElse expected Is Nothing Then
            Return False
        End If
        If received_from.Equals(expected) Then
            Return True
        Else
            Dim rp As UInt16 = uint16_0
            Dim ep As UInt16 = uint16_0
            rp = received_from.Port()
            ep = expected.Port()
            If rp = ep OrElse ep = socket_invalid_port Then
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
                            ' IPAddress does not contain a sub network mask field, so make it simpler,
                            ' if one byte is 255, treat it as broadcast mask.
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
End Module
