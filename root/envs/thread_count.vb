
Imports osi.root.constants

Public Module _thread_count
    Public ReadOnly thread_count As Int32
    Public ReadOnly queue_runner_thread_count As Int32

    Sub New()
        If Not env_value(env_keys("thread", "count"), thread_count) OrElse
           thread_count <= 0 OrElse
           thread_count > Environment.ProcessorCount() Then
            thread_count = npos
        End If

        If Not env_value(env_keys("queue", "runner", "thread", "count"),
                         queue_runner_thread_count) OrElse
           queue_runner_thread_count <= 0 OrElse
           queue_runner_thread_count > Environment.ProcessorCount() Then
            queue_runner_thread_count = npos
        End If
    End Sub
End Module
