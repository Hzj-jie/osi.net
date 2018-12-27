
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class enum_type_info(Of T)
    Public Shared ReadOnly underlying_type As Type

    Shared Sub New()
        assert(type_info(Of T).is_enum)
        underlying_type = GetType(T).GetEnumUnderlyingType()
    End Sub

    Private Sub New()
    End Sub
End Class
