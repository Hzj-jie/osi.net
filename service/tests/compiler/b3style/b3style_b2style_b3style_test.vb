
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

    <test>
    Private Shared Sub __func__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(New b2style.parse_wrapper_b3style(
                              New interrupts(+io)).parse(_b2style_test_data.__func__.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output().Trim(), String.Join(character.newline,
            "::type0 main([])",
            "::type0 ::std_out:C__struct__type__id__type([this.C__struct__type__id: ::C__struct__type__id__type&])",
            "::type0 ::N__print([])",
            "::type0 ::N__f2:Integer:String([N__x: ::Integer, N__s: ::String])"))
    End Sub

    Private Sub New()
    End Sub
End Class
