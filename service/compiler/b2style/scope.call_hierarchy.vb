
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Public NotInheritable Shadows Class call_hierarchy_t
            Inherits scope_b(Of call_hierarchy_t, scope).call_hierarchy_t

            Public Shared Function from_value_clause() As Func(Of typed_node, Boolean)
                Return Function(ByVal n As typed_node) As Boolean
                           assert(Not n Is Nothing)
                           ' From value-clause or value-definition
                           assert(n.child_count() = 3 OrElse n.child_count() = 4)
                           ' TODO: This is a very simple solution with noticable amount of false-positive cases. But it
                           ' won't impact the result, cause bstyle will do the function cleanup again.
                           If n.last_child().only_descendant(n) AndAlso n.type_name.Equals("raw-name") Then
                               scope.current().call_hierarchy().to(n.word().str())
                           End If
                           Return True
                       End Function
            End Function

            Public Shadows Sub [to](ByVal name As String)
                to_bstyle_function(_namespace.bstyle_format.of(name))
            End Sub

            Public Sub to_bstyle_function(ByVal name As String)
                MyBase.to(name)
            End Sub

            Protected Overrides Function current_function_name() As [optional](Of String)
                Return scope.current().current_function().name()
            End Function

            Public NotInheritable Class calculator
                Public Shared Sub register(ByVal p As statements(Of rewriters.typed_node_writer))
                    calculator(Of rewriters.typed_node_writer).register(p)
                End Sub

                Private Sub New()
                End Sub
            End Class
        End Class
    End Class
End Class
