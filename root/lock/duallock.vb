
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_CROSS_THREAD_LOCK = False
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports lock_t = osi.root.lock.slimlock.monitorlock
#If USE_CROSS_THREAD_LOCK Then
'Imports cross_thread_lock_t = osi.root.lock.slimlock.eventlock
Imports cross_thread_lock_t = osi.root.lock.slimlock.simplelock
#End If

'multi writer / multi reader model
'lock / unlock is for writer, which will also have a sequentialLock to protect.
'wait / release is for reader, suppose reader will not change, but only read, so no protection needed.
'readers will not block other readers, except for fl check & sl increment / decrement
'writers will block other writers, meanwhile any readers
Public Structure duallock
    Private fl As lock_t
    Private sl As Int32
#If USE_CROSS_THREAD_LOCK Then
    Private sz As cross_thread_lock_t
#End If

    <global_init(global_init_level.runtime_checkers)>
    Private NotInheritable Class assertions
        Private Shared Sub init()
#If USE_CROSS_THREAD_LOCK Then
            assert(New cross_thread_lock_t().can_cross_thread())
#End If
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Sub lock()
        fl.wait()
#If USE_CROSS_THREAD_LOCK Then
        sz.wait()
#Else
        lazy_wait_when(Function(ByRef x) x > 0, sl)
#End If
    End Sub

    Public Sub unlock()
        assert(sl = 0)
#If USE_CROSS_THREAD_LOCK Then
        sz.release()
#End If
        fl.release()
    End Sub

    Public Sub writer_wait()
        lock()
    End Sub

    Public Sub writer_release()
        unlock()
    End Sub

    Public Sub wait()
        fl.wait()
#If USE_CROSS_THREAD_LOCK Then
        If Interlocked.Increment(sl) = 1 Then
            sz.wait()
        End If
#Else
        Interlocked.Increment(sl)
#End If
        fl.release()
    End Sub

    Public Sub release()
        Dim r As Int32 = 0
        r = Interlocked.Decrement(sl)
        assert(r >= 0)
#If USE_CROSS_THREAD_LOCK Then
        If r = 0 Then
            sz.release()
        End If
#End If
    End Sub

    Public Sub reader_wait()
        wait()
    End Sub

    Public Sub reader_release()
        release()
    End Sub
End Structure
