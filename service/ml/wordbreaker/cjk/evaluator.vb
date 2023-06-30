
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class wordbreaker
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class evaluator
            Private ReadOnly e As onebound(Of Char).evaluator

            Public Sub New(ByVal m As onebound(Of Char).model)
                assert(Not m Is Nothing)
                ' Do not allow a word to contain over 8 characters.
                e = New onebound(Of Char).evaluator(m, 8)
            End Sub

            Default Public ReadOnly Property eva(ByVal i As String) As vector(Of String)
                Get
                    Dim r As vector(Of String) = Nothing
                    r = New vector(Of String)()
                    break(i,
                      Sub(ByVal s As String)
                          assert(Not s.null_or_whitespace())
                          r.emplace_back(s)
                      End Sub)
                    Return r
                End Get
            End Property

            Public Sub break(ByVal ss As IEnumerable(Of String), ByVal r As Action(Of String))
                assert(Not r Is Nothing)
                For Each s As String In ss
                    If s.null_or_whitespace() Then
                        Continue For
                    End If
                    s.strsep(AddressOf _character.cjk,
                     Sub(ByVal l As UInt32, ByVal i As UInt32)
                         sentence(s, l, i, r)
                     End Sub)
                Next
            End Sub

            Public Sub break(ByVal s As String, ByVal r As Action(Of String))
                assert(Not s.null_or_whitespace())
                break({s}, r)
            End Sub

            Private Sub sentence(ByVal s As String,
                             ByVal start As UInt32,
                             ByVal [end] As UInt32,
                             ByVal o As Action(Of String))
                assert([end] >= start)
                assert(Not o Is Nothing)
                If [end] = start Then
                    Return
                End If
                Dim r As vector(Of vector(Of Char)) = Nothing
                r = e(vector.of(s.Substring(CInt(start), CInt([end] - start)).c_str()))
                Dim i As UInt32 = 0
                While i < r.size()
                    o(New String(+r(i)))
                    i += uint32_1
                End While
            End Sub
        End Class
    End Class
End Class
