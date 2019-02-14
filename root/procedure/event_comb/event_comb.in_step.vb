
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class event_comb
    Public Function not_started() As Boolean
        Return debug_reenterable_locked(Function() As Boolean
                                            Return in_step(not_started_step)
                                        End Function)
    End Function

    Public Function started() As Boolean
        Return Not not_started()
    End Function

    Public Function working() As Boolean
        Return debug_reenterable_locked(Function() As Boolean
                                            Return assert_step_is_valid() AndAlso
                                                   valid_working_step([step])
                                        End Function)
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
               assert(end_result_raw().notunknown()) AndAlso
               assert(callback_resume_ready())
    End Function

    Public Function callback_resume_ready() As Boolean
        Return debug_reenterable_locked(AddressOf _callback_resume_ready)
    End Function
End Class
