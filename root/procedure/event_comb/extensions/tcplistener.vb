
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Net.Sockets
Imports osi.root.connector

Public Module _tcplistener
    <Extension()> Public Function accept_tcp_client(ByVal listener As TcpListener,
                                                    ByVal r As ref(Of TcpClient)) As event_comb
        Return create(Function() As Boolean
                          Return Not listener Is Nothing AndAlso
                                 Not r Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                                     Return listener.BeginAcceptTcpClient(ac, Nothing)
                                                                 End Function,
                                                                 Function(ar As IAsyncResult) As TcpClient
                                                                     Return listener.EndAcceptTcpClient(ar)
                                                                 End Function,
                                                                 r)
                      End Function)
    End Function

    <Extension()> Public Function accept_socket(ByVal listener As TcpListener,
                                                ByVal r As ref(Of Socket)) As event_comb
        Return create(Function() As Boolean
                          Return Not listener Is Nothing AndAlso
                                 Not r Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                                     Return listener.BeginAcceptSocket(ac, Nothing)
                                                                 End Function,
                                                                 Function(ar As IAsyncResult) As Socket
                                                                     Return listener.EndAcceptSocket(ar)
                                                                 End Function,
                                                                 r)
                      End Function)
    End Function
End Module
