
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation

Public Module _constant
    <Extension()> Public Function get_or_null(Of T)(ByVal this As constant(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.get()
        End If
    End Function
End Module

Public NotInheritable Class constant
    Public Shared Function [New](Of T)(ByVal v As T) As constant(Of T)
        Return New constant(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class constant(Of T)
    Implements IComparable(Of constant(Of T)), IComparable(Of T), IComparable, ICloneable, ICloneable(Of constant(Of T))

    Private ReadOnly v As T

    Public Sub New(ByVal v As T)
        Me.v = v
    End Sub

    Public Function [get]() As T
        Return v
    End Function

    Public Function empty() As Boolean
        Return v Is Nothing
    End Function

    Public Shared Operator +(ByVal this As constant(Of T)) As T
        Return this.get_or_null()
    End Operator

    Public Overrides Function ToString() As String
        Return strcat("constant(", v, ")")
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return If(v Is Nothing, 0, v.GetHashCode())
    End Function

    Public Function CompareTo(ByVal other As constant(Of T)) As Int32 Implements IComparable(Of constant(Of T)).CompareTo
        Return CompareTo(+other)
    End Function

    Public Function CompareTo(ByVal other As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(v, other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim v As constant(Of T) = Nothing
        If cast(obj, v) Then
            Return CompareTo(v)
        Else
            Return CompareTo(cast(Of T)(obj, False))
        End If
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As constant(Of T) Implements ICloneable(Of constant(Of T)).Clone
        Return constant.[New](copy_no_error(v))
    End Function
End Class
