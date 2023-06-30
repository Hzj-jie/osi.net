
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.utils

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](ByVal i As String) As string_subject
        Return New string_subject(i)
    End Function

    Public NotInheritable Class string_subject
        Inherits T_subject(Of String)

        Public Sub New(ByVal i As String)
            MyBase.New(i)
        End Sub

        Public Function contains(ByVal ParamArray exps() As String) As Boolean
            Return check(Of IS_TRUE_FUNC).str_contains(i, exps)
        End Function

        Public Function not_contains(ByVal ParamArray not_exps() As String) As Boolean
            Return check(Of IS_TRUE_FUNC).str_not_contains(i, not_exps)
        End Function

        Public Function starts_with(ByVal exp As String, ByVal ParamArray msg() As Object) As Boolean
            Return is_not_null() AndAlso
                   check(Of IS_TRUE_FUNC).is_true(i.StartsWith(exp),
                                                  "Expect ", i, " starting with ", exp, ". ", msg)
        End Function

        Public Function match_pattern(ByVal pattern As String, ByVal ParamArray msg() As Object) As Boolean
            Return is_not_null() AndAlso
                   check(Of IS_TRUE_FUNC).is_true(i.match_pattern(pattern, True),
                                                  "Expect ", i, " matching ", pattern, ". ", msg)
        End Function
    End Class
End Class
