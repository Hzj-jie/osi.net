
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Public Module _headers
    Private ReadOnly _unsafe_add_header As invoker(Of Action(Of String, String))
    Private ReadOnly _allow_http_response_header As invoker(Of Func(Of Boolean))
    Private ReadOnly _allow_http_request_header As invoker(Of Func(Of Boolean))

    Sub New()
        _unsafe_add_header = invoker.of(_unsafe_add_header).
                                 with_type(Of WebHeaderCollection)().
                                 with_binding_flags(binding_flags.instance_private_method).
                                 with_name("AddWithoutValidate").
                                 build()
        assert(_unsafe_add_header.valid())
        assert(_unsafe_add_header.post_binding())

        invoker.of(_allow_http_response_header).
            with_type(Of WebHeaderCollection)().
            with_binding_flags(binding_flags.instance_private_method).
            with_name("get_AllowHttpResponseHeader").
            build(_allow_http_response_header)
        'for mono
        If Not _allow_http_response_header.valid() OrElse
           Not _allow_http_response_header.post_binding() Then
            raise_error(error_type.warning,
                        "get_AllowHttpResponseHeader is not presented in the implementation, ",
                        "the performance of get method will be impacted")
        End If

        invoker.of(_allow_http_request_header).
            with_type(Of WebHeaderCollection)().
            with_binding_flags(binding_flags.instance_private_method).
            with_name("get_AllowHttpRequestHeader").
            build(_allow_http_request_header)
        'for mono
        If Not _allow_http_request_header.valid() OrElse
           Not _allow_http_request_header.post_binding() Then
            raise_error(error_type.warning,
                        "get_AllowHttpRequestHeader is not presented in the implementation, ",
                        "the performance of get method will be impacted")
        End If
    End Sub

    <Extension()> Public Function unsafe_add_header(ByVal c As WebHeaderCollection,
                                                    ByVal name As String,
                                                    ByVal value As String) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Try
                _unsafe_add_header(c)(name, value)
                Return True
            Catch
                Return False
            End Try
        End If
    End Function

    <Extension()> Public Function response_header(ByVal c As WebHeaderCollection) As Boolean
        Dim o As Func(Of Boolean) = Nothing
        If _allow_http_response_header.post_bind(c, o) Then
            Try
                Return o()
            Catch
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function request_header(ByVal c As WebHeaderCollection) As Boolean
        Dim o As Func(Of Boolean) = Nothing
        If _allow_http_request_header.post_bind(c, o) Then
            Try
                Return o()
            Catch
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function copy_to(ByVal i As WebHeaderCollection,
                                          ByVal o As WebHeaderCollection) As Boolean
        If i Is Nothing OrElse o Is Nothing Then
            Return False
        Else
            For Each v As String In i.AllKeys()
                For Each s As String In i.GetValues(v)
                    If Not o.unsafe_add_header(v, s) Then
                        Return False
                    End If
                Next
            Next
            Return True
        End If
    End Function

    Private Function copy_range(ByVal i As HttpListenerRequest, ByVal o As HttpWebRequest) As Boolean
        If i Is Nothing OrElse o Is Nothing Then
            Return False
        End If
        Dim s As pair(Of String, vector(Of pair(Of Int64, Int64))) = Nothing
        s = i.range_series()
        If s Is Nothing OrElse s.second.null_or_empty() Then
            Return True
        End If

        For j As UInt32 = 0 To s.second.size() - uint32_1
            '.net 3.5 does not support int64 as range
            If s.second(j).first > max_int32 OrElse s.second(j).first < min_int32 OrElse
               s.second(j).second > max_int32 OrElse s.second(j).second < min_int32 Then
                Return False
            End If
            If s.second(j).first = constants.headers.values.range.not_presented Then
                o.AddRange(s.first, -CInt(s.second(j).second))
            ElseIf s.second(j).second = constants.headers.values.range.not_presented Then
                o.AddRange(s.first, CInt(s.second(j).first))
            Else
                o.AddRange(s.first, CInt(s.second(j).first), CInt(s.second(j).second))
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function copy_headers_to(ByVal i As HttpListenerRequest,
                                                  ByVal o As HttpWebRequest) As Boolean
        If i Is Nothing OrElse o Is Nothing Then
            Return False
        ElseIf cast(Of WebHeaderCollection)(i.Headers()).copy_to(o.Headers()) Then
            If i.chunked_transfer() Then
                o.SendChunked() = True
            Else
                o.ContentLength() = i.ContentLength64()
            End If
            'it's a known issue in .net framework, the user agent is missing in http listener request headers
            o.UserAgent() = i.UserAgent()
            o.ContentType() = i.ContentType()
            o.Expect() = i.cast_headers()(HttpRequestHeader.Expect)
            o.IfModifiedSince() = i.if_modified_since()
            o.KeepAlive() = i.KeepAlive()
            o.Referer() = i.referer()
            o.UserAgent() = i.UserAgent()

            Return copy_range(i, o)
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function copy_headers_to(ByVal i As HttpListenerContext,
                                                  ByVal o As HttpWebRequest) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return copy_headers_to(i.Request(), o)
        End If
    End Function

    <Extension()> Public Function copy_headers_to(ByVal i As HttpWebResponse,
                                                  ByVal o As HttpListenerResponse) As Boolean
        If i Is Nothing OrElse o Is Nothing OrElse i.Headers() Is Nothing Then
            Return False
        ElseIf i.Headers().copy_to(o.Headers()) Then
            If i.Headers().chunked_transfer() Then
                o.SendChunked() = True
            Else
                o.ContentLength64() = i.ContentLength()
            End If
            o.KeepAlive() = i.Headers().keep_alive()
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function copy_headers_to(ByVal i As HttpWebResponse,
                                                  ByVal o As HttpListenerContext) As Boolean
        If o Is Nothing Then
            Return False
        Else
            Return copy_headers_to(i, o.Response())
        End If
    End Function
End Module
