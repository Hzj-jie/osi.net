
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.http.constants

Partial Public NotInheritable Class client
    Public Class memory_stream_response
        Inherits response
        Public ReadOnly ms As MemoryStream

        Public Sub New()
            ms = New MemoryStream()
        End Sub

        Public Sub close()
            ms.Close()
            ms.Dispose()
        End Sub

        Friend Shared Shadows Function from(ByVal this As memory_stream_response,
                                            ByVal that As stream_response) As Boolean
            Return this Is Nothing OrElse
                   (response.from(this, that) AndAlso
                    (this.ms.Seek(0, SeekOrigin.Begin) = 0))
        End Function

        Friend Shadows Function eva(ByVal status As ref(Of HttpStatusCode),
                                    ByVal headers As ref(Of WebHeaderCollection),
                                    ByVal result As ref(Of MemoryStream)) As Boolean
            Return MyBase.eva(status, headers) AndAlso
                   _eva.eva(result, Me.ms)
        End Function

        Protected Overrides Sub Finalize()
            close()
            MyBase.Finalize()
        End Sub
    End Class

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal result As memory_stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As stream_response = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New stream_response(If(result Is Nothing, Nothing, result.ms))
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
                                 memory_stream_response.from(result, r) AndAlso
                                 goto_end()
                      End Function)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As memory_stream_response = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New memory_stream_response()
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
                                         r.eva(status, headers, result) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal result As memory_stream_response,
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As memory_stream_response,
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As memory_stream_response,
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As memory_stream_response,
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As memory_stream_response,
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As memory_stream_response,
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       DirectCast(Nothing, map(Of String, vector(Of String))),
                       result,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal headers As ref(Of WebHeaderCollection),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal status As ref(Of HttpStatusCode),
                                   ByVal result As ref(Of MemoryStream),
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
                                   ByVal result As ref(Of MemoryStream),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       result,
                       request_comm,
                       response_comm)
    End Function
End Class
