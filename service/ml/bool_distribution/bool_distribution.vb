
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class bool_distribution
    Implements distribution, IEquatable(Of distribution)

    Private ReadOnly n As UInt64
    Private ReadOnly p As Double

    Shared Sub New()
        struct(Of bool_distribution).register()
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
        Throw New NotImplementedException()
    End Function

    Public Function possibility(ByVal v As Double) As Double Implements distribution.possibility
        Throw New NotImplementedException()
    End Function

    Public Function range_possibility(ByVal min As Double, ByVal max As Double) As Double Implements distribution.range_possibility
        Throw New NotImplementedException()
    End Function

    Public Overloads Function Equals(ByVal other As distribution) As Boolean Implements IEquatable(Of distribution).Equals
        Throw New NotImplementedException()
    End Function
End Class
