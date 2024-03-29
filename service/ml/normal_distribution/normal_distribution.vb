
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports integralor = osi.service.math.integralor
Imports SysMath = System.Math

Partial Public NotInheritable Class normal_distribution
    Implements distribution, IEquatable(Of normal_distribution)

    Private Const default_incremental As Double = 0.000001
    Public ReadOnly mean As Double
    Public ReadOnly variance As Double

    Shared Sub New()
        struct(Of normal_distribution).register()
    End Sub

    ' For typed_constructor / serialization
    Private Sub New()
    End Sub

    Public Sub New(ByVal mean As Double, ByVal variance As Double)
        assert(variance > 0)
        Me.mean = mean
        Me.variance = variance
    End Sub

    Public Function possibility(ByVal v As Double) As Double Implements distribution.possibility
        Return (SysMath.E ^ (-((v - mean) ^ 2) / 2 / variance)) / SysMath.Sqrt(2 * SysMath.PI * variance)
    End Function

    Public Function range_possibility(ByVal min As Double, ByVal max As Double, ByVal incremental As Double) As Double
        assert(incremental > 0)
        Return New integralor().
                       with_function(AddressOf possibility).
                       with_start(min).
                       with_end(max).
                       with_incremental(incremental).
                       calculate()
    End Function

    Public Function range_possibility(ByVal min As Double,
                                      ByVal max As Double) As Double Implements distribution.range_possibility
        Return range_possibility(min, max, default_incremental)
    End Function

    Public Function cumulative_distribute(ByVal v As Double, ByVal incremental As Double) As Double
        Return range_possibility(mean - 8 * SysMath.Sqrt(variance), v, incremental)
    End Function

    Public Function cumulative_distribute(ByVal v As Double) As Double Implements distribution.cumulative_distribute
        Return cumulative_distribute(v, default_incremental)
    End Function

    Public Function parameter_space() As one_of(Of tuple(Of Double, Double), vector(Of Double)) _
            Implements distribution.parameter_space
        Return tuple.of(Double.MinValue, Double.MaxValue).as_range()
    End Function

    ' phi(3) ~= 0.9987, extremely significant already.
    Public Function significant_range() As tuple(Of Double, Double) Implements distribution.significant_range
        Return tuple.of(mean - variance * 3, mean + variance * 3)
    End Function

    Public Function near_match(ByVal other As normal_distribution, ByVal diff As Double) As Boolean
        If other Is Nothing Then
            Return False
        End If
        Return SysMath.Abs(mean - other.mean) <= diff AndAlso SysMath.Abs(variance - other.variance) <= diff
    End Function

    Public Function near_match(ByVal other As normal_distribution) As Boolean
        Return near_match(other, 0.001)
    End Function

    Public Overloads Function Equals(ByVal other As normal_distribution) As Boolean _
            Implements IEquatable(Of normal_distribution).Equals
        If other Is Nothing Then
            Return False
        End If
        Return mean = other.mean AndAlso variance = other.variance
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Equals(cast(Of normal_distribution)().from(obj, False))
    End Function

    Public Overrides Function ToString() As String
        Return strcat("{mean = ", mean, ", variance = ", variance, "}")
    End Function
End Class
