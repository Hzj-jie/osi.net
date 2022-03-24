
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        ' TODO: Could is_defined() be false?
        Protected Overrides Function current_function_name() As [optional](Of String)
            Return [optional].optionally(scope.current().current_function().is_defined(),
                                             AddressOf scope.current().current_function().name)
        End Function
    End Class
End Class
