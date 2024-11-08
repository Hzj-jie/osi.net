
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock

<global_init(global_init_level.threading_and_procedure)>
Partial Public NotInheritable Class queue_runner
    Public Shared ReadOnly thread_count As UInt32 =
        Function() As UInt32
            assert((queue_runner_thread_count > 0 AndAlso
                    queue_runner_thread_count <= Environment.ProcessorCount()) OrElse
                   queue_runner_thread_count = npos)
            Return CUInt(If(queue_runner_thread_count <> npos,
                            queue_runner_thread_count,
                            max(1, Environment.ProcessorCount() >> 4)))
        End Function()
    Private Shared ReadOnly LENGTH As Int64 = counter.register_average_and_last_average("QUEUE_RUNNER_LENGTH")
    Private Shared ReadOnly INTERVAL As Int64 = counter.register_average_and_last_average("QUEUE_RUNNER_INTERVAL_TICKS")
    Private Shared ReadOnly USED As Int64 = counter.register_average_and_last_average("QUEUE_RUNNER_USED_TICKS")
    Private Shared ReadOnly q As New qless2(Of Func(Of Boolean))()
    Private Shared ReadOnly are As AutoResetEvent = If(busy_wait, Nothing, New AutoResetEvent(False))
    <ThreadStatic()> Private Shared current_thread As Boolean

    Private Shared Sub init()
        start()
        application_lifetime.stopping_handle(Sub()
                                                 If Not are Is Nothing Then
                                                     are.Close()
                                                 End If
                                             End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub trigger()
        If Not are Is Nothing Then
            are.Set()
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub check_push(ByVal e As Func(Of Boolean))
        assert(Not e Is Nothing)
        If do_(e, False) Then
            q.emplace(e)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub start()
        For i As UInt32 = 0 To thread_count - uint32_1
            Dim id As UInt32 = i
            managed_thread_pool.push(Sub()
                                         assert(processor_affinity = npos OrElse
                                                (processor_affinity >= 0 AndAlso
                                                 processor_affinity < Environment.ProcessorCount()))
                                         If processor_affinity <> npos Then
                                             loop_set_thread_affinity(CUInt(processor_affinity) + id)
                                         End If
                                         envs._thread.current_thread().Name() = "QUEUE_RUNNER_WORKTHREAD"
                                         current_thread = True
                                         While application_lifetime.running()
                                             Dim size As UInt32 = q.size()
                                             counter.increase(LENGTH, size)
                                             If thread_count > 1 Then
                                                 size = CUInt(Math.Ceiling(size / thread_count))
                                             End If
                                             counter.record_time_begin()
                                             Dim e As Func(Of Boolean) = Nothing
                                             While size > 0 AndAlso q.pop(e)
                                                 check_push(e)
                                                 size -= uint32_1
                                             End While
                                             counter.record_time_ticks(USED)
                                             counter.record_time_begin()
                                             If Not are Is Nothing Then
                                                 yield_wait(are, queue_runner_interval_ms)
                                             End If
                                             counter.record_time_ticks(INTERVAL)
                                         End While
                                     End Sub)
        Next
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function running_in_current_thread() As Boolean
        Return current_thread
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function push(ByVal p As Action(Of Func(Of Boolean)), ByVal d As Func(Of Boolean)) As Boolean
        assert(Not p Is Nothing)
        If d Is Nothing Then
            Return False
        End If
        p(d)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function push(ByVal d As Func(Of Boolean)) As Boolean
        Return push(AddressOf check_push, d)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function push_only(ByVal d As Func(Of Boolean)) As Boolean
        Return push(AddressOf q.emplace, d)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function size() As UInt32
        Return q.size()
    End Function

    Private Sub New()
    End Sub
End Class
