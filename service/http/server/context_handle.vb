
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.procedure

Public Class context_handle
    Public Event handle_context(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
    Public Event handle_context_sync(ByVal ctx As HttpListenerContext)
    Public Event handle_context_async(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
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

    Public Shared Function [New](ByVal server As server) As context_handle
        Return New context_handle(server)
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
        assert_begin(New event_comb(Function() As Boolean
                                        If event_attached(handle_contextEvent) Then
                                            RaiseEvent handle_context(ctx.context, ec)
                                            If ec Is Nothing Then
                                                ctx.finish()
                                                Return goto_end()
                                            Else
                                                Return waitfor(ec, ctx.response_timeout_ms) AndAlso
                                                       goto_next()
                                            End If
                                        ElseIf event_attached(handle_context_syncEvent) Then
                                            RaiseEvent handle_context_sync(ctx.context)
                                            ctx.finish()
                                            Return goto_end()
                                        ElseIf event_attached(handle_context_asyncEvent) Then
                                            RaiseEvent handle_context_async(ctx.context, ec)
                                            Return waitfor(ec, ctx.response_timeout_ms) AndAlso
                                                   goto_next()
                                        ElseIf event_attached(handle_context_offlineEvent) Then
                                            RaiseEvent handle_context_offline(ctx.context,
                                                                              ctx.response_timeout_ms,
                                                                              Sub()
                                                                                  ctx.finish()
                                                                              End Sub)
                                            Return goto_end()
                                        Else
                                            ctx.context.Response().StatusCode() = HttpStatusCode.InternalServerError
                                            ctx.context.Response().StatusDescription() = "NOT_IMPLEMENTED"
                                            ctx.finish()
                                            Return goto_end()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        assert(Not ec Is Nothing)
                                        ctx.finish(Not ec.end_result())
                                        Return goto_end()
                                    End Function))
    End Sub
End Class
