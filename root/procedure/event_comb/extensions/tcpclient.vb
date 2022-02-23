
Imports System.Runtime.CompilerServices
Imports System.Net
Imports System.Net.Sockets

Public Module _tcpclient
    <Extension()> Public Function connect(ByVal client As TcpClient,
                                          ByVal remote_endpoint As IPEndPoint) As event_comb
        Return create(Function() As Boolean
                          Return client IsNot Nothing AndAlso
                                 remote_endpoint IsNot Nothing AndAlso
                                 Not client.Connected()
                      End Function,
                      Function() As event_comb
                          Return New event_comb_async_operation(
                                            Function(ac As AsyncCallback) As IAsyncResult
                                                Return client.BeginConnect(remote_endpoint.Address(),
                                                                           remote_endpoint.Port(),
                                                                           ac,
                                                                           Nothing)
                                            End Function,
                                            Sub(ar As IAsyncResult)
                                                client.EndConnect(ar)
                                            End Sub)
                      End Function)
    End Function
End Module
