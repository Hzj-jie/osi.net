
Imports osi.root.lock
Imports osi.root.connector

Public MustInherit Class count_case
    Inherits [case]

    Protected MustOverride Function run_case() As Boolean
    Private ReadOnly r As running_counter

    Protected Sub New(ByVal compare As Boolean,
                      ByVal scale As Double)
        r = New running_counter(compare, scale)
    End Sub

    Protected Sub New(ByVal compare As Boolean)
        r = New running_counter(compare:=compare)
    End Sub

    Protected Sub New(ByVal scale As Double)
        r = New running_counter(scale:=scale)
    End Sub

    Protected Sub New()
        r = New running_counter()
    End Sub

    Public Sub trigger()
        r.trigger()
    End Sub

    Public Sub inc_run()
        r.inc_run()
    End Sub

    Public Sub inc_finish()
        r.inc_finish()
    End Sub

    Public Function run_times() As Int32
        Return r.run_times()
    End Function

    Public Function finish_times() As Int32
        Return r.finish_times()
    End Function

    Public Function trigger_times() As Int32
        Return r.trigger_times()
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        inc_run()
        Try
            Return run_case()
        Finally
            inc_finish()
        End Try
    End Function

    Public Overrides Function finish() As Boolean
        r.finish()
        Return MyBase.finish()
    End Function
End Class

Public Class running_counter
    Private ReadOnly _run As atomic_int
    Private ReadOnly _finish As atomic_int
    Private ReadOnly _trigger As atomic_int
    Private ReadOnly scale As Double
    Private ReadOnly compare As Boolean

    Public Sub New(Optional ByVal compare As Boolean = True,
                   Optional ByVal scale As Double = 1)
        Me.compare = compare
        Me.scale = scale
        Me._run = New atomic_int()
        Me._finish = New atomic_int()
        Me._trigger = New atomic_int()
    End Sub

    Public Sub trigger()
        _trigger.increment()
    End Sub

    Public Sub inc_run()
        _run.increment()
    End Sub

    Public Sub inc_finish()
        _finish.increment()
    End Sub

    Public Function run_times() As Int32
        Return +_run
    End Function

    Public Function finish_times() As Int32
        Return +_finish
    End Function

    Public Function trigger_times() As Int32
        Return +_trigger
    End Function

    Public Sub finish()
        timeslice_sleep_wait_until(Function() (+_run) = (+_finish))
        raise_error("finished running_counter case ",
                    Me.GetType().Name(),
                    ", run_times ",
                    run_times(),
                    ", trigger_times ",
                    trigger_times())
        If compare Then
            assert_equal((+_run) * scale, +_trigger)
        End If
    End Sub
End Class
