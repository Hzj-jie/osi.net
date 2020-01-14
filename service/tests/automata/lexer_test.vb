
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.automata

Public Class lexer_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New lexer_basic_case(), 16),
                   repeat(New lexer_random_case(), 16))
    End Sub

    Private Class lexer_basic_case
        Inherits [case]

        Private Shared ReadOnly words() As String
        Private Shared ReadOnly unknown_words() As String

        Shared Sub New()
            Dim cc As Int32 = 0
            cc = Convert.ToInt32(character.z) - Convert.ToInt32(character.a) + 1
            ReDim words(cc * cc - 1)
            ReDim unknown_words(cc * cc - 1)
            For i As Int32 = Convert.ToInt32(character.a) To Convert.ToInt32(character.z)
                For j As Int32 = Convert.ToInt32(character.a) To Convert.ToInt32(character.z)
                    Dim p As Int32 = 0
                    p = (i - Convert.ToInt32(character.a)) * cc + j - Convert.ToInt32(character.a)
                    Dim s As String = Nothing
                    s = Convert.ToChar(i) + Convert.ToChar(j)
                    words(p) = strtolower(s)
                    unknown_words(p) = strtoupper(s)
                Next
            Next
        End Sub

        Private Shared Function generate_sentense(ByRef o As String) As vector(Of lexer.word)
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            Dim r As vector(Of lexer.word) = Nothing
            r = New vector(Of lexer.word)()
            Dim last_is_unknown As Boolean = False
            For i As Int32 = 0 To rnd_int(1000, 10000) - 1
                Dim j As UInt32 = 0
                j = rnd_int(0, array_size(words) + If(last_is_unknown, 0, array_size(unknown_words)))
                If j < array_size(words) Then
                    s.Append(words(j))
                    r.emplace_back(New lexer.word(words(j), j + lexer.first_user_type))
                    last_is_unknown = False
                Else
                    s.Append(unknown_words(j - array_size(words)))
                    r.emplace_back(lexer.word.unknown_word(unknown_words(j - array_size(words))))
                    last_is_unknown = True
                End If
            Next
            r.emplace_back(lexer.word.end_word)
            o = Convert.ToString(s)
            Return r
        End Function

        Private Shared Function run_case(ByVal l As lexer) As Boolean
            assert(Not l Is Nothing)
            Dim exp As vector(Of lexer.word) = Nothing
            Dim s As String = Nothing
            exp = generate_sentense(s)
            Dim r As vector(Of lexer.word) = Nothing
            assertion.is_true(l.parse(s, r))
            If assertion.is_not_null(r) Then
                assertion.equal(exp.size(), r.size())
                For i As Int32 = 0 To min(exp.size(), r.size()) - 1
                    assertion.equal(exp(i).type, r(i).type)
                    assertion.equal(exp(i).text, r(i).text)
                Next
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim l As lexer = Nothing
            l = New lexer(False, False)
            For i As Int32 = 0 To array_size(words) - 1
                assertion.is_true(l.define(words(i), i + lexer.first_user_type))
            Next
            For i As Int32 = 0 To 100 - 1
                If Not run_case(l) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class

    Private Class lexer_random_case
        Inherits [case]

        Private Shared Function accepted_string(ByVal ws() As pair(Of String, UInt32),
                                                ByVal count As Int32,
                                                ByVal do_not_accept_contains As Boolean) As String
            Dim s As String = Nothing
            Dim accepted As Boolean = False
            Do
                ' Why cannot this test pass with rnd_chars?
                s = rnd_utf8_chars(rnd_int(1, 16))
                accepted = True
                For j As Int32 = 0 To count - 1
                    If strstartwith(s, ws(j).first) OrElse
                       strstartwith(ws(j).first, s) OrElse
                       (do_not_accept_contains AndAlso
                        have_common_subsequence(s, ws(j).first)) Then
                        accepted = False
                    End If
                Next
            Loop Until accepted
            Return s
        End Function

        Private Shared Function words() As pair(Of String, UInt32)()
            Dim r() As pair(Of String, UInt32) = Nothing
            ReDim r(rnd_int(10, 100))
            For i As Int32 = 0 To array_size(r) - 1
                r(i) = pair.of(accepted_string(r, i, False),
                                 rnd_uint(0, 100) + lexer.first_user_type)
            Next
            Return r
        End Function

        Private Shared Function generate_unknown_word(ByVal ws() As pair(Of String, UInt32)) As String
            Return accepted_string(ws, array_size(ws), True)
        End Function

        Private Shared Function generate_sentense(ByVal ws() As pair(Of String, UInt32),
                                                  ByRef o As String) As vector(Of lexer.word)
            assert(Not isemptyarray(ws))
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            Dim r As vector(Of lexer.word) = Nothing
            r = New vector(Of lexer.word)()
            Dim last_is_unknown As Boolean = False
            For i As Int32 = 0 To rnd_int(1000, 10000) - 1
                Dim j As UInt32 = 0
                j = rnd_int(0, array_size(ws) * If(last_is_unknown, 1, 1.25))
                If j < array_size(ws) Then
                    s.Append(ws(j).first)
                    r.emplace_back(New lexer.word(ws(j).first, ws(j).second))
                    last_is_unknown = False
                Else
                    Dim u As String = Nothing
                    u = generate_unknown_word(ws)
                    s.Append(u)
                    r.emplace_back(lexer.word.unknown_word(u))
                    last_is_unknown = True
                End If
            Next
            r.emplace_back(lexer.word.end_word)
            o = Convert.ToString(s)
            Return r
        End Function

        Private Shared Function run_case(ByVal ws() As pair(Of String, UInt32),
                                         ByVal l As lexer) As Boolean
            assert(Not l Is Nothing)
            Dim exp As vector(Of lexer.word) = Nothing
            Dim s As String = Nothing
            exp = generate_sentense(ws, s)
            Dim r As vector(Of lexer.word) = Nothing
            assertion.is_true(l.parse(s, r))
            If assertion.is_not_null(r) Then
                assertion.equal(exp.size(), r.size())
                For i As Int32 = 0 To max(exp.size(), r.size()) - 1
                    If i >= exp.size() Then
                        assertion.is_true(False, r(i).type, ":", r(i).text)
                        Exit For
                    ElseIf i >= r.size() Then
                        assertion.is_true(False, exp(i).type, ":", exp(i).text)
                        Exit For
                    ElseIf Not assertion.equal(exp(i).type, r(i).type, exp(i).text, " - ", r(i).text) OrElse
                           Not assertion.equal(exp(i).text, r(i).text, exp(i).type, " - ", r(i).type) Then
                        Exit For
                    End If
                Next
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim l As lexer = Nothing
            l = New lexer(False, False)
            Dim ws() As pair(Of String, UInt32) = Nothing
            ws = words()
            assert(Not isemptyarray(ws))
            assertion.is_true(l.define(ws))
            For i As Int32 = 0 To 100 - 1
                If Not run_case(ws, l) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class
End Class
