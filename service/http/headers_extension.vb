
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation

Public Module _headers_extension
    <Extension()> Public Function cast_headers(ByVal i As HttpListenerRequest) As WebHeaderCollection
        If i Is Nothing Then
            Return Nothing
        End If
        Return direct_cast(Of WebHeaderCollection)(i.Headers())
    End Function

    <Extension()> Public Function [get](ByVal i As WebHeaderCollection,
                                        ByVal reqh As HttpRequestHeader,
                                        ByVal resh As HttpResponseHeader) As String
        If i Is Nothing Then
            Return Nothing
        End If

        If i.response_header() Then
            Return i(resh)
        End If

        If i.request_header() Then
            Return i(reqh)
        End If

        Try
            Return i(resh)
        Catch
        End Try

        Try
            Return i(reqh)
        Catch
        End Try

        Return Nothing
    End Function

    <Extension()> Public Function gzip(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return strsame(i(HttpResponseHeader.ContentEncoding),
                       constants.headers.values.content_encoding.gzip,
                       False) OrElse
               strsame(i(HttpResponseHeader.ContentEncoding),
                       constants.headers.values.content_encoding.deflate,
                       False)
    End Function

    <Extension()> Public Function gzip(ByVal i As HttpListenerContext) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return gzip(i.Request())
    End Function

    <Extension()> Public Function gzip(ByVal i As HttpListenerRequest) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return gzip(i.cast_headers())
    End Function

    <Extension()> Public Function keep_alive(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return strsame(i(HttpResponseHeader.KeepAlive), constants.headers.values.keep_alive.true, False) OrElse
               strsame(i(HttpResponseHeader.Connection), constants.headers.values.connection.keep_alive, False)
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As HttpListenerContext) As Date
        If i Is Nothing Then
            Return Nothing
        End If
        Return if_modified_since(i.Request())
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As HttpListenerRequest) As Date
        If i Is Nothing Then
            Return Nothing
        End If
        Return if_modified_since(i.cast_headers())
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As WebHeaderCollection) As Date
        If i Is Nothing Then
            Return Nothing
        End If
        Return if_modified_since(i(HttpRequestHeader.IfModifiedSince))
    End Function

    <Extension()> Public Function if_modified_since(ByVal i As String) As Date
        Dim o As Date = Nothing
        If Date.TryParse(i, o) Then
            Return o
        End If
        Return Nothing
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As HttpListenerContext) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return chunked_transfer(i.Request())
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As HttpListenerRequest) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return chunked_transfer(i.cast_headers())
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As WebHeaderCollection) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return chunked_transfer([get](i,
                                      HttpRequestHeader.TransferEncoding,
                                      HttpResponseHeader.TransferEncoding))
    End Function

    <Extension()> Public Function chunked_transfer(ByVal i As String) As Boolean
        Return strsame(i, constants.headers.values.transfer_encoding.chunked, False)
    End Function

    <Extension()> Public Function referer(ByVal i As HttpListenerContext) As String
        If i Is Nothing Then
            Return Nothing
        End If
        Return referer(i.Request())
    End Function

    <Extension()> Public Function referer(ByVal i As HttpListenerRequest) As String
        If i Is Nothing Then
            Return Nothing
        End If
        Return i.UrlReferrer().AbsoluteUri()
    End Function

    <Extension()> Public Function range_series(ByVal i As HttpListenerContext) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        End If
        Return range_series(i.Request())
    End Function

    <Extension()> Public Function range_series(ByVal i As HttpListenerRequest) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        End If
        Return range_series(i.cast_headers())
    End Function

    <Extension()> Public Function range_series(ByVal i As WebHeaderCollection) As  _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i Is Nothing Then
            Return Nothing
        End If
        Return range_series(i(HttpRequestHeader.Range))
    End Function

    <Extension()> Public Function range_series(ByVal i As String) As _
                                              pair(Of String, vector(Of pair(Of Int64, Int64)))
        If i.null_or_empty() Then
            Return Nothing
        End If
        Dim unit As String = Nothing
        If Not strsep(i, unit, i, constants.headers.patterns.range.unit_range_set_separator) OrElse
           i.null_or_empty() Then
            Return Nothing
        End If
        Dim vs() As String = Nothing
        vs = i.Split(constants.headers.patterns.range.range_separator_array,
                     StringSplitOptions.RemoveEmptyEntries)
        If isemptyarray(vs) Then
            Return Nothing
        End If

        Dim rs As vector(Of pair(Of Int64, Int64)) = Nothing
        rs = New vector(Of pair(Of Int64, Int64))()
        For j As Int32 = 0 To vs.Length() - 1
            Dim f As String = Nothing
            Dim s As String = Nothing
            Dim fi As Int64 = 0
            Dim si As Int64 = 0
            If Not strsep(vs(j), f, s, constants.headers.patterns.range.range_value_separator) Then
                Return Nothing
            End If
            If f.null_or_empty() Then
                fi = constants.headers.values.range.not_presented
            ElseIf Not Int64.TryParse(f, fi) Then
                Return Nothing
            End If
            If s.null_or_empty() Then
                si = constants.headers.values.range.not_presented
            ElseIf Not Int64.TryParse(s, si) Then
                Return Nothing
            End If
            If fi = constants.headers.values.range.not_presented AndAlso
               si = constants.headers.values.range.not_presented Then
                Return Nothing
            End If
            rs.push_back(pair.of(fi, si))
        Next

        Return pair.of(unit, rs)
    End Function

    ' Returns false if request body is not www-form-urlencoded or the encoder is not supported by the system.
    <Extension()> Public Function is_www_form_urlencoded(ByVal r As HttpListenerRequest,
                                                         Optional ByRef cs As String = Nothing,
                                                         Optional ByRef encoder As Text.Encoding = Nothing) As Boolean
        Return Not r Is Nothing AndAlso
               r.HasEntityBody() AndAlso
               strsame(r.ContentType(),
                       constants.headers.values.content_type.request.www_form_urlencoded,
                       strlen(constants.headers.values.content_type.request.www_form_urlencoded),
                       False) AndAlso
               parse_charset(r.ContentType(), cs) AndAlso
               parse_encoding(r.ContentType(), encoder)
    End Function

    <Extension()> Public Function is_www_form_urlencoded(ByVal ctx As HttpListenerContext,
                                                         Optional ByRef charset As String = Nothing,
                                                         Optional ByRef encoder As Text.Encoding = Nothing) As Boolean
        Return Not ctx Is Nothing AndAlso
               is_www_form_urlencoded(ctx.Request(), charset, encoder)
    End Function
End Module
