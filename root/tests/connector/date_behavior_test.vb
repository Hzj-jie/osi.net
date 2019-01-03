
Imports System.Globalization
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class date_behavior_test
    Inherits [case]

    Private Shared ReadOnly success_cases() As tuple(Of String, String, CultureInfo, Date)
    Private Shared ReadOnly failure_cases() As tuple(Of String, String, CultureInfo)

    Private Shared Function make_case(ByVal str As String,
                                      ByVal format As String,
                                      ByVal culture As CultureInfo,
                                      ByVal d As Date) As tuple(Of String, String, CultureInfo, Date)
        Return make_tuple(str, format, culture, d)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal culture As CultureInfo, ByVal d As Date) _
                                     As tuple(Of String, String, CultureInfo, Date)
        Return make_case(str, git_time_format, culture, d)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal format As String, ByVal d As Date) _
                                     As tuple(Of String, String, CultureInfo, Date)
        Return make_case(str, format, culture_info.en_US, d)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal d As Date) _
                                     As tuple(Of String, String, CultureInfo, Date)
        Return make_case(str, git_time_format, d)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal format As String, ByVal culture As CultureInfo) _
                                     As tuple(Of String, String, CultureInfo)
        Return make_tuple(str, format, culture)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal culture As CultureInfo) _
                                     As tuple(Of String, String, CultureInfo)
        Return make_case(str, git_time_format, culture)
    End Function

    Private Shared Function make_case(ByVal str As String, ByVal format As String) _
                                     As tuple(Of String, String, CultureInfo)
        Return make_case(str, format, culture_info.en_US)
    End Function

    Private Shared Function make_case(ByVal str As String) As tuple(Of String, String, CultureInfo)
        Return make_case(str, git_time_format)
    End Function

    Shared Sub New()
        success_cases = {make_case("Sat Dec 31 19:57:18 2016 -0800", New Date(2017, 1, 1, 3, 57, 18, DateTimeKind.Utc))}
        failure_cases = {make_case("Sat Dec 31 19:57:18 2016 -0800", culture_info.zh_CN)}
    End Sub

    Private Shared Function try_parse_exact_case() As Boolean
        Dim t As Date = Nothing
        For i As Int32 = 0 To array_size(success_cases) - 1
            assertion.is_true(Date.TryParseExact(success_cases(i)._1(),
                                           success_cases(i)._2(),
                                           success_cases(i)._3(),
                                           DateTimeStyles.AdjustToUniversal,
                                           t))
            assertion.equal(t, success_cases(i)._4())
        Next
        For i As Int32 = 0 To array_size(failure_cases) - 1
            assertion.is_false(Date.TryParseExact(failure_cases(i)._1(),
                                            failure_cases(i)._2(),
                                            failure_cases(i)._3(),
                                            DateTimeStyles.AdjustToUniversal,
                                            Nothing))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return try_parse_exact_case()
    End Function
End Class
