
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class paragraph
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

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Using sw As scope_wrapper = scope_wrapper.[New](scope, o)
                For i As UInt32 = 0 To s.size() - uint32_1
                    assert(Not s(i) Is Nothing)
                    If Not s(i).export(sw.scope(), o) Then
                        Return False
                    End If
                Next
            End Using
            Return True
        End Function
    End Class
End Namespace
