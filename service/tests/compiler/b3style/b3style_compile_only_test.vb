
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_compile_only_test
    <test>
    Private Shared Sub template_template_case1()
        ' No main defined, but b3style should still be able to generate logic code.
        assertion.is_true(b3style.with_default_functions().build(
                          _b2style_test_data.template_template_case1.as_text(), New logic_writer()))
    End Sub

    <test>
    Private Shared Sub two_template_type_parameters()
        ' No main defined, but b3style should still be able to generate logic code.
        assertion.is_true(b3style.with_default_functions().build(
                          _b2style_test_data.two_template_type_parameters.as_text(), New logic_writer()))
    End Sub
End Class
