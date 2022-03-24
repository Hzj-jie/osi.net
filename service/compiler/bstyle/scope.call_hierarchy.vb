
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Shadows Class call_hierarchy_t
            Inherits scope_b(Of call_hierarchy_t, scope).call_hierarchy_t

            Protected Overrides Function current_function_name() As [optional](Of String)
                Return [optional].optionally(scope.current().current_function().is_defined(),
                                             AddressOf scope.current().current_function().name)
            End Function

            Public NotInheritable Class calculator
                Public Shared Sub register(ByVal p As statements(Of compiler.logic.logic_writer))
                    calculator(Of compiler.logic.logic_writer).register(p)
                End Sub

                Private Sub New()
                End Sub
            End Class
        End Class
    End Class
End Class
