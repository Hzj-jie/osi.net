
Imports System.Text
Imports System.Runtime.CompilerServices

Public Module _reverse
    <Extension()> Public Function reverse(ByVal str As String) As String
        Return Convert.ToString((New StringBuilder(str)).reverse())
    End Function

    <Extension()> Public Function reverse(ByVal s As StringBuilder) As StringBuilder
        Dim l As Int32 = 0
        l = strlen(s)
        For i As Int32 = 0 To (l >> 1) - 1
            swap(s(i), s(l - i - 1))
        Next
        Return s
    End Function
End Module
