
Namespace fullstack.executor
    Public Class while_loop
        Implements sentence

        Private ReadOnly condition As expression
        Private ReadOnly body As sentence

        Public Sub New(Optional ByVal condition As expression = Nothing,
                       Optional ByVal body As sentence = Nothing)
            Me.condition = condition
            Me.body = body
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            While condition Is Nothing OrElse
                  condition.execute(domain).true()
                If Not body Is Nothing Then
                    Try
                        body.execute(domain)
                    Catch ex As break_exception
                        Exit While
                    End Try
                End If
            End While
        End Sub
    End Class
End Namespace
