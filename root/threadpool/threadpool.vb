
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public NotInheritable Class threadpool
    Public Shared ReadOnly default_thread_count As UInt32 =
        If(envs.thread_count <> npos,
           CUInt(envs.thread_count),
           CUInt(max(If(envs.busy_wait,
                        Environment.ProcessorCount() - CInt(queue_runner.thread_count),
                        Environment.ProcessorCount()),
                     1)))

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function in_restricted_threadpool_thread() As Boolean
        Return qless_threadpool.in_managed_thread() OrElse
               slimqless2_threadpool.in_managed_thread() OrElse
               slimheapless_threadpool.in_managed_thread()
    End Function

    Private Sub New()
    End Sub
End Class
