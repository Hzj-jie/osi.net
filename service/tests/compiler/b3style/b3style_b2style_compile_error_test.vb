
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler

<test>
Public NotInheritable Class b3style_b2style_compile_error_test
    Inherits b2style_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String) As Boolean
        Return b3style.with_default_functions().compile(content, Nothing)
        Throw New NotImplementedException()
    End Function

    <test>
    Private Sub cycle_typedef()
        run(_b2style_test_data.errors_cycle_typedef, "::CYCLE_TYPEDEF::A", "typedef C A")
    End Sub

    <test>
    Private Sub reinterpret_cast_without_type_id()
        run(_b2style_test_data.errors_reinterpret_cast_without_type_id, "s.::S2__struct__type__id")
    End Sub

    <test>
    Private Sub reinterpret_cast_to_a_different_class_type()
        ' b3style can detect the error correctly.
        run(_b2style_test_data.reinterpret_cast_to_a_different_class_type,
            "s.::S2__struct__type__id",
            b3style.class_initializer.failed_to_build_destructor_message("s"))
    End Sub

    Private Sub New()
    End Sub
End Class
