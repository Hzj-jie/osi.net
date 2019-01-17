
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class event_comb
    Private Function _goto(ByVal [step] As Int32) As Boolean
        assert_in_lock()
        If Not debug_assert_valid_step([step]) Then
            Return False
        End If
        Me.step = [step]
        Return True
    End Function

    Private Sub assert_goto(ByVal [step] As Int32)
        assert(_goto([step]))
    End Sub

    Private Function _goto_end() As Boolean
        If working() Then
            trace_finish()
        End If
        Return _goto(end_step)
    End Function

    Private Sub assert_goto_end()
        assert(_goto_end())
    End Sub

    Private Function _goto_not_started() As Boolean
        Return _goto(not_started_step)
    End Function

    Private Sub assert_goto_not_started()
        assert(_goto_not_started())
    End Sub

    Private Function _goto_begin() As Boolean
        If not_started() Then
            trace_start()
        End If
        Return _goto(first_step)
    End Function

    Private Sub assert_goto_begin()
        assert(_goto_begin())
    End Sub

    Private Function _goto_prev() As Boolean
        assert_in_lock()
        Return _goto([step] - 1)
    End Function

    Private Function _goto_next() As Boolean
        assert_in_lock()
        Return _goto([step] + 1)
    End Function

    Private Function _goto_last() As Boolean
        Return _goto(CInt(ds_len - uint32_1))
    End Function

    Public Shared Function [goto](ByVal [step] As Int32) As Boolean
        Return current()._goto([step])
    End Function

    Public Shared Function goback() As Boolean
        Return goto_prev()
    End Function

    Public Shared Function goto_prev() As Boolean
        Return current()._goto_prev()
    End Function

    Public Shared Function goto_end() As Boolean
        Return current()._goto_end()
    End Function

    Public Shared Function goto_next() As Boolean
        Return current()._goto_next()
    End Function

    Public Shared Function goto_last() As Boolean
        Return current()._goto_last()
    End Function

    Public Shared Function goto_begin() As Boolean
        Return current()._goto_begin()
    End Function
End Class
