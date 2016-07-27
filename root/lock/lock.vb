
Imports System.DateTime
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock.slimlock

Public Structure lock(Of T As {Structure, islimlock})
    Implements ilock
    Private ownerTID As Int32
    Private l As T

    Shared Sub New()
        assert(INVALID_THREAD_ID = DirectCast(Nothing, Int32))
        Dim l As T
        assert(l.can_thread_owned())
        assert(Not TypeOf l Is ilock)
    End Sub

    Public Function held_in_thread() As Boolean Implements ilock.held_in_thread
        Return ownerTID = current_thread_id()
    End Function

    Public Function held() As Boolean Implements ilock.held
        Return ownerTID <> INVALID_THREAD_ID
    End Function

    Private Sub debug_wait()
        Dim n As Int64 = 0
        n = Now().milliseconds()
        l.wait()
        If Now().milliseconds() - n > half_timeslice_length_ms Then
            raise_error(error_type.performance,
                          callstack(),
                          " is using ",
                          Now().milliseconds() - n,
                          "ms to wait for another thread to finish")
        End If
    End Sub

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
        assert(ownerTID = INVALID_THREAD_ID)
        ownerTID = current_thread_id()
    End Sub

    Public Sub release() Implements ilock.release
        assert(can_cross_thread() OrElse ownerTID = current_thread_id())
        ownerTID = INVALID_THREAD_ID
        l.release()
    End Sub

    Public Function can_thread_owned() As Boolean Implements ilock.can_thread_owned
        Return l.can_thread_owned()
    End Function

    Public Function can_cross_thread() As Boolean Implements ilock.can_cross_thread
        Return l.can_cross_thread()
    End Function
End Structure
