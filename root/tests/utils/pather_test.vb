
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public Class pather_test
    Inherits [case]

    Private Class case_result
        Public ReadOnly splitted As vector(Of String)
        Public ReadOnly normalized As String
        Public ReadOnly parent As String
        Public ReadOnly name As String

        Public Sub New(ByVal splitted As vector(Of String),
                       ByVal normalized As String,
                       ByVal parent As String,
                       ByVal name As String)
            Me.splitted = splitted
            Me.normalized = normalized
            Me.parent = parent
            Me.name = name
        End Sub
    End Class

    Private Shared ReadOnly cases As vector(Of pair(Of String, case_result))
    Private Shared ReadOnly p As pather

    Private Shared Sub add_case(ByVal path As String,
                                ByVal normalized As String,
                                ByVal parent As String,
                                ByVal name As String,
                                ByVal v() As String)
        Dim r As vector(Of String) = Nothing
        r = New vector(Of String)()
        r.emplace_back(v)
        cases.emplace_back(emplace_make_pair(path, New case_result(r, normalized, parent, name)))
    End Sub

    Shared Sub New()
        p = pather.default

        cases = New vector(Of pair(Of String, case_result))()

        add_case("/",
                 "/",
                 Nothing,
                 Nothing,
                 {empty_string})
        add_case("c:\windows\",
                 "c:/windows",
                 "c:",
                 "windows",
                 {"c:",
                  "windows"})
        add_case("c:\windows\.\",
                 "c:/windows",
                 "c:",
                 "windows",
                 {"c:",
                  "windows"})
        add_case("c:\windows\.",
                 "c:/windows",
                 "c:",
                 "windows",
                 {"c:",
                  "windows"})
        add_case("c:\windows\..\abc.txt",
                 "c:/abc.txt",
                 "c:",
                 "abc.txt",
                 {"c:",
                  "abc.txt"})
        add_case("..\windows\",
                 "../windows",
                 "..",
                 "windows",
                 {"..",
                  "windows"})
        add_case("..\windows\.",
                 "../windows",
                 "..",
                 "windows",
                 {"..",
                  "windows"})
        add_case(".\windows\",
                 "./windows",
                 ".",
                 "windows",
                 {".",
                  "windows"})
        add_case("/dev/../dev/./mnt/",
                 "/dev/mnt",
                 "/dev",
                 "mnt",
                 {empty_string,
                  "dev",
                  "mnt"})
        add_case("./dev/../dev/./mnt",
                 "./dev/mnt",
                 "./dev",
                 "mnt",
                 {".",
                  "dev",
                  "mnt"})
        add_case("/dev/../dev/././././/mnt/",
                 "/dev/mnt",
                 "/dev",
                 "mnt",
                 {empty_string,
                  "dev",
                  "mnt"})
        add_case("./dev/../bin/../usr//local/././sbin/*",
                 "./usr/local/sbin/*",
                 "./usr/local/sbin",
                 "*",
                 {".",
                  "usr",
                  "local",
                  "sbin",
                  "*"})
    End Sub

    Private Shared Function run_cases() As Boolean
        For i As Int32 = 0 To cases.size() - 1
            Dim r1 As pointer(Of String) = Nothing
            Dim r2 As pointer(Of vector(Of String)) = Nothing
            Dim r3 As String = Nothing

            r1 = New pointer(Of String)()
            assert_true(p(cases(i).first, r1), cases(i).first)
            assert_equal(+r1, cases(i).second.normalized, cases(i).first)

            r2 = New pointer(Of vector(Of String))()
            assert_true(p(cases(i).first, r2), cases(i).first)
            assert_equal(+r2, cases(i).second.splitted, cases(i).first)

            If cases(i).second.parent Is Nothing Then
                assert_false(p.parent_path(cases(i).first, Nothing), cases(i).first)
            Else
                r3 = Nothing
                assert_true(p.parent_path(cases(i).first, r3), cases(i).first)
                assert_equal(r3, cases(i).second.parent, cases(i).first)
            End If

            If cases(i).second.name Is Nothing Then
                assert_false(p.last_level_name(cases(i).first, Nothing), cases(i).first)
            Else
                r3 = Nothing
                assert_true(p.last_level_name(cases(i).first, r3), cases(i).first)
                assert_equal(r3, cases(i).second.name, cases(i).first)
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_cases()
    End Function
End Class
