
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock.slimlock

Public Structure lock(Of T As {Structure, islimlock})
    Implements ilock
    Private owner_tid As Int32
    Private l As T

    Shared Sub New()
        assert(INVALID_THREAD_ID = DirectCast(Nothing, Int32))
        Dim l As T = Nothing
        assert(l.can_thread_owned())
        assert(Not TypeOf l Is ilock)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function held_in_thread() As Boolean Implements ilock.held_in_thread
        Return owner_tid = current_thread_id()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function held() As Boolean Implements ilock.held
        Return owner_tid <> INVALID_THREAD_ID
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub debug_wait()
        Dim n As Int64 = 0
        n = Now().milliseconds()
        l.wait()
        If Now().milliseconds() - n > half_timeslice_length_ms Then
            raise_error(error_type.performance,
                        callstack(),
                        " used ",
                        Now().milliseconds() - n,
                        "ms to wait for another thread to finish.")
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait() Implements ilock.wait
        Try
            If lock_trace Then
                debug_wait()
            Else
                l.wait()
            End If
        Catch ex As ThreadAbortException
            raise_error(error_type.warning, "thread abort")
        End Try
        assert(owner_tid = INVALID_THREAD_ID)
        owner_tid = current_thread_id()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub release() Implements ilock.release
        assert(can_cross_thread() OrElse owner_tid = current_thread_id())
        owner_tid = INVALID_THREAD_ID
        l.release()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function can_thread_owned() As Boolean Implements ilock.can_thread_owned
        Return l.can_thread_owned()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function can_cross_thread() As Boolean Implements ilock.can_cross_thread
        Return l.can_cross_thread()
    End Function
End Structure
