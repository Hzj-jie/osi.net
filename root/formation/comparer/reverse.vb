
Imports osi.root.connector

Public Class reverse(Of T)
    Implements IComparable(Of reverse(Of T)), IComparable(Of T), IComparable

    Private ReadOnly v As T

    Public Sub New(ByVal i As T)
        Me.v = i
    End Sub

    Public Sub New(ByVal i As reverse(Of T))
        Me.New(+i)
    End Sub

    Public Shared Operator +(ByVal this As reverse(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.v
        End If
    End Operator

    Public Function CompareTo(ByVal other As reverse(Of T)) As Int32 _
                             Implements IComparable(Of reverse(Of T)).CompareTo
        Return compare(+other, +Me)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim v As reverse(Of T) = Nothing
        If cast(Of reverse(Of T))(obj, v) Then
            Return CompareTo(v)
        Else
            Return CompareTo(cast(Of T)(obj, False))
        End If
    End Function

    Public Function CompareTo(ByVal other As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(other, +Me)
    End Function
End Class
