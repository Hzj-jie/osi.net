
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock

Public Module _waitfor
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(ec, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal ec As event_comb, ByVal timeout_ms As Int64)
        assert(waitfor(ec, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal ec As event_comb) As Boolean
        Return event_comb.waitfor(ec)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor_or_null(ByVal ec As event_comb) As Boolean
        Return event_comb.waitfor_or_null(ec)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal ec As event_comb)
        assert(waitfor(ec))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitone(ByVal ParamArray ecs() As event_comb) As Boolean
        Return event_comb.waitone(ecs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitone(ByVal ParamArray ecs() As event_comb)
        assert(waitone(ecs))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(action, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal action As callback_action, ByVal timeout_ms As Int64)
        assert(waitfor(action, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal action As callback_action) As Boolean
        Return event_comb.waitfor(action)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal action As callback_action)
        assert(waitfor(action))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal ParamArray ecs() As event_comb) As Boolean
        Return event_comb.waitfor(ecs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal ParamArray ecs() As event_comb)
        assert(waitfor(ecs))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor_or_null(ByVal ParamArray ecs() As event_comb) As Boolean
        Return event_comb.waitfor_or_null(ecs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor_or_null(ByVal ParamArray ecs() As event_comb)
        assert(waitfor_or_null(ecs))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal d As Action) As Boolean
        Return event_comb.waitfor(d)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal d As Action)
        assert(waitfor(d))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal d As Action, ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(d, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal d As Action, ByVal timeout_ms As Int64)
        assert(waitfor(d, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(Of T)(ByVal d As Func(Of T), ByVal r As pointer(Of T)) As Boolean
        Return event_comb.waitfor(d, r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(Of T)(ByVal d As Func(Of T), ByVal r As pointer(Of T))
        assert(waitfor(d, r))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(Of T)(ByVal d As Func(Of T),
                                  ByVal r As pointer(Of T),
                                  ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(d, r, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(Of T)(ByVal d As Func(Of T), ByVal r As pointer(Of T), ByVal timeout_ms As Int64)
        assert(waitfor(d, r, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal [try] As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor([try], timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal [try] As Func(Of Boolean), ByVal timeout_ms As Int64)
        assert(waitfor([try], timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal [try] As Func(Of Boolean),
                            ByVal try_result As pointer(Of Boolean),
                            ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor([try], try_result, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal [try] As Func(Of Boolean),
                              ByVal try_result As pointer(Of Boolean),
                              ByVal timeout_ms As Int64)
        assert(waitfor([try], try_result, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal [try] As Func(Of Boolean)) As Boolean
        Return event_comb.waitfor([try])
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal [try] As Func(Of Boolean))
        assert(waitfor([try]))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal ms As Int64) As Boolean
        Return event_comb.waitfor(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal ms As Int64)
        assert(waitfor(ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal l As pointer(Of event_comb_lock)) As Boolean
        Return event_comb.waitfor(l)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal l As pointer(Of event_comb_lock))
        assert(waitfor(l))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal i As pointer(Of singleentry)) As Boolean
        Return event_comb.waitfor(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal i As pointer(Of singleentry))
        assert(waitfor(i))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal i As pointer(Of singleentry), ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(i, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal i As pointer(Of singleentry), ByVal timeout_ms As Int64)
        assert(waitfor(i, timeout_ms))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(Of T)(ByVal l As multilock(Of event_comb_lock), ByVal i As T) As Boolean
        Return event_comb.waitfor(l, i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(Of T)(ByVal l As multilock(Of event_comb_lock), ByVal i As T)
        assert(waitfor(l, i))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal e As WaitHandle, ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(e, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal e As WaitHandle) As Boolean
        Return event_comb.waitfor(e)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal e As WaitHandle)
        assert(waitfor(e))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor_nap() As Boolean
        Return event_comb.waitfor_nap()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor_nap()
        assert(waitfor_nap())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor_yield() As Boolean
        Return event_comb.waitfor_yield()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor_yield()
        assert(waitfor_yield())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal i As attachable_event) As Boolean
        Return event_comb.waitfor(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal i As attachable_event)
        assert(waitfor(i))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function waitfor(ByVal i As attachable_event, ByVal timeout_ms As Int64) As Boolean
        Return event_comb.waitfor(i, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_waitfor(ByVal i As attachable_event, ByVal timeout_ms As Int64)
        assert(waitfor(i, timeout_ms))
    End Sub
End Module
