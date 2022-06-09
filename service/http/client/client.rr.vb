
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports System.Net
Imports System.Text
Imports System.Web
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports uri = osi.root.connector.uri

Public Module _client_rr
    Public Const undefined_content_length As UInt64 = 0
    Private ReadOnly default_max_service_point_idle_time As Int32

    Sub New()
        assert(npos < 0)
        ServicePointManager.DefaultConnectionLimit() = max_int32
        ServicePointManager.MaxServicePoints() = max_int32
        ServicePointManager.UseNagleAlgorithm() = False
        default_max_service_point_idle_time = ServicePointManager.MaxServicePointIdleTime()
    End Sub

    Public Sub set_connection_reuse(ByVal enable As Boolean)
        If enable Then
            enable_connection_reuse()
        Else
            disable_connection_reuse()
        End If
    End Sub

    Public Sub enable_connection_reuse()
        ServicePointManager.MaxServicePointIdleTime() = default_max_service_point_idle_time
    End Sub

    Public Sub disable_connection_reuse()
        ServicePointManager.MaxServicePointIdleTime() = 0
    End Sub

    <Extension()> Public Sub close(ByVal this As ref(Of HttpWebResponse))
        If Not (+this) Is Nothing Then
            this.get().Close()
        End If
    End Sub

    <Extension()> Public Function url_encoded_parameters(ByVal i As map(Of String, String),
                                                         Optional ByVal e As Text.Encoding = Nothing) As String
        If i Is Nothing Then
            Return Nothing
        End If
        If e Is Nothing Then
            e = default_encoding
        End If
        Dim it As map(Of String, String).iterator = Nothing
        it = i.begin()
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        While it <> i.end()
            If r.Length() > 0 Then
                r.Append(constants.uri.argument_separator)
            End If
            r.Append(HttpUtility.UrlEncode((+it).first, e)) _
                 .Append(constants.uri.argument_name_value_separator) _
                 .Append(HttpUtility.UrlEncode((+it).second, e))
            it += 1
        End While
        Return Convert.ToString(r)
    End Function

    'always start from /, so it can be used directly in http request
    <Extension()> Public Function url_encoded_path(ByVal i As vector(Of String),
                                                   Optional ByVal e As Encoding = Nothing) As String
        If i.null_or_empty() Then
            Return constants.uri.path_separator
        End If
        Dim o As StringWriter = Nothing
        o = New StringWriter()
        For j As UInt32 = 0 To i.size() - uint32_1
            o.Write(constants.uri.path_separator)
            uri.path_encoder.encode(i(j), o, e)
        Next
        Return Convert.ToString(o)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i() As Byte,
                                                     ByVal offset As UInt32,
                                                     ByVal count As UInt32,
                                                     ByVal send_rate_sec As UInt32) As event_comb
        Return read_from_bytes(i,
                               offset,
                               count,
                               o,
                               send_rate_sec,
                               AddressOf set_content_length,
                               AddressOf fetch_stream)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i() As Byte,
                                                     ByVal offset As UInt32,
                                                     ByVal count As UInt32,
                                                     ByVal ls As link_status) As event_comb
        Return read_from_bytes(i,
                               offset,
                               count,
                               o,
                               ls.this_or_unlimited().rate_sec,
                               AddressOf set_content_length,
                               AddressOf fetch_stream)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i As String,
                                                     ByVal offset As UInt32,
                                                     ByVal count As UInt32,
                                                     ByVal enc As Text.Encoding,
                                                     ByVal send_rate_sec As UInt32) As event_comb
        Return read_from_string(i,
                                offset,
                                count,
                                enc,
                                o,
                                send_rate_sec,
                                AddressOf set_content_length,
                                AddressOf fetch_stream)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i As String,
                                                     ByVal offset As UInt32,
                                                     ByVal count As UInt32,
                                                     ByVal enc As Text.Encoding,
                                                     ByVal ls As link_status) As event_comb
        Return write_request_body(o,
                                  i,
                                  offset,
                                  count,
                                  enc,
                                  ls.this_or_unlimited().rate_sec)
    End Function

    <Extension()> Public Function write_request_body(
                                           ByVal o As HttpWebRequest,
                                           ByVal i As Stream,
                                           ByVal send_link_status As link_status,
                                           ByVal receive_link_status As link_status,
                                           ByVal close_input_stream As Boolean,
                                           Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return write_request_body(o,
                                  i,
                                  send_link_status.this_or_unlimited().buff_size,
                                  send_link_status.this_or_unlimited().rate_sec,
                                  receive_link_status.this_or_unlimited().rate_sec,
                                  close_input_stream,
                                  result)
    End Function

    <Extension()> Public Function write_request_body(
                                           ByVal o As HttpWebRequest,
                                           ByVal i As Stream,
                                           ByVal buff_size As UInt32,
                                           ByVal send_rate_sec As UInt32,
                                           ByVal receive_rate_sec As UInt32,
                                           ByVal close_input_stream As Boolean,
                                           Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return read_from_stream(i,
                                o,
                                buff_size,
                                receive_rate_sec,
                                send_rate_sec,
                                AddressOf set_chunked_transfer,
                                AddressOf fetch_stream,
                                result,
                                close_input_stream)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i As Stream,
                                                     ByVal count As UInt64,
                                                     ByVal send_link_status As link_status,
                                                     ByVal receive_link_status As link_status,
                                                     ByVal close_input_stream As Boolean) As event_comb
        Return write_request_body(o,
                                  i,
                                  count,
                                  send_link_status.this_or_unlimited().buff_size,
                                  send_link_status.this_or_unlimited().rate_sec,
                                  receive_link_status.this_or_unlimited().rate_sec,
                                  close_input_stream)
    End Function

    <Extension()> Public Function write_request_body(ByVal o As HttpWebRequest,
                                                     ByVal i As Stream,
                                                     ByVal count As UInt64,
                                                     ByVal buff_size As UInt32,
                                                     ByVal send_rate_sec As UInt32,
                                                     ByVal receive_rate_sec As UInt32,
                                                     ByVal close_input_stream As Boolean) As event_comb
        Return read_from_stream(i,
                                o,
                                count,
                                buff_size,
                                send_rate_sec,
                                receive_rate_sec,
                                AddressOf set_content_length,
                                AddressOf fetch_stream,
                                close_input_stream)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As ref(Of String),
                                                     ByVal enc As Text.Encoding,
                                                     ByVal buff_size As UInt32,
                                                     ByVal receive_rate_sec As UInt32,
                                                     ByVal max_content_length As UInt64) As event_comb
        Return write_to_string(i,
                               o,
                               enc,
                               buff_size,
                               receive_rate_sec,
                               max_content_length,
                               AddressOf fetch_headers,
                               AddressOf fetch_stream,
                               AddressOf get_content_length)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As ref(Of String),
                                                     ByVal enc As Text.Encoding,
                                                     ByVal ls As link_status) As event_comb
        Return read_response_body(i,
                                  o,
                                  enc,
                                  ls.this_or_unlimited().buff_size,
                                  ls.this_or_unlimited().rate_sec,
                                  ls.this_or_unlimited().max_content_length)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As ref(Of Byte()),
                                                     ByVal buff_size As UInt32,
                                                     ByVal receive_rate_sec As UInt32,
                                                     ByVal max_content_length As UInt64) As event_comb
        Return write_to_bytes(i,
                              o,
                              buff_size,
                              receive_rate_sec,
                              max_content_length,
                              AddressOf fetch_headers,
                              AddressOf fetch_stream,
                              AddressOf get_content_length)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As ref(Of Byte()),
                                                     ByVal ls As link_status) As event_comb
        Return read_response_body(i,
                                  o,
                                  ls.this_or_unlimited().buff_size,
                                  ls.this_or_unlimited().rate_sec,
                                  ls.this_or_unlimited().max_content_length)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As Stream,
                                                     ByVal receive_link_status As link_status,
                                                     ByVal send_link_status As link_status,
                                                     Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return read_response_body(i,
                                  o,
                                  receive_link_status.this_or_unlimited().buff_size,
                                  receive_link_status.this_or_unlimited().rate_sec,
                                  send_link_status.this_or_unlimited().rate_sec,
                                  receive_link_status.this_or_unlimited().max_content_length,
                                  result)
    End Function

    <Extension()> Public Function read_response_body(ByVal i As HttpWebResponse,
                                                     ByVal o As Stream,
                                                     ByVal buff_size As UInt32,
                                                     ByVal receive_rate_sec As UInt32,
                                                     ByVal send_rate_sec As UInt32,
                                                     ByVal max_content_length As UInt64,
                                                     Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return write_to_stream(i,
                               o,
                               buff_size,
                               receive_rate_sec,
                               send_rate_sec,
                               max_content_length,
                               result,
                               AddressOf fetch_headers,
                               AddressOf fetch_stream,
                               AddressOf get_content_length)
    End Function

    Private Function parse_response_encoding_from_headers(ByVal h As WebHeaderCollection,
                                                          ByRef o As Encoding) As Boolean
        If h Is Nothing Then
            Return False
        End If
        assert(Not h.gzip())
        Return try_get_encoding(h(HttpResponseHeader.ContentEncoding), o) OrElse
               parse_encoding(h(HttpResponseHeader.ContentType), o)
    End Function

    Private Function parse_response_encoding_from_meta(ByVal s As String,
                                                       ByRef o As Encoding) As Boolean
        Const regex As String = "<meta.*charset=(.*)"""
        Dim match As RegularExpressions.Match = Nothing
        match = RegularExpressions.Regex.Match(s,
                                               regex,
                                               RegularExpressions.RegexOptions.IgnoreCase Or
                                               RegularExpressions.RegexOptions.Multiline)
        Return match.Success() AndAlso
               Not match.Groups() Is Nothing AndAlso
               match.Groups().Count() = 2 AndAlso
               match.Groups()(1).Success() AndAlso
               try_get_encoding(match.Groups()(1).Value(), o)
    End Function

    Private Function parse_response_encoding_from_pseduo(ByVal s As String,
                                                         ByRef o As Encoding) As Boolean
        Const regex As String = "<?xml.*encoding=""(.*)"""
        Dim match As RegularExpressions.Match = Nothing
        match = RegularExpressions.Regex.Match(s,
                                               regex,
                                               RegularExpressions.RegexOptions.IgnoreCase Or
                                               RegularExpressions.RegexOptions.Multiline)
        Return match.Success() AndAlso
               Not match.Groups() Is Nothing AndAlso
               match.Groups().Count() = 2 AndAlso
               match.Groups()(1).Success() AndAlso
               try_get_encoding(match.Groups()(1).Value(), o)
    End Function

    Private Function parse_response_encoding_from_body(ByVal i As MemoryStream,
                                                       ByRef o As Encoding) As Boolean
        Const readsize As Int32 = 512
        If i Is Nothing OrElse Not i.CanRead() OrElse Not i.CanSeek() Then
            Return False
        End If
        Dim readcount As Int32 = 0
        Dim buff(readsize - 1) As Byte
        assert(i.Seek(0, SeekOrigin.Begin) = 0)
        readcount = i.Read(buff, 0, readsize)
        Dim s As String = Nothing
        Return Encoding.ASCII().try_get_string(buff, 0, readcount, s) AndAlso
               assert(i.Seek(0, SeekOrigin.Begin) = 0) AndAlso
               (parse_response_encoding_from_meta(s, o) OrElse
                parse_response_encoding_from_pseduo(s, o))
    End Function

    Public Function parse_response_encoding(ByVal h As WebHeaderCollection,
                                            ByVal i As MemoryStream,
                                            ByRef o As Encoding,
                                            ByRef gziped As Boolean) As Boolean
        If h Is Nothing Then
            Return False
        End If
        gziped = h.gzip()
        If gziped Then
            Return parse_response_encoding_from_body(i, o)
        End If
        Return parse_response_encoding_from_headers(h, o) OrElse
               parse_response_encoding_from_body(i, o)
    End Function

    Public Function parse_response_body(ByVal h As WebHeaderCollection,
                                        ByVal i As MemoryStream,
                                        ByRef o As String) As Boolean
        If h Is Nothing OrElse i Is Nothing Then
            Return False
        End If
        Dim e As Encoding = Nothing
        Dim gzip As Boolean = False
        If Not parse_response_encoding(h, i, e, gzip) Then
            e = default_encoding
            gzip = False
        End If

        Dim s As Stream = Nothing
        If gzip Then
            s = New GZipStream(i, CompressionMode.Decompress, True)
        Else
            s = i
        End If
        Using r As TextReader = New StreamReader(s, e, False)
            o = r.ReadToEnd()
        End Using
        If gzip Then
            s.Close()
            s.Dispose()
        End If
        Return True
    End Function

    Public Function generate_url(ByVal host As String,
                                 ByVal port As UInt16,
                                 Optional ByVal path_query As String = constants.uri.path_separator) As String
        Dim extra As String = Nothing
        If Not path_query.null_or_empty() AndAlso path_query(0) <> constants.uri.path_separator Then
            extra = constants.uri.path_separator
        End If
        If port = constants.default_value.port Then
            Return strcat(constants.protocol_address_head.http, host, extra, path_query)
        End If
        Return strcat(constants.protocol_address_head.http, host, constants.uri.port_mark, port, extra, path_query)
    End Function

    Public Function try_create_web_request(ByVal uri As String, ByRef o As WebRequest) As Boolean
        Try
            o = WebRequest.Create(uri)
            Return True
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to create WebRequest from uri ",
                        uri,
                        ", ex ",
                        ex.Message())
            Return False
        End Try
    End Function

    Public Function try_create_http_web_request(ByVal uri As String, ByRef o As HttpWebRequest) As Boolean
        Dim r As WebRequest = Nothing
        If Not try_create_web_request(uri, r) Then
            Return False
        End If
        If cast(Of HttpWebRequest)(r, o) Then
            Return True
        End If
        raise_error(error_type.warning, "uri ", uri, " is not an http url")
        r.Abort()
        Return False
    End Function

    <Extension()> Public Function to_http_headers(ByVal this(,) As String,
                                                  ByRef o As map(Of String, vector(Of String))) As Boolean
        If isemptyarray(this) OrElse this.GetLength(1) <> 2 Then
            Return False
        End If
        If o Is Nothing Then
            o = New map(Of String, vector(Of String))()
        Else
            o.clear()
        End If
        For i As Int32 = 0 To CInt(array_size(this)) - 1
            If this(i, 0).null_or_empty() Then
                Return False
            End If
            o(this(i, 0)).push_back(this(i, 1))
        Next
        Return True
    End Function

    <Extension()> Public Function to_http_headers(ByVal this(,) As String) As map(Of String, vector(Of String))
        Dim o As map(Of String, vector(Of String)) = Nothing
        assert(to_http_headers(this, o))
        Return o
    End Function
End Module
