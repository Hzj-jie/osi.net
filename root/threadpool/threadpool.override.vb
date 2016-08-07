
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class threadpool
    Public MustOverride Property thread_count As UInt32 Implements ithreadpool.thread_count

    Protected MustOverride Function managed_threads() As Thread()
    Protected MustOverride Sub queue_job(ByVal wi As work_info)

    Protected Overridable Sub before_stop()
    End Sub

    Protected Overridable Sub after_stop()
    End Sub

    Public Overridable Function execute_job() As Boolean Implements ithreadpool.execute_job
        Return False
    End Function

    Public Overridable Function wait_job(Optional ByVal ms As Int64 = npos) As Boolean Implements ithreadpool.wait_job
        sleep(ms)
        Return False
    End Function
End Class
