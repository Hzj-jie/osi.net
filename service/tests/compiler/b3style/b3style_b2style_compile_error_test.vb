
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_compile_error_test
    Inherits b2style_compile_error_test(Of parse)

    Public NotInheritable Class parse
        Inherits __do(Of String, executor, Boolean)

        Public Overrides Function at(ByRef j As String, ByRef k As executor) As Boolean
            Return New b3style.parse_wrapper(interrupts.default).compile(j, k)
        End Function
    End Class

    <test>
    Private Shared Sub cycle_typedef()
        run(_b2style_test_data.errors_cycle_typedef, "::CYCLE_TYPEDEF::A", "typedef C A")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_without_type_id()
        run(_b2style_test_data.errors_reinterpret_cast_without_type_id, "s.::S2__struct__type__id")
    End Sub

    Private Sub New()
    End Sub
End Class
