
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading

Public NotInheritable Class atomic_bool
    Private ReadOnly init_value As Int32
    Private v As Int32

    Public Sub New(ByVal init_value As Int32)
        Me.init_value = init_value
        Me.v = init_value
    End Sub

    Public Sub New(ByVal init_value As Boolean)
        Me.New(If(init_value, 1, 0))
    End Sub

    Public Sub New()
        Me.New(False)
    End Sub

    Public Function init_state() As Boolean
        Return v = init_value
    End Function

    Public Function reset() As Boolean
        If init_state() Then
            Return False
        Else
            v = init_value
            Return True
        End If
    End Function

    Private Shared Function true_(ByVal v As Int32) As Boolean
        Return v > 0
    End Function

    Public Function true_() As Boolean
        Return true_(v)
    End Function

    Public Function false_() As Boolean
        Return Not true_()
    End Function

    Public Function inc() As Boolean
        Return true_(Interlocked.Increment(v))
    End Function

    Public Function dec() As Boolean
        Return true_(Interlocked.Decrement(v))
    End Function

    Public Function [set](ByVal v As Boolean) As Boolean
        Return If(v, inc(), dec())
    End Function

    Public Shared Widening Operator CType(ByVal this As atomic_bool) As Boolean
        Return +this
    End Operator

    Public Shared Operator Not(ByVal this As atomic_bool) As Boolean
        Return Not (+this)
    End Operator

    Public Shared Operator +(ByVal this As atomic_bool) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.true_()
        End If
    End Operator
End Class
