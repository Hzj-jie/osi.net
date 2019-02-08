
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_LOCK_T = False
Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
#If USE_LOCK_T Then
Imports osi.root.lock
#End If

Partial Public Class event_comb
#If Not USE_LOCK_T AndAlso DEBUG Then
    Private lock_thread_id As Int32 = npos
#End If

    Private Sub debug_reenterable_locked(ByVal f As Action)
#If Not USE_LOCK_T OrElse DEBUG Then
        reenterable_locked(f)
#Else
        f()
#End If
    End Sub

    Private Function debug_reenterable_locked(ByVal f As Func(Of Boolean)) As Boolean
#If Not USE_LOCK_T OrElse DEBUG Then
        Return reenterable_locked(f)
#Else
        Return f()
#End If
    End Function

    Private Sub assert_in_lock()
#If USE_LOCK_T AndAlso DEBUG Then
        assert(_l.held_in_thread())
#ElseIf DEBUG Then
        assert(lock_thread_id = current_thread_id())
#End If
    End Sub

    Private Sub _reenterable_locked(ByVal d As Action)
#If USE_LOCK_T Then
        _l.reenterable_locked(d)
#ElseIf DEBUG Then
        SyncLock Me
            If lock_thread_id = npos Then
                lock_thread_id = current_thread_id()
                assert(lock_thread_id <> npos)
                d()
                assert(lock_thread_id = current_thread_id())
                lock_thread_id = npos
            Else
                assert(lock_thread_id = current_thread_id())
                d()
            End If
        End SyncLock
#Else
        SyncLock Me
            d()
        End SyncLock
#End If
    End Sub

    Private Sub reenterable_locked(ByVal d As Action)
        assert(Not d Is Nothing)
        If lock_trace AndAlso event_comb_trace Then
            Dim n As Int64 = 0
            n = Now().milliseconds()
            _reenterable_locked(d)
            If Now().milliseconds() - n > half_timeslice_length_ms Then
                raise_error(error_type.performance,
                            callstack(), ":", [step],
                            " is using ", Now().milliseconds() - n, "ms to wait for another thread to finish")
            End If
        Else
            _reenterable_locked(d)
        End If
    End Sub

    Private Function reenterable_locked(ByVal f As Func(Of Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim r As Boolean = False
        reenterable_locked(Sub()
                               r = f()
                           End Sub)
        Return r
    End Function
End Class
