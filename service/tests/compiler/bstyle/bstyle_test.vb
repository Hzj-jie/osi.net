
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
Public NotInheritable Class bstyle_test
    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of(bstyle.nlexer_rule, bstyle.syntaxer_rule, Nothing))
    End Sub

    'TODO: Does not work
    '<test>
    Private Shared Sub case1()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(bstyle.with_functions(New interrupts(+io)).parse(_bstyle_test_data.case1.as_text(), e))
    End Sub

    Private Sub New()
    End Sub
End Class
