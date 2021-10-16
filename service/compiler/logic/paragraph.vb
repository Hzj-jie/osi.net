
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class paragraph
        Implements exportable

        Private ReadOnly s As vector(Of exportable)

        Public Sub New()
            Me.s = New vector(Of exportable)()
        End Sub

        Public Sub New(ByVal statements As unique_ptr(Of exportable()))
            Me.New()
            Me.s.emplace_back(statements.release())
        End Sub

        Public Sub New(ByVal statements As unique_ptr(Of vector(Of exportable)))
            Me.New()
            If statements Then
                Me.s = statements.release()
            End If
        End Sub

        Public Sub New(ByVal ParamArray statements() As exportable)
            Me.New()
            Me.s.emplace_back(statements)
        End Sub

        Public Function push(ByVal e As exportable) As Boolean
            If e Is Nothing Then
                Return False
            Else
                s.emplace_back(e)
                Return True
            End If
        End Function

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Using sw As New scope_wrapper(o)
                Dim i As UInt32 = 0
                While i < s.size()
                    assert(Not s(i) Is Nothing)
                    If Not s(i).export(o) Then
                        Return False
                    End If
                    i += uint32_1
                End While
            End Using
            Return True
        End Function
    End Class
End Namespace
