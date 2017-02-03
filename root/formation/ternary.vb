
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Structure ternary
    Implements IComparable(Of ternary), IComparable, ICloneable(Of ternary), ICloneable

    Private Const _unknown As Byte = 0
    Private Const _true As Byte = 1
    Private Const _false As Byte = 2
    Public Shared ReadOnly unknown As ternary = New ternary(_unknown)
    Public Shared ReadOnly [true] As ternary = New ternary(_true)
    Public Shared ReadOnly [false] As ternary = New ternary(_false)

    Private d As Int32

    Shared Sub New()
        assert(DirectCast(Nothing, Int32) = _unknown)
    End Sub

    Public Shared Operator =(ByVal this As ternary, ByVal that As ternary) As Boolean
        Return this.d = that.d
    End Operator

    Public Shared Operator <>(ByVal this As ternary, ByVal that As ternary) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Widening Operator CType(ByVal this As ternary) As Boolean
        Return this.true_()
    End Operator

    Public Shared Widening Operator CType(ByVal this As Boolean) As ternary
        Return If(this, ternary.true, ternary.false)
    End Operator

    Public Function true_() As Boolean
        Return d = _true
    End Function

    Public Function false_() As Boolean
        Return d = _false
    End Function

    Public Function unknown_() As Boolean
        Return d = _unknown
    End Function

    Public Function nottrue() As Boolean
        Return Not true_()
    End Function

    Public Function notfalse() As Boolean
        Return Not false_()
    End Function

    Public Function notunknown() As Boolean
        Return Not unknown_()
    End Function

    Public Sub New(ByVal i As ternary)
        Me.New(i.d)
    End Sub

    Private Sub New(ByVal i As Int32)
        d = i
    End Sub

    Public Function reverse() As ternary
        If d = _true Then
            Return ternary.false
        ElseIf d = _false Then
            Return ternary.true
        Else
            Return ternary.unknown
        End If
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As ternary Implements ICloneable(Of ternary).Clone
        Return New ternary(Me)
    End Function

    Public Function CompareTo(ByVal other As ternary) As Int32 Implements IComparable(Of ternary).CompareTo
        Return compare(d, other.d)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of ternary)(obj, False))
    End Function
End Structure
