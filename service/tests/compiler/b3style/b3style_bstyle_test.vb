
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_bstyle_test
    Inherits bstyle_test_runner

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Using b3style.disable_namespace()
            Return b3style.with_functions(New interrupts(+io)).compile(content, o)
        End Using
    End Function

    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of_file(b3style.nlexer_rule, b3style.syntaxer_rule, Nothing))
    End Sub

    <test>
    Private Shared Sub real__file__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        Using b3style.parse_wrapper.with_current_file("real__file__.txt")
            assertion.is_true(b3style.with_functions(New interrupts(+io)).
                                      compile(_bstyle_test_data.real__file__.as_text(), e))
        End Using
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "real__file__.txt")
    End Sub

    Private Sub New()
    End Sub
End Class
