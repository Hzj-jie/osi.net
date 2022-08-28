
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Shadows Class call_hierarchy_t
            Inherits scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope).call_hierarchy_t

            Public NotInheritable Class calculator
                Inherits calculator(Of calculator)

                Protected Overrides Function current() As _
                        scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope).call_hierarchy_t
                    Return scope.current().call_hierarchy()
                End Function
            End Class
        End Class

        Public Shadows Function call_hierarchy() As call_hierarchy_t
            Return MyBase.call_hierarchy(Of call_hierarchy_t)()
        End Function
    End Class
End Class
