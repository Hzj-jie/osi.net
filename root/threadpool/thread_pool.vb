
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public NotInheritable Class thread_pool
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub push(ByVal v As Action)
        assert(v IsNot Nothing)
        assert(ref.q.push(v))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function execute() As Boolean
        Return ref.q.execute()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function wait(ByVal ms As Int64) As Boolean
        Return ref.q.wait(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function thread_count() As UInt32
        Return ref.q.thread_count()
    End Function

    Private NotInheritable Class ref
        Public Shared ReadOnly q As slimqless2_threadpool = create_thread_pool()

        Private Shared Function create_thread_pool() As slimqless2_threadpool
            Dim r As New slimqless2_threadpool()
            application_lifetime.stopping_handle(Sub()
                                                     r.stop()
                                                 End Sub)
            Return r
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
