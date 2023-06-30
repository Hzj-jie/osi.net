
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock

Partial Public Class event_comb
#If DEBUG Then
    Private lock_thread_id As Int32 = npos
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub debug_locked(ByVal f As Action)
#If DEBUG Then
        locked(f)
#Else
        f()
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function debug_locked(ByVal f As Func(Of Boolean)) As Boolean
#If DEBUG Then
        Return locked(f)
#Else
        Return f()
#End If
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub assert_in_lock()
#If DEBUG Then
        assert(lock_thread_id = current_thread_id(), callstack())
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub assert_not_in_lock()
#If DEBUG Then
        assert(lock_thread_id = npos, callstack())
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub _locked(ByVal d As Action)
#If DEBUG Then
        SyncLock Me
            assert_not_in_lock()
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
            assert_not_in_lock()
            d()
        End SyncLock
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub locked(ByVal d As Action)
        assert(Not d Is Nothing)
        If lock_trace AndAlso event_comb_trace Then
            Dim n As Int64 = Now().milliseconds()
            _locked(d)
            If lock_tracer.wait_too_long(n) Then
                raise_error(error_type.performance,
                            callstack(), ":", [step],
                            " is using ", Now().milliseconds() - n, "ms to wait for another thread to finish")
            End If
        Else
            _locked(d)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function locked(ByVal f As Func(Of Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim r As Boolean = False
        locked(Sub()
                   r = f()
               End Sub)
        Return r
    End Function
End Class
