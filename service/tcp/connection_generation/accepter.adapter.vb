
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.commander

Partial Public NotInheritable Class accepter
    Private NotInheritable Class adapter
        Private ReadOnly d As itoken_defender(Of powerpoint, TcpClient)
        Private ReadOnly v4 As listener
        Private ReadOnly v6 As listener

        Public Sub New(ByVal p As powerpoint)
            assert(Not p Is Nothing)
            d = powerpoint.defender.[New](p)
            v4 = New listener(IPAddress.Any, p.port)
            v6 = New listener(IPAddress.IPv6Any, p.port)
            AddHandler v4.accepted, AddressOf accept
            AddHandler v6.accepted, AddressOf accept
        End Sub

        Public Sub attach(ByVal p As powerpoint)
            assert(Not p Is Nothing)
            assert(Not p.is_outgoing)
            assert(Not v4.valid OrElse p.port = v4.port())
            assert(Not v6.valid OrElse p.port = v6.port())
            AddHandler p.ref_client_manual_device_exporter().after_start, Sub()
                                                                              v4.bind()
                                                                              v6.bind()
                                                                              d.attach(p)
                                                                          End Sub
            AddHandler p.ref_client_manual_device_exporter().after_stop, Sub()
                                                                             v4.release()
                                                                             v6.release()
                                                                             d.detach(p)
                                                                         End Sub
        End Sub

        Public Function attached_powerpoint_count() As UInt32
            Return d.attached_powerpoint_count()
        End Function

        Private Sub accept(ByVal c As TcpClient)
            assert(Not c Is Nothing)
            Dim tp As ref(Of powerpoint) = Nothing
            Dim ec As event_comb = Nothing
            assert_begin(New event_comb(Function() As Boolean
                                            If d.empty() Then
                                                Return False
                                            Else
                                                c.set_no_delay(True)
                                                tp.renew()
                                                ec = d(c, tp)
                                                Return waitfor(ec) AndAlso
                                                       goto_next()
                                            End If
                                        End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso Not tp.empty() Then
                                          If (+tp).ref_client_manual_device_exporter().inject(connection.[New](+tp, c)) Then
                                              Return goto_end()
                                          Else
                                              raise_error("cannot insert the connection to assosiated powerpoint ",
                                                          (+tp).identity,
                                                          ", ",
                                                          "close the connection ",
                                                          c.identity())
                                              c.shutdown()
                                              Return False
                                          End If
                                      Else
                                          Return False
                                      End If
                                  End Function))
        End Sub
    End Class
End Class
