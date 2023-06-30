
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.automata.nlexer

Namespace nlexer
    <test>
    Public NotInheritable Class rule_test
        <test>
        Private Shared Sub raw_string()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("abc", r))

            assertions.of(r.match("abc")).has_value(3)
            assertions.of(r.match("abcd")).has_value(3)
            assertions.of(r.match("abcd", 1)).is_empty()
            assertions.of(r.match("babcd")).is_empty()
            assertions.of(r.match("babcd", 1)).has_value(4)
        End Sub

        <test>
        Private Shared Sub two_raw_strings()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[abc][bcd]", r))

            assertions.of(r.match("abcbcd")).has_value(6)
            assertions.of(r.match("abcbcde")).has_value(6)
            assertions.of(r.match("abcbcde", 1)).is_empty()
            assertions.of(r.match("babcbcd")).is_empty()
            assertions.of(r.match("babcbcde", 1)).has_value(7)
        End Sub

        <test>
        Private Shared Sub multiple_positive_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[a,bc,def]", r))

            assertions.of(r.match("a")).has_value(1)
            assertions.of(r.match("abc")).has_value(1)
            assertions.of(r.match("bc")).has_value(2)
            assertions.of(r.match("def")).has_value(3)
            assertions.of(r.match("baba")).is_empty()
        End Sub

        <test>
        Private Shared Sub no_negative_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[a,bc,def|]", r))

            assertions.of(r.match("a")).is_empty()
            assertions.of(r.match("abc")).is_empty()
            assertions.of(r.match("bc")).is_empty()
            assertions.of(r.match("def")).is_empty()
            assertions.of(r.match("baba")).is_empty()
        End Sub

        <test>
        Private Shared Sub longest_positive_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[abcd,abc,ab,a]", r))

            assertions.of(r.match("abcd")).has_value(4)
            assertions.of(r.match("abc")).has_value(3)
            assertions.of(r.match("ab")).has_value(2)
            assertions.of(r.match("a")).has_value(1)
            assertions.of(r.match("abce")).has_value(3)
        End Sub

        <test>
        Private Shared Sub negative_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[|a]", r))

            assertions.of(r.match("a")).is_empty()
            assertions.of(r.match("b")).has_value(0)
        End Sub

        <test>
        Private Shared Sub multiple_negative_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[|a,b,c]", r))

            assertions.of(r.match("a")).is_empty()
            assertions.of(r.match("b")).is_empty()
            assertions.of(r.match("b")).is_empty()
            assertions.of(r.match("d")).has_value(0)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
