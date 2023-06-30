
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utils
Imports osi.service.http

Public Module test_http_server
    Private Sub handle_context(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
        ec = New event_comb(Function() As Boolean
                                If ctx Is Nothing Then
                                    Return False
                                Else
                                    Dim buff() As Byte = Nothing
                                    buff = Text.Encoding.UTF8().GetBytes(guid_str())
                                    ctx.Response().StatusCode() = HttpStatusCode.OK
                                    ctx.Response().StatusDescription() = "OK"
                                    ctx.Response().ContentLength64() = array_size(buff)
                                    Return waitfor(ctx.Response().write_response(buff, 0, array_size(buff), 0)) AndAlso
                                           goto_end()
                                End If
                            End Function)
    End Sub

    Sub New()
        enable_domain_unhandled_exception_handler()
        register_slimqless2_threadpool()
    End Sub

    Public Sub main(ByVal args() As String)
        set_not_debug_mode()
        register_slimqless2_threadpool()
        ServicePointManager.DefaultConnectionLimit() = max_int32
        ServicePointManager.MaxServicePoints() = max_int32
        Dim port As UInt16 = 0
        If isemptyarray(args) OrElse Not UInt16.TryParse(args(0), port) OrElse port = 0 Then
            port = CUShort(rnd_int(10000, 60001))
        End If
        Dim s As server = Nothing
        s = New server(New server.configuration() With {.max_connection_count = max_int32})
        AddHandler http_listener_context_handle.[New](s).handle_context, AddressOf handle_context
        assert(s.add_port(port))
        assert(s.start())
        write_console_line(strcat("server started on port ", port))
        AddHandler control_c.press, Sub() s.stop(30)
        gc_trigger()
    End Sub
End Module
