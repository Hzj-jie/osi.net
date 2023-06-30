
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.procedure

' Convert server.context_received event into various and handy formats for different scenarios. In general, this class
' should be used instead of using server.context_received event directly.
Public NotInheritable Class http_listener_context_handle
    ' The event_comb is optional.
    Public Event handle_context(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
    ' No further operation should be performed after the return of this event handler.
    Public Event handle_context_sync(ByVal ctx As HttpListenerContext)
    ' The event_comb is required.
    Public Event handle_context_async(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
    ' The after_respond should be called once the handling has been finished.
    Public Event handle_context_offline(ByVal ctx As HttpListenerContext,
                                        ByVal response_timeout_ms As Int64,
                                        ByVal after_respond As Action)

    Private ReadOnly handle As server.context_receivedEventHandler

    Public Sub New()
        handle = AddressOf context_received
    End Sub

    Public Sub New(ByVal server As server)
        Me.New()
        attach(server)
    End Sub

    Public Shared Function [New](ByVal server As server) As http_listener_context_handle
        Return New http_listener_context_handle(server)
    End Function

    Public Sub attach(ByVal server As server)
        assert(Not server Is Nothing)
        AddHandler server.context_received, handle
    End Sub

    Public Sub detach(ByVal server As server)
        assert(Not server Is Nothing)
        RemoveHandler server.context_received, handle
    End Sub

    Private Sub context_received(ByVal ctx As server.context)
        assert(Not ctx Is Nothing)
        Dim ec As event_comb = Nothing
        If event_attached(handle_contextEvent) Then
            RaiseEvent handle_context(ctx.context, ec)
        ElseIf event_attached(handle_context_syncEvent) Then
            RaiseEvent handle_context_sync(ctx.context)
            ctx.finish()
            Return
        ElseIf event_attached(handle_context_asyncEvent) Then
            RaiseEvent handle_context_async(ctx.context, ec)
            If ec Is Nothing Then
                ctx.unexpected_error()
                Return
            End If
        ElseIf event_attached(handle_context_offlineEvent) Then
            RaiseEvent handle_context_offline(ctx.context,
                                              ctx.ls.timeout_ms,
                                              Sub()
                                                  ctx.finish()
                                              End Sub)
            Return
        Else
            ctx.not_implemented()
            Return
        End If

        procedure_handle.process_context(ctx, ec)
    End Sub
End Class
