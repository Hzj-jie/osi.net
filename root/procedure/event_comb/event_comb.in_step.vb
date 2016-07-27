
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.delegates

Partial Public Class event_comb
    Private Function valid_working_step(ByVal i As Int32) As Boolean
        Return i < ds_len AndAlso i >= 0
    End Function

    Private Function valid_step(ByVal i As Int32) As Boolean
        Return valid_working_step(i) OrElse i = end_step OrElse i = not_started_step
    End Function

    Private Function in_step(ByVal exp As Int32) As Boolean
        Return assert(valid_step([step])) AndAlso ([step] = exp)
    End Function

    'the step is in end_step, but may still have pendings
    Private Function in_end_step() As Boolean
        Return in_step(end_step)
    End Function

    'the callback resume process is ready to be started, but it may or may not happen
    Private Function callback_resume_ready() As Boolean
        Return in_end_step() AndAlso not_pending()
    End Function

    Public Function not_started() As Boolean
        Return in_step(not_started_step)
    End Function

    Public Function working() As Boolean
        Return assert(valid_step([step])) AndAlso valid_working_step([step])
    End Function

    'no pendings, but the resume process has not been started
    Public Function ending() As Boolean
        Return callback_resume_ready() AndAlso Not [end]()
    End Function

    'no pendings, the resume process has been finished,
    'since the resume will happen right after end_ticks has been set
    'if not being called within the lock, which usually happens in another sync thread,
    'do not guarantee the chained event_comb has been resumed already.
    'but for the event_comb waiting for this event_comb, it's surely safe.
    Public Function [end]() As Boolean
        Return end_ticks() <> npos AndAlso
               assert(callback_resume_ready()) AndAlso
               assert(end_result_raw().notunknown())
    End Function
End Class
