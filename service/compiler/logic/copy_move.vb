
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Namespace logic
    Public MustInherit Class copy_move
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly target As String
        Private ReadOnly source As String
        Private ReadOnly variable_operation As out_bool(Of variable, variable, String)

        Public Sub New(ByVal types As types,
                       ByVal target As String,
                       ByVal source As String,
                       ByVal variable_operation As out_bool(Of variable, variable, String))
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not String.IsNullOrEmpty(source))
            assert(Not variable_operation Is Nothing)
            Me.types = types
            Me.target = target
            Me.source = source
            Me.variable_operation = variable_operation
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim t As variable = Nothing
            Dim s As variable = Nothing
            Dim c As String = Nothing
            If variable.[New](scope, types, target, t) AndAlso
               variable.[New](scope, types, source, s) AndAlso
               variable_operation(t, s, c) Then
                o.emplace_back(c)
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
