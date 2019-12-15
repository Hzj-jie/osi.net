
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.automata.nlexer

<test>
Public NotInheritable Class rule_suffix_test
    <test>
    Private Shared Sub with_optional_suffix()
        Dim r As rule = Nothing
        assertion.is_true(rule.of("[abc]?def", r))

        assertions.of(r.match("abcdef")).has_value(6)
        assertions.of(r.match("abcdefg")).has_value(6)
        assertions.of(r.match("def")).has_value(3)
        assertions.of(r.match("defg")).has_value(3)
        assertions.of(r.match("abdef")).is_empty()
    End Sub

    <test>
    Private Shared Sub with_0_or_more_suffix()
        Dim r As rule = Nothing
        assertion.is_true(rule.of("[abc]*def", r))

        assertions.of(r.match("abcdef")).has_value(6)
        assertions.of(r.match("abcdefg")).has_value(6)
        assertions.of(r.match("abcabcdef")).has_value(9)
        assertions.of(r.match("abcabcdefg")).has_value(9)
        assertions.of(r.match("abcabcabcdef")).has_value(12)
        assertions.of(r.match("abcabcabcdefg")).has_value(12)
        assertions.of(r.match("def")).has_value(3)
        assertions.of(r.match("defg")).has_value(3)
        assertions.of(r.match("abdef")).is_empty()
    End Sub

    <test>
    Private Shared Sub with_1_or_more_suffix()
        Dim r As rule = Nothing
        assertion.is_true(rule.of("[abc]+def", r))

        assertions.of(r.match("abcdef")).has_value(6)
        assertions.of(r.match("abcdefg")).has_value(6)
        assertions.of(r.match("abcabcdef")).has_value(9)
        assertions.of(r.match("abcabcdefg")).has_value(9)
        assertions.of(r.match("abcabcabcdef")).has_value(12)
        assertions.of(r.match("abcabcabcdefg")).has_value(12)
        assertions.of(r.match("def")).is_empty()
        assertions.of(r.match("defg")).is_empty()
        assertions.of(r.match("abdef")).is_empty()
    End Sub

    <test>
    Private Shared Sub multiple_suffixes_should_be_ignored()
        Dim r As rule = Nothing
        assertion.is_true(rule.of("[abc]+*", r))

        assertions.of(r.match("abc*")).has_value(4)
        assertions.of(r.match("abcabc*")).has_value(7)
        assertions.of(r.match("abcabc")).is_empty()
        assertions.of(r.match("*")).is_empty()
    End Sub

    Private Sub New()
    End Sub
End Class
