
Imports osi.root.connector
Imports osi.root.utils

Namespace fullstack.executor
    Public Class paragraph
        Implements sentence

        Private ReadOnly s() As sentence

        Public Sub New(ByVal sentences() As sentence)
            Me.s = sentences
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            If Not isemptyarray(s) Then
                Dim d As domain = Nothing
                Using domain.create_disposer(d)
                    For i As Int32 = 0 To array_size(s) - 1
                        s(i).execute(d)
                    Next
                End Using
            End If
        End Sub
    End Class
End Namespace
