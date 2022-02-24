
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation

Partial Public Class accepter
    Private Class listener
        Inherits reference_count_event_comb_1

        Shared Sub New()
            assert(start_after_trigger())
        End Sub

        Public Event accepted(ByVal c As TcpClient)
        Public ReadOnly valid As Boolean
        Private ReadOnly l As TcpListener

        Public Sub New(ByVal address As IPAddress, ByVal port As UInt16)
            MyBase.New()
            assert(port >= IPEndPoint.MinPort AndAlso port <= IPEndPoint.MaxPort)
            l = create_listener(address, port)
            valid = (Not l Is Nothing)
        End Sub

        Public Function port() As UInt16
            Try
                Return If(valid, CUShort(l.LocalEndpoint().direct_cast_to(Of IPEndPoint).Port()), max_uint16)
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to get port of listener, ex ", ex.Message())
                Return max_uint16
            End Try
        End Function

        Protected Overrides Function init() As event_comb
            Return sync_async(Function() start(l))
        End Function

        'TODO: confirm whether a TcpListener can only
        'accept one connection concurrentyly
        'http://social.msdn.microsoft.com/Forums/vstudio/en-US/7cb83d93-17cf-4241-a916-5b3fd673075e/can-i-beginaccepttcpclient-for-more-than-once-in-tcplistener
        Protected Overrides Function work() As event_comb
            Dim ec As event_comb = Nothing
            Dim c As ref(Of TcpClient) = Nothing
            Return New event_comb(Function() As Boolean
                                      assert(valid)
                                      c = New ref(Of TcpClient)()
                                      ec = l.accept_tcp_client(c)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso
                                         Not +c Is Nothing Then
                                          RaiseEvent accepted(+c)
                                          Return goto_end()
                                      Else
                                          Return False
                                      End If
                                  End Function)
        End Function

        Protected Overrides Function final() As event_comb
            Return sync_async(Sub() [stop](l))
        End Function
    End Class
End Class
