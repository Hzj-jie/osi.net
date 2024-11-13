
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class b3style
    Private NotInheritable Class value_declaration
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal n As typed_node,
                                      ByVal class_construct As Boolean,
                                      ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 2)
            Dim type As String = n.child(0).input_without_ignored()
            Dim name As String = n.child(1).input_without_ignored()
            If Not struct.define_in_stack(type, name, o) Then
                Return declare_primitive_type(type, name, o)
            End If
            If scope.current().classes().is_defined(type) Then
                If class_construct Then
                    value_list.with_empty()
                    If Not class_initializer.construct(name, o) Then
                        Return False
                    End If
                End If
                class_initializer.destruct(name, o)
            End If
            Return True
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            Return build(n, True, o)
        End Function

        Public Shared Function without_class_construct(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
            Return build(n, False, o)
        End Function

        Public Shared Function declare_primitive_type(ByVal type As String,
                                                      ByVal name As String,
                                                      ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)
            If Not scope.current().structs().types().defined(type) AndAlso
               scope.current().variables().define(type, name) AndAlso
               builders.of_define(scope.fully_qualified_variable_name.of(name),
                                  scope.normalized_type.of(type)).to(o) Then
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
