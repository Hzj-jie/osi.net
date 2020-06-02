
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public NotInheritable Class threadpool
    Public Shared ReadOnly default_thread_count As UInt32

    Shared Sub New()
        default_thread_count = If(envs.thread_count <> npos,
                                  CUInt(envs.thread_count),
                                  CUInt(max(If(envs.busy_wait,
                                               Environment.ProcessorCount() - CInt(queue_runner.thread_count),
                                               Environment.ProcessorCount()),
                                            1)))
    End Sub

    Public Shared Function in_restricted_threadpool_thread() As Boolean
        Return qless_threadpool2.in_managed_thread() OrElse
               slimqless2_threadpool2.in_managed_thread() OrElse
               slimheapless_threadpool2.in_managed_thread()
    End Function

    Private Sub New()
    End Sub
End Class
