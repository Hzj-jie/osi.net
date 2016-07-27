
Imports System.Threading
Imports osi.root.utils
Imports osi.root.threadpool
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class callback_manager
    Public Shared ReadOnly [global] As callback_manager = Nothing

    Shared Sub New()
        [global] = New callback_manager()
    End Sub

    Public Shared Function current_callback_action() As callback_action
        Return callback_action.current()
    End Function

    Private Sub action_end(ByVal action As callback_action)
        assert(Not action Is Nothing)
        action.action_end()
        assert(action.finished())
        trigger_check()
    End Sub

    Private Sub work(ByVal action As callback_action)
        If action.begining() Then
            action.action_begin()
            If action.pending() Then
                begin_check(action)
            ElseIf action.ending() Then
                'only here when begin failed
                action_end(action)
            Else
                assert(False, "should cover all situations.")
            End If
        ElseIf action.ending() Then
            action_end(action)
        ElseIf action.finished() Then
            'insert a predefined finished callbackAction will trigger a check,
            'since in some cases, no further action is needed, such as readstream3
            trigger_check()
        Else
            assert(False, "should cover all situations.")
        End If
    End Sub

    Private Shared Function action_not_valid(ByVal action As callback_action) As Boolean
        Return action Is Nothing OrElse action.pending()
    End Function

    Private Shared Function current_pending_timeout() As Boolean
        Return callback_action.in_callback_action_thread() AndAlso
               callback_action.current().pending() AndAlso
               callback_action.current().timeout()
    End Function

    Public Function begin(ByVal action As callback_action) As Boolean
        If action_not_valid(action) OrElse
           current_pending_timeout() Then
            Return False
        Else
            thread_pool().queue_job(Sub()
                                        work(action)
                                    End Sub)
            Return True
        End If
    End Function

    Public Function begin(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        If begin(action) Then
            assert(Not action Is Nothing)
            If timeout_ms >= 0 Then
                action.set_timeout_ms(timeout_ms)
            End If
            Return True
        Else
            Return False
        End If
    End Function
End Class
