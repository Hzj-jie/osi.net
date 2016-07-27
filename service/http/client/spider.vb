
Option Strict On

Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.http.constants
Imports osi.service.http.constants.request_method

Partial Public Class client
    Private Shared Function spider_consume(ByVal url As String,
                                           ByVal request_method As request_method,
                                           ByVal request_headers As map(Of String, vector(Of String)),
                                           ByVal request_body As Stream,
                                           ByVal request_length As UInt64,
                                           ByVal result As response,
                                           ByVal request_comm As link_status,
                                           ByVal response_comm As link_status) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As stream_response = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New stream_response(Stream.Null)
                                  ec = request(url,
                                               request_method,
                                               request_headers,
                                               request_body,
                                               request_length,
                                               r,
                                               request_comm,
                                               response_comm)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     response.from(result, r) Then
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Shared Function spider_not_consume(ByVal url As String,
                                               ByVal request_method As request_method,
                                               ByVal request_headers As map(Of String, vector(Of String)),
                                               ByVal request_body As Stream,
                                               ByVal request_length As UInt64,
                                               ByVal result As response,
                                               ByVal request_comm As link_status,
                                               ByVal response_comm As link_status) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of HttpWebResponse) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of HttpWebResponse)()
                                  ec = request(url,
                                               request_method,
                                               request_headers,
                                               request_body,
                                               request_length,
                                               r,
                                               request_comm,
                                               response_comm)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     response.from(result, +r) Then
                                      r.close()
                                      Return goto_end()
                                  Else
                                      r.close()
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_method As request_method,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal request_body As Stream,
                                  ByVal request_length As UInt64,
                                  ByVal result As response,
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = If(consume_data,
                                          spider_consume(url,
                                                         request_method,
                                                         request_headers,
                                                         request_body,
                                                         request_length,
                                                         result,
                                                         request_comm,
                                                         response_comm),
                                          spider_not_consume(url,
                                                             request_method,
                                                             request_headers,
                                                             request_body,
                                                             request_length,
                                                             result,
                                                             request_comm,
                                                             response_comm))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_method As request_method,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal request_body As Stream,
                                  ByVal request_length As UInt64,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As response = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New response()
                                  ec = spider(url,
                                              request_method,
                                              request_headers,
                                              request_body,
                                              request_length,
                                              r,
                                              request_comm,
                                              response_comm,
                                              consume_data)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         r.eva(status, headers) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal request_body As Stream,
                                  ByVal request_length As UInt64,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      If(request_body Is Nothing, request_method.GET, request_method.POST),
                      request_headers,
                      request_body,
                      request_length,
                      status,
                      headers,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal request_body As Stream,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      request_headers,
                      request_body,
                      undefined_content_length,
                      status,
                      headers,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_body As Stream,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      Nothing,
                      request_body,
                      status,
                      headers,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      request_headers,
                      Nothing,
                      status,
                      headers,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal request_headers As map(Of String, vector(Of String)),
                                  ByVal status As pointer(Of HttpStatusCode),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      request_headers,
                      status,
                      Nothing,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  ByVal headers As pointer(Of WebHeaderCollection),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      DirectCast(Nothing, map(Of String, vector(Of String))),
                      status,
                      headers,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  ByVal status As pointer(Of HttpStatusCode),
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      status,
                      Nothing,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function

    Public Shared Function spider(ByVal url As String,
                                  Optional ByVal request_comm As link_status = Nothing,
                                  Optional ByVal response_comm As link_status = Nothing,
                                  Optional ByVal consume_data As Boolean = False) As event_comb
        Return spider(url,
                      Nothing,
                      request_comm,
                      response_comm,
                      consume_data)
    End Function
End Class
