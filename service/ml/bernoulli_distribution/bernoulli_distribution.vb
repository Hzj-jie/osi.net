
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports SysMath = System.Math

Partial Public NotInheritable Class bernoulli_distribution
    Implements distribution, IEquatable(Of bernoulli_distribution)

    Private ReadOnly p As Double

    Shared Sub New()
        struct(Of bernoulli_distribution).register()
    End Sub

    ' For typed_constructor / serialization
    Private Sub New()
    End Sub

    Public Sub New(ByVal p As Double)
        assert(p >= 0 AndAlso p <= 1)
        Me.p = p
    End Sub

    Public Function cumulative_distribute(ByVal v As Double) As Double Implements distribution.cumulative_distribute
        If v < 1 Then
            Return 1 - p
        End If
        Return 1
    End Function

    Public Function possibility(ByVal v As Double) As Double Implements distribution.possibility
        Return If(v = 1, p, 1 - p)
    End Function

    Public Function range_possibility(ByVal min As Double,
                                      ByVal max As Double) As Double Implements distribution.range_possibility
        Return cumulative_distribute(max) - cumulative_distribute(min)
    End Function

    Public Function near_match(ByVal other As bernoulli_distribution, ByVal diff As Double) As Boolean
        If other Is Nothing Then
            Return False
        End If
        Return SysMath.Abs(other.p - p) <= diff
    End Function

    Public Function near_match(ByVal other As bernoulli_distribution) As Boolean
        Return near_match(other, 0.001)
    End Function

    Public Overloads Function Equals(ByVal other As bernoulli_distribution) As Boolean _
            Implements IEquatable(Of bernoulli_distribution).Equals
        If other Is Nothing Then
            Return False
        End If
        Return p = other.p
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Equals(cast(Of bernoulli_distribution).from(obj, False))
    End Function
End Class
