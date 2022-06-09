
Imports System.Runtime.CompilerServices
#If Not NO_REFERENCE Then
Imports osi.root.constants
#End If

Public Module _char_detection
    <Extension()> Public Function null_or_empty(ByVal s As String) As Boolean
        Return s.null_or_empty()
    End Function

    <Extension()> Public Function null_or_whitespace(ByVal s As String) As Boolean
        If s.null_or_empty() Then
            Return True
        Else
#If NO_REFERENCE Then
            For i As Int32 = 0 To s.Length() - 1
                If Not Char.IsWhiteSpace(s(i)) Then
#Else
            For i As UInt32 = 0 To strlen(s) - uint32_1
                If Not s(i).space() Then
#End If
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    <Extension()> Public Function empty_or_whitespace(ByVal s As String) As Boolean
#If Not NO_REFERENCE Then
        assert(Not s Is Nothing)
#End If
        Return null_or_whitespace(s)
    End Function

    <Extension()> Public Function is_empty(ByVal s As String) As Boolean
#If Not NO_REFERENCE Then
        assert(Not s Is Nothing)
#End If
        Return s.Length() = 0
    End Function
End Module
