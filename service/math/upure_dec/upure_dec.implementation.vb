
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class upure_dec
    Implements ICloneable, ICloneable(Of upure_dec), IComparable, IComparable(Of upure_dec)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As upure_dec Implements ICloneable(Of upure_dec).Clone
        Return New upure_dec(n.CloneT(), d.CloneT())
    End Function

    Public Function CompareTo(ByVal other As upure_dec) As Int32 Implements IComparable(Of upure_dec).CompareTo
        Return compare(Me, other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of upure_dec).from(obj, False))
    End Function
End Class
