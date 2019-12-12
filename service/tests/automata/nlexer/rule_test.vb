
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
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_3)
            End If

            o = r.match("abcd")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_3)
            End If

            o = r.match("abcd", uint32_1)
            assertion.is_false(o)

            o = r.match("babcd")
            assertion.is_false(o)

            o = r.match("babcd", uint32_1)
            If assertion.is_true(o) Then
                assertion.equal(+o, CUInt(4))
            End If
        End Sub

        <test>
        Private Shared Sub two_raw_strings()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[abc][bcd]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("abcbcd")
            If assertion.is_true(o) Then
                assertion.equal(+o, CUInt(6))
            End If

            o = r.match("abcbcde")
            If assertion.is_true(o) Then
                assertion.equal(+o, CUInt(6))
            End If

            o = r.match("abcbcde", uint32_1)
            assertion.is_false(o)

            o = r.match("babcbcd")
            assertion.is_false(o)

            o = r.match("babcbcde", uint32_1)
            If assertion.is_true(o) Then
                assertion.equal(+o, CUInt(7))
            End If
        End Sub

        <test>
        Private Shared Sub multiple_positive_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[a,bc,def]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("a")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_1)
            End If

            o = r.match("abc")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_1)
            End If

            o = r.match("bc")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_2)
            End If

            o = r.match("def")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_3)
            End If

            o = r.match("baba")
            assertion.is_false(o)
        End Sub

        <test>
        Private Shared Sub no_negative_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[a,bc,def|]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("a")
            assertion.is_false(o)

            o = r.match("abc")
            assertion.is_false(o)

            o = r.match("bc")
            assertion.is_false(o)

            o = r.match("def")
            assertion.is_false(o)

            o = r.match("baba")
            assertion.is_false(o)
        End Sub

        <test>
        Private Shared Sub longest_positive_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[abcd,abc,ab,a]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("abcd")
            If assertion.is_true(o) Then
                assertion.equal(+o, CUInt(4))
            End If

            o = r.match("abc")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_3)
            End If

            o = r.match("ab")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_2)
            End If

            o = r.match("a")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_1)
            End If

            o = r.match("abce")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_3)
            End If
        End Sub

        <test>
        Private Shared Sub negative_matches()
            Dim r As rule = Nothing
            assertion.is_true(rule.of("[|a]", r))

            Dim o As [optional](Of UInt32) = Nothing
            o = r.match("a")
            assertion.is_false(o)

            o = r.match("b")
            If assertion.is_true(o) Then
                assertion.equal(+o, uint32_0)
            End If
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
