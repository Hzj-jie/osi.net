
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Module _thread_count
    Public ReadOnly thread_count As Int32 = calculate_thread_count()
    Public ReadOnly queue_runner_thread_count As Int32 = calculate_queue_runner_thread_count()

    Private Function calculate_thread_count() As Int32
        Dim thread_count As Int32
        If Not env_value(env_keys("thread", "count"), thread_count) OrElse
           thread_count <= 0 OrElse
           thread_count > Environment.ProcessorCount() Then
            thread_count = npos
        End If
        Return thread_count
    End Function

    Private Function calculate_queue_runner_thread_count() As Int32
        Dim queue_runner_thread_count As Int32
        If Not env_value(env_keys("queue", "runner", "thread", "count"),
                         queue_runner_thread_count) OrElse
           queue_runner_thread_count <= 0 OrElse
           queue_runner_thread_count > Environment.ProcessorCount() Then
            queue_runner_thread_count = npos
        End If
        Return queue_runner_thread_count
    End Function
End Module
