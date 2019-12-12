﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _strsep
    Private Function str_sep(ByVal input As String,
                             ByRef f As String,
                             ByRef s As String,
                             ByVal sep As String,
                             ByVal indexof As Func(Of String, String, Boolean, Int64),
                             ByVal case_sensitive As Boolean) As Boolean
        assert(Not indexof Is Nothing)
        Dim i As Int64 = 0
        i = indexof(input, sep, case_sensitive)
        If i = npos Then
            Return False
        End If
        Dim f1 As String = Nothing
        Dim s1 As String = Nothing
        f1 = strleft(input, CUInt(i))
        s1 = strmid(input, CUInt(i) + strlen(sep))
        'no matter whether f is input or s is input, it's safe
        f = f1
        s = s1
        Return True
    End Function

    <Extension()> Public Function strsep(ByVal input As String,
                                         ByRef f As String,
                                         ByRef s As String,
                                         ByVal sep As String,
                                         Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return str_sep(input, f, s, sep, AddressOf strindexof, case_sensitive)
    End Function

    <Extension()> Public Function strrsep(ByVal input As String,
                                          ByRef f As String,
                                          ByRef s As String,
                                          ByVal sep As String,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return str_sep(input, f, s, sep, AddressOf strlastindexof, case_sensitive)
    End Function
End Module
