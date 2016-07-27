
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
    Public Class stream_response
        Inherits response
        Public ReadOnly result As Stream

        Public Sub New(ByVal result As Stream)
            Me.result = result
        End Sub

        Friend Shadows Function eva(ByVal status As pointer(Of HttpStatusCode),
                                    ByVal headers As pointer(Of WebHeaderCollection)) As Boolean
            Return MyBase.eva(status, headers)
        End Function
    End Class

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal response As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
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
                                     stream_response.from(response, +r) Then
                                      'though the request will fail if response is nothing,
                                      'the request has been sent to the server,
                                      'which is also valueable in some scenario.
                                      'so all the request methods are in this kind of design.
                                      If response Is Nothing Then
                                          r.close()
                                          Return goto_end()
                                      Else
                                          ec = (+r).read_response_body(response.result,
                                                                       response_comm,
                                                                       Nothing)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  Else
                                      r.close()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  r.close()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As stream_response = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New stream_response(result)
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
                                  Return ec.end_result() AndAlso
                                         r.eva(status, headers) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       If(request_body Is Nothing, request_method.GET, request_method.POST),
                       request_headers,
                       request_body,
                       request_length,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       If(request_body Is Nothing, request_method.GET, request_method.POST),
                       request_headers,
                       request_body,
                       request_length,
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       request_length,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       request_length,
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       request_body,
                       undefined_content_length,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       request_body,
                       undefined_content_length,
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       Nothing,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       Nothing,
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal result As stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       DirectCast(Nothing, map(Of String, vector(Of String))),
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal headers As pointer(Of WebHeaderCollection),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       DirectCast(Nothing, map(Of String, vector(Of String))),
                       status,
                       headers,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal status As pointer(Of HttpStatusCode),
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       status,
                       Nothing,
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal result As Stream,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       result,
                       request_comm,
                       response_comm)
    End Function
End Class
