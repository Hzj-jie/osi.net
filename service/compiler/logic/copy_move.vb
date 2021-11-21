
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
        Private ReadOnly cmd As command

        Public Sub New(ByVal types As types,
                       ByVal target As String,
                       ByVal source As String,
                       ByVal cmd As command)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not String.IsNullOrEmpty(source))
            Me.types = types
            Me.target = target
            Me.source = source
            Me.cmd = cmd
        End Sub

        Protected Shared Function export(ByVal cmd As command,
                                         ByVal target As variable,
                                         ByVal source As variable,
                                         ByVal o As vector(Of String)) As Boolean
            assert(Not target Is Nothing)
            assert(Not source Is Nothing)
            assert(Not o Is Nothing)
            If target.is_assignable_from(source) Then
                o.emplace_back(instruction_builder.str(cmd, target, source))
                Return True
            End If
            Return False
        End Function

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim t As variable = Nothing
            Dim s As variable = Nothing
            Return variable.of(types, target, o, t) AndAlso
                   variable.of(types, source, o, s) AndAlso
                   export(cmd, t, s, o)
        End Function
    End Class
End Namespace
