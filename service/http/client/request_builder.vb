
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure

Public NotInheritable Class request_builder
    Private url As String
    Private method As constants.request_method
    Private headers As map(Of String, vector(Of String))
    Private body As Stream
    Private length As UInt64
    Private request_link_status As link_status
    Private response_link_status As link_status

    Public Sub New()
        url = empty_string
        method = constants.request_method.GET
        without_request_length()
    End Sub

    Public Shared Function [New]() As request_builder
        Return New request_builder()
    End Function

    Public Function with_url(ByVal url As String) As request_builder
        Me.url = url
        Return Me
    End Function

    Public Function without_method() As request_builder
        Me.method = Nothing
        Return Me
    End Function

    Public Function with_default_method() As request_builder
        Me.method = If(body Is Nothing, constants.request_method.GET, constants.request_method.POST)
        Return Me
    End Function

    Public Function with_method(ByVal method As constants.request_method) As request_builder
        Me.method = method
        Return Me
    End Function

    Public Function with_headers(ByVal headers As map(Of String, vector(Of String))) As request_builder
        Me.headers = headers
        Return Me
    End Function

    Public Function with_headers() As request_builder
        Return with_headers(New map(Of String, vector(Of String))())
    End Function

    Public Function with_header(ByVal key As String, ByVal value As vector(Of String)) As request_builder
        with_headers().headers(key) = value
        Return Me
    End Function

    Public Function with_header(ByVal key As String, ByVal value As String) As request_builder
        with_headers().headers(key).emplace_back(value)
        Return Me
    End Function

    Public Function with_header(ByVal key As String, ByVal value As Object) As request_builder
        with_headers().headers(key).emplace_back(Convert.ToString(value))
        Return Me
    End Function

    Public Function without_body() As request_builder
        Me.body = Nothing
        without_request_length()
        Return with_method(constants.request_method.GET)
    End Function

    Public Function with_body(ByVal stream As Stream) As request_builder
        If stream Is Nothing Then
            Return without_body()
        Else
            Me.body = stream
            Return with_method(constants.request_method.POST)
        End If
    End Function

    Public Function with_body(ByVal ms As MemoryStream) As request_builder
        If ms Is Nothing Then
            Return without_body()
        Else
            Dim r As Int64 = 0
            r = ms.Length()
            If r < 0 Then
                Return without_body()
            Else
                with_body(direct_cast(Of Stream)(ms))
                Return with_request_length(CULng(r))
            End If
        End If
    End Function

    Public Function with_body(ByVal body() As Byte) As request_builder
        Return with_body(New piece(body))
    End Function

    Public Function with_body(ByVal p As piece) As request_builder
        If p Is Nothing Then
            Return without_body()
        Else
            Dim ms As MemoryStream = Nothing
            If memory_stream.create(p.buff, CInt(p.offset), CInt(p.count), ms) Then
                Return with_body(ms)
            Else
                Return without_body()
            End If
        End If
    End Function

    Public Function with_body(ByVal s As String, Optional ByVal encoder As Encoding = Nothing) As request_builder
        Return with_body(memory_stream.create(s, encoder))
    End Function

    Public Function without_request_length() As request_builder
        Return with_request_length(undefined_content_length)
    End Function

    Public Function with_request_length(ByVal length As UInt64) As request_builder
        Me.length = length
        Return Me
    End Function

    Public Function with_request_link_status(ByVal ls As link_status) As request_builder
        Me.request_link_status = ls
        Return Me
    End Function

    Public Function with_response_link_status(ByVal ls As link_status) As request_builder
        Me.response_link_status = ls
        Return Me
    End Function

    Public Function request(ByVal r As pointer(Of HttpWebResponse)) As event_comb
        Return client.request(url, method, headers, body, length, r, request_link_status, response_link_status)
    End Function

    Public Function request(ByVal r As client.stream_response) As event_comb
        Return client.request(url, method, headers, body, length, r, request_link_status, response_link_status)
    End Function

    Public Function request(ByVal r As client.memory_stream_response) As event_comb
        Return client.request(url, method, headers, body, length, r, request_link_status, response_link_status)
    End Function

    Public Function request(ByVal r As client.string_response) As event_comb
        Return client.request(url, method, headers, body, length, r, request_link_status, response_link_status)
    End Function

    Public Function spider(ByVal r As client.response,
                           Optional ByVal consume_data As Boolean = True) As event_comb
        Return client.spider(url,
                             method,
                             headers,
                             body,
                             length,
                             r,
                             request_link_status,
                             response_link_status,
                             consume_data)
    End Function

    Public Function spider(Optional ByVal consume_data As Boolean = True) As event_comb
        Return spider(New client.response(), consume_data)
    End Function
End Class
