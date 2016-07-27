
Imports System.Runtime.CompilerServices

Public Module _strcontains
    <Extension()> Public Function strcontains(ByVal s As String, ByVal search As String) As Boolean
        Return strcontains(s, search, True)
    End Function

    <Extension()> Public Function strcontains(ByVal s As String,
                                              ByVal search As String,
                                              ByVal case_sensitive As Boolean) As Boolean
        If String.IsNullOrEmpty(search) Then
            Return True
        ElseIf String.IsNullOrEmpty(s) Then
            Return False
        Else
            If case_sensitive Then
                Return s.Contains(search)
            Else
                Return s.ToLower().Contains(search.ToLower())
            End If
        End If
    End Function
End Module
