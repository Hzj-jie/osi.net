
Imports System.Runtime.CompilerServices

Public Module _c_str
    <Extension()> Public Function c_str(ByVal s As String) As Char()
        If s Is Nothing Then
            Return Nothing
        Else
            Return s.ToCharArray()
        End If
    End Function
End Module
