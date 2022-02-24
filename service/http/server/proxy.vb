
Imports System.Net
Imports System.IO
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.constants

Public Module _proxy
    Sub New()
        ServicePointManager.DefaultConnectionLimit() = max_int32
    End Sub

    Private Function sync_request(ByVal i As HttpListenerContext,
                                  ByVal o As HttpWebRequest,
                                  ByVal comm As link_status) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(
                        Function() As Boolean
                            If i Is Nothing OrElse
                               o Is Nothing OrElse
                               comm Is Nothing OrElse
                               Not i.copy_headers_to(o) Then
                                Return False
                            ElseIf i.Request().HasEntityBody() Then
                                If i.chunked_transfer() Then
                                    ec = o.write_request_body(i.Request().InputStream(),
                                                              comm,
                                                              comm,
                                                              False)
                                Else
                                    ec = o.write_request_body(i.Request().InputStream(),
                                                              i.Request().ContentLength64(),
                                                              comm,
                                                              comm,
                                                              False)
                                End If
                                Return waitfor(ec) AndAlso
                                       goto_next()
                            Else
                                Return goto_end()
                            End If
                        End Function,
                        Function() As Boolean
                            i.Request().InputStream().Close()
                            Return ec.end_result() AndAlso
                                   goto_end()
                        End Function)
    End Function

    Private Function sync_response(ByVal i As HttpWebRequest,
                                   ByVal o As HttpListenerContext,
                                   ByVal comm As link_status) As event_comb
        Dim r As ref(Of HttpWebResponse) = Nothing
        Dim cl As ref(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(
                        Function() As Boolean
                            If i Is Nothing OrElse o Is Nothing OrElse comm Is Nothing Then
                                Return False
                            Else
                                r = New ref(Of HttpWebResponse)()
                                ec = i.get_response(r)
                                Return waitfor(ec, comm.timeout_ms) AndAlso
                                       goto_next()
                            End If
                        End Function,
                        Function() As Boolean
                            If ec.end_result() AndAlso
                               Not (+r) Is Nothing AndAlso
                               (+r).copy_headers_to(o) Then
                                ec = (+r).read_response_body(o.Response().OutputStream(),
                                                             comm,
                                                             comm)
                                Return waitfor(ec) AndAlso
                                       goto_next()
                            Else
                                r.close()
                                o.Response().OutputStream().Close()
                                o.Response().Close()
                                Return False
                            End If
                        End Function,
                        Function() As Boolean
                            r.close()
                            o.Response().OutputStream().Close()
                            o.Response().Close()
                            Return ec.end_result() AndAlso
                                   goto_end()
                        End Function)
    End Function

    Public Function proxy(ByVal i As HttpListenerContext,
                          ByVal o As String,
                          Optional ByVal request_comm As link_status = Nothing,
                          Optional ByVal response_comm As link_status = Nothing) As event_comb
        Dim request As HttpWebRequest = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(
                        Function() As Boolean
                            If i Is Nothing OrElse String.IsNullOrEmpty(o) Then
                                Return False
                            Else
                                If request_comm Is Nothing Then
                                    request_comm = link_status.request
                                End If
                                If try_create_http_web_request(o, request) Then
                                    ec = sync_request(i, request, request_comm)
                                    Return waitfor(ec) AndAlso
                                           goto_next()
                                Else
                                    Return False
                                End If
                            End If
                        End Function,
                        Function() As Boolean
                            If ec.end_result() Then
                                If response_comm Is Nothing Then
                                    response_comm = link_status.response
                                End If
                                ec = sync_response(request, i, response_comm)
                                Return waitfor(ec) AndAlso
                                       goto_next()
                            Else
                                Return False
                            End If
                        End Function,
                        Function() As Boolean
                            Return ec.end_result() AndAlso
                                   goto_end()
                        End Function)
    End Function
End Module
