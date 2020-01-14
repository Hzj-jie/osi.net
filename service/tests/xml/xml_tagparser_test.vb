
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.xml

Public Class xml_tagparser_test
    Inherits [case]

    Private Class case_t
        Public ReadOnly s As String
        Public ReadOnly [return] As Boolean
        Public ReadOnly tag As String
        Public ReadOnly attr As vector(Of pair(Of String, String))
        Public ReadOnly self_close As Boolean
        Public ReadOnly close_tag As Boolean

        Public Sub New(ByVal s As String,
                       ByVal [return] As Boolean,
                       ByVal tag As String,
                       ByVal attr As vector(Of pair(Of String, String)),
                       ByVal self_close As Boolean,
                       ByVal close_tag As Boolean)
            Me.s = s
            Me.return = [return]
            Me.tag = tag
            Me.attr = attr
            Me.self_close = self_close
            Me.close_tag = close_tag
        End Sub
    End Class

    Private Shared ReadOnly predefined_cases() As case_t

    Shared Sub New()
        predefined_cases = {New case_t("<abc k1=""v1"" k2=v2 k3='v3' k4=""v 4"" />",
                                       True,
                                       "abc",
                                       vector.of(pair.emplace_of("k1", "v1"),
                                                 pair.emplace_of("k2", "v2"),
                                                 pair.emplace_of("k3", "v3"),
                                                 pair.emplace_of("k4", "v 4")),
                                       True,
                                       False),
                            New case_t("  <  / abc > ",
                                       True,
                                       "abc",
                                       Nothing,
                                       False,
                                       True),
                            New case_t("<  // abc>",
                                       False,
                                       Nothing,
                                       Nothing,
                                       False,
                                       False),
                            New case_t("< < abc>",
                                       False,
                                       Nothing,
                                       Nothing,
                                       False,
                                       False)}
    End Sub

    Private Shared Function run_case(ByVal c As case_t) As Boolean
        assert(Not c Is Nothing)
        Dim tag As String = Nothing
        Dim attr As vector(Of pair(Of String, String)) = Nothing
        Dim self_close As Boolean = False
        Dim close_tag As Boolean = False
        assertion.equal(parse_tag(c.s, tag, attr, self_close, close_tag, True), c.return)
        If c.return Then
            assertion.equal(tag, c.tag)
            assertion.vector_equal(attr, c.attr)
            assertion.equal(self_close, c.self_close)
            assertion.equal(close_tag, c.close_tag)
        End If
        Return True
    End Function

    Private Shared Function run_predefined_cases() As Boolean
        For i As Int32 = 0 To array_size_i(predefined_cases) - 1
            If Not run_case(predefined_cases(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function run_random_cases() As Boolean
        For i As Int32 = 0 To 1024 - 1
            Dim tag As String = Nothing
            Dim attrs As vector(Of pair(Of String, String)) = Nothing
            Dim self_close As Boolean = False
            Dim s As String = Nothing
            Dim close_tag As Boolean = False
            close_tag = rnd_bool()
            If close_tag Then
                s = rnd_end_tag(tag)
            Else
                s = rnd_start_tag(tag, attrs, self_close)
            End If
            If Not run_case(New case_t(s, True, tag, attrs, self_close, close_tag)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_predefined_cases() AndAlso
               run_random_cases()
    End Function
End Class
