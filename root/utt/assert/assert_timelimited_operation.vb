
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector

Public Module _assert_timelimited_operation
    Public Function assert_now_in_time_range(ByVal l As Int64, ByVal u As Int64) As Boolean
        assert(l <= u)
        Dim n As Int64 = 0
        n = Now().milliseconds()
        Return assert_more_or_equal(n, l) AndAlso
               assert_less_or_equal(n, u)
    End Function

    Friend Sub set_time_range(ByRef exp_l As Int64, ByRef exp_u As Int64, ByVal low As Int64, ByVal up As Int64)
        assert(low <= up)
        Dim n As Int64 = 0
        n = Now().milliseconds()
        exp_l = n + low
        exp_u = n + up
    End Sub
End Module

Public Structure auto_assert_timelimited_operation
    Implements IDisposable

    Private ReadOnly exp_l As Int64
    Private ReadOnly exp_u As Int64

    Public Sub New(ByVal low As Int64, ByVal up As Int64)
        set_time_range(exp_l, exp_u, low, up)
    End Sub

    Public Sub New(ByVal ms As Int64)
        Me.New(0, ms)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        assert(exp_l <> DirectCast(Nothing, Int64))
        assert(exp_u <> DirectCast(Nothing, Int64))
        assert_now_in_time_range(exp_l, exp_u)
    End Sub
End Structure

Public Class manual_assert_timelimited_operation
    Private ReadOnly exp_l As Int64
    Private ReadOnly exp_u As Int64
    Private finished As Boolean

    Public Sub New(ByVal low As Int64, ByVal up As Int64)
        set_time_range(exp_l, exp_u, low, up)
        finished = False
    End Sub

    Public Sub New(ByVal ms As Int64)
        Me.New(0, ms)
    End Sub

    Public Sub finish()
        assert(Not finished)
        finished = True
        assert_now_in_time_range(exp_l, exp_u)
    End Sub
End Class
