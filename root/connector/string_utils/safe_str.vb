
Imports System.Runtime.CompilerServices
Imports osi.root.constants

' Seems the issue has been resolved
#If 0 Then
Public Module _safe_str
    ' Some invalid characters make trouble to .net String.Contains and String.IndexOf functions.
    <Extension()> Public Function safe_strindexof(ByVal s As String, ByVal search As String) As Int32
        If strlen(s) < strlen(search) Then
            Return npos
        ElseIf String.IsNullOrEmpty(search) Then
            Return 0
        Else
            For i As Int32 = 0 To strlen(s) - strlen(search)
                Dim j As Int32 = 0
                For j = 0 To strlen(search) - 1
                    If Convert.ToInt32(s(i + j)) <> Convert.ToInt32(search(j)) Then
                        Exit For
                    End If
                Next
                If j = strlen(search) Then
                    Return i
                End If
            Next
            Return npos
        End If
    End Function

    <Extension()> Public Function safe_strcontains(ByVal s As String, ByVal search As String) As Boolean
        Return safe_strindexof(s, search) <> npos
    End Function

    <Extension()> Public Function safe_strstartwith(ByVal s As String, ByVal search As String) As Boolean
        Return safe_strindexof(s, search) = 0
    End Function
End Module
#End If
