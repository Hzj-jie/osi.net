
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class permutations
    Public Shared Function [of](ByVal n As UInt64, ByVal k As UInt64) As UInt64
        assert(k <= n)
        If k = 0 Then
            Return 1
        End If
        Dim r As UInt64 = 1
        For i As UInt64 = 0 To k - uint64_1
            r *= (n - i)
        Next
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
