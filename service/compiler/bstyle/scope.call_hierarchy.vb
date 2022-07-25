
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Shadows Class call_hierarchy_t
            Inherits scope(Of scope).call_hierarchy_t

            Protected Overrides Function current_function_name() As [optional](Of String)
                Return scope.current().current_function().name_opt()
            End Function

            Public NotInheritable Class calculator
                Inherits calculator(Of compiler.logic.logic_writer, calculator)

                Protected Overrides Function current() As scope(Of scope).call_hierarchy_t
                    Return scope.current().call_hierarchy()
                End Function
            End Class
        End Class

        Public Function call_hierarchy() As call_hierarchy_t
            Return from_root(Function(ByVal i As scope) As call_hierarchy_t
                                 assert(Not i Is Nothing)
                                 Return i.fc
                             End Function)
        End Function
    End Class
End Class
