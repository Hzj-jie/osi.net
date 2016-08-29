
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.event
Imports osi.root.lock
Imports QUEUE_TYPE = osi.root.formation.heapless(Of System.Action)

Public NotInheritable Class fast_threadpool_instance
    Public Shared ReadOnly g As fast_threadpool

    Shared Sub New()
        g = New fast_threadpool()
    End Sub

    Private Sub New()
    End Sub
End Class

' A not-inheritable, no virtual functions threadpool with compiling time switches only.
Public NotInheritable Class fast_threadpool
    <ThreadStatic> Private Shared managed_thread As Boolean
    Private ReadOnly ts() As Thread
    Private ReadOnly q As QUEUE_TYPE
    Private ReadOnly e As count_reset_event(Of threadpool._default_thread_count)
    Private s As singleentry

    Public Shared Function in_managed_thread() As Boolean
        Return managed_thread
    End Function

    Public Sub New()
        q = New QUEUE_TYPE()
        _new(e)
        ReDim ts(threadpool.default_thread_count - uint32_1)
        For i As UInt32 = uint32_0 To threadpool.default_thread_count - uint32_1
            ts(i) = New Thread(AddressOf worker)
            ts(i).Name() = "FAST_THREADPOOL_WORKTHREAD"
            ts(i).Start()
        Next
    End Sub

    Public Sub queue_job(ByVal work As Action)
        assert(Not work Is Nothing)
        If stopping() Then
            If threadpool_trace Then
                raise_error("should not queue_job after stop, ", callstack())
            End If
            Return
        End If

        q.emplace(work)
        e.set()
    End Sub

    Public Function thread_count() As UInt32
        Return threadpool.default_thread_count
    End Function

    Public Function stopping() As Boolean
        Return s.in_use()
    End Function

    Public Function [stop](Optional ByVal stop_wait_seconds As Int64 = constants.default_stop_threadpool_wait_seconds) _
                          As Boolean
        If object_compare(Me, fast_threadpool_instance.g) <> 0 AndAlso s.mark_in_use() Then
            Dim until_ms As Int64 = int64_0
            until_ms = nowadays.milliseconds() + seconds_to_milliseconds(stop_wait_seconds)
            Dim a As Action = Nothing
            a = Sub()
                    For i As UInt32 = uint32_0 To array_size(ts) - uint32_1
                        ts(i).Abort()
                        ts(i).Join(until_ms - nowadays.milliseconds())
                    Next
                End Sub
            If in_managed_thread() Then
                queue_in_managed_threadpool(a)
            Else
                a()
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Function idle() As Boolean
        Return q.empty()
    End Function

    Public Function wait_job(Optional ByVal ms As Int64 = npos) As Boolean
        Return e.wait(ms)
    End Function

    Public Function execute_job() As Boolean
        Dim w As Action = Nothing
        If q.pop(w) Then
            assert(Not w Is Nothing)
            Try
                w()
            Catch ex As Exception
                log_unhandled_exception(ex)
            End Try
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub worker()
        managed_thread = True
        While s.not_in_use()
            If Not execute_job() Then
                assert(wait_job())
            End If
        End While
        managed_thread = False
    End Sub

    Public Shared Operator +(ByVal this As fast_threadpool, ByVal that As Action) As fast_threadpool
        assert(Not this Is Nothing)
        this.queue_job(that)
        Return this
    End Operator
End Class
