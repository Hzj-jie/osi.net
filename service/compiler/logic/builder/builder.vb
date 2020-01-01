
Option Explicit On
Option Infer Off
Option Strict On

Namespace logic
    Partial Public MustInherit Class builder
        Public MustOverride Sub [to](ByVal o As writer)

        Public Shared Function of_type(ByVal name As String, ByVal size As UInt32) As type_builder
            Return New type_builder(name, size)
        End Function
    End Class
End Namespace
