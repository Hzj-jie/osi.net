
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class event_comb
    Public Shared Widening Operator CType(ByVal this As promise) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Dim result As Boolean = False
            Dim t As Action = Nothing
            Return New event_comb(Function() As Boolean
                                      t = event_comb.wait()
                                      this.then(Sub()
                                                    result = True
                                                    t()
                                                End Sub,
                                                Sub()
                                                    result = False
                                                    t()
                                                End Sub)
                                      Return goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return result AndAlso
                                             goto_end()
                                  End Function)
        End If
    End Operator
End Class
