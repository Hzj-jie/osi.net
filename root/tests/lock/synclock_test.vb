
Imports System.Threading
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.template

Public Class synclock_test
    Inherits synclock_test(Of _true)
End Class

Public Class monitor_enter_exit_test
    Inherits synclock_test(Of _false)
End Class

'this is a copy of islimlock_test, to compare the performance
Public Class synclock_test(Of S As _boolean)
    Inherits processor_measured_case_wrapper

    Private Shared ReadOnly size As Int64
    Private Shared ReadOnly thread_count As Int64
    Private Shared ReadOnly sync_lock As Boolean

    Shared Sub New()
        size = 524288 * If(isdebugbuild(), 1, 8)
        thread_count = 4 * If(isdebugbuild(), 1, 4)
        sync_lock = +(alloc(Of S)())
    End Sub

    Protected Sub New(ByVal lc As synclock_case)
        MyBase.New(multithreading(repeat(lc, size), thread_count), timeslice_length_ms)
    End Sub

    Protected Sub New()
        Me.New(New synclock_case())
    End Sub

    Protected Class synclock_case
        Inherits [case]

        Private ReadOnly run_times As atomic_int
        Private i As Int32
        Private l As Object

        Public Sub New()
            run_times = New atomic_int()
            l = New Object()
        End Sub

        Protected Overridable Sub before_wait(ByRef l As Object)
        End Sub

        Protected Overridable Sub after_wait(ByRef l As Object)
        End Sub

        Protected Overridable Sub before_release(ByRef l As Object)
        End Sub

        Protected Overridable Sub after_release(ByRef l As Object)
        End Sub

        Private Sub inner()
            after_wait(l)
            i += 1
            before_release(l)
        End Sub

        Public NotOverridable Overrides Function run() As Boolean
            run_times.increment()
            before_wait(l)
            If sync_lock Then
                SyncLock l
                    inner()
                End SyncLock
            Else
                Monitor.Enter(l)
                inner()
                Monitor.Exit(l)
            End If
            after_release(l)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_equal(i, +run_times)
            Return MyBase.finish()
        End Function
    End Class
End Class
