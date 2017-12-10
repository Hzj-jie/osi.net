
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure

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

    Private Sub New()
    End Sub
End Class
