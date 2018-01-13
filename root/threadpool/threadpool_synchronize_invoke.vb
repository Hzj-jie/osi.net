
#If DEPRECATED Then
Imports System.ComponentModel
#End If
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.threading_and_procedure)>
Public NotInheritable Class threadpool_synchronize_invoke
    Inherits synchronize_invoke

    Public Shared ReadOnly instance As threadpool_synchronize_invoke

    Shared Sub New()
        instance = New threadpool_synchronize_invoke()
#If DEPRECATED Then
        implementation_of(Of ISynchronizeInvoke).register(instance)
#End If
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub push(ByVal v As Action)
        assert(thread_pool().queue_job(v))
    End Sub

    Protected Overrides Function synchronously() As Boolean
        Return in_restricted_threadpool_thread() OrElse
               Thread.CurrentThread().IsThreadPoolThread()
    End Function

    Private Shared Sub init()
    End Sub
End Class
