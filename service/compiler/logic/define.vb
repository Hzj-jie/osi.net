
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define a variable with @name.
    Public NotInheritable Class _define
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly push As Boolean

        Public Sub New(ByVal name As String, ByVal type As String)
            Me.New(name, type, True)
        End Sub

        Private Sub New(ByVal name As String, ByVal type As String, ByVal push As Boolean)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.name = name
            Me.type = type
            Me.push = push
        End Sub

        Public Shared Function export(ByVal name As String,
                                      ByVal type As String,
                                      ByVal o As vector(Of String)) As Boolean
            Return New _define(name, type).build(o)
        End Function

        Public Shared Function forward(ByVal name As String,
                                       ByVal type As String,
                                       ByVal o As vector(Of String)) As Boolean
            Return New _define(name, type, False).build(o)
        End Function

        Private Function define_variable(ByVal type As String, ByVal o As vector(Of String)) As Boolean
            assert(o IsNot Nothing)
            If Not scope.current().types().retrieve(type, Nothing) Then
                Return False
            End If
            If Not scope.current().variables().define_stack(name, type) Then
                Return False
            End If
            If push Then
                o.emplace_back(command_str(command.push))
                scope.current().when_end_scope(Sub()
                                                   o.emplace_back(command_str(command.pop))
                                               End Sub)
            End If
            Return True
        End Function

        Private Function define_callee_ref(ByVal o As vector(Of String)) As Boolean
            Return define_variable(scope.type_t.ptr_type, o) AndAlso
                   scope.current().anchor_refs().define(type, name)
        End Function

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            If define_variable(type, o) OrElse define_callee_ref(o) Then
                Return True
            End If
            errors.type_undefined(type, name)
            Return False
        End Function
    End Class
End Namespace
