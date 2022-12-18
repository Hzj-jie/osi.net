
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class b3style
    Private NotInheritable Class _string
        Inherits bstyle._string(Of scope.value_target_t.with_primitive_type_temp_target_t)

        Public Overloads Shared Function build(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return _string.build(New data_block(s), o)
        End Function
    End Class
End Class
