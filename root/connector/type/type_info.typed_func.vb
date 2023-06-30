
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Partial Public NotInheritable Class type_info
    Public NotInheritable Class full_name_func
        Inherits typed_func(Of String)

        Public Overrides Function at(Of T)() As String
            Return type_info(Of T).fullname
        End Function
    End Class
End Class
