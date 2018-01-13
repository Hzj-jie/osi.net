
Option Explicit On
Option Infer Off
Option Strict On

#If DEPRECATED Then
Imports System.ComponentModel
#End If
Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.foundamental)>
Public NotInheritable Class queue_runner_synchronize_invoke
    Inherits synchronize_invoke

    Public Shared ReadOnly instance As queue_runner_synchronize_invoke

    Shared Sub New()
        instance = New queue_runner_synchronize_invoke()
#If DEPRECATED Then
        implementation_of(Of ISynchronizeInvoke).register(instance)
#End If
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub push(ByVal v As Action)
        assert(queue_runner.once(v))
    End Sub

    Protected Overrides Function synchronously() As Boolean
        Return queue_runner.running_in_current_thread()
    End Function

    Private Shared Sub init()
    End Sub
End Class
