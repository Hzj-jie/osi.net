
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils

Public Class pattern_match_test
    Inherits [case]

    Private Shared ReadOnly match_one_cases As vector(Of pair(Of pair(Of String, String), Byte))

    Private Shared Sub match_case(ByVal pattern As String, ByVal str As String)
        match_one_cases.emplace_back(emplace_make_pair(emplace_make_pair(pattern, str),
                                                       stringfilter.fit_true))
    End Sub

    Private Shared Sub unmatch_case(ByVal pattern As String, ByVal str As String)
        match_one_cases.emplace_back(emplace_make_pair(emplace_make_pair(pattern, str),
                                                       stringfilter.fit_false))
    End Sub

    Private Shared Sub undetermined_case(ByVal pattern As String, ByVal str As String)
        match_one_cases.emplace_back(emplace_make_pair(emplace_make_pair(pattern, str),
                                                       stringfilter.fit_undertermind))
    End Sub

    Shared Sub New()
        match_one_cases = New vector(Of pair(Of pair(Of String, String), Byte))()
        match_case("", "")
        match_case("*", "abc")
        match_case("***", "abc")
        match_case("a?c", "abc")
        match_case("???", "abc")
        match_case("*??", "abc")
        match_case("*bc", "abc")
        match_case("*b?", "abc")
        match_case("**?*??***", "abc")
        match_case("*???*", "abc")

        undetermined_case("", "abc")
        undetermined_case("abc", "bcd")
        undetermined_case("a??", "ab")
        undetermined_case("??c", "bc")
        undetermined_case("???", "abcd")
        undetermined_case("????", "abc")
        undetermined_case("**??****??***", "abc")
        undetermined_case("*?*?*?d", "bcd")

        unmatch_case("-abc", "abc")
        unmatch_case("-*ab", "bbbbbab")
        unmatch_case("-***", "lajdsljaldsjf")
    End Sub

    Private Shared Function match_one_case()
        For i As UInt32 = 0 To CUInt(match_one_cases.size() - 1)
            Dim result As Byte = 0
            result = match_one_cases(i).first.second.match_one(match_one_cases(i).first.first)
            assert_equal(result,
                         match_one_cases(i).second,
                         "pattern == ",
                         match_one_cases(i).first.first,
                         ", str == ",
                         match_one_cases(i).first.second,
                         ", expected == ",
                         match_one_cases(i).second,
                         ", real == ",
                         result)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return match_one_case()
    End Function
End Class
