
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
    Public Class response
        Private _headers As WebHeaderCollection
        Private _mutually_authenticated As Boolean
        Private _from_cache As Boolean
        Private _protocol_version As Version
        Private _response_uri As System.Uri
        Private _status As HttpStatusCode
        Private _status_description As String

        Public Function headers() As WebHeaderCollection
            Return _headers
        End Function

        Public Function mutually_authenticated() As Boolean
            Return _mutually_authenticated
        End Function

        Public Function from_cache() As Boolean
            Return _from_cache
        End Function

        Public Function protocol_version() As Version
            Return _protocol_version
        End Function

        Public Function response_uri() As System.Uri
            Return _response_uri
        End Function

        Public Function status() As HttpStatusCode
            Return _status
        End Function

        Public Function status_description() As String
            Return _status_description
        End Function

        Friend Shared Function from(ByVal this As response, ByVal o As HttpWebResponse) As Boolean
            If o Is Nothing Then
                Return False
            ElseIf Not this Is Nothing Then
                this._headers = o.Headers()
                'for mono
                Try
                    this._mutually_authenticated = o.IsMutuallyAuthenticated()
                Catch
                    this._mutually_authenticated = False
                End Try
                Try
                    this._from_cache = o.IsFromCache()
                Catch
                    this._from_cache = False
                End Try
                this._protocol_version = o.ProtocolVersion()
                this._response_uri = o.ResponseUri()
                this._status = o.StatusCode()
                this._status_description = o.StatusDescription()
            End If
            Return True
        End Function

        Friend Shared Function from(ByVal this As response, ByVal that As response) As Boolean
            assert(Not that Is Nothing)
            If Not this Is Nothing Then
                this._headers = that.headers()
                this._mutually_authenticated = that.mutually_authenticated()
                this._from_cache = that.from_cache()
                this._protocol_version = that.protocol_version()
                this._response_uri = that.response_uri()
                this._status = that.status()
                this._status_description = that.status_description()
            End If
            Return True
        End Function

        Friend Function eva(ByVal status As pointer(Of HttpStatusCode),
                            ByVal headers As pointer(Of WebHeaderCollection)) As Boolean
            Return _eva.eva(status, Me.status()) AndAlso
                   _eva.eva(headers, Me.headers())
        End Function
    End Class

    Public Shared Function request(ByVal url As String,
                                   ByVal request_method As request_method,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As HttpWebRequest = Nothing
        Return New event_comb(Function() As Boolean
                                  If String.IsNullOrEmpty(url) Then
                                      Return False
                                  Else
                                      If request_comm Is Nothing Then
                                          request_comm = link_status.request
                                      End If
                                      If response_comm Is Nothing Then
                                          response_comm = link_status.response
                                      End If
                                      If try_create_http_web_request(url, r) Then
                                          r.Method() = request_method.str()
                                          If Not request_headers Is Nothing AndAlso Not request_headers.empty() Then
                                              Dim it As map(Of String, vector(Of String)).iterator = Nothing
                                              it = request_headers.begin()
                                              While it <> request_headers.end()
                                                  If Not (+it).second Is Nothing AndAlso Not (+it).second.empty() Then
                                                      For i As UInt32 = 0 To CUInt((+it).second.size() - 1)
                                                          r.Headers().unsafe_add_header((+it).first,
                                                                                        (+it).second(i))
                                                      Next
                                                  End If
                                                  it = DirectCast((it + 1), map(Of String, vector(Of String)).iterator)
                                              End While
                                          End If
                                          If Not request_body Is Nothing Then
                                              If request_length = undefined_content_length Then
                                                  ec = r.write_request_body(request_body,
                                                                            request_comm,
                                                                            response_comm,
                                                                            False)
                                              Else
                                                  ec = r.write_request_body(request_body,
                                                                            request_length,
                                                                            request_comm,
                                                                            response_comm,
                                                                            False)
                                              End If
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          Else
                                              ec = Nothing
                                              Return goto_next()
                                          End If
                                      Else
                                          Return False
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not r Is Nothing)
                                  If ec.end_result_or_null() Then
                                      ec = r.get_response(response)
                                      Return waitfor(ec, response_comm.timeout_ms) AndAlso
                                             goto_next()
                                  Else
                                      r.Abort()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return goto_end()
                                  Else
                                      assert(Not r Is Nothing)
                                      r.Abort()
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       If(request_body Is Nothing, request_method.GET, request_method.POST),
                       request_headers,
                       request_body,
                       request_length,
                       response,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal request_length As UInt64,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       request_length,
                       response,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal request_body As Stream,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       request_body,
                       undefined_content_length,
                       response,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_body As Stream,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       Nothing,
                       request_body,
                       response,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal request_headers As map(Of String, vector(Of String)),
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       request_headers,
                       Nothing,
                       response,
                       request_comm,
                       response_comm)
    End Function

    Public Shared Function request(ByVal url As String,
                                   ByVal response As pointer(Of HttpWebResponse),
                                   Optional ByVal request_comm As link_status = Nothing,
                                   Optional ByVal response_comm As link_status = Nothing) As event_comb
        Return request(url,
                       DirectCast(Nothing, map(Of String, vector(Of String))),
                       response,
                       request_comm,
                       response_comm)
    End Function
End Class
