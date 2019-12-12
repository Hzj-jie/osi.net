
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
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

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("abc")
            assertion.is_true(o)
            assertion.equal(+o, uint32_3)

            o = r.match("abcd")
            assertion.is_true(o)
            assertion.equal(+o, uint32_3)

            o = r.match("abcd", uint32_1)
            assertion.is_false(o)

            o = r.match("babcd")
            assertion.is_false(o)

            o = r.match("babcd", uint32_1)
            assertion.is_true(o)
            assertion.equal(+o, CUInt(4))
        End Sub

        <test>
        Private Shared Sub two_raw_strings()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[abc][bcd]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("abcbcd")
            assertion.is_true(o)
            assertion.equal(+o, CUInt(6))

            o = r.match("abcbcde")
            assertion.is_true(o)
            assertion.equal(+o, CUInt(6))

            o = r.match("abcbcde", uint32_1)
            assertion.is_false(o)

            o = r.match("babcbcd")
            assertion.is_false(o)

            o = r.match("babcbcde", uint32_1)
            assertion.is_true(o)
            assertion.equal(+o, CUInt(7))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
