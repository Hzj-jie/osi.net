
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.service.resource

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

            Public Shared Function train(ByVal s As String) As onebound(Of Char).model
                assert(Not s.null_or_whitespace())
                Return train({s})
            End Function

            Public Shared Function train(ByVal ss As IEnumerable(Of String)) As onebound(Of Char).model
                Dim t As New onebound(Of Char).trainer()
                ml.cjk.per_str_from(ss, Sub(ByVal s As String, ByVal a As UInt32, ByVal b As UInt32)
                                            sentence(s, a, b, t)
                                        End Sub)
                Return t.dump()
            End Function

            Public Shared Function train(ByVal reader As tar.reader) As onebound(Of Char).model
                assert(Not reader Is Nothing)
                Dim t As New onebound(Of Char).trainer()
                ml.cjk.per_str_from(reader, Sub(ByVal s As String, ByVal a As UInt32, ByVal b As UInt32)
                                                sentence(s, a, b, t)
                                            End Sub)
                Return t.dump()
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
