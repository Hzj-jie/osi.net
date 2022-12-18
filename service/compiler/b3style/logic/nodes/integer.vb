
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class b3style
    Private NotInheritable Class _integer
        Inherits bstyle._integer(Of scope.value_target_t.with_primitive_type_temp_target_t)

        Public Overloads Shared Function build(ByVal i As Int32, ByVal o As logic_writer) As Boolean
            Return _integer.build(New data_block(i), o)
        End Function
    End Class
End Class
