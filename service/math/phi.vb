
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _phi
    <Extension()> Public Function phi(ByVal x As Int32) As Int32
        Dim pd As vector(Of UInt32) = Nothing
        pd = prime_divisors(x)
        If pd Is Nothing Then
            Return npos
        End If
        If pd.empty() Then
            'phi(0) = 0
            'phi(1) = 1
            Return x
        End If
        Dim r As Int32 = 0
        r = x
        For i As Int32 = 1 To CInt(pd.size())
            assert(choose(Sub(ByVal a() As UInt32)
                              Dim t As Int32 = 1
                              For j As Int32 = 0 To array_size_i(a) - 1
                                  t *= CInt(a(j))
                              Next
                              If (array_size_i(a) And 1) = 1 Then
                                  r -= x \ t
                              Else
                                  r += x \ t
                              End If
                          End Sub,
                          +pd,
                          i))
        Next
        Return r
    End Function
End Module
