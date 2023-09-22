
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_b2style_b3style_test
    Inherits b2style_test(Of parse)

    Public NotInheritable Class parse
        Inherits __do(Of console_io.test_wrapper, String, executor, Boolean)

        Public Overrides Function at(ByRef i As console_io.test_wrapper,
                                     ByRef j As String,
                                     ByRef k As executor) As Boolean
            Return New b2style.parse_wrapper_b3style(New interrupts(+i)).parse(j, k)
        End Function
    End Class

    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of_file(b3style.nlexer_rule, b3style.syntaxer_rule, Nothing))
    End Sub

    Private Sub New()
    End Sub
End Class
