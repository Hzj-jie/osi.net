
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_ufloat
    Implements ICloneable, ICloneable(Of big_ufloat), IComparable, IComparable(Of big_ufloat), IEquatable(Of big_ufloat)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As big_ufloat Implements ICloneable(Of big_ufloat).Clone
        Return New big_ufloat(i.CloneT(), d.CloneT())
    End Function

    Public Function CompareTo(ByVal other As big_ufloat) As Int32 Implements IComparable(Of big_ufloat).CompareTo
        Return compare(Me, other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of big_ufloat)().from(obj, False))
    End Function

    Public Overloads Function Equals(ByVal other As big_ufloat) As Boolean Implements IEquatable(Of big_ufloat).Equals
        Return compare(Me, other) = 0
    End Function
End Class
