
Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.transmitter

<type_attribute()>
Public Class server_dev
    Implements text

    Private Shared ReadOnly content_type As String
    Private ReadOnly id As String
    Private ReadOnly ctx As HttpListenerContext
    Private ReadOnly ls As link_status
    Private ReadOnly after_respond As Action
    Private finished As Boolean

    Shared Sub New()
        content_type = strcat(constants.commander_content_type,
                              root.constants.content_type_charset_prefix,
                              constants.dev_enc.WebName())
        assert(strlen(strcat(constants.uri.path_separator, constants.uri.query_mark)) = uint32_2)
        type_attribute.of(Of server_dev).set(osi.service.transmitter.transmitter.[New]().
            with_transmit_mode(osi.service.transmitter.transmitter.mode_t.receive_send).
            with_one_off(True))
    End Sub

    Public Sub New(ByVal ctx As HttpListenerContext,
                   Optional ByVal ls As link_status = Nothing,
                   Optional ByVal after_respond As Action = Nothing)
        Me.ctx = ctx
        If ls Is Nothing Then
            Me.ls = link_status.server
        Else
            Me.ls = ls
        End If
        Me.after_respond = after_respond
        Me.id = If(ctx Is Nothing,
                   "NO_CONTEXT_ERROR",
                   strcat(ctx.Request().RemoteEndPoint(),
                          character.at,
                          ctx.Request().Url()))
        Me.finished = False
    End Sub

    Public Sub New(ByVal ctx As HttpListenerContext,
                   ByVal buff_size As UInt32,
                   ByVal rate_sec As UInt32,
                   ByVal max_content_length As UInt64,
                   Optional ByVal after_respond As Action = Nothing)
        Me.New(ctx,
               New link_status(uint32_0, buff_size, rate_sec, max_content_length),
               after_respond)
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements text.sense
        Return sync_async(Function() As Boolean
                              If finished Then
                                  Return False
                              Else
                                  Return eva(pending, True)
                              End If
                          End Function)
    End Function

    Public Function send(ByVal s As String,
                         ByVal offset As UInt32,
                         ByVal len As UInt32) As event_comb Implements text.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If finished Then
                                      Return False
                                  ElseIf ctx Is Nothing OrElse
                                     strlen(s) < offset + len Then
                                      ctx.Response().StatusCode() = HttpStatusCode.InternalServerError
                                      ctx.Response().StatusDescription() = "CANNOT_FULFILL_REQUEST"
                                      finished = True
                                      Return goto_end()
                                  Else
                                      ctx.Response().StatusCode() = HttpStatusCode.OK
                                      ctx.Response().StatusDescription() = "OK"
                                      ctx.Response().ContentType() = content_type
                                      ec = ctx.Response().write_response(s,
                                                                         offset,
                                                                         len,
                                                                         constants.dev_enc,
                                                                         ls)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  finished = True
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal r As pointer(Of String)) As event_comb Implements text.receive
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If ctx Is Nothing OrElse finished Then
                                      Return False
                                  Else
                                      If ctx.Request().HasEntityBody() Then
                                          ec = ctx.Request().read_request_body(r, constants.dev_enc, ls)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Dim s As String = Nothing
                                          s = strmid(ctx.Request().Url().PathAndQuery(), uint32_2)
                                          Return eva(r,
                                                     If(String.IsNullOrEmpty(s),
                                                        s,
                                                        constants.dev_enc.GetString(
                                                            Convert.FromBase64String(s)))) AndAlso
                                                 goto_end()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function validate(ByVal this As server_dev) As Boolean
        assert(Not this Is Nothing)
        Return Not this.finished
    End Function

    Private Shared Sub close(ByVal this As server_dev)
        assert(Not this Is Nothing)
        this.ctx.shutdown()
        If Not this.after_respond Is Nothing Then
            this.after_respond()
        End If
    End Sub

    Private Shared Function identity(ByVal this As server_dev) As String
        assert(Not this Is Nothing)
        Return this.id
    End Function

    Public Shared Operator +(ByVal this As server_dev) As idevice(Of text)
        Return New delegate_device(Of text)(this, AddressOf validate, AddressOf close, AddressOf identity)
    End Operator

    Public Shared Function device_exporter(ByVal s As server,
                                           Optional ByVal ls As link_status = Nothing) _
                                          As imanual_device_exporter(Of text)
        assert(Not s Is Nothing)
        Dim o As imanual_device_exporter(Of text) = Nothing
        o = New manual_device_exporter(Of text)(s.identity())
        AddHandler s.handle_context_offline,
            Sub(ctx As HttpListenerContext, response_timeout_ms As Int64, after_respond As Action)
                ' TODO: the response_timeout_ms cannot be sent to responder
                If Not o.inject(+(New server_dev(ctx, ls, after_respond))) Then
                    ' Should never happen
                    raise_error(error_type.exclamation, "failed to inject http request to device exporter.")
                End If
            End Sub
        Return o
    End Function

    Public Shared Function device_pool(ByVal s As server,
                                       Optional ByVal ls As link_status = Nothing,
                                       Optional ByVal max_connection As UInt32 = uint32_0) As idevice_pool(Of text)
        assert(Not s Is Nothing)
        Return manual_pre_generated_device_pool.[New](device_exporter(s, ls), max_connection, s.identity())
    End Function
End Class
