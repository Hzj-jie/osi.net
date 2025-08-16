
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler

<test>
Public NotInheritable Class b3style_compile_error_test
    Inherits compile_error_test_runner

    <test>
    Private Sub define_class_constructor_for_non_class()
        run(_b3style_test_data.errors_define_class_constructor_for_non_class, " int ", " x ", " class ")
    End Sub

    Protected Overrides Function parse(ByVal content As String) As Boolean
        Return b3style.with_default_functions().compile(content, Nothing)
    End Function
End Class
