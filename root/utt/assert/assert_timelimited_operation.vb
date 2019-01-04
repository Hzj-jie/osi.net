
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' TODO: Move to check
Public Structure auto_assert_timelimited_operation
    Implements IDisposable

    Private ReadOnly exp_l As Int64
    Private ReadOnly exp_u As Int64

    Public Sub New(ByVal low As Int64, ByVal up As Int64)
        assertion.set_time_range(exp_l, exp_u, low, up)
    End Sub

    Public Sub New(ByVal ms As Int64)
        Me.New(0, ms)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        assert(exp_l <> DirectCast(Nothing, Int64))
        assert(exp_u <> DirectCast(Nothing, Int64))
        assertion.now_in_time_range(exp_l, exp_u)
    End Sub
End Structure

Public Class manual_assert_timelimited_operation
    Private ReadOnly exp_l As Int64
    Private ReadOnly exp_u As Int64
    Private finished As Boolean

    Public Sub New(ByVal low As Int64, ByVal up As Int64)
        assertion.set_time_range(exp_l, exp_u, low, up)
        finished = False
    End Sub

    Public Sub New(ByVal ms As Int64)
        Me.New(0, ms)
    End Sub

    Public Sub finish()
        assert(Not finished)
        finished = True
        assertion.now_in_time_range(exp_l, exp_u)
    End Sub
End Class
