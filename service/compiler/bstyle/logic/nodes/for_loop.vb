
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class for_loop
        Inherits logic_gen_wrapper
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of for_loop)()
        End Sub

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

        Private Function condition_value(ByVal n As ref, ByVal o As writer) As Boolean
            Return n.second Is Nothing OrElse l.of(n.second).build(o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not o Is Nothing)
            Return builders.start_scope(o).of(
                       Function() As Boolean
                           Using New scope_wrapper()
                               Dim ref As New ref(n)
                               If Not ref.first Is Nothing AndAlso Not l.of(ref.first).build(o) Then
                                   Return False
                               End If
                               If Not condition_value(ref, o) Then
                                   Return False
                               End If
                               Using read_target As read_scoped(Of value.target).ref(Of String) =
                                       l.typed_code_gen(Of value)().read_target_single_data_slot()
                                   Dim condition As String = Nothing
                                   ' TODO: May want to restrict the type of condition.
                                   If Not read_target.retrieve(condition) Then
                                       raise_error(error_type.user, "Condition of for-loop cannot be a struct.")
                                       Return False
                                   End If
                                   Return builders.of_while_then(condition,
                                                                 Function() As Boolean
                                                                     Return l.of(ref.paragraph).build(o) AndAlso
                                                                            (ref.third Is Nothing OrElse
                                                                             l.of(ref.third).build(o)) AndAlso
                                                                            condition_value(ref, o)
                                                                 End Function).to(o)
                               End Using
                           End Using
                       End Function)
        End Function
    End Class
End Class
