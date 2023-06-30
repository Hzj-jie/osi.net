
Imports System.Threading
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.delegates
Imports osi.root.threadpool
Imports osi.root.lock
Imports osi.root.envs
Imports osi.root.connector

Partial Public Class callback_manager
    Friend Sub trigger_check()
        queue_runner.trigger()
    End Sub

    Private Sub begin_check(ByVal action As callback_action)
        assert(Not action Is Nothing)
        assert(queue_runner.push(AddressOf action.action_check))
    End Sub
End Class
