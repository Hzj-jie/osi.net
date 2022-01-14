
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _substring
    <Extension()> Public Function strleft(ByVal str As String, ByVal length As UInt32) As String
        If length <= 0 Then
            Return Nothing
        End If
        If strlen(str) > length Then
            Return str.Substring(0, CInt(length))
        End If
        Return str
    End Function

    <Extension()> Public Function strright(ByVal str As String, ByVal length As UInt32) As String
        Dim len As UInt32 = 0
        len = strlen(str)
        If length = 0 Then
            Return Nothing
        End If
        If length >= len Then
            Return str
        End If
        Return str.Substring(CInt(len - length))
    End Function

    <Extension()> Public Function strmid(ByVal s As String,
                                         ByVal start As UInt32,
                                         Optional ByVal len As UInt32 = max_uint32) As String
        If s Is Nothing OrElse s.Length() < start Then
            Return Nothing
        End If
        If len = uint32_0 Then
            Return ""
        End If
        If start = uint32_0 Then
            If len = max_uint32 OrElse len >= s.Length() Then
                Return s
            End If
            Return s.Substring(0, CInt(len))
        End If
        If len = max_uint32 OrElse len + start >= s.Length() Then
            Return s.Substring(CInt(start))
        End If
        Return s.Substring(CInt(start), CInt(len))
    End Function

    <Extension()> Public Function strmid(ByVal s As StringBuilder,
                                         ByVal start As UInt32,
                                         Optional ByVal len As UInt32 = max_uint32) As String
        If s Is Nothing OrElse s.Length() < start Then
            Return Nothing
        End If
        If len = 0 Then
            Return ""
        End If
        If len = max_uint32 OrElse len + start >= s.Length() Then
            Return s.ToString(CInt(start), s.Length() - CInt(start))
        End If
        Return s.ToString(CInt(start), CInt(len))
    End Function

    <Extension()> Public Function char_at(ByVal s As String, ByVal pos As UInt32) As Char
        Return s(CInt(pos))
    End Function

    <Extension()> Public Function remove_last(ByVal s As String, ByVal len As UInt32) As String
        If s Is Nothing Then
            Return s
        End If

        If strlen(s) <= len Then
            Return ""
        End If

        Return s.Substring(0, CInt(strlen(s) - len))
    End Function
End Module
