
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.resource

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class tracer
            Private Shared Sub sentence(ByVal s As String,
                                        ByVal start As UInt32,
                                        ByVal [end] As UInt32,
                                        ByVal trainer As onebound(Of Char).trainer)
                assert([end] >= start)
                assert(trainer IsNot Nothing)
                If [end] = start Then
                    Return
                End If
                For i As Int32 = CInt(start) To CInt([end]) - 2
                    trainer.accumulate(s(i), s(i + 1))
                Next
            End Sub

            Private Shared Sub one_str(ByVal s As String, ByVal t As onebound(Of Char).trainer)
                If s.null_or_whitespace() Then
                    Return
                End If
                s.strsep(AddressOf _character.not_cjk,
                             Sub(ByVal l As UInt32, ByVal i As UInt32)
                                 sentence(s, l, i, t)
                             End Sub)
            End Sub

            Public Shared Function train(ByVal s As String) As onebound(Of Char).model
                assert(Not s.null_or_whitespace())
                Return train({s})
            End Function

            Public Shared Function train(ByVal ss As IEnumerable(Of String)) As onebound(Of Char).model
                Dim t As New onebound(Of Char).trainer()
                For Each s As String In ss
                    one_str(s, t)
                Next
                Return t.dump()
            End Function

            Public Shared Function train(ByVal reader As tar.reader) As onebound(Of Char).model
                assert(reader IsNot Nothing)
                Dim t As New onebound(Of Char).trainer()
                reader.foreach(Sub(ByVal name As String, ByVal p As Double, ByVal r As StreamReader)
                                   If p < 0.8 Then
                                       raise_error(error_type.user,
                                                   "ignroe ",
                                                   name,
                                                   ", the encoding possibility is ",
                                                   p)
                                       Return
                                   End If
                                   Dim line As String = r.ReadLine()
                                   While line IsNot Nothing
                                       one_str(line, t)
                                       line = r.ReadLine()
                                   End While
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
