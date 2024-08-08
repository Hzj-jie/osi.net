
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
        assertion.is_true(b3style.with_default_functions().generate(
                          _b2style_test_data.template_template_case1.as_text(), New logic_writer()))
    End Sub
End Class
