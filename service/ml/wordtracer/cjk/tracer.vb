
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class tracer
            Private Shared Sub sentence(ByVal s As String,
                                    ByVal start As UInt32,
                                    ByVal [end] As UInt32,
                                    ByVal trainer As onebound(Of Char).trainer)
                assert([end] >= start)
                assert(Not trainer Is Nothing)
                If [end] = start Then
                    Return
                End If
                For i As Int32 = CInt(start) To CInt([end]) - 2
                    trainer.accumulate(s(i), s(i + 1))
                Next
            End Sub

            Public Shared Function train(ByVal s As String,
                                         ByVal lower_bound As Double) As onebound(Of Char).model
                assert(Not s.null_or_whitespace())
                Return train({s}, lower_bound)
            End Function

            Public Shared Function train(ByVal s As String) As onebound(Of Char).model
                Return train(s, 0.0)
            End Function

            Public Shared Function train(ByVal ss As IEnumerable(Of String),
                                         ByVal lower_bound As Double) As onebound(Of Char).model
                Dim t As onebound(Of Char).trainer = Nothing
                t = New onebound(Of Char).trainer()
                For Each s As String In ss
                    If s.null_or_whitespace() Then
                        Continue For
                    End If
                    s.strsep(AddressOf _character.cjk,
                             Sub(ByVal l As UInt32, ByVal i As UInt32)
                                 sentence(s, l, i, t)
                             End Sub)
                Next
                Return t.dump().filter(lower_bound)
            End Function

            Public Shared Function train(ByVal ss As IEnumerable(Of String)) As onebound(Of Char).model
                Return train(ss, 0.0)
            End Function

            Private Sub New()
            End Sub
        End Class

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
