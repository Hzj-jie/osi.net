
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class wordbreaker_cjk
    Public NotInheritable Class trainer
        Private Shared Sub sentence(ByVal s As String,
                                    ByVal start As Int32,
                                    ByVal [end] As Int32,
                                    ByVal trainer As onebound(Of Char).trainer)
            assert([end] >= start)
            assert(Not trainer Is Nothing)
            If [end] = start Then
                Return
            End If
            If [end] - start = 1 Then
                trainer.accumulate(s([end] - 1), 1)
            Else
                For i As Int32 = start To [end] - 2
                    trainer.accumulate(s(i), s(i + 1), 1)
                Next
                ' The last character is not "independent", but it's required to be identified as "end-of-a-word".
                trainer.accumulate(s([end] - 1), 0.1)
            End If
        End Sub

        Public Shared Function train(ByVal s As String) As onebound(Of Char).model
            assert(Not s.null_or_whitespace())
            Dim t As onebound(Of Char).trainer = Nothing
            t = New onebound(Of Char).trainer()
            Dim l As Int32 = 0
            For i As Int32 = 0 To s.Length()
                If Not s(i).cjk() OrElse i = s.Length() Then
                    sentence(s, l, i, t)
                    l = i + 1
                End If
            Next
            Return t.dump()
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
