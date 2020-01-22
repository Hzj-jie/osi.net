
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.template

Public Module _strcmp
    <Extension()> Public Function charsame(ByVal c1 As Char,
                                           ByVal c2 As Char,
                                           Optional ByVal carecase As Boolean = True) As Boolean
        If carecase Then
            Return c1 = c2
        Else
            Return Char.ToLower(c1) = Char.ToLower(c2)
        End If
    End Function

    <Extension()> Public Function strcmp(ByVal input1 As String,
                                         ByVal start1 As UInt32,
                                         ByVal len1 As UInt32,
                                         ByVal input2 As String,
                                         ByVal start2 As UInt32,
                                         ByVal len2 As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        If case_sensitive Then
            Return String.CompareOrdinal(strmid(input1, start1, len1),
                                         strmid(input2, start2, len2))
        Else
            Return String.CompareOrdinal(strtolower(strmid(input1, start1, len1)),
                                         strtolower(strmid(input2, start2, len2)))
        End If
    End Function

    <Extension()> Public Function strcmp(ByVal input1 As String,
                                         ByVal start1 As UInt32,
                                         ByVal input2 As String,
                                         ByVal start2 As UInt32,
                                         ByVal len As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strcmp(input1, start1, len, input2, start2, len, case_sensitive)
    End Function

    <Extension()> Public Function strcmp(ByVal s1 As String,
                                         ByVal s2 As String,
                                         ByVal len As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strcmp(s1, uint32_0, len, s2, uint32_0, len, case_sensitive)
    End Function

    <Extension()> Public Function strcmp(ByVal s1 As String,
                                         ByVal s2 As String,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strcmp(s1, uint32_0, strlen(s1), s2, uint32_0, strlen(s2), case_sensitive)
    End Function

    <Extension()> Public Function strcmp(Of case_sensitive As _boolean)(ByVal s1 As String, ByVal s2 As String) As Int32
        Return strcmp(s1, s2, boolean_cache(Of case_sensitive).v)
    End Function

    <Extension()> Public Function strcmp(ByVal s1 As StringBuilder,
                                         ByVal start1 As UInt32,
                                         ByVal len1 As UInt32,
                                         ByVal s2 As StringBuilder,
                                         ByVal start2 As UInt32,
                                         ByVal len2 As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strcmp(strmid(s1, start1, len1), strmid(s2, start2, len2), case_sensitive)
    End Function

    <Extension()> Public Function strcmp(ByVal s1 As StringBuilder,
                                         ByVal start1 As UInt32,
                                         ByVal len1 As UInt32,
                                         ByVal s2 As String,
                                         ByVal start2 As UInt32,
                                         ByVal len2 As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strcmp(strmid(s1, start1, len1), uint32_0, s2, start2, len2, case_sensitive)
    End Function

    <Extension()> Public Function strcmp(ByVal s1 As String,
                                         ByVal start1 As UInt32,
                                         ByVal len1 As UInt32,
                                         ByVal s2 As StringBuilder,
                                         ByVal start2 As UInt32,
                                         ByVal len2 As UInt32,
                                         Optional ByVal case_sensitive As Boolean = True) As Int32
        Return -strcmp(s2, start2, len2, s1, start1, len1, case_sensitive)
    End Function

    <Extension()> Public Function strsame(ByVal input1 As String,
                                          ByVal start1 As UInt32,
                                          ByVal input2 As String,
                                          ByVal start2 As UInt32,
                                          ByVal len As UInt32,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        If len = 0 Then
            Return True
        ElseIf String.IsNullOrEmpty(input1) AndAlso String.IsNullOrEmpty(input2) Then
            Return True
        ElseIf String.IsNullOrEmpty(input1) OrElse String.IsNullOrEmpty(input2) Then
            Return False
        ElseIf start1 >= input1.Length() OrElse start2 >= input2.Length() Then
            Return False
        Else
            Return strcmp(input1, start1, len, input2, start2, len, case_sensitive) = 0
        End If
    End Function

    <Extension()> Public Function strsame(ByVal input1 As String,
                                          ByVal start1 As UInt32,
                                          ByVal len1 As UInt32,
                                          ByVal input2 As String,
                                          ByVal start2 As UInt32,
                                          ByVal len2 As UInt32,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        If len1 <> len2 Then
            Return False
        Else
            Return strsame(input1, start1, input2, start2, len1, case_sensitive)
        End If
    End Function

    <Extension()> Public Function strsame(ByVal input1 As String,
                                          ByVal input2 As String,
                                          ByVal len As UInt32,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return strsame(input1, uint32_0, input2, uint32_0, len, case_sensitive)
    End Function

    <Extension()> Public Function strsame(ByVal input1 As String,
                                          ByVal len1 As UInt32,
                                          ByVal input2 As String,
                                          ByVal len2 As UInt32,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return strsame(input1, uint32_0, len1, input2, uint32_0, len2, case_sensitive)
    End Function

    <Extension()> Public Function strsame(ByVal input1 As String,
                                          ByVal input2 As String,
                                          Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return strsame(input1, uint32_0, strlen(input1), input2, uint32_0, strlen(input2), case_sensitive)
    End Function

    <Extension()> Public Function strendwith(ByVal s1 As String,
                                             ByVal s2 As String,
                                             Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return strsame(s1, strlen(s1) - strlen(s2), s2, uint32_0, strlen(s2), case_sensitive)
    End Function

    <Extension()> Public Function strstartwith(ByVal s1 As String,
                                               ByVal s2 As String,
                                               Optional ByVal case_sensitive As Boolean = True) As Boolean
        Return strsame(s1, uint32_0, s2, uint32_0, strlen(s2), case_sensitive)
    End Function
End Module
