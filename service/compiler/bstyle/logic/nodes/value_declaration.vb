
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public Class value_declaration(Of BUILDER As func_t(Of String, logic_writer, Boolean),
                                      CODE_GENS As func_t(Of code_gens(Of logic_writer)),
                                      T As scope(Of logic_writer, BUILDER, CODE_GENS, T))
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return build(n.child(0), n.child(1), o)
        End Function

        Public Shared Function build(ByVal type As typed_node,
                                     ByVal name As typed_node,
                                     ByVal o As logic_writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            Dim t As String = type.input_without_ignored()
            Dim n As String = name.input_without_ignored()
            Return struct(Of BUILDER, CODE_GENS, T).define_in_stack(t, n, o) OrElse
                   declare_primitive_type(t, n, o)
        End Function

        Public Shared Function declare_primitive_type(ByVal type As String,
                                                      ByVal name As String,
                                                      ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)

            If Not scope.current().structs().types().defined(type) AndAlso
               scope.current().variables().define(type, name) AndAlso
               builders.of_define(name,
                                  scope.current().type_alias()(type)).
                        to(o) Then
                Return True
            End If
            raise_error(error_type.user,
                        "Failed to declare ",
                        type,
                        " with name ",
                        name,
                        " as a primitive type variable.")
            Return False
        End Function
    End Class

    Private NotInheritable Class value_declaration
        Inherits value_declaration(Of code_builder_proxy, code_gens_proxy, scope)
    End Class
End Class
