
Option Explicit On
Option Infer Off
Option Strict On

#Const INPLACE_REVERSE = True
Imports System.Text
Imports System.Runtime.CompilerServices


Public Module _reverse
    <Extension()> Public Function reverse(ByVal str As String) As String
        Return Convert.ToString((New StringBuilder(str)).reverse())
    End Function

#If INPLACE_REVERSE Then
    <Extension()> Public Function reverse(ByVal s As StringBuilder) As StringBuilder
        Dim l As Int32 = 0
        l = strlen_i(s)
        For i As Int32 = 0 To (l >> 1) - 1
            swap(s(i), s(l - i - 1))
        Next
        Return s
#Else
    <Extension()> Public Function reverse(ByRef s As StringBuilder) As StringBuilder
        Dim r As StringBuilder = Nothing
        r = New StringBuilder(strlen_i(s))
        For i As Int32 = strlen_i(s) - 1 To 0 Step -1
            r.Append(s(i))
        Next
        s = r
        Return r
#End If
    End Function
End Module
