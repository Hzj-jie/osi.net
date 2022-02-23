
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants

' Provides similar APIs as osi.root.threadpool.thread_pool and error protections, but delegates the works to the
' ManagedThreadPool in .Net.
Public NotInheritable Class managed_thread_pool
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub push(ByVal v As Action)
        assert(v IsNot Nothing)
        assert(ThreadPool.UnsafeQueueUserWorkItem(Sub(ByVal state As Object)
                                                      v()
                                                  End Sub, Nothing))
    End Sub

    Public Shared Sub with_timeout(ByVal d As Action, ByVal timeout_ms As Int64)
        assert(d IsNot Nothing)
        If timeout_ms < 0 OrElse timeout_ms > max_int32 Then
            push(d)
            Return
        End If
        Using are As AutoResetEvent = New AutoResetEvent(False)
            Dim t As Thread = Nothing
            push(Sub()
                     Try
                         t = Thread.CurrentThread()
                         d()
                         assert(are.Set())
                     Catch ex As ThreadAbortException
                         Thread.ResetAbort()
                     End Try
                 End Sub)
            If are.WaitOne(CInt(timeout_ms)) Then
                Return
            End If
            sleep_wait_when(Function() As Boolean
                                Return t Is Nothing
                            End Function,
                            1)
            assert(t.ManagedThreadId() <> Thread.CurrentThread().ManagedThreadId())
            t.Abort()
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
