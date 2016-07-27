
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class callback_action
    Private Shared Function finished_action(ByVal end_result As Boolean) As callback_action
        Dim action As callback_action = Nothing
        action = New callback_action()
        action.begin_ticks() = nowadays.ticks()
        action.check_ticks() = nowadays.ticks()
        action.end_ticks() = nowadays.ticks()
        action.finish_ticks() = nowadays.ticks()
        action.to_finished()
        action.begin_result() = ternary.true
        action.check_result() = ternary.true
        action.end_result() = end_result
        Return action
    End Function

    Public Shared Function finished_success() As callback_action
        Return finished_action(True)
    End Function

    Public Shared Function finished_failed() As callback_action
        Return finished_action(False)
    End Function
End Class
