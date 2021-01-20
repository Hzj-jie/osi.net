
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants
Imports osi.root.delegates

Public Module _threadpool
    ' DISABLED <global_init(global_init_level.foundamental)>
    Private NotInheritable Class min_thread_count
        Private Shared ReadOnly run_shared_sub_new As cctor_delegator = New cctor_delegator(
            Sub()
                assert(ThreadPool.SetMinThreads(Environment.ProcessorCount() >> 1, Environment.ProcessorCount() >> 1))
            End Sub)

        Private Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub queue_in_managed_threadpool(ByVal d As WaitCallback)
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        d(Nothing)
                                    End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub queue_in_managed_threadpool(ByVal d As WaitCallback, ByVal o As Object)
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        d(o)
                                    End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub queue_in_managed_threadpool(ByVal d As Action)
        assert(Not d Is Nothing)
        assert(ThreadPool.QueueUserWorkItem(Sub()
                                                void_(d)
                                            End Sub))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub queue_in_managed_threadpool(Of T)(ByVal d As void(Of T), ByVal i As T)
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        d(i)
                                    End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub queue_in_managed_threadpool(Of T)(ByVal d As Action(Of T), ByVal i As T)
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        d(i)
                                    End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function start_thread(ByRef t As Thread, ByVal v As ThreadStart) As Boolean
        If v Is Nothing Then
            Return False
        End If
        t = New Thread(v)
        t.Start()
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function start_thread(ByVal v As ThreadStart) As Thread
        Dim x As Thread = Nothing
        assert(start_thread(x, v))
        Return x
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function start_thread(ByRef t As Thread,
                                               ByVal v As ParameterizedThreadStart,
                                               ByVal p As Object) As Boolean
        If v Is Nothing Then
            Return False
        End If
        t = New Thread(v)
        t.Start(p)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function start_thread(ByVal v As ParameterizedThreadStart, ByVal p As Object) As Thread
        Dim t As Thread = Nothing
        assert(start_thread(t, v, p))
        Return t
    End Function
End Module
