
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
    Public Class string_response
        Inherits response
        Private _result As String

        Public Function result() As String
            Return _result
        End Function

        Friend Shared Shadows Function from(ByVal this As string_response,
                                            ByVal that As memory_stream_response) As Boolean
            Return this Is Nothing OrElse
                   (response.from(this, that) AndAlso
                    (this.parse_response_body(that)))
        End Function

        Private Function parse_response_body(ByVal that As memory_stream_response) As Boolean
            assert(Not that Is Nothing)
            Dim r As Boolean = False
            r = _client_rr.parse_response_body(that.headers, that.ms, _result)
            that.close()
            Return r
        End Function

        Friend Shadows Function eva(ByVal status As pointer(Of HttpStatusCode),
                                    ByVal headers As pointer(Of WebHeaderCollection),
                                    ByVal result As pointer(Of String)) As Boolean
            Return MyBase.eva(status, headers) AndAlso
                   _eva.eva(result, Me.result())
        End Function
    End Class

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal result As string_response,
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
                                         string_response.from(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
