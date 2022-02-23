
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class paragraph
        Implements instruction_gen

        Private ReadOnly s As New vector(Of instruction_gen)()

        Public Sub New()
        End Sub

        ' @VisibleForTesting
        Public Sub New(ByVal ParamArray statements() As instruction_gen)
            s.emplace_back(statements)
        End Sub

        Public Function push(ByVal e As instruction_gen) As Boolean
            If e Is Nothing Then
                Return False
            Else
                s.emplace_back(e)
                Return True
            End If
        End Function

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(o IsNot Nothing)
            Using scope.current().start_scope()
                Dim i As UInt32 = 0
                While i < s.size()
                    assert(s(i) IsNot Nothing)
                    If Not s(i).build(o) Then
                        Return False
                    End If
                    i += uint32_1
                End While
            End Using
            Return True
        End Function
    End Class
End Namespace
