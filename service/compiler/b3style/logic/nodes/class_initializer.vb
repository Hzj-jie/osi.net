
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    ' VisibleForTesting
    Public NotInheritable Class class_initializer
        Implements code_gen(Of logic_writer)

        Public Shared Function construct(ByVal name As String, ByVal o As logic_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)

            If Not function_call.ignore_parameters.without_return(b2style.function_call.build_struct_function(
                                                                      name,
                                                                      scope.class_def.construct),
                                                                  o) Then
                Return False
            End If
            scope.current().
                  call_hierarchy().
                  to(scope.namespace_t.fully_qualified_name(scope.class_def.construct))

            Return True
        End Function

        ' VisibleForTesting
        Public Shared Function failed_to_build_destructor_message(ByVal name As String) As String
            assert(Not name.null_or_whitespace())
            Return String.Concat("Failed to build the destructor of the variable ",
                                 name,
                                 ", this shouldn't happen unless the reinterpret_cast is used ",
                                 "which changed the definition of the variable.")
        End Function

        Public Shared Sub destruct(ByVal name As String, ByVal o As logic_writer)
            ' Functions are always in global namespace.
            ' Use of this function should only be in the !_disable_namespace.
            scope.current().when_end_scope(
                Sub()
                    value_list.with_empty()
                    ' when_end_scope uses IDisposable, even the build failed in the middle, it will still be executed
                    ' and may cause the crash if reinterpret_cast is used. See
                    ' b2style_compile_error_test.reinterpret_cast_without_type_id.
                    ' Likely the destructor shouldn't be executed in the case.
                    ' TODO: Avoid running this when_end_scope callback if the build failed in the middle.
                    If Not function_call.ignore_parameters.without_return(b2style.function_call.build_struct_function(
                                                                              name,
                                                                              scope.class_def.destruct),
                                                                          o) Then
                        raise_error(error_type.user, failed_to_build_destructor_message(name))
                    End If
                End Sub)
            scope.current().
                  call_hierarchy().
                  to(scope.namespace_t.fully_qualified_name(scope.class_def.destruct))
        End Sub

        ' VisibleForTesting
        Public Shared Function failed_to_build_constructor_message(ByVal class_name As String,
                                                                   ByVal var_name As String) As String
            assert(Not class_name.null_or_whitespace())
            assert(Not var_name.null_or_whitespace())
            Return String.Concat(class_name,
                                 " is not a class or struct, and the variable ",
                                 var_name,
                                 " cannot be initialized as a class variable.")
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4 OrElse n.child_count() = 5)
            If Not struct.define_in_stack(n.child(0), n.child(1), o) Then
                raise_error(error_type.user,
                            failed_to_build_constructor_message(n.child(0).input(), n.child(1).input()))
                Return False
            End If
            If n.child_count() = 4 Then
                value_list.with_empty()
            ElseIf Not code_gen_of(n.child(3)).build(o) Then
                Return False
            End If

            Dim name As String = scope.fully_qualified_variable_name.of(n.child(1))
            If construct(name, o) Then
                destruct(name, o)
                Return True
            End If
            Return False
        End Function
    End Class
End Class
