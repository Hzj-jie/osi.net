
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock

Partial Public Class event_comb
    Private Sub debug_reenterable_locked(ByVal f As Action)
#If DEBUG Then
        reenterable_locked(f)
#Else
        f()
#End If
    End Sub

    Private Function debug_reenterable_locked(ByVal f As Func(Of Boolean)) As Boolean
#If DEBUG Then
        Return reenterable_locked(f)
#Else
        Return f()
#End If
    End Function

    Private Sub assert_in_lock()
#If DEBUG Then
        assert(_l.held_in_thread())
#End If
    End Sub

    Private Sub reenterable_locked(ByVal d As Action)
        assert(Not d Is Nothing)
        If lock_trace AndAlso event_comb_trace Then
            Dim n As Int64 = 0
            n = Now().milliseconds()
            _l.reenterable_locked(d)
            If Now().milliseconds() - n > half_timeslice_length_ms Then
                raise_error(error_type.performance,
                            callstack(), ":", [step],
                            " is using ", Now().milliseconds() - n, "ms to wait for another thread to finish")
            End If
        Else
            _l.reenterable_locked(d)
        End If
    End Sub

    Private Function reenterable_locked(ByVal f As Func(Of Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim r As Boolean = False
        reenterable_locked(Sub()
                               r = f()
                           End Sub)
        Return r
    End Function
End Class
