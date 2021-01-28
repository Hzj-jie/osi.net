
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class array_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim a As array(Of Int32) = Nothing
        a = New array(Of Int32)(100)
        assertion.equal(a.size(), CUInt(100))
        For i As Int32 = 0 To 100 - 1
            a(CUInt(i)) = i
        Next
        For i As Int32 = 0 To 100 - 1
            assertion.equal(a(CUInt(i)), i)
        Next
        Return True
    End Function
End Class
