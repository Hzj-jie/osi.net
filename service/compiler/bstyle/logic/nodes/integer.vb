
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _integer
        Inherits compiler.logic._integer(Of value.with_single_data_slot_temp_target_t)

        Public Overloads Shared Function build(ByVal i As Int32, ByVal o As logic_writer) As Boolean
            Return code_gens().typed(Of _integer)().build(New data_block(i), o)
        End Function
    End Class
End Class
