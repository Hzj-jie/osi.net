
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class strsplit_test
    Inherits [case]

    Private NotInheritable Class case_t
        Public ReadOnly str As String
        Public ReadOnly separators() As String
        Public ReadOnly surround_strs() As pair(Of String, String)
        Public ReadOnly ignore_empty_entity As Boolean
        Public ReadOnly ignore_surround_strs As Boolean
        Public ReadOnly case_sensitive As Boolean
        Public ReadOnly [return] As Boolean
        Public ReadOnly results() As String

        Public Sub New(ByVal str As String,
                       ByVal separators() As String,
                       ByVal surround_strs() As pair(Of String, String),
                       ByVal ignore_empty_entity As Boolean,
                       ByVal ignore_surround_strs As Boolean,
                       ByVal case_sensitive As Boolean,
                       ByVal [return] As Boolean,
                       ByVal results() As String)
            Me.str = str
            Me.separators = separators
            Me.surround_strs = surround_strs
            Me.ignore_empty_entity = ignore_empty_entity
            Me.ignore_surround_strs = ignore_surround_strs
            Me.case_sensitive = case_sensitive
            Me.return = [return]
            Me.results = results
        End Sub
    End Class

    Private Shared ReadOnly predefined_cases As vector(Of case_t)

    Shared Sub New()
        predefined_cases = New vector(Of case_t)()
        predefined_cases.emplace_back(New case_t("a b ""c d"" ",
                                                 {" "},
                                                 {pair.emplace_of("""", """")},
                                                 True,
                                                 True,
                                                 True,
                                                 True,
                                                 {"a", "b", "c d"}))
        predefined_cases.emplace_back(New case_t("a b ""c d"" ",
                                                 {" "},
                                                 {pair.emplace_of("""", """")},
                                                 True,
                                                 False,
                                                 True,
                                                 True,
                                                 {"a", "b", """c d"""}))
        predefined_cases.emplace_back(New case_t(" a b c d  ",
                                                 {" "},
                                                 {pair.emplace_of("""", """")},
                                                 False,
                                                 True,
                                                 True,
                                                 True,
                                                 {"", "a", "b", "c", "d", "", ""}))
        predefined_cases.emplace_back(New case_t("a b ""c d",
                                                 {" "},
                                                 {pair.emplace_of("""", """")},
                                                 False,
                                                 False,
                                                 True,
                                                 True,
                                                 {"a", "b", """c d"}))
        predefined_cases.emplace_back(New case_t(" a  b  c  d ",
                                                 {" "},
                                                 {},
                                                 False,
                                                 False,
                                                 True,
                                                 True,
                                                 {"", "a", "", "b", "", "c", "", "d", ""}))
    End Sub

    Private Shared Function run_case(ByVal c As case_t) As Boolean
        Dim v As vector(Of String) = Nothing
        assert(Not c Is Nothing)
        assertion.equal(c.return,
                     strsplit(c.str,
                              c.separators,
                              c.surround_strs,
                              v,
                              c.ignore_empty_entity,
                              c.ignore_surround_strs,
                              c.case_sensitive))
        assertion.array_equal(c.results, +v)
        Return True
    End Function

    Private Shared Function contains(ByVal s As String, ByVal a() As String) As Boolean
        assert(Not s Is Nothing)
        For i As Int32 = 0 To array_size_i(a) - 1
            If s.Contains(a(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function contains(ByVal s As String, ByVal a() As pair(Of String, String)) As Boolean
        assert(Not s Is Nothing)
        For i As Int32 = 0 To array_size_i(a) - 1
            assert(Not a(i) Is Nothing)
            If s.Contains(a(i).first) OrElse
               s.Contains(a(i).second) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function rnd_in(ByVal a() As String) As String
        assert(Not isemptyarray(a))
        Return a(rnd_int(0, array_size_i(a)))
    End Function

    Private Shared Function generate_independed_piece(ByVal separators() As String,
                                                      ByVal surround_strs() As pair(Of String, String)) As String
        While True
            Dim s As String = Nothing
            s = rnd_en_chars(rnd_int(0, 30))
            If Not contains(s, separators) AndAlso
               Not contains(s, surround_strs) Then
                Return s
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    Private Shared Function generate_surrounded_random_piece(ByVal separators() As String,
                                                             ByVal surround_strs() As pair(Of String, String)) As String
        assert(Not isemptyarray(separators) AndAlso
               Not isemptyarray(surround_strs))
        Dim id As Int32 = rnd_int(0, array_size_i(surround_strs))
        Dim r As String = surround_strs(id).first
        For i As Int32 = 0 To 10
            r += generate_independed_piece(separators,
                                           surround_strs)
            If rnd_bool() Then
                r += rnd_in(separators)
            End If
            If rnd_bool() Then
                Dim t As Int32 = 0
                t = id
                While t = id
                    t = rnd_int(0, array_size_i(surround_strs))
                End While
                If rnd_bool() Then
                    r += surround_strs(t).first
                Else
                    r += surround_strs(t).second
                End If
            End If
        Next
        r += surround_strs(id).second
        Return r
    End Function

    Private Shared Function generate_random_piece(ByVal separators() As String,
                                                  ByVal surround_strs() As pair(Of String, String)) As String
        If rnd_bool() Then
            Return generate_independed_piece(separators,
                                             surround_strs)
        End If
        If rnd_bool() Then
            Return generate_surrounded_random_piece(separators,
                                                    surround_strs)
        End If
        If rnd_bool() Then
            Return generate_independed_piece(separators,
                                             surround_strs) +
                   generate_surrounded_random_piece(separators,
                                                    surround_strs)
        End If
        If rnd_bool() Then
            Return generate_surrounded_random_piece(separators,
                                                    surround_strs) +
                   generate_independed_piece(separators,
                                             surround_strs)
        End If
        If rnd_bool() Then
            Return generate_independed_piece(separators,
                                             surround_strs) +
                   generate_surrounded_random_piece(separators,
                                                    surround_strs) +
                   generate_independed_piece(separators,
                                             surround_strs)
        End If
        Return generate_surrounded_random_piece(separators,
                                                surround_strs) +
               generate_independed_piece(separators,
                                         surround_strs) +
               generate_surrounded_random_piece(separators,
                                                surround_strs)
    End Function

    Private Shared Function generate_random_run_case(ByVal separators() As String,
                                                     ByVal surround_strs() As pair(Of String, String)) As case_t
        Dim v As vector(Of String) = Nothing
        Dim s As String = Nothing
        v = New vector(Of String)()
        For i As Int32 = 0 To 100
            v.emplace_back(generate_random_piece(separators,
                                                 surround_strs))
            s += v.back()
            s += rnd_in(separators)
        Next
        v.emplace_back("")
        Return New case_t(s,
                          separators,
                          surround_strs,
                          False,
                          False,
                          True,
                          True,
                          +v)
    End Function

    Private Shared Function run_random_cases(ByVal separators() As String,
                                             ByVal surround_strs() As pair(Of String, String)) As Boolean
        For i As Int32 = 0 To 1024 - 1
            If Not run_case(generate_random_run_case(separators,
                                                     surround_strs)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function run_random_cases_1() As Boolean
        Dim sps() As String = Nothing
        ReDim sps(strlen_i(space_chars) - 1)
        For i As Int32 = 0 To strlen_i(space_chars) - 1
            sps(i) = Convert.ToString(space_chars(i))
        Next
        Return run_random_cases(sps,
                                {pair.emplace_of(Convert.ToString(character.quote),
                                                   Convert.ToString(character.quote)),
                                 pair.emplace_of(Convert.ToString(character.single_quotation),
                                                   Convert.ToString(character.single_quotation)),
                                 pair.emplace_of(Convert.ToString(character.backquote),
                                                   Convert.ToString(character.backquote))})
    End Function

    Private Shared Function run_random_cases() As Boolean
        Return run_random_cases_1()
    End Function

    Private Shared Function run_predefined_cases() As Boolean
        assert(Not predefined_cases Is Nothing AndAlso
               Not predefined_cases.empty())
        For i As UInt32 = 0 To predefined_cases.size() - uint32_1
            If Not run_case(predefined_cases(i)) Then
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
