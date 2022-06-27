
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class for_loop
        Implements code_gen(Of logic_writer)

        Private NotInheritable Class ref
            Public ReadOnly first As typed_node
            Public ReadOnly second As typed_node
            Public ReadOnly third As typed_node
            Public ReadOnly paragraph As typed_node

            Public Sub New(ByVal n As typed_node)
                assert(Not n Is Nothing)
                assert(n.child_count() >= 6)
                assert(n.child_count() <= 9)
                Dim m As vector(Of typed_node) = n.named_children().nodes("semi-colon")
                assert(m.size() = 2)
                If n.child_index(m(0)) = 3 Then
                    first = n.child(2)
                Else
                    first = Nothing
                End If
                If n.child_index(m(1)) - n.child_index(m(0)) = 2 Then
                    second = n.child(n.child_index(m(0)) + uint32_1)
                Else
                    second = Nothing
                End If
                If n.child_count() - n.child_index(m(1)) = 4 Then
                    third = n.child(n.child_count() - uint32_3)
                Else
                    third = Nothing
                End If
                paragraph = n.named_children().node("paragraph")
            End Sub
        End Class

        Private Function condition_value(ByVal n As ref, ByVal o As logic_writer, ByRef condition As String) As Boolean
            If Not n.second Is Nothing AndAlso Not code_gen_of(n.second).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                    value.read_target_single_data_slot()
                ' TODO: May want to restrict the type of condition.
                If Not read_target.retrieve(condition) Then
                    raise_error(error_type.user, "Condition of for-loop cannot be a struct.")
                    Return False
                End If
                Return True
            End Using
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not o Is Nothing)
            Return builders.start_scope(o).of(
                       Function() As Boolean
                           Using scope.current().start_scope()
                               Dim ref As New ref(n)
                               Dim condition As String = Nothing
                               Return (ref.first Is Nothing OrElse code_gen_of(ref.first).build(o)) AndAlso
                                      condition_value(ref, o, condition) AndAlso
                                      builders.of_while_then(condition,
                                                             Function() As Boolean
                                                                 Dim cur_condition As String = Nothing
                                                                 Return code_gen_of(ref.paragraph).build(o) AndAlso
                                                                        (ref.third Is Nothing OrElse
                                                                         code_gen_of(ref.third).build(o)) AndAlso
                                                                        condition_value(ref, o, cur_condition) AndAlso
                                                                        builders.of_copy(condition, cur_condition).to(o)
                                                             End Function).to(o)
                           End Using
                       End Function)
        End Function
    End Class
End Class
