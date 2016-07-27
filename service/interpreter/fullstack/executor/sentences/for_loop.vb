
Namespace fullstack.executor
    Public Class for_loop
        Implements sentence

        Private ReadOnly init As sentence
        Private ReadOnly condition As expression
        Private ReadOnly increase As sentence
        Private ReadOnly body As sentence

        Public Sub New(Optional ByVal init As sentence = Nothing,
                       Optional ByVal condition As sentence = Nothing,
                       Optional ByVal increase As sentence = Nothing,
                       Optional ByVal body As sentence = Nothing)
            Me.init = init
            Me.condition = condition
            Me.increase = increase
            Me.body = body
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            If Not init Is Nothing Then
                init.execute(domain)
            End If
            While condition Is Nothing OrElse
                  condition.execute(domain).true()
                If Not body Is Nothing Then
                    Try
                        body.execute(domain)
                    Catch ex As break_exception
                        Exit While
                    End Try
                End If
                If Not increase Is Nothing Then
                    increase.execute(domain)
                End If
            End While
        End Sub
    End Class
End Namespace
