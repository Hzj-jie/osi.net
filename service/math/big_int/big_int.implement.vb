
Partial Public Class big_int
    Implements ICloneable, IComparable(Of big_int), IComparable(Of big_uint)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New big_int(Me)
    End Function

    Public Function CompareTo(ByVal other As big_int) As Int32 Implements IComparable(Of big_int).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function
End Class
