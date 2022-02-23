
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports Text = System.Text

Public Module _server_rr
    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
                                                    ByVal r As ref(Of String),
                                                    ByVal enc As Text.Encoding,
                                                    ByVal buff_size As UInt32,
                                                    ByVal receive_rate_sec As UInt32,
                                                    ByVal max_content_length As UInt64) As event_comb
        Return write_to_string(i,
                               r,
                               enc,
                               buff_size,
                               receive_rate_sec,
                               max_content_length,
                               AddressOf fetch_headers,
                               AddressOf fetch_stream,
                               AddressOf get_content_length)
    End Function

    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
                                                    ByVal r As ref(Of String),
                                                    ByVal enc As Text.Encoding,
                                                    ByVal ls As link_status) As event_comb
        Return read_request_body(i,
                                 r,
                                 enc,
                                 ls.this_or_unlimited().buff_size,
                                 ls.this_or_unlimited().rate_sec,
                                 ls.this_or_unlimited().max_content_length)
    End Function

    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
                                                    ByVal r As ref(Of Byte()),
                                                    ByVal buff_size As UInt32,
                                                    ByVal receive_rate_sec As UInt32,
                                                    ByVal max_content_length As UInt64) As event_comb
        Return write_to_bytes(i,
                              r,
                              buff_size,
                              receive_rate_sec,
                              max_content_length,
                              AddressOf fetch_headers,
                              AddressOf fetch_stream,
                              AddressOf get_content_length)
    End Function

    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
                                                    ByVal r As ref(Of Byte()),
                                                    ByVal ls As link_status) As event_comb
        Return read_request_body(i,
                                 r,
                                 ls.this_or_unlimited().buff_size,
                                 ls.this_or_unlimited().rate_sec,
                                 ls.this_or_unlimited().max_content_length)
    End Function

    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
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
                               Function(ByVal x As HttpListenerRequest) As WebHeaderCollection
                                   Return direct_cast(Of WebHeaderCollection)(x.Headers())
                               End Function,
                               Function(ByVal x As HttpListenerRequest) As Stream
                                   Return x.InputStream()
                               End Function,
                               Function(ByVal x As HttpListenerRequest) As Int64
                                   Return x.ContentLength64()
                               End Function)
    End Function

    <Extension()> Public Function read_request_body(ByVal i As HttpListenerRequest,
                                                    ByVal o As Stream,
                                                    ByVal receive_link_status As link_status,
                                                    ByVal send_link_status As link_status,
                                                    Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return read_request_body(i,
                                 o,
                                 receive_link_status.this_or_unlimited().buff_size,
                                 receive_link_status.this_or_unlimited().rate_sec,
                                 send_link_status.this_or_unlimited().rate_sec,
                                 receive_link_status.this_or_unlimited().max_content_length,
                                 result)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
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

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
                                                 ByVal i() As Byte,
                                                 ByVal offset As UInt32,
                                                 ByVal count As UInt32,
                                                 ByVal ls As link_status) As event_comb
        Return write_response(o,
                              i,
                              offset,
                              count,
                              ls.this_or_unlimited().rate_sec)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
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

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
                                                 ByVal i As String,
                                                 ByVal offset As UInt32,
                                                 ByVal count As UInt32,
                                                 ByVal enc As Text.Encoding,
                                                 ByVal ls As link_status) As event_comb
        Return write_response(o,
                              i,
                              offset,
                              count,
                              enc,
                              ls.this_or_unlimited().rate_sec)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
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
                                receive_rate_sec,
                                send_rate_sec,
                                Sub(ByVal x As HttpListenerResponse, ByVal l As Int64)
                                    x.ContentLength64() = l
                                End Sub,
                                Function(ByVal x As HttpListenerResponse,
                                         ByVal os As ref(Of Stream)) As event_comb
                                    Return sync_async(Sub()
                                                          eva(os, x.OutputStream())
                                                      End Sub)
                                End Function,
                                close_input_stream)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
                                                 ByVal i As Stream,
                                                 ByVal count As UInt64,
                                                 ByVal send_link_status As link_status,
                                                 ByVal receive_link_status As link_status,
                                                 ByVal close_input_stream As Boolean) As event_comb
        Return write_response(o,
                              i,
                              count,
                              send_link_status.this_or_unlimited().buff_size,
                              send_link_status.this_or_unlimited().rate_sec,
                              receive_link_status.this_or_unlimited().rate_sec,
                              close_input_stream)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
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
                                Sub(ByVal x As HttpListenerResponse)
                                    x.SendChunked() = True
                                End Sub,
                                Function(ByVal x As HttpListenerResponse,
                                         ByVal os As ref(Of Stream)) As event_comb
                                    Return sync_async(Sub()
                                                          eva(os, x.OutputStream())
                                                      End Sub)
                                End Function,
                                result,
                                close_input_stream)
    End Function

    <Extension()> Public Function write_response(ByVal o As HttpListenerResponse,
                                                 ByVal i As Stream,
                                                 ByVal send_link_status As link_status,
                                                 ByVal receive_link_status As link_status,
                                                 ByVal close_input_stream As Boolean,
                                                 Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        Return write_response(o,
                              i,
                              send_link_status.this_or_unlimited().buff_size,
                              send_link_status.this_or_unlimited().rate_sec,
                              receive_link_status.this_or_unlimited().rate_sec,
                              close_input_stream,
                              result)
    End Function

    <Extension()> Public Sub shutdown(ByVal ctx As HttpListenerContext,
                                      Optional ByVal abort As Boolean = False)
        Try
            If ctx IsNot Nothing Then
                ctx.Request().InputStream().Close()
                ctx.Request().InputStream().Dispose()
            End If
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to close request streams of HttpListenerContext, ex ",
                        ex.Message())
        End Try

        Try
            If ctx IsNot Nothing Then
                ctx.Response().OutputStream().Close()
                ctx.Response().OutputStream().Dispose()
            End If
        Catch 'ex As Exception
            'exception may throw, but do not want to waste disk resource
#If 0 Then
            raise_error(error_type.warning,
                        "failed to close response streams of HttpListenerContext, ex ",
                        ex.Message())
#End If
        End Try

        Try
            If ctx IsNot Nothing Then
                If abort Then
                    ctx.Response().Abort()
                Else
                    ctx.Response().Close()
                End If
            End If
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to close HttpListenerContext, ex ",
                        ex.Message())
        End Try
    End Sub
End Module
