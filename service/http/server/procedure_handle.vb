
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure

' A set of static functions to help execute an event_comb on a server.context. I.e. call the server.context.finish()
' when the event_comb has been finished or timed out.
Public NotInheritable Class procedure_handle
    Public Shared Sub process_context(ByVal ctx As server.context, ByVal ec As event_comb)
        assert(Not ctx Is Nothing)
        If ec Is Nothing Then
            ctx.finish()
        Else
            assert_begin(New event_comb(Function() As Boolean
                                            Return waitfor(ec, ctx.ls.timeout_ms) AndAlso
                                                   goto_next()
                                        End Function,
                                        Function() As Boolean
                                            assert(Not ec Is Nothing)
                                            ctx.finish(Not ec.end_result())
                                            Return goto_end()
                                        End Function))
        End If
    End Sub

    Public Shared Function process_context(ByVal ctx As server.context,
                                           ByVal f As _do_val_ref(Of server.context, event_comb, Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim ec As event_comb = Nothing
        If f(ctx, ec) Then
            process_context(ctx, ec)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function process_context(ByVal ctx As server.context,
                                           ByVal f As _do(Of event_comb, Boolean)) As Boolean
        assert(Not f Is Nothing)
        Return process_context(ctx,
                               Function(ByVal c As server.context, ByRef o As event_comb) As Boolean
                                   Return f(o)
                               End Function)
    End Function

    Private Sub New()
    End Sub
End Class
