
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _httplistener
    <Extension()> Public Function get_context(ByVal listener As HttpListener,
                                              ByVal ctx As pointer(Of HttpListenerContext)) As event_comb
        Return create(Function() As Boolean
                          Return Not listener Is Nothing AndAlso
                                 listener.IsListening() AndAlso
                                 Not ctx Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(
                                        Function(ac As AsyncCallback) As IAsyncResult
                                            Return listener.BeginGetContext(ac, Nothing)
                                        End Function,
                                        Function(ar As IAsyncResult) As HttpListenerContext
                                            Return listener.EndGetContext(ar)
                                        End Function,
                                        ctx)
                      End Function)
    End Function
End Module
