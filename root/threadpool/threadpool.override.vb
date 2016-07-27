
Imports System.Threading

Partial Public Class threadpool
    Public MustOverride Property thread_count As UInt32 Implements ithreadpool.thread_count

    Protected MustOverride Function managed_threads() As Thread()
    Protected MustOverride Sub queue_job(ByVal wi As work_info)

    Protected Overridable Sub before_stop()
    End Sub

    Protected Overridable Sub after_stop()
    End Sub
End Class
