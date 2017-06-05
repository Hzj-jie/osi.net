
Imports osi.root.connector

Partial Public Class big_uint
    Implements ICloneable, ICloneable(Of big_uint), IComparable(Of big_uint)

    Public Function CloneT() As big_uint Implements ICloneable(Of big_uint).Clone
        Return New big_uint(Me)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function
End Class
