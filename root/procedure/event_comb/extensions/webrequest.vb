
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Public Module _webrequest
    <Extension()> Public Function get_response(ByVal w As WebRequest,
                                               ByVal r As ref(Of WebResponse)) As event_comb
        Return create(Function() As Boolean
                          Return Not w Is Nothing AndAlso Not r Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(
                                        Function(ac As AsyncCallback) As IAsyncResult
                                            Return w.BeginGetResponse(ac, Nothing)
                                        End Function,
                                        Function(ar As IAsyncResult) As WebResponse
                                            Try
                                                Return w.EndGetResponse(ar)
                                            Catch ex As WebException
                                                If ex.Response() Is Nothing Then
                                                    Throw
                                                Else
                                                    Return ex.Response()
                                                End If
                                            End Try
                                        End Function,
                                        r)
                      End Function)
    End Function

    <Extension()> Public Function get_response(ByVal w As HttpWebRequest,
                                               ByVal r As ref(Of HttpWebResponse)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As ref(Of WebResponse) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of WebResponse)()
                                  ec = get_response(w, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() OrElse
                                      (+p) Is Nothing Then
                                      Return False
                                  End If
                                  Dim h As HttpWebResponse = Nothing
                                  If direct_cast(Of HttpWebResponse)(+p, h) Then
                                      Return eva(r, h) AndAlso
                                             goto_end()
                                  End If
                                  p.get().Close()
                                  Return False
                              End Function)
    End Function

    <Extension()> Public Function get_request_stream(ByVal w As WebRequest,
                                                     ByVal r As ref(Of Stream)) As event_comb
        Return create(Function() As Boolean
                          Return Not w Is Nothing AndAlso Not r Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(
                                        Function(ac As AsyncCallback) As IAsyncResult
                                            Return w.BeginGetRequestStream(ac, Nothing)
                                        End Function,
                                        Function(ar As IAsyncResult) As Stream
                                            Return w.EndGetRequestStream(ar)
                                        End Function,
                                        r)
                      End Function)
    End Function
End Module
