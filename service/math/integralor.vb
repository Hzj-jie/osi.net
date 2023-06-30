
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' A silly but simple way to calculate integral by using accumulation.
Public NotInheritable Class integralor
    Private start As Double
    Private [end] As Double
    Private incremental As Double
    Private f As Func(Of Double, Double)

    Public Function with_start(ByVal start As Double) As integralor
        Me.start = start
        Return Me
    End Function

    Public Function with_end(ByVal [end] As Double) As integralor
        Me.end = [end]
        Return Me
    End Function

    Public Function with_incremental(ByVal incremental As Double) As integralor
        assert(incremental <> 0)
        Me.incremental = incremental
        Return Me
    End Function

    Public Function with_function(ByVal f As Func(Of Double, Double)) As integralor
        assert(Not f Is Nothing)
        Me.f = f
        Return Me
    End Function

    Public Function calculate() As Double
        assert(Not f Is Nothing)
        assert(incremental <> 0)
        If start = [end] Then
            Return 0
        End If
        Dim s As Double = 0
        Dim e As Double = 0
        Dim inc As Double = 0
        If start < [end] Then
            s = start
            e = [end]
        Else
            s = [end]
            e = start
        End If
        inc = incremental.abs()
        Dim r As Double = 0
        For i As Double = s + inc / 2 To e Step inc
            r += inc * f(i)
        Next
        If ([end] - start) * incremental > 0 Then
            Return r
        End If
        Return -r
    End Function

    Public Shared Widening Operator CType(ByVal this As integralor) As Double
        If this Is Nothing Then
            Return 0
        End If
        Return this.calculate()
    End Operator
End Class
