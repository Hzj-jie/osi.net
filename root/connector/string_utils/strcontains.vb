
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _strcontains
    <Extension()> Public Function strcontains(ByVal s As String, ByVal search As String) As Boolean
        Return strcontains(s, search, True)
    End Function

    <Extension()> Public Function strcontains(ByVal s As String,
                                              ByVal search As String,
                                              ByVal case_sensitive As Boolean) As Boolean
        If search.null_or_empty() Then
            Return True
        End If
        If s.null_or_empty() Then
            Return False
        End If
        If case_sensitive Then
            Return s.Contains(search)
        End If
        Return s.ToLower().Contains(search.ToLower())
    End Function

    <Extension()> Public Function contains_all(ByVal s As String,
                                               ByVal case_sensitive As Boolean,
                                               ByVal ParamArray searches() As String) As Boolean
        If searches.isemptyarray() Then
            Return True
        End If
        For Each search As String In searches
            If Not strcontains(s, search, case_sensitive) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function contains_all(ByVal s As String,
                                               ByVal ParamArray searches() As String) As Boolean
        Return contains_all(s, True, searches)
    End Function
End Module
