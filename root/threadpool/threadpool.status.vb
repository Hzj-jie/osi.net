
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.envs
Imports counter = osi.root.utils.counter

Partial Public Class threadpool
    Private ReadOnly THREADPOOL_WORK_INFO_INQUEUE_TICKS As Int64 = 0
    Private ReadOnly THREADPOOL_QUEUE_LENGTH As Int64 = 0
    Private ReadOnly THREADPOOL_WORKING_THREAD As Int64 = 0
    Private ReadOnly TYPE_NAME As String = Nothing
    Private ql As Int32 = 0
    Private wt As Int32 = 0
    Private stop_signal As singleentry

    Public Function stopping() As Boolean Implements ithreadpool.stopping
        Return stop_signal.in_use()
    End Function

    Public Function idle() As Boolean Implements ithreadpool.idle
        Return queue_length() = uint32_0 AndAlso working_threads() = uint32_0
    End Function

    Private Function set_stopping() As Boolean
        Return stop_signal.mark_in_use()
    End Function

    Protected Sub reset_stop_signal()
        stop_signal.mark_not_in_use()
    End Sub

    Protected Function queue_length() As UInt32
        assert(ql >= 0)
        Return CUInt(ql)
    End Function

    Protected Function working_threads() As UInt32
        assert(wt >= 0)
        Return CUInt(wt)
    End Function

    Private Sub start_work_info(ByVal wi As work_info)
        assert(Not wi Is Nothing)
        If threadpool_trace Then
            counter.record_time_ticks(THREADPOOL_WORK_INFO_INQUEUE_TICKS, wi.ctor_ticks)
            counter.increase(THREADPOOL_WORKING_THREAD, Interlocked.Increment(wt))
        Else
            Interlocked.Increment(wt)
        End If
    End Sub

    Private Sub finish_work_info()
        If threadpool_trace Then
            counter.increase(THREADPOOL_WORKING_THREAD, Interlocked.Decrement(wt))
        Else
            Interlocked.Decrement(wt)
        End If
    End Sub

    Private Sub push_queue()
        If threadpool_trace Then
            counter.increase(THREADPOOL_QUEUE_LENGTH, Interlocked.Increment(ql))
        Else
            Interlocked.Increment(ql)
        End If
    End Sub

    Private Sub pop_queue()
        If threadpool_trace Then
            counter.increase(THREADPOOL_QUEUE_LENGTH, Interlocked.Decrement(ql))
        Else
            Interlocked.Decrement(ql)
        End If
    End Sub
End Class
