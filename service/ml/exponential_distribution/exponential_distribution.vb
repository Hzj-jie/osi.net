
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports SysMath = System.Math

Partial Public NotInheritable Class exponential_distribution
    Implements distribution, IEquatable(Of exponential_distribution)

    Public ReadOnly lambda As Double

    Shared Sub New()
        struct(Of exponential_distribution).register()
    End Sub

    ' For typed_constructor / serialization
    Private Sub New()
    End Sub

    Public Sub New(ByVal lambda As Double)
        assert(lambda > 0)
        Me.lambda = lambda
    End Sub

    Public Function possibility(ByVal v As Double) As Double Implements distribution.possibility
        If v < 0 Then
            Return 0
        End If
        Return lambda / (SysMath.E ^ (lambda * v))
    End Function

    Public Function range_possibility(ByVal min As Double,
                                      ByVal max As Double) As Double Implements distribution.range_possibility
        Return cumulative_distribute(max) - cumulative_distribute(min)
    End Function

    Public Function cumulative_distribute(ByVal v As Double) As Double Implements distribution.cumulative_distribute
        If v < 0 Then
            Return 0
        End If
        Return 1 - (1 / (SysMath.E ^ (lambda * v)))
    End Function

    Public Function parameter_space() As one_of(Of tuple(Of Double, Double), vector(Of Double)) _
            Implements distribution.parameter_space
        Return tuple.of(double_0, Double.MaxValue).as_range()
    End Function

    ' exp(lambda*x) >= 1000 to make 1-exp(-lambda*x) >= 0.999
    Public Function significant_range() As tuple(Of Double, Double) Implements distribution.significant_range
        Return tuple.of(double_0, SysMath.Log(1000) / lambda)
    End Function

    Public Function near_match(ByVal lambda As Double, ByVal diff As Double) As Boolean
        Return SysMath.Abs(Me.lambda - lambda) <= diff
    End Function

    Public Function near_match(ByVal other As exponential_distribution, ByVal diff As Double) As Boolean
        If other Is Nothing Then
            Return False
        End If
        Return near_match(other.lambda, diff)
    End Function

    Public Function near_match(ByVal lambda As Double) As Boolean
        Return near_match(lambda, 0.001)
    End Function

    Public Function near_match(ByVal other As exponential_distribution) As Boolean
        Return near_match(other, 0.001)
    End Function

    Public Overloads Function Equals(ByVal other As exponential_distribution) As Boolean _
                                  Implements IEquatable(Of exponential_distribution).Equals
        If other Is Nothing Then
            Return False
        End If
        Return lambda = other.lambda
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Equals(cast(Of exponential_distribution)().from(obj, False))
    End Function

    Public Overrides Function ToString() As String
        Return strcat("{lambda = ", lambda, "}")
    End Function
End Class
