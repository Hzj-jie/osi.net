
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class one_off_test
    Inherits [case]

    Private ReadOnly x As one_off(Of Object)
    Private ReadOnly mre As ManualResetEvent
    Private ReadOnly suc_times As atomic_int
    Private ReadOnly read_thread_count As atomic_int
    Private running As Boolean
    Private is_get As Boolean

    Public Sub New()
        x = New one_off(Of Object)()
        mre = New ManualResetEvent(False)
        suc_times = New atomic_int()
        read_thread_count = New atomic_int()
    End Sub

    Private Sub read_thread()
        read_thread_count.increment()
        While running
            assert(mre.WaitOne())
            If is_get Then
                If x.get(Nothing) Then
                    suc_times.increment()
                End If
            Else
                If x.set(New Object()) Then
                    suc_times.increment()
                End If
            End If
        End While
        read_thread_count.decrement()
    End Sub

    Private Sub run_case()
        Const thread_count As Int32 = 8
        running = True
        For i As Int32 = 0 To thread_count - 1
            start_thread(AddressOf read_thread)
        Next
        assertion.is_true(timeslice_sleep_wait_until(Function() As Boolean
                                                         Return (+read_thread_count) > 1
                                                     End Function,
                                               seconds_to_milliseconds(10)))
        Const times As Int32 = 1024
        suc_times.exchange(0)
        If Not is_get Then
            assertion.is_true(x.set(New Object()))
        End If
        For i As Int32 = 0 To times - 1
            If is_get Then
                assertion.is_true(x.set(New Object()))
            Else
                assertion.is_true(x.get(Nothing))
            End If
            assert(mre.Set())
            lazy_wait_when(Function() As Boolean
                               If is_get Then
                                   Return x.has()
                               Else
                                   Return Not x.has()
                               End If
                           End Function)
            assert(mre.Reset())
        Next
        assertion.is_true(timeslice_sleep_wait_until(Function() As Boolean
                                                         Return ((+suc_times) = times)
                                                     End Function,
                                               seconds_to_milliseconds(10)))
        assertion.equal(+suc_times, times)
        If Not is_get Then
            assertion.is_true(x.get(Nothing))
        End If
        running = False
        assert(mre.Set())
        assertion.is_true(timeslice_sleep_wait_when(Function() As Boolean
                                                        Return (+read_thread_count) > 0
                                                    End Function,
                                              seconds_to_milliseconds(10)))
        assert(mre.Reset())
    End Sub

    Public Overrides Function run() As Boolean
        is_get = True
        run_case()
        is_get = False
        run_case()
        Return True
    End Function

    Protected Overrides Sub Finalize()
        mre.Close()
        mre.Dispose()
        MyBase.Finalize()
    End Sub
End Class
