
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

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Dim type As String = n.child(0).input_without_ignored()
            Dim name As String = value_definition.name_of(n.child(1))
            If struct.define_in_stack(type, name, o) Then
                If scope.current().classes().is_defined(type) Then
                    ' No extra parameters to call the constructor.
                    value_list.with_empty()
                    Return class_initializer.construct_and_destruct(name, o)
                End If
                Return True
            End If
            Return declare_primitive_type(type, name, o)
        End Function

        Public Shared Function [of](ByVal type As typed_node,
                                    ByVal name As typed_node,
                                    ByVal o As logic_writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            Dim t As String = type.input_without_ignored()
            Dim n As String = value_definition.name_of(name)
            Return struct.define_in_stack(t, n, o) OrElse declare_primitive_type(t, n, o)
        End Function


        Public Shared Function declare_struct_type(ByVal type As typed_node,
                                                   ByVal name As typed_node,
                                                   ByVal o As logic_writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            Return struct.define_in_stack(type.input_without_ignored(), value_definition.name_of(name), o)
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
