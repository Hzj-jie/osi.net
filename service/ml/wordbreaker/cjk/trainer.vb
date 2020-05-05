
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class wordbreaker_cjk
    Public NotInheritable Class trainer
        Private Shared Sub sentence(ByVal s As String,
                                    ByVal start As UInt32,
                                    ByVal [end] As UInt32,
                                    ByVal trainer As onebound(Of Char).trainer)
            assert([end] >= start)
            assert(Not trainer Is Nothing)
            If [end] = start Then
                Return
            End If
            If [end] - start = 1 Then
                trainer.accumulate(s(CInt([end]) - 1), 1)
            Else
                For i As Int32 = CInt(start) To CInt([end]) - 2
                    trainer.accumulate(s(i), s(i + 1), double_1 / ([end] - start))
                Next
                ' The last character is not "independent", but it's required to be identified as "end-of-a-word".
                trainer.accumulate(s(CInt([end]) - 1), onebound(Of Char).trainer.min_possibility)
            End If
        End Sub

        Public Shared Function train(ByVal s As String) As onebound(Of Char).model
            assert(Not s.null_or_whitespace())
            Dim t As onebound(Of Char).trainer = Nothing
            t = New onebound(Of Char).trainer()
            s.strsep(AddressOf cjk,
                     Sub(ByVal l As UInt32, ByVal i As UInt32)
                         sentence(s, l, i, t)
                     End Sub)
            Return t.dump()
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
