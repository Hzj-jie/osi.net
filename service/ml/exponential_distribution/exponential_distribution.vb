
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports SysMath = System.Math

Partial Public NotInheritable Class exponential_distribution
    Implements distribution, IEquatable(Of exponential_distribution)

    Public ReadOnly lambda As Double

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

    Public Function near_match(ByVal other As exponential_distribution, ByVal diff As Double) As Boolean
        If other Is Nothing Then
            Return False
        End If
        Return SysMath.Abs(lambda - other.lambda) <= diff
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
