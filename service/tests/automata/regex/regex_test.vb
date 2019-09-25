
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.rlexer

Namespace rlexer
    Public Class regex_test
        Inherits [case]

        Private Shared ReadOnly cases() As regex_case

        Private Class regex_case
            Public ReadOnly regex As String
            Public ReadOnly matches() As String
            Public ReadOnly unmatches() As String

            Public Sub New(ByVal regex As String,
                           ByVal matches() As String,
                           ByVal unmatches() As String)
                Me.regex = regex
                Me.matches = matches
                Me.unmatches = unmatches
            End Sub

            Public Sub New(ByVal regex As String, ByVal matches() As String)
                Me.New(regex, matches, Nothing)
            End Sub
        End Class

        Shared Sub New()
            cases = {New regex_case("[a,b,c]def[g,h,i]",
                                    {"adefg", "bdefg", "cdefg", "adefh", "bdefh", "cdefh", "adefi", "bdefi", "cdefi"}),
                     New regex_case("[a,b,c]*def[g,h,i]", {"aadefg", "abdefg", "acdefg", "abcdefg"}),
                     New regex_case("[a,b,c]*abc",
                                    {"aaabc", "abcabc", "abbccaabbabc"},
                                    {"abcabcab"}),
                     New regex_case("[a,b,c]?def",
                                    {"adef", "bdef", "cdef", "def"},
                                    {"ddef", "aadef"}),
                     New regex_case("abc[de,f]!",
                                    {"abcdf", "abc", "abcx", "abcd", "abcxx"},
                                    {"abcde", "abcf", "abcdef", "abcxxx"}),
                     New regex_case("[if]-[*]*",
                                    {"else", "ii"},
                                    {"if"}),
                     New regex_case("[\d]*[.]?[\d]+",
                                    {"3.131", "100", "2993.22", "0"},
                                    {"2.2.2.2", "2.", "88ab3"}),
                     New regex_case("[+,-]?[\d]+[.]?[\d]+",
                                    {"3.131", "+3888", "-382923.2323"},
                                    {"2.2.2.2", "--3.3", "+-33", "883aaddd3399"}),
                     New regex_case("""[""]!*""",
                                    {"""string""", """asdjfllk  iasdlf jlkasdf   asldk jfladf """},
                                    {"""asdf", "lajfdl""", "asldfj", """asldfal""falsfjl"}),
                     New regex_case("return[\w,\d,_]-",
                                    {"return"},
                                    {"returna", "return_", "return1", "retuRn"}),
                     New regex_case("\D+", {"abc", "def"}, {"123", "ab3"}), ' , ""}), TODO: Make empty string case work.
                     New regex_case("\W+", {"**(", "123"}, {"**a", "12c"})} ' , ""})}
        End Sub

        Public Overrides Function run() As Boolean
            For i As Int32 = 0 To array_size_i(cases) - 1
                assert(Not cases(i) Is Nothing)
                Dim c As regex = Nothing
                If assertion.is_true(regex.create(macros.default.expand(cases(i).regex), c)) AndAlso
                   assertion.is_not_null(c) Then
                    For j As Int32 = 0 To array_size_i(cases(i).matches) - 1
                        assertion.is_true(c.match_to_end(cases(i).matches(j)),
                                    cases(i).regex,
                                    " does not match ",
                                    cases(i).matches(j))
                    Next
                    For j As Int32 = 0 To array_size_i(cases(i).unmatches) - 1
                        assertion.is_false(c.match_to_end(cases(i).unmatches(j)),
                                     cases(i).regex,
                                     " matches ",
                                     cases(i).unmatches(j))
                    Next
                End If
            Next
            Return True
        End Function
    End Class
End Namespace
