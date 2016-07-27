
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _substring
    <Extension()> Public Function strleft(ByVal str As String, ByVal length As UInt32) As String
        If length > 0 Then
            If strlen(str) > length Then
                Return str.Substring(0, CInt(length))
            Else
                Return str
            End If
        Else
            Return Nothing
        End If
    End Function

    <Extension()> Public Function strright(ByVal str As String, ByVal length As UInt32) As String
        Dim len As UInt32 = 0
        len = strlen(str)
        If length = 0 Then
            Return Nothing
        ElseIf length >= len Then
            Return str
        Else
            Return str.Substring(len - length)
        End If
    End Function

    <Extension()> Public Function strmid(ByVal s As String,
                                         ByVal start As UInt32,
                                         Optional ByVal len As UInt32 = max_uint32) As String
        If s Is Nothing OrElse s.Length() < start Then
            Return Nothing
        ElseIf len = uint32_0 Then
            Return empty_string
        ElseIf start = uint32_0 Then
            If len = max_uint32 OrElse len >= s.Length() Then
                Return s
            Else
                Return s.Substring(0, CInt(len))
            End If
        Else
            If len = max_uint32 OrElse len + start >= s.Length() Then
                Return s.Substring(CInt(start))
            Else
                Return s.Substring(CInt(start), CInt(len))
            End If
        End If
    End Function

    <Extension()> Public Function strmid(ByVal s As StringBuilder,
                                         ByVal start As UInt32,
                                         Optional ByVal len As UInt32 = max_uint32) As String
        If s Is Nothing OrElse s.Length() < start Then
            Return Nothing
        ElseIf len = 0 Then
            Return empty_string
        ElseIf len = max_uint32 OrElse len + start >= s.Length() Then
            Return s.ToString(start, s.Length() - CInt(start))
        Else
            Return s.ToString(start, len)
        End If
    End Function
End Module
