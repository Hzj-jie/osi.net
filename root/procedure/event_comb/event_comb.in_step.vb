
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class event_comb
    Public Function not_started() As Boolean
        Return debug_reenterable_locked(AddressOf _not_started)
    End Function

    Public Function started() As Boolean
        Return Not not_started()
    End Function

    Public Function working() As Boolean
        Return debug_reenterable_locked(AddressOf _working)
    End Function

    Public Function ending() As Boolean
        Return debug_reenterable_locked(AddressOf _ending)
    End Function

    Public Function [end]() As Boolean
        Return debug_reenterable_locked(AddressOf _end)
    End Function

    Public Function callback_resume_ready() As Boolean
        Return debug_reenterable_locked(AddressOf _callback_resume_ready)
    End Function
End Class
