
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _string
        Inherits compiler.logic._string(Of value.with_single_data_slot_temp_target_t)

        Public Overloads Shared Function build(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return code_gens().typed(Of _string)().build(New data_block(s), o)
        End Function
    End Class
End Class
