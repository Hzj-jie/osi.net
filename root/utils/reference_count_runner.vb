
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.template
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class reference_count_runner
    Inherits reference_count_runner(Of _false, _false)

    Private Shared Function proxy(ByVal v As Action(Of reference_count_runner)) _
                                 As Action(Of reference_count_runner(Of _false, _false))
        If v Is Nothing Then
            Return Nothing
        Else
            Return Sub(x As reference_count_runner(Of _false, _false))
                       Dim y As reference_count_runner = Nothing
                       y = DirectCast(x, reference_count_runner)
                       assert(Not y Is Nothing)
                       v(y)
                   End Sub
        End If
    End Function

    Public Sub New(Optional ByVal start_process As Action(Of reference_count_runner) = Nothing,
                   Optional ByVal stop_process As Action(Of reference_count_runner) = Nothing)
        MyBase.New(proxy(start_process), proxy(stop_process))
    End Sub
End Class

' The real process will start (call _start_process) if reference count is larger than 0, and stop (call _stop_process)
' if reference count equals to 0.
' When the instance has been GCed, the process will always stop.
Public Class reference_count_runner(Of AUTO_MARK_STARTED As _boolean, AUTO_MARK_STOPPED As _boolean)
    Inherits disposer

    Public Event after_start()
    Public Event after_stop()
    Private Shared ReadOnly _auto_mark_started As Boolean
    Private Shared ReadOnly _auto_mark_stopped As Boolean
    Private ReadOnly _started As ManualResetEvent
    Private ReadOnly _stopped As ManualResetEvent
    Private ReadOnly _start_process As Action(Of reference_count_runner(Of AUTO_MARK_STARTED, AUTO_MARK_STOPPED))
    Private ReadOnly _stop_process As Action(Of reference_count_runner(Of AUTO_MARK_STARTED, AUTO_MARK_STOPPED))
    Private b As Int32
    Private l As lock_t

    Shared Sub New()
        _auto_mark_started = +alloc(Of AUTO_MARK_STARTED)()
        _auto_mark_stopped = +alloc(Of AUTO_MARK_STOPPED)()
    End Sub

    Public Sub New(Optional ByVal start_process As  _
                                  Action(Of reference_count_runner(Of AUTO_MARK_STARTED, AUTO_MARK_STOPPED)) = Nothing,
                   Optional ByVal stop_process As  _
                                  Action(Of reference_count_runner(Of AUTO_MARK_STARTED, AUTO_MARK_STOPPED)) = Nothing)
        Me._start_process = start_process
        Me._stop_process = stop_process
        _started = New ManualResetEvent(False)
        _stopped = New ManualResetEvent(True)
    End Sub

    ' Users expect to call this function in start_process or somewhere else if AUTO_MARK_STARTED is false.
    Public Sub mark_started()
        assert(_started.force_set())
        RaiseEvent after_start()
    End Sub

    ' Users expect to call this function in stop_process or somewhere else if AUTO_MARK_STOPPED is false.
    Public Sub mark_stopped()
        assert(_stopped.force_set())
        RaiseEvent after_stop()
    End Sub

    Public Function binding_count() As UInt32
        Dim r As Int32 = 0
        r = atomic.read(b)
        assert(r >= 0)
        Return CUInt(r)
    End Function

    Public Function binding() As Boolean
        Return binding_count() > 0
    End Function

    Public Function not_binding() As Boolean
        Return binding_count() = 0
    End Function

    ' bind() has been called, wait for mark_started().
    Public Function starting() As Boolean
        Return binding() AndAlso Not _started.wait(0)
    End Function

    ' mark_started() has been called, but release() has not been called.
    Public Function started() As Boolean
        Return binding() AndAlso _started.wait(0)
    End Function

    ' release() has been called, wait for mark_stopped().
    Public Function stopping() As Boolean
        Return not_binding() AndAlso Not _stopped.wait(0)
    End Function

    ' mark_stopped() has been called, but bind() has not been called.
    Public Function stopped() As Boolean
        Return not_binding() AndAlso _stopped.wait(0)
    End Function

    ' mark_started() has been called, but mark_stopped() has not been called.
    Public Function running() As Boolean
        Return _started.wait(0) AndAlso Not _stopped.wait(0)
    End Function

    ' mark_stopped() has been called, but mark_started() has not been called.
    Public Function not_running() As Boolean
        Return Not _started.wait(0) AndAlso _stopped.wait(0)
    End Function

    ' Blocks current thread until started
    Public Sub wait_for_start()
        assert(_started.wait())
    End Sub

    ' Returns true if this function call took effect.
    Public Function bind() As Boolean
        l.wait()
        Try
            If b = 0 Then
                If Not _auto_mark_stopped Then
                    wait_for_stop() ' Wait for last run to finish
                End If
                b = 1
                assert(_stopped.force_reset())
                start_process()
                If Not _auto_mark_started Then
                    wait_for_start()
                End If
                Return True
            Else
                assert(b > 0)
                b += 1
                Return False
            End If
        Finally
            l.release()
        End Try
    End Function

    ' Blocks current thread for a while, until timeout or started.
    Public Function wait_for_stop(ByVal ms As Int64) As Boolean
        Return _stopped.wait(ms)
    End Function

    Public Sub wait_for_stop()
        assert(_stopped.wait())
    End Sub

    ' Returns true if this function call took effect.
    Public Function release() As Boolean
        l.wait()
        Try
            If b = 1 Then
                If Not _auto_mark_started Then
                    wait_for_start() ' Wait for last run to start
                End If
                b = 0
                assert(_started.force_reset())
                stop_process()
                If Not _auto_mark_stopped Then
                    wait_for_stop()
                End If
                Return True
            Else
                assert(b > 0)
                b -= 1
                Return False
            End If
        Finally
            l.release()
        End Try
    End Function

    Protected Overridable Sub start_process()
        void_(_start_process, Me)
        If _auto_mark_started Then
            mark_started()
        End If
    End Sub

    Protected Overridable Sub stop_process()
        void_(_stop_process, Me)
        If _auto_mark_stopped Then
            mark_stopped()
        End If
    End Sub

    Protected Overrides Sub disposer()
        If binding() Then
            b = 1
            assert(release())
        End If
        _started.Close()
        _stopped.Close()
    End Sub
End Class
