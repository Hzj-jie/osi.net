
#Const USE_COUNT_RESET_EVENT = False
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.event
Imports osi.root.lock
Imports QUEUE_TYPE = osi.root.formation.slimheapless(Of System.Action)

' A not-inheritable, no virtual functions threadpool with compiling time switches only.
Public NotInheritable Class fast_threadpool
    <ThreadStatic> Private Shared managed_thread As Boolean
    Private ReadOnly ts() As Thread
    Private ReadOnly q As QUEUE_TYPE
#If USE_COUNT_RESET_EVENT Then
    Private ReadOnly e As count_reset_event(Of threadpool._default_thread_count)
#Else
    Private ReadOnly e As AutoResetEvent
#End If
    Private s As singleentry

    Public Shared Function in_managed_thread() As Boolean
        Return managed_thread
    End Function

    Public Sub New()
        q = New QUEUE_TYPE()
#If USE_COUNT_RESET_EVENT Then
        _new(e)
#Else
        e = New AutoResetEvent(False)
#End If
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
#If USE_COUNT_RESET_EVENT Then
        e.set()
#Else
        e.force_set()
#End If
    End Sub

    Public Function thread_count() As UInt32
        Return threadpool.default_thread_count
    End Function

    Public Function stopping() As Boolean
        Return s.in_use()
    End Function

    Public Function [stop](Optional ByVal stop_wait_seconds As Int64 = constants.default_stop_threadpool_wait_seconds) _
                          As Boolean
        If object_compare(Me, newable_global_instance(Of fast_threadpool).ref()) <> 0 AndAlso
           s.mark_in_use() Then
            If stop_wait_seconds < 0 Then
                stop_wait_seconds = constants.default_stop_threadpool_wait_seconds
            End If
            Dim until_ms As Int64 = int64_0
            until_ms = nowadays.milliseconds() + seconds_to_milliseconds(stop_wait_seconds)
            Dim a As Action = Nothing
            a = Sub()
                    For i As UInt32 = uint32_0 To array_size(ts) - uint32_1
                        ts(i).Abort()
                        Dim wait_ms As Int64 = 0
                        wait_ms = until_ms - nowadays.milliseconds()
                        If wait_ms > 0 Then
                            ts(i).Join(wait_ms)
                        End If
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
        While Not stopping()
            If Not execute_job() Then
                ' wait_job should only fail when thread is aborting.
                assert(wait_job() OrElse stopping())
            End If
        End While
        managed_thread = False
    End Sub

    Public Shared Operator +(ByVal this As fast_threadpool, ByVal that As Action) As fast_threadpool
        assert(Not this Is Nothing)
        this.queue_job(that)
        Return this
    End Operator

    Protected Overrides Sub Finalize()
        [stop](0)
#If Not USE_COUNT_RESET_EVENT Then
        e.Close()
#End If
        MyBase.Finalize()
    End Sub
End Class
