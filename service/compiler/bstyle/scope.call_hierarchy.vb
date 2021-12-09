
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class call_hierarchy_t
            Inherits scope(Of scope).call_hierarchy

            Protected Overrides Function current_function_name() As [optional](Of String)
                Return [optional].optionally(scope.current().current_function().is_defined(),
                                             AddressOf scope.current().current_function().name)
            End Function
        End Class

        Public Shadows Function call_hierarchy() As call_hierarchy_t
            If is_root() Then
                assert(Not fc Is Nothing)
                Return fc
            End If
            assert(fc Is Nothing)
            Return (+root).fc
        End Function
    End Class
End Class
