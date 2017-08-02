
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector
Imports osi.service.udp

Public Class udp_shared_component_test
    Inherits event_comb_case

    Private Shared ReadOnly end_bytes() As Byte
    Private Shared ReadOnly local_port As UInt16
    Private ReadOnly c As collection
    Private ReadOnly failure_count As atomic_int
    Private incoming_powerpoint As powerpoint
    Private outgoing_powerpoint As powerpoint
    Private listen_component As ref_instance(Of UdpClient)
    Private listen_dispenser As dispenser(Of Byte(), const_pair(Of String, UInt16))

    Shared Sub New()
        end_bytes = random_data()
        local_port = rnd_port()
    End Sub

    Public Sub New()
        MyBase.New()
        c = New collection()
        failure_count = New atomic_int()
    End Sub

    Private Shared Function random_data() As Byte()
        Dim r() As Byte = Nothing
        Do
            r = rnd_bytes(rnd_uint(max_uint8, max_uint8 << 2))
        Loop While memcmp(r, end_bytes) = 0
        Return r
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            incoming_powerpoint = powerpoint.creator.[New]().
                                                     with_ipv4().
                                                     with_local_port(local_port).
                                                     create()
            outgoing_powerpoint = powerpoint.creator.[New]().
                                                     with_ipv4().
                                                     with_remote_port(local_port).
                                                     with_host_or_ip("localhost").
                                                     create()
            failure_count.set(0)
            assert_true(c.[New](incoming_powerpoint,
                                incoming_powerpoint.local_port,
                                listen_component))
            assert_true(c.[New](incoming_powerpoint,
                                incoming_powerpoint.local_port,
                                listen_component,
                                listen_dispenser))
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function finish() As Boolean
        incoming_powerpoint.close()
        Return MyBase.finish()
    End Function

    Private Function ping() As event_comb
        Const round As Int32 = 1000
        Dim sc As shared_component(Of UInt16, String, UdpClient, Byte(), powerpoint) = Nothing
        Dim ec As event_comb = Nothing
        Dim i As Int32 = 0
        Dim data() As Byte = Nothing
        Dim r As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  sc = shared_component(Of UInt16, String, UdpClient, Byte(), powerpoint).creator.
                                           [New]().
                                           with_parameter(outgoing_powerpoint).
                                           with_remote(outgoing_powerpoint.remote_endpoint()).
                                           with_collection(c).
                                           with_functor(Of functor)().
                                           create()
                                  If assert_not_nothing(sc) Then
                                      r = New pointer(Of Byte())()
                                      Return goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  i += 1
                                  If i = round Then
                                      Return goto_end()
                                  ElseIf i = round - 1 Then
                                      data = end_bytes
                                  Else
                                      data = random_data()
                                  End If
                                  assert(Not data Is Nothing)
                                  ec = sc.sender.send(data)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = sc.receiver.receive(r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      failure_count.increment()
                                      Return [goto](1)
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert_array_equal(+r, data)
                                  Else
                                      failure_count.increment()
                                  End If
                                  Return [goto](1)
                              End Function)
    End Function

    Private Sub echo(ByVal c As shared_component(Of UInt16, String, UdpClient, Byte(), powerpoint))
        assert(Not c Is Nothing)
        Dim p As pointer(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        p = New pointer(Of Byte())()
                                        Return goto_next()
                                    End Function,
                                    Function() As Boolean
                                        ec = c.receiver.receive(p)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If ec.end_result() Then
                                            ec = c.sender.send(+p)
                                            Return waitfor(ec) AndAlso
                                                   goto_next()
                                        Else
                                            failure_count.increment()
                                            Return [goto](1)
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        If Not ec.end_result() Then
                                            failure_count.increment()
                                        End If

                                        If memcmp(+p, end_bytes) = 0 Then
                                            Return goto_end()
                                        Else
                                            Return [goto](1)
                                        End If
                                    End Function))
    End Sub

    Public Overrides Function create() As event_comb
        AddHandler c.new_shared_component_exported,
                   Sub(ByVal new_component As shared_component(Of UInt16, String, UdpClient, Byte(), powerpoint))
                       If assert_not_nothing(new_component) Then
                           echo(new_component)
                       End If
                   End Sub

        Const size As Int32 = 10
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim ecs(size - 1)
                                  For i As Int32 = 0 To size - 1
                                      ecs(i) = ping()
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ecs.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
