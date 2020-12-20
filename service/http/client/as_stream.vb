
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.http.constants

Partial Public NotInheritable Class client
    Public Class stream_response
        Inherits response
        Public ReadOnly result As Stream

        Public Sub New(ByVal result As Stream)
            Me.result = result
        End Sub

        Friend Shadows Function eva(ByVal status As ref(Of HttpStatusCode),
                                    ByVal headers As ref(Of WebHeaderCollection)) As Boolean
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
        Dim r As ref(Of HttpWebResponse) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of HttpWebResponse)()
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
End Class
