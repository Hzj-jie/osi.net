
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

    <Extension()> Public Sub strsep(ByVal s As String,
                                    ByVal sep As Func(Of Char, Boolean),
                                    ByVal f As Action(Of UInt32, UInt32))
        assert(Not sep Is Nothing)
        assert(Not f Is Nothing)
        If s.null_or_empty() Then
            Return
        End If

        Dim l As Int32 = 0
        For i As Int32 = 0 To s.Length() - 1
            If sep(s(i)) Then
                f(CUInt(l), CUInt(i))
                l = i + 1
            End If
        Next
        f(CUInt(l), s.strlen())
    End Sub
End Module
