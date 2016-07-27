
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.utils

Public Module _phi
    <Extension()> Public Function phi(ByVal x As Int32) As Int32
        Dim pd As vector(Of Int32) = Nothing
        pd = prime_divisors(x)
        If pd Is Nothing Then
            Return npos
        ElseIf pd.empty() Then
            'phi(0) = 0
            'phi(1) = 1
            Return x
        Else
            Dim r As Int32 = 0
            r = x
            For i As Int32 = 1 To pd.size()
                assert(choose(Sub(a() As Int32)
                                  Dim t As Int32 = 1
                                  For j As Int32 = 0 To array_size(a) - 1
                                      t *= a(j)
                                  Next
                                  If ((array_size(a) And 1) = 1) Then
                                      r -= x \ t
                                  Else
                                      r += x \ t
                                  End If
                              End Sub,
                              +pd,
                              i))
            Next
            Return r
        End If
    End Function
End Module
