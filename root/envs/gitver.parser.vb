
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Globalization
Imports osi.root.constants
Imports osi.root.connector

Partial Public NotInheritable Class gitver
    Public NotInheritable Class commit_info
        Public ReadOnly hash As String = source_control.unknown_value
        Public ReadOnly short_hash As String = source_control.unknown_value
        Public ReadOnly author As String = source_control.unknown_value
        Public ReadOnly author_email As String = source_control.unknown_value
        Public ReadOnly author_date As String = source_control.unknown_value
        Public ReadOnly committer As String = source_control.unknown_value
        Public ReadOnly committer_email As String = source_control.unknown_value
        Public ReadOnly committer_date As String = source_control.unknown_value
        Public ReadOnly subject As String = source_control.unknown_value
        Public ReadOnly body As String = source_control.unknown_value
        Public ReadOnly trackable_version As String = source_control.unknown_value

        Private Shared Function parse_date_to_ver(ByVal d As String) As String
            Dim t As Date = Nothing
            assert(Date.TryParseExact(d, git_time_format, culture_info.en_US, DateTimeStyles.AdjustToUniversal, t))
            Return strcat("UTC-",
                          t.Year(),
                          "-",
                          t.Month(),
                          "-",
                          t.Day(),
                          "_",
                          t.Hour(),
                          "-",
                          t.Minute(),
                          "-",
                          t.Second())
        End Function

        Friend Sub New(ByVal s() As String)
            assert(array_size(s) = 10)
            Me.hash = s(0)
            Me.short_hash = s(1)
            Me.author = s(2)
            Me.author_email = s(3)
            Me.author_date = s(4)
            Me.committer = s(5)
            Me.committer_email = s(6)
            Me.committer_date = s(7)
            Me.subject = s(8)
            Me.body = s(9)
            Me.trackable_version = strcat(parse_date_to_ver(Me.author_date), "_", short_hash)
        End Sub

        Friend Sub New()
        End Sub
    End Class

    Public Shared ReadOnly latest As commit_info = parse(latest_commit_raw)
    Public Shared ReadOnly current As commit_info = parse(current_commit_raw)
    Public Shared ReadOnly diff As String = calculate_diff()
    Private Const separator As String = "  |-+-|  "
    Private Const field_count As Int32 = 10
    Private Shared ReadOnly titles() As String = {"CommitHash:",
                                                  "ShortCommitHash:",
                                                  "Author:",
                                                  "AuthorEMail:",
                                                  "AuthorDate:",
                                                  "Committer:",
                                                  "CommitterEMail:",
                                                  "CommitterDate:",
                                                  "Subject:",
                                                  "Body:"}

    Private Shared Function calculate_diff() As String
        Dim diff As String = bytes_str(Convert.FromBase64String(diff_base64))
        If diff.null_or_whitespace() Then
            diff = "<identical>"
        End If
        Return diff
    End Function

    Private Shared ReadOnly run_shared_sub_new As cctor_delegator = New cctor_delegator(
        Sub()
            assert(array_size(titles) = field_count)
        End Sub)

    Private Shared Function parse(ByVal commit_str As String) As commit_info
        Dim r As commit_info = Nothing
        If parse(commit_str, r) Then
            assert(Not r Is Nothing)
            Return r
        Else
            Return New commit_info()
        End If
    End Function

    Private Shared Function parse(ByVal commit_str As String, ByRef commit As commit_info) As Boolean
        Dim s() As String = Nothing
        ReDim s(field_count - 1)
        For i As Int32 = 0 To field_count - 1
            Dim f As String = Nothing
            If i = field_count - 1 Then
                f = commit_str
            Else
                If Not strsep(commit_str, f, commit_str, separator) Then
                    Return False
                End If
            End If
            If Not strstartwith(f, titles(i)) Then
                Return False
            End If
            s(i) = strmid(f, strlen(titles(i)))
        Next
        commit = New commit_info(s)
        Return True
    End Function

    Private Sub New()
    End Sub
End Class

