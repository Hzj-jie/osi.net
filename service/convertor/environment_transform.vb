
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Public Module _environment_transform
    Public Const environment_transform_default_start_str As String = "$("
    Public Const environment_transform_default_end_str As String = ")"
    Private Const default_start_str As String = environment_transform_default_start_str
    Private Const default_end_str As String = environment_transform_default_end_str

    Sub New()
        assert(npos = -1)
    End Sub

    <Extension()> Public Function env_transform(ByVal i As String,
                                                Optional ByVal start_str As String = default_start_str,
                                                Optional ByVal end_str As String = default_end_str) As String
        assert(Not start_str.null_or_empty())
        assert(Not end_str.null_or_empty())
        If i.null_or_empty() Then
            Return i
        End If
        Dim s As Int32 = strindexof(i, start_str)
        While s <> npos
            Dim e As Int32 = strindexof(i, end_str, CUInt(s) + strlen(start_str), uint32_1)
            If e = npos Then
                Exit While
            End If
            Dim r As String = Nothing
            If env_value(i.Substring(s + start_str.Length(), e - s - start_str.Length()), r) Then
                i.strrplc(CUInt(s), CUInt(e - s + end_str.Length()), r)
                s += r.strlen_i()
            Else
                s += start_str.strlen_i()
            End If
            s = i.IndexOf(start_str, s)
        End While

        Return i
    End Function

    <Extension()> Public Function env_transform(ByVal i() As String,
                                                Optional ByVal start_str As String = default_start_str,
                                                Optional ByVal end_str As String = default_end_str) As String()
        If isemptyarray(i) Then
            Return i
        End If

        Dim r() As String = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = i(j).env_transform(start_str, end_str)
        Next
        Return r
    End Function

    <Extension()> Public Function env_transform(ByVal i() As pair(Of String, String),
                                                Optional ByVal start_str As String = default_start_str,
                                                Optional ByVal end_str As String = default_end_str) _
                                               As pair(Of String, String)()
        If isemptyarray(i) Then
            Return i
        End If
        Dim r() As pair(Of String, String) = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            If Not i(j) Is Nothing Then
                r(j) = pair.emplace_of(i(j).first.env_transform(start_str, end_str),
                                       i(j).second.env_transform(start_str, end_str))
            End If
        Next
        Return r
    End Function

    <Extension()> Public Function env_transform(ByVal i As vector(Of pair(Of String, String)),
                                                Optional ByVal start_str As String = default_start_str,
                                                Optional ByVal end_str As String = default_end_str) _
                                               As vector(Of pair(Of String, String))
        If i.null_or_empty() Then
            Return i
        End If
        Dim r As vector(Of pair(Of String, String)) = Nothing
        r = New vector(Of pair(Of String, String))(i.size())
        For j As UInt32 = 0 To i.size() - uint32_1
            If Not i(j) Is Nothing Then
                r.emplace_back(pair.emplace_of(i(j).first.env_transform(start_str, end_str),
                                               i(j).second.env_transform(start_str, end_str)))
            End If
        Next
        Return r
    End Function
End Module
