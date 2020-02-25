
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_int
    Implements ICloneable,
               ICloneable(Of big_int),
               IComparable,
               IComparable(Of big_uint),
               IComparable(Of big_int),
               IEquatable(Of big_int)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As big_int Implements ICloneable(Of big_int).Clone
        Return New big_int(Me)
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As big_int) As Int32 Implements IComparable(Of big_int).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As Object) As Int32 Implements IComparable.CompareTo
        Return compare(cast(Of big_int)().from(other, False))
    End Function

    Public Overloads Function Equals(ByVal other As big_int) As Boolean Implements IEquatable(Of big_int).Equals
        Return equal(other)
    End Function
End Class
