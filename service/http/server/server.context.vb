
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public NotInheritable Class server
    Partial Public NotInheritable Class context
        Public ReadOnly server As server
        Public ReadOnly context As HttpListenerContext
        Public ReadOnly ls As link_status
        Public ReadOnly encoder As Encoding
        Private ReadOnly f As once_action
        Private ReadOnly se As stopwatch.event

        Public abort As Boolean = False

        ' To keep a consistent interface, all the variables are wrapped by a constant.
        ' null: uninitialized.
        ' constant(Of ).empty(): failed to initialize.

        ' The raw request-body, or empty() is request has no body.
        Private request_body As [optional](Of piece)
        ' The parsed path.
        Private path As [optional](Of vector(Of String))
        ' The parsed query string from url.
        Private query As [optional](Of map(Of String, vector(Of String)))
        ' The parsed request-body in www-form-urlencoded format.
        Private www_form_urlencoded_body As [optional](Of www_form_urlencoded)
        ' The merged query from both url and www-form-urlencoded request body.
        Private merged_query As [optional](Of map(Of String, vector(Of String)))
        ' The request method.
        Private method As [optional](Of constants.request_method)

        Public Sub New(ByVal server As server,
                       ByVal context As HttpListenerContext,
                       ByVal ls As link_status,
                       ByVal encoder As Encoding)
            assert(server IsNot Nothing)
            assert(context IsNot Nothing)
            assert(ls IsNot Nothing)
            assert(encoder IsNot Nothing)
            Me.server = server
            Me.context = context
            Me.ls = ls
            Me.encoder = encoder
            Me.f = New once_action(Sub()
                                       server.end_context(context, abort)
                                   End Sub)
            Me.se = stopwatch.push(ls.timeout_ms,
                                   Sub()
                                       f.run()
                                   End Sub)
            assert(se IsNot Nothing)
        End Sub

        Public Sub finish()
            f.run()
            se.cancel()
        End Sub

        Public Sub finish(ByVal abort As Boolean)
            Me.abort = abort
            finish()
        End Sub

        Public Function set_status(ByVal code As HttpStatusCode, ByVal description As String) As Boolean
            Try
                context.Response().StatusCode() = code
                context.Response().StatusDescription() = description
            Catch ex As ObjectDisposedException
                raise_error("Failed to set status of the request ", path, ", ", ex.details())
                Return False
            End Try
            Return True
        End Function

        Public Sub unexpected_error()
            set_status(HttpStatusCode.InternalServerError, "UNEXPECTED_ERROR")
            finish()
        End Sub

        Public Sub not_implemented()
            set_status(HttpStatusCode.NotImplemented, "NOT_IMPLEMENTED")
            finish()
        End Sub

        Public Function respond(ByVal b As piece) As event_comb
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If Not set_status(HttpStatusCode.OK, "OK") Then
                                          Return False
                                      End If

                                      If b Is Nothing Then
                                          Return goto_end()
                                      Else
                                          ec = context.Response().write_response(
                                                       b.buff,
                                                       b.offset,
                                                       b.count,
                                                       ls)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Function respond(ByVal b() As Byte) As event_comb
            Return respond(New piece(b))
        End Function

        Public Function respond(ByVal s As String) As event_comb
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      Dim b() As Byte = Nothing
                                      Try
                                          b = encoder.GetBytes(s)
                                      Catch ex As Exception
                                          raise_error(error_type.warning,
                                                      "Cannot encode [",
                                                      strleft(s, 10),
                                                      "] with ",
                                                      encoder.WebName(),
                                                      ", fallback to use ",
                                                      constants.default_value.encoder.WebName(),
                                                      ", ",
                                                      ex.details())
                                      End Try
                                      If b Is Nothing Then
                                          Try
                                              b = constants.default_value.encoder.GetBytes(s)
                                          Catch ex As Exception
                                              raise_error(error_type.warning,
                                                          "Cannot encode [",
                                                          strleft(s, 10),
                                                          "] with ",
                                                          constants.default_value.encoder.WebName(),
                                                          ", ",
                                                          ex.details())
                                              Return False
                                          End Try
                                      End If
                                      assert(b IsNot Nothing)
                                      ec = respond(b)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Function read_request_body(ByVal o As ref(Of piece)) As event_comb
            Dim p As ref(Of Byte()) = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If request_body Then
                                          Return Not request_body.empty() AndAlso
                                                 eva(o, +request_body) AndAlso
                                                 goto_end()
                                      End If

                                      p = New ref(Of Byte())()
                                      ec = context.Request().read_request_body(p, ls)
                                      Return waitfor(ec) AndAlso
                                                 goto_next()
                                  End Function,
                                  Function() As Boolean
                                      ' Do not expect to be called in two procedures.
                                      assert(Not request_body)
                                      If ec.end_result() Then
                                          If p.empty() Then
                                              request_body = [optional].of(piece.blank)
                                          Else
                                              request_body = [optional].of(New piece(+p))
                                          End If
                                          Return eva(o, +request_body) AndAlso
                                                 goto_end()
                                      Else
                                          request_body = [optional].of(piece.null)
                                          Return False
                                      End If
                                  End Function)
        End Function

        Public Function parse_www_form_encoded_body(ByVal result As ref(Of www_form_urlencoded)) As event_comb
            Dim ec As event_comb = Nothing
            Dim b As ref(Of piece) = Nothing
            Return New event_comb(Function() As Boolean
                                      If www_form_urlencoded_body Then
                                          Return Not www_form_urlencoded_body.empty() AndAlso
                                                 eva(result, +www_form_urlencoded_body) AndAlso
                                                 goto_end()
                                      End If

                                      b = New ref(Of piece)()
                                      ec = read_request_body(b)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      ' Do not expect to be called in two procedures.
                                      assert(Not www_form_urlencoded_body)
                                      If ec.end_result() Then
                                          Dim m As map(Of String, vector(Of String)) = Nothing
                                          Dim charset As String = Nothing
                                          Dim encoder As Encoding = Nothing
                                          Dim is_www_form_urlencoded As Boolean = False
                                          is_www_form_urlencoded = url_query.parse_www_form_urlencoded_request_body(
                                                                       context.Request(), +b, m, charset, encoder)
                                          If Not is_www_form_urlencoded Then
                                              raise_error(error_type.user,
                                                          "Failed to parse www-form-urlencoded request body.")
                                          End If
                                          www_form_urlencoded_body = [optional].of(New www_form_urlencoded(
                                                                      Me, is_www_form_urlencoded, charset, encoder, m))
                                          Return eva(result, +www_form_urlencoded_body) AndAlso
                                                 goto_end()
                                      Else
                                          www_form_urlencoded_body = [optional].of(
                                                                         [default](Of www_form_urlencoded).null)
                                          Return False
                                      End If
                                  End Function)
        End Function

        Public Function parse_query(ByVal include_post As Boolean,
                                    ByVal result As ref(Of map(Of String, vector(Of String)))) As event_comb
            Dim ec As event_comb = Nothing
            Dim url_query As map(Of String, vector(Of String)) = Nothing
            Dim p As ref(Of www_form_urlencoded) = Nothing
            Return New event_comb(Function() As Boolean
                                      url_query = parse_query()
                                      If url_query Is Nothing Then
                                          Return False
                                      End If
                                      If Not include_post Then
                                          Return eva(result, url_query) AndAlso
                                                 goto_end()
                                      End If

                                      If merged_query Then
                                          Return Not merged_query.empty() AndAlso
                                                 eva(result, +merged_query) AndAlso
                                                 goto_end()
                                      End If

                                      p = New ref(Of www_form_urlencoded)()
                                      ec = parse_www_form_encoded_body(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() Then
                                          assert(Not p.empty())
                                          Dim m As map(Of String, vector(Of String)) = Nothing
                                          m = url_query.clone()
                                          Dim it As map(Of String, vector(Of String)).iterator = Nothing
                                          it = (+p).query.begin()
                                          While it <> (+p).query.end()
                                              assert(m((+it).first).emplace_back((+it).second))
                                          End While
                                          merged_query = [optional].of(m)
                                          Return eva(result, m) AndAlso
                                                 goto_end()
                                      Else
                                          merged_query = [optional].of(
                                                             [default](Of map(Of String, vector(Of String))).null)
                                          Return False
                                      End If
                                  End Function)
        End Function

        Public Function parse_post_query(ByVal result As ref(Of map(Of String, vector(Of String)))) As event_comb
            Return parse_query(True, result)
        End Function

        Public Function parse_query() As map(Of String, vector(Of String))
            If Not query Then
                Dim m As map(Of String, vector(Of String)) = Nothing
                If Not url_query.parse(context.Request(), m, encoder) Then
                    assert(m Is Nothing)
                    raise_error(error_type.user, "Failed to parse url query.")
                End If
                query = [optional].of(m)
            End If
            assert(query)
            Return +query
        End Function

        Public Function parse_path() As vector(Of String)
            If Not path Then
                Dim v As vector(Of String) = Nothing
                If Not url_path.parse(context.Request(), v, encoder) Then
                    assert(v Is Nothing)
                    raise_error(error_type.user, "Failed to parse url path.")
                End If
                path = [optional].of(v)
            End If
            assert(path)
            Return +path
        End Function

        Public Function parse_method() As constants.request_method
            If Not method Then
                Dim m As constants.request_method = Nothing
                m = constants.request_method.GET
                Dim s As String = Nothing
                s = context.Request().HttpMethod()
                For i As Int32 = 0 To array_size_i(constants.request_method_str) - 1
                    If strsame(constants.request_method_str(i), s, False) Then
                        m = enum_def(Of constants.request_method).from(i)
                        Exit For
                    End If
                Next

                method = [optional].of(m)
            End If
            assert(method)
            Return +method
        End Function
    End Class
End Class
