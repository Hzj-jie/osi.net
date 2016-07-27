
Namespace fullstack.executor
    Public Class break
        Implements sentence

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            Throw break_exception.instance
        End Sub
    End Class
End Namespace
