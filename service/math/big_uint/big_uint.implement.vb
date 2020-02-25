
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_uint
    Implements ICloneable, ICloneable(Of big_uint), IComparable, IComparable(Of big_uint), IEquatable(Of big_uint)

    Public Function CloneT() As big_uint Implements ICloneable(Of big_uint).Clone
        Return New big_uint(Me)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return compare(cast(Of big_uint)().from(obj, False))
    End Function

    Public Overloads Function Equals(ByVal other As big_uint) As Boolean Implements IEquatable(Of big_uint).Equals
        Return equal(other)
    End Function
End Class
