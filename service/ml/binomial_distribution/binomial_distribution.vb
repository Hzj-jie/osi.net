
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports SysMath = System.Math

Partial Public NotInheritable Class binomial_distribution
    Implements distribution, IEquatable(Of binomial_distribution)

    Private ReadOnly n As UInt64
    Private ReadOnly p As Double
    Private ReadOnly sr As lazier(Of tuple(Of Double, Double)) =
        lazier.of(Function() As tuple(Of Double, Double)
                      Return tuple.of(p * n - SysMath.Sqrt(p * (1 - p) / n) * 3,
                                      p * n + SysMath.Sqrt(p * (1 - p) / n) * 3)
                  End Function)

    Shared Sub New()
        struct(Of binomial_distribution).register()
    End Sub

    ' For typed_constructor / serialization
    Private Sub New()
    End Sub

    Public Sub New(ByVal n As UInt64, ByVal p As Double)
        assert(n > 0)
        assert(p >= 0 AndAlso p <= 1)
        Me.n = n
        Me.p = p
    End Sub

    Public Function cumulative_distribute(ByVal v As Double) As Double Implements distribution.cumulative_distribute
        If v < 0 Then
            Return 0
        End If
        If v = 0 Then
            Return possibility(v)
        End If
        Dim v2 As UInt64 = assert_which.of(v).can_floor_to_uint64()
        If v2 >= n Then
            Return 1
        End If
        Dim r As Double = 0
        For i As UInt64 = 0 To v2 - uint64_1
            r += possibility(i)
        Next
        Return r
    End Function

    Public Function possibility(ByVal v As Double) As Double Implements distribution.possibility
        If v < 0 Then
            Return 0
        End If
        Dim v2 As UInt64 = assert_which.of(v).can_floor_to_uint64()
        If v2 > n Then
            Return 0
        End If
        Return combinatories.of(v2, n) * (p ^ v2) * (1 - p) ^ (n - v2)
    End Function

    Public Function range_possibility(ByVal min As Double,
                                      ByVal max As Double) As Double Implements distribution.range_possibility
        Return cumulative_distribute(max) - cumulative_distribute(min)
    End Function

    Public Function parameter_space() As one_of(Of tuple(Of Double, Double), vector(Of Double)) _
            Implements distribution.parameter_space
        Return tuple.of(double_0, CDbl(n)).as_range()
    End Function

    ' ~= N(p*n, sqrt(p*(1-p)/n))
    Public Function significant_range() As tuple(Of Double, Double) Implements distribution.significant_range
        Return sr
    End Function

    Public Overloads Function Equals(ByVal other As binomial_distribution) As Boolean _
            Implements IEquatable(Of binomial_distribution).Equals
        If other Is Nothing Then
            Return False
        End If
        Return other.n = n AndAlso other.p = p
    End Function

    Public Function near_match(ByVal other As binomial_distribution, ByVal diff As Double) As Boolean
        If other Is Nothing Then
            Return False
        End If
        Return SysMath.Abs(p - other.p) <= diff AndAlso n = other.n
    End Function

    Public Function near_match(ByVal other As binomial_distribution) As Boolean
        Return near_match(other, 0.001)
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Equals(cast(Of binomial_distribution).from(obj, False))
    End Function
End Class
