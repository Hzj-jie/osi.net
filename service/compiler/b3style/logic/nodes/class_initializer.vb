
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class class_initializer
        Implements code_gen(Of logic_writer)

        Public Shared Function construct(ByVal name As String, ByVal o As logic_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)

            If Not function_call.without_return(b2style.function_call.build_struct_function(
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

        Public Shared Sub destruct(ByVal name As String, ByVal o As logic_writer)
            ' Functions are always in global namespace.
            ' Use of this function should only be in the !_disable_namespace.
            scope.current().when_end_scope(
                Sub()
                    value_list.with_empty()
                    assert(function_call.without_return(b2style.function_call.build_struct_function(
                                                            name,
                                                            scope.class_def.destruct),
                                                        o))
                End Sub)
            scope.current().
                  call_hierarchy().
                  to(scope.namespace_t.fully_qualified_name(scope.class_def.destruct))
        End Sub

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4 OrElse n.child_count() = 5)
            If Not struct.define_in_stack(n.child(0), n.child(1), o) Then
                Return False
            End If
            If n.child_count() = 4 Then
                value_list.with_empty()
            ElseIf Not code_gen_of(n.child(3)).build(o) Then
                Return False
            End If

            Dim name As String = n.child(1).input_without_ignored()
            If construct(name, o) Then
                destruct(name, o)
                Return True
            End If
            Return False
        End Function
    End Class
End Class
