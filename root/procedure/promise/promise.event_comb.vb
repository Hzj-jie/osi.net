
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class promise
    Public Shared Widening Operator CType(ByVal this As event_comb) As promise
        If this Is Nothing Then
            Return Nothing
        Else
            Return New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                                   assert_begin(New event_comb(Function() As Boolean
                                                                   Return waitfor(this) AndAlso
                                                                          goto_next()
                                                               End Function,
                                                               Function() As Boolean
                                                                   If this.end_result() Then
                                                                       resolve(Nothing)
                                                                   Else
                                                                       reject(Nothing)
                                                                   End If
                                                                   Return goto_end()
                                                               End Function))
                               End Sub)
        End If
    End Operator
End Class
