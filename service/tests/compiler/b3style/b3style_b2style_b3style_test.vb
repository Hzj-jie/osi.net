
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_b3style_test
    Inherits b2style_test_runner

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Return New b2style.compile_wrapper_b3style(New interrupts(+io)).compile(content, o)
    End Function

    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of_file(b3style.nlexer_rule, b3style.syntaxer_rule, Nothing))
    End Sub

    Private Sub New()
    End Sub
End Class
