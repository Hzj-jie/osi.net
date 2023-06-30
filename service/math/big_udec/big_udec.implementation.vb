
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Implements ICloneable, ICloneable(Of big_udec), IComparable, IComparable(Of big_udec), IEquatable(Of big_udec)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As big_udec Implements ICloneable(Of big_udec).Clone
        Return New big_udec(n.CloneT(), d.CloneT())
    End Function

    Public Function CompareTo(ByVal other As big_udec) As Int32 Implements IComparable(Of big_udec).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of big_udec).from(obj, False))
    End Function

    Public Overloads Function Equals(ByVal other As big_udec) As Boolean Implements IEquatable(Of big_udec).Equals
        Return equal(other)
    End Function
End Class
