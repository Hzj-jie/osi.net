
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class value_declaration
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
            Return struct.define_in_stack(type, name, o) OrElse
                   declare_primitive_type(scope.type_name.of(type), scope.variable_name.of(name), o)
        End Function

        Public Shared Function declare_primitive_type(ByVal type As String,
                                                      ByVal name As String,
                                                      ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)
            If Not scope.current().structs().types().defined(type) AndAlso
               scope.current().variables().define(type, name) AndAlso
               builders.of_define(name, scope.normalized_type.of(type)).to(o) Then
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
End Class
