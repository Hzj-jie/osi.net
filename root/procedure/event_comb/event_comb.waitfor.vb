
Imports System.Timers
Imports System.Threading
Imports osi.root.event
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.delegates
Imports osi.root.lock.slimlock
Imports osi.root.formation

Partial Public Class event_comb
    Public Shared Function waitfor(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(ec, timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal ec As event_comb) As Boolean
        Return current()._waitfor(ec)
    End Function

    Public Shared Function waitfor_or_null(ByVal ec As event_comb) As Boolean
        Return current()._waitfor_or_null(ec)
    End Function

    Public Shared Function waitfor(ByVal ParamArray ecs() As event_comb) As Boolean
        Return current()._waitfor(ecs)
    End Function

    Public Shared Function waitfor_or_null(ByVal ParamArray ecs() As event_comb) As Boolean
        Return current()._waitfor_or_null(ecs)
    End Function

    Public Shared Function waitone(ByVal ParamArray ecs() As event_comb) As Boolean
        Return current()._waitone(ecs)
    End Function

    Public Shared Function waitfor(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(action, timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal action As callback_action) As Boolean
        Return current()._waitfor(action)
    End Function

    Public Shared Function waitfor(ByVal [try] As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor([try], timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal [try] As Func(Of Boolean),
                                   ByVal try_result As pointer(Of Boolean),
                                   ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor([try], try_result, timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal [try] As Func(Of Boolean)) As Boolean
        Return current()._waitfor([try])
    End Function

    Public Shared Function waitfor(ByVal d As Action) As Boolean
        Return current()._waitfor(d)
    End Function

    Public Shared Function waitfor(ByVal d As Action, ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(d, timeout_ms)
    End Function

    Public Shared Function waitfor(Of T)(ByVal d As Func(Of T), ByVal r As pointer(Of T)) As Boolean
        Return current()._waitfor(d, r)
    End Function

    Public Shared Function waitfor(Of T)(ByVal d As Func(Of T),
                                         ByVal r As pointer(Of T),
                                         ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(d, r, timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal e As WaitHandle, ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(e, timeout_ms)
    End Function

    Public Shared Function waitfor(ByVal e As WaitHandle) As Boolean
        Return current()._waitfor(e)
    End Function

    Public Shared Function waitfor(ByVal ms As Int64) As Boolean
        Return current()._waitfor(ms)
    End Function

    Public Shared Function waitfor(ByVal l As ref(Of event_comb_lock)) As Boolean
        Return current()._waitfor(l)
    End Function

    Public Shared Function waitfor(ByVal i As ref(Of singleentry)) As Boolean
        Return current()._waitfor(i)
    End Function

    Public Shared Function waitfor(ByVal i As ref(Of singleentry), ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(i, timeout_ms)
    End Function

    Public Shared Function waitfor(Of T)(ByVal l As multilock(Of event_comb_lock), ByVal i As T) As Boolean
        Return current()._waitfor(l, i)
    End Function

    Public Shared Function waitfor_nap() As Boolean
        Return current()._waitfor_nap()
    End Function

    Public Shared Function waitfor_yield() As Boolean
        Return current()._waitfor_yield()
    End Function

    Public Shared Function waitfor(ByVal i As attachable_event) As Boolean
        Return current()._waitfor(i)
    End Function

    Public Shared Function waitfor(ByVal i As attachable_event, ByVal timeout_ms As Int64) As Boolean
        Return current()._waitfor(i, timeout_ms)
    End Function

    'do not see if any reason we should export this function, friend for event_comb_lock only
    Friend Shared Function wait() As Action
        Return current()._wait()
    End Function
End Class
