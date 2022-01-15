
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define a variable with @name.
    Public NotInheritable Class define
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String

        Public Sub New(ByVal types As types,
                       ByVal name As String,
                       ByVal type As String)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.types = types
            Me.name = name
            Me.type = type
        End Sub

        Public Shared Function export(ByVal types As types,
                                      ByVal name As String,
                                      ByVal type As String,
                                      ByVal o As vector(Of String)) As Boolean
            Return New define(types, name, type).export(o)
        End Function

        Public Shared Function export(ByVal name As String,
                                      ByVal type As String,
                                      ByVal o As vector(Of String)) As Boolean
            Return export(types.default, name, type, o)
        End Function

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            If Not types.retrieve(type, Nothing) Then
                errors.type_undefined(type, name)
                Return False
            End If
            If Not scope.current().variables().define_stack(name, type) Then
                Return False
            End If
            If debug_dump Then
                o.emplace_back(comment_builder.str("+++ define ", name, type))
            End If
            o.emplace_back(command_str(command.push))
            Return True
        End Function
    End Class
End Namespace
