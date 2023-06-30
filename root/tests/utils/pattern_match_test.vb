
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class pattern_match_test
    Inherits [case]

    Private Const fit_true As Byte = pattern_match.fit_true
    Private Const fit_false As Byte = pattern_match.fit_false
    Private Const fit_undetermind As Byte = pattern_match.fit_undertermined

    Private Shared ReadOnly fit_pattern_cases As vector(Of pair(Of pair(Of String, String), Byte))
    Private Shared ReadOnly match_pattern_cases As vector(Of pair(Of pair(Of String, String), Boolean))

    Private Shared Sub fit_pattern_case(ByVal pattern As String, ByVal str As String, ByVal result As Byte)
        fit_pattern_cases.emplace_back(pair.emplace_of(pair.emplace_of(pattern, str), result))
    End Sub

    Private Shared Sub match_pattern_case(ByVal pattern As String, ByVal str As String, ByVal result As Boolean)
        match_pattern_cases.emplace_back(pair.emplace_of(pair.emplace_of(pattern, str), result))
    End Sub

    Shared Sub New()
        fit_pattern_cases = New vector(Of pair(Of pair(Of String, String), Byte))()
        fit_pattern_case("", "", fit_true)
        fit_pattern_case("*", "abc", fit_true)
        fit_pattern_case("***", "abc", fit_true)
        fit_pattern_case("a?c", "abc", fit_true)
        fit_pattern_case("???", "abc", fit_true)
        fit_pattern_case("*??", "abc", fit_true)
        fit_pattern_case("*bc", "abc", fit_true)
        fit_pattern_case("*b?", "abc", fit_true)
        fit_pattern_case("**?*??***", "abc", fit_true)
        fit_pattern_case("*???*", "abc", fit_true)

        fit_pattern_case("", "abc", fit_undetermind)
        fit_pattern_case("abc", "bcd", fit_undetermind)
        fit_pattern_case("a??", "ab", fit_undetermind)
        fit_pattern_case("??c", "bc", fit_undetermind)
        fit_pattern_case("???", "abcd", fit_undetermind)
        fit_pattern_case("????", "abc", fit_undetermind)
        fit_pattern_case("**??****??***", "abc", fit_undetermind)
        fit_pattern_case("*?*?*?d", "bcd", fit_undetermind)

        fit_pattern_case("-abc", "abc", fit_false)
        fit_pattern_case("-*ab", "bbbbbab", fit_false)
        fit_pattern_case("-***", "lajdsljaldsjf", fit_false)

        fit_pattern_case("osi.tests.root.lock.atomic_test", "osi.tests.root.lock.*lock*", fit_undetermind)

        match_pattern_cases = New vector(Of pair(Of pair(Of String, String), Boolean))()
        match_pattern_case("", "", True)
        match_pattern_case("*", "abc", True)

        match_pattern_case("-???", "ab", True)

        match_pattern_case("???", "ab", False)

        match_pattern_case("-*", "abc", False)
        match_pattern_case("-?b?", "abc", False)
    End Sub

    Private Shared Function run_fit_pattern_case() As Boolean
        For i As UInt32 = 0 To fit_pattern_cases.size() - uint32_1
            Dim result As Byte = 0
            result = fit_pattern_cases(i).first.second.fit_pattern(fit_pattern_cases(i).first.first)
            assertion.equal(result,
                         fit_pattern_cases(i).second,
                         "pattern == ",
                         fit_pattern_cases(i).first.first,
                         ", str == ",
                         fit_pattern_cases(i).first.second,
                         ", expected == ",
                         fit_pattern_cases(i).second,
                         ", real == ",
                         result)
        Next
        Return True
    End Function

    Private Shared Function run_match_pattern_case() As Boolean
        For i As UInt32 = 0 To match_pattern_cases.size() - uint32_1
            Dim result As Boolean = False
            result = match_pattern_cases(i).first.second.match_pattern(match_pattern_cases(i).first.first)
            assertion.equal(result,
                         match_pattern_cases(i).second,
                         "pattern == ",
                         match_pattern_cases(i).first.first,
                         ", str == ",
                         match_pattern_cases(i).first.second,
                         ", expected == ",
                         match_pattern_cases(i).second,
                         ", real == ",
                         result)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_fit_pattern_case() AndAlso
               run_match_pattern_case()
    End Function
End Class
