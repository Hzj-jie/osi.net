
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_test
    <test>
    Private Shared Sub __func__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b3style.with_functions(New interrupts(+io)).compile(_b3style_test_data.__func__.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output().Trim(), String.Join(character.newline,
            "::type0 main([])",
            "::type0 ::N::print([])",
            "::type0 ::N::f2:::Integer:::String([x: ::Integer, s: ::String])"))
    End Sub

    <test>
    Private Shared Sub class_destruct_in_declaration()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b3style.with_functions(New interrupts(+io)).compile(
                          _b3style_test_data.destruction_in_declaration.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "constructfinishdestruct")
    End Sub

    Private Sub New()
    End Sub
End Class
