
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class pather_test
    Inherits [case]

    Private NotInheritable Class case_result
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
        cases.emplace_back(pair.emplace_of(path, New case_result(r, normalized, parent, name)))
    End Sub

    Shared Sub New()
        p = pather.default

        cases = New vector(Of pair(Of String, case_result))()

        add_case("/",
                 "/",
                 Nothing,
                 Nothing,
                 {""})
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
                 {"",
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
                 {"",
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
        For i As UInt32 = 0 To cases.size() - uint32_1
            Dim r1 As ref(Of String) = Nothing
            Dim r2 As ref(Of vector(Of String)) = Nothing
            Dim r3 As String = Nothing

            r1 = New ref(Of String)()
            assertion.is_true(p(cases(i).first, r1), cases(i).first)
            assertion.equal(+r1, cases(i).second.normalized, cases(i).first)

            r2 = New ref(Of vector(Of String))()
            assertion.is_true(p(cases(i).first, r2), cases(i).first)
            assertion.equal(+r2, cases(i).second.splitted, cases(i).first)

            If cases(i).second.parent Is Nothing Then
                assertion.is_false(p.parent_path(cases(i).first, Nothing), cases(i).first)
            Else
                r3 = Nothing
                assertion.is_true(p.parent_path(cases(i).first, r3), cases(i).first)
                assertion.equal(r3, cases(i).second.parent, cases(i).first)
            End If

            If cases(i).second.name Is Nothing Then
                assertion.is_false(p.last_level_name(cases(i).first, Nothing), cases(i).first)
            Else
                r3 = Nothing
                assertion.is_true(p.last_level_name(cases(i).first, r3), cases(i).first)
                assertion.equal(r3, cases(i).second.name, cases(i).first)
            End If
        Next
        Return True
    End Function

    Private Shared Function directory_name_cases() As Boolean
        assertion.equal(p.directory_name("/a/"), "/a/")
        assertion.equal(p.directory_name("/a"), "/a/")
        assertion.equal(p.directory_name("a"), "a/")
        assertion.equal(p.directory_name("\a\"), "\a\")
        assertion.equal(p.directory_name("\a"), "\a/")
        assertion.equal(p.directory_name("a"), "a/")
        Return True
    End Function

    Private Shared Function relative_path_cases() As Boolean
        Dim r As Action(Of String, String, String) = Nothing
        r = Sub(ByVal root As String, ByVal path As String, ByVal exp As String)
                Dim o As String = Nothing
                assertion.is_true(p.relative_path(root, path, o))
                assertion.equal(o, exp)
            End Sub

        r("c:", "c:\a\b/c.txt", "a/b/c.txt")
        r("c:/a\", "c:\a\b/c.txt", "b/c.txt")
        r("c:\", "d:\a\b\c.txt", "d:/a/b/c.txt")

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_cases() AndAlso
               directory_name_cases() AndAlso
               relative_path_cases()
    End Function
End Class
