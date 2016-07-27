
Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _headers_extension
    <Extension()> Public Function cast_headers(ByVal i As HttpListenerRequest) As WebHeaderCollection
        If i Is Nothing Then
            Return Nothing
        Else
            Return cast(Of WebHeaderCollection)(i.Headers())
        End If
    End Function

    <Extension()> Public Function [get](ByVal i As WebHeaderCollection,
                                        ByVal reqh As HttpRequestHeader,
                                        ByVal resh As HttpResponseHeader) As String
        If i Is Nothing Then
            Return Nothing
        ElseIf i.response_header() Then
            Return i(resh)
        ElseIf i.request_header() Then
            Return i(reqh)
        Else
            Try
                Return i(resh)
            Catch
            End Try
            Try
                Return i(reqh)
            Catch
            End Try
            Return Nothing
        End If
    End Function

    <Extension()> Public Function gzip(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return strsame(i(HttpResponseHeader.ContentEncoding),
                           constants.headers.values.content_encoding.gzip,
                           False) OrElse
                   strsame(i(HttpResponseHeader.ContentEncoding),
                           constants.headers.values.content_encoding.deflate,
                           False)
        End If
    End Function

    <Extension()> Public Function gzip(ByVal i As HttpListenerContext) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return gzip(i.Request())
        End If
    End Function

    <Extension()> Public Function gzip(ByVal i As HttpListenerRequest) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return gzip(i.cast_headers())
        End If
    End Function

    <Extension()> Public Function keep_alive(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return strsame(i(HttpResponseHeader.KeepAlive), constants.headers.values.keep_alive.true, False) OrElse
                   strsame(i(HttpResponseHeader.Connection), constants.headers.values.connection.keep_alive, False)
        End If
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As HttpListenerContext) As Date
        If i Is Nothing Then
            Return Nothing
        Else
            Return if_modified_since(i.Request())
        End If
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As HttpListenerRequest) As Date
        If i Is Nothing Then
            Return Nothing
        Else
            Return if_modified_since(i.cast_headers())
        End If
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As WebHeaderCollection) As Date
        If i Is Nothing Then
            Return Nothing
        Else
            Return if_modified_since(i(HttpRequestHeader.IfModifiedSince))
        End If
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As String) As Date
        Dim o As Date = Nothing
        If Date.TryParse(i, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As HttpListenerContext) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return chunked_transfer(i.Request())
        End If
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As HttpListenerRequest) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return chunked_transfer(i.cast_headers())
        End If
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return chunked_transfer([get](i,
                                          HttpRequestHeader.TransferEncoding,
                                          HttpResponseHeader.TransferEncoding))
        End If
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As String) As Boolean
        Return strsame(i, constants.headers.values.transfer_encoding.chunked, False)
    End Function

    <Extension()> Public Function referer(ByVal i As HttpListenerContext) As String
        If i Is Nothing Then
            Return Nothing
        Else
            Return referer(i.Request())
        End If
    End Function

    <Extension()> Public Function referer(ByVal i As HttpListenerRequest) As String
        If i Is Nothing Then
            Return Nothing
        Else
            Return i.UrlReferrer().AbsoluteUri()
        End If
    End Function

    <Extension()> Public Function range_series(ByVal i As HttpListenerContext) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        Else
            Return range_series(i.Request())
        End If
    End Function

    <Extension()> Public Function range_series(ByVal i As HttpListenerRequest) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        Else
            Return range_series(i.cast_headers())
        End If
    End Function

    <Extension()> Public Function range_series(ByVal i As WebHeaderCollection) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        Else
            Return range_series(i(HttpRequestHeader.Range))
        End If
    End Function

    <Extension()> Public Function range_series(ByVal i As String) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If String.IsNullOrEmpty(i) Then
            Return Nothing
        Else
            Dim unit As String = Nothing
            If strsep(i, unit, i, constants.headers.patterns.range.unit_range_set_separator) AndAlso
               Not String.IsNullOrEmpty(i) Then
                Dim vs() As String = Nothing
                vs = i.Split(constants.headers.patterns.range.range_separator_array,
                             StringSplitOptions.RemoveEmptyEntries)
                If array_size(vs) > 0 Then
                    Dim rs As vector(Of pair(Of Int64, Int64)) = Nothing
                    rs = New vector(Of pair(Of Int64, Int64))()
                    For j As Int32 = 0 To vs.Length() - 1
                        Dim f As String = Nothing
                        Dim s As String = Nothing
                        Dim fi As Int64 = 0
                        Dim si As Int64 = 0
                        If strsep(vs(j), f, s, constants.headers.patterns.range.range_value_separator) Then
                            If String.IsNullOrEmpty(f) Then
                                fi = constants.headers.values.range.not_presented
                            ElseIf Not Int64.TryParse(f, fi) Then
                                Return Nothing
                            End If
                            If String.IsNullOrEmpty(s) Then
                                si = constants.headers.values.range.not_presented
                            ElseIf Not Int64.TryParse(s, si) Then
                                Return Nothing
                            End If
                            If fi = constants.headers.values.range.not_presented AndAlso
                               si = constants.headers.values.range.not_presented Then
                                Return Nothing
                            Else
                                rs.push_back(make_pair(fi, si))
                            End If
                        Else
                            Return Nothing
                        End If
                    Next

                    Return make_pair(unit, rs)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End If
    End Function

    <Extension()> Public Function is_www_form_urlencoded(ByVal r As HttpListenerRequest,
                                                         Optional ByRef cs As String = Nothing) As Boolean
        Return Not r Is Nothing AndAlso
               r.HasEntityBody() AndAlso
               strsame(r.ContentType(),
                       constants.headers.values.content_type.request.www_form_urlencoded,
                       strlen(constants.headers.values.content_type.request.www_form_urlencoded),
                       False) AndAlso
               eva(cs, parse_charset(r.ContentType()))
    End Function

    <Extension()> Public Function is_www_form_urlencoded(ByVal ctx As HttpListenerContext,
                                                         Optional ByRef charset As String = Nothing) As Boolean
        Return Not ctx Is Nothing AndAlso
               is_www_form_urlencoded(ctx.Request(), charset)
    End Function
End Module
