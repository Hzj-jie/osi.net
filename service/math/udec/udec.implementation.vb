
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class udec
    Implements ICloneable, ICloneable(Of udec), IComparable, IComparable(Of udec), IEquatable(Of udec)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As udec Implements ICloneable(Of udec).Clone
        Return New udec(n.CloneT(), d.CloneT())
    End Function

    Public Function CompareTo(ByVal other As udec) As Int32 Implements IComparable(Of udec).CompareTo
        Return compare(Me, other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of udec).from(obj, False))
    End Function

    Public Overloads Function Equals(ByVal other As udec) As Boolean Implements IEquatable(Of udec).Equals
        Return equal(other)
    End Function
End Class
