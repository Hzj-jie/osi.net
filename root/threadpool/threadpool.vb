
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports counter = osi.root.utils.counter

Partial Public MustInherit Class threadpool
    Implements ithreadpool

    Public Shared ReadOnly default_thread_count As UInt32

    Public Class _default_thread_count
        Inherits _int64

        Protected Overrides Function at() As Int64
            Return default_thread_count
        End Function
    End Class

    Protected Class work_info
        Public ReadOnly ctor_ticks As Int64 = 0
        Public ReadOnly work As Action = Nothing

        Public Sub New(ByVal work As Action)
            assert(Not work Is Nothing)
            Me.work = work
            If threadpool_trace Then
                Me.ctor_ticks = nowadays.ticks()
            End If
        End Sub
    End Class

    Shared Sub New()
        assert((envs.thread_count > 0 AndAlso
                envs.thread_count <= Environment.ProcessorCount()) OrElse
               envs.thread_count = npos)
        default_thread_count = If(envs.thread_count <> npos,
                                  CUInt(envs.thread_count),
                                  CUInt(max(If(busy_wait,
                                               Environment.ProcessorCount() - CInt(queue_runner.thread_count),
                                               Environment.ProcessorCount()),
                                            1)))
    End Sub

    Protected Sub New()
        TYPE_NAME = Me.GetType().Name()
        If threadpool_trace Then
            THREADPOOL_WORK_INFO_INQUEUE_TICKS =
                counter.register_average_and_last_average(strcat(TYPE_NAME, "_WORK_INFO_INQUEUE_TICKS"))
            THREADPOOL_QUEUE_LENGTH =
                counter.register_average_and_last_average(strcat(TYPE_NAME, "_QUEUE_LENGTH"))
            THREADPOOL_WORKING_THREAD =
                counter.register_average_and_last_average(strcat(TYPE_NAME, "_WORKING_THREAD"))
            If threadpool_trace Then
                THREADPOOL_WAIT_TICKS = counter.register_average_and_last_average(strcat(TYPE_NAME, "_WAIT_TICKS"))
                THREADPOOL_IDLE_ROUNDS = counter.register_rate_and_last_rate(strcat(TYPE_NAME, "_IDLE_ROUNDS"))
            End If
        End If
        application_lifetime_binder.instance.insert(New disposer(Of threadpool)(Me,
                                                                                disposer:=Sub(x) x.stop()))
    End Sub

    Protected Sub work_on(ByVal wi As work_info)
        assert(Not wi Is Nothing)
        assert(Not wi.work Is Nothing)
        ' increment wt before decrement ql to make sure idle() can correctly the status.
        start_work_info(wi)
        pop_queue()
        void_(wi.work)
        finish_work_info()
    End Sub

    Protected Sub worker()
        While Not stopping()
            If Not execute_job() Then
                If threadpool_trace Then
                    Dim startticks As Int64 = 0
                    startticks = nowadays.high_res_ticks()
                    assert(wait_job())
                    counter.record_time_ticks(THREADPOOL_WAIT_TICKS, startticks)
                    counter.increase(THREADPOOL_IDLE_ROUNDS)
                Else
                    assert(wait_job())
                End If
            End If
        End While
    End Sub

    Private Shared Sub stop_thread(ByVal thread As Thread, ByRef stop_wait_seconds As Int64)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Try
            thread.Abort()
            If stop_wait_seconds > 0 Then
                thread.Join(TimeSpan.FromSeconds(stop_wait_seconds))
            End If
        Catch
        Finally
            stop_wait_seconds -= nowadays.milliseconds() - start_ms
        End Try
    End Sub

    Private Sub stop_threads(ByVal threads() As Thread,
                             ByVal stop_wait_seconds As Int64)
        If stop_wait_seconds < 0 Then
            stop_wait_seconds = constants.default_stop_threadpool_wait_seconds
        End If
        raise_error("threadpool starts to stop, running before step.")
        before_stop()
        For i As Int32 = 0 To array_size_i(threads) - 1
#If Not (PocketPC OrElse Smartphone) Then
            If Not threads(i) Is Nothing AndAlso threads(i).IsAlive() Then
                stop_thread(threads(i), stop_wait_seconds)
            End If
#Else
                If Not threads(i) Is Nothing Then
                    stop_thread(threads(i), stop_wait_seconds)
                End If
#End If
        Next
        after_stop()
        raise_error("threadpool finished stopping.")
    End Sub

    Protected Overrides Sub Finalize()
        [stop](0)
        MyBase.Finalize()
    End Sub

    Protected Shared Sub register(Of T As ithreadpool)(ByVal i As T)
        assert(Not i Is Nothing)
        Dim j As ithreadpool = Nothing
        If global_resolver.resolve(j) Then
            assert(Not j Is Nothing)
        Else
            assert(j Is Nothing)
        End If
        If j Is Nothing OrElse
           object_compare(i, j) = object_compare_undetermined Then
            global_resolver(Of ithreadpool).register(i)
            If Not j Is Nothing Then
                j.stop()
            End If
        End If
    End Sub
End Class
