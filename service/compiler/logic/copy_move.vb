
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public MustInherit Class copy_move
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly target As String
        Private ReadOnly source As String

        Public Sub New(ByVal types As types, ByVal target As String, ByVal source As String)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not String.IsNullOrEmpty(source))
            Me.types = types
            Me.target = target
            Me.source = source
        End Sub

        Protected MustOverride Function instruction() As command

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim t As variable = Nothing
            Dim s As variable = Nothing
            Dim c As String = Nothing
            If variable.[New](scope, types, target, t) AndAlso
               variable.[New](scope, types, source, s) AndAlso
               t.copy_or_move_from(s, instruction(), c) Then
                o.emplace_back(c)
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
