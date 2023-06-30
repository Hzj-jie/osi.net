
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.root.utt

Public Class islimlock_test(Of T As {islimlock, Structure})
    Inherits processor_measured_case_wrapper

    Private Shared ReadOnly size As Int64
    Private Shared ReadOnly thread_count As UInt32

    Shared Sub New()
        size = 524288
        thread_count = 4
    End Sub

    Protected Sub New(ByVal lc As islimlock_case, Optional ByVal small_size As Boolean = False)
        MyBase.New(multithreading(repeat(lc, If(small_size, size, size * If(isdebugbuild(), 1, 8))),
                                  If(small_size, thread_count, thread_count * CUInt(If(isdebugbuild(), 1, 4)))),
                   timeslice_length_ms)
    End Sub

    Protected Sub New(Optional ByVal small_size As Boolean = False)
        Me.New(New islimlock_case(), small_size)
    End Sub

    Protected Class islimlock_case
        Inherits [case]

        Private ReadOnly run_times As atomic_int
        Private i As Int32
        Private l As T

        Public Sub New()
            run_times = New atomic_int()
        End Sub

        Protected Overridable Sub before_wait(ByRef l As T)
        End Sub

        Protected Overridable Sub after_wait(ByRef l As T)
        End Sub

        Protected Overridable Sub before_release(ByRef l As T)
        End Sub

        Protected Overridable Sub after_release(ByRef l As T)
        End Sub

        Public NotOverridable Overrides Function run() As Boolean
            run_times.increment()
            before_wait(l)
            If rnd_bool() Then
                l.wait()
                after_wait(l)
                i += 1
                before_release(l)
                l.release()
            Else
                l.locked(Sub()
                             after_wait(l)
                             i += 1
                             before_release(l)
                         End Sub)
            End If
            after_release(l)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            raise_error("finished islimlock_test case ",
                        Me.GetType().Name(),
                        ", run_times ",
                        +run_times,
                        ", trigger_times ",
                        i)
            assertion.equal(i, +run_times)
            Return MyBase.finish()
        End Function
    End Class
End Class
