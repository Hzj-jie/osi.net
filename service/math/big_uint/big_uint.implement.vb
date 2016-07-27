
Imports osi.root.connector

Partial Public Class big_uint
    Implements ICloneable, IComparable(Of big_uint)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New big_uint(Me)
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function
End Class
