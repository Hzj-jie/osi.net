
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class reverse
    Public Shared Function [New](Of T)(ByVal i As T) As reverse(Of T)
        Return New reverse(Of T)(i)
    End Function

    Public Shared Function [New](Of T)(ByVal i As reverse(Of T)) As reverse(Of T)
        Return New reverse(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class reverse(Of T)
    Implements IComparable(Of reverse(Of T)), IComparable(Of T), IComparable, ICloneable, ICloneable(Of reverse(Of T))

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
        End If
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As reverse(Of T), ByVal that As reverse(Of T)) As reverse(Of T)
        If this Is Nothing Then
            Return that
        End If
        If that Is Nothing Then
            Return this
        End If
        Return reverse.[New](binary_operator.add(+this, +that))
    End Operator

    Public Function CompareTo(ByVal other As reverse(Of T)) As Int32 _
                             Implements IComparable(Of reverse(Of T)).CompareTo
        Return compare(+other, +Me)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim v As reverse(Of T) = Nothing
        If cast(obj, v) Then
            Return CompareTo(v)
        Else
            Return CompareTo(cast(Of T)(obj, False))
        End If
    End Function

    Public Function CompareTo(ByVal other As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(other, +Me)
    End Function

    Public Overrides Function ToString() As String
        Return strcat("reverse(", v, ")")
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return If(v IsNot Nothing, 0, v.GetHashCode())
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As reverse(Of T) Implements ICloneable(Of reverse(Of T)).Clone
        Return reverse.[New](copy_no_error(v))
    End Function
End Class
