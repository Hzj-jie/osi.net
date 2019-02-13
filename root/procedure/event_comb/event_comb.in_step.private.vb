
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class event_comb
    Private Function valid_working_step(ByVal i As Int32) As Boolean
        Return i < ds_len AndAlso i >= 0
    End Function

    Private Function valid_step(ByVal i As Int32) As Boolean
        Return valid_working_step(i) OrElse i = end_step OrElse i = not_started_step
    End Function

    Private Function assert_step_is_valid() As Boolean
        assert_in_lock()
        Return assert(valid_step([step]), "event ", callstack(), ":", [step])
    End Function

    Private Function debug_assert_valid_step(ByVal i As Int32) As Boolean
        Return debug_assert(valid_working_step(i) OrElse i = end_step OrElse i = not_started_step)
    End Function

    Private Function in_step(ByVal exp As Int32) As Boolean
        assert_in_lock()
        Return assert_step_is_valid() AndAlso ([step] = exp)
    End Function

    'the step is in end_step, but may still have pendings
    Private Function in_end_step() As Boolean
        Return in_step(end_step)
    End Function

    'the callback resume process is ready to be started, but it may or may not happen
    Private Function _callback_resume_ready() As Boolean
        Return in_end_step() AndAlso not_pending()
    End Function
End Class
