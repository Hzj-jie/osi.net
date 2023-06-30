
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
Imports osi.root.threadpool
Imports osi.root.utils

Partial Public Class event_comb
    Private Function _waitfor(ByVal ec As event_comb, ByVal begin As Func(Of Boolean)) As Boolean
        assert(Not begin Is Nothing)
        assert_in_lock()
        If ec Is Nothing OrElse object_compare(Me, ec) = 0 Then
            Return False
        End If
        Return ec.locked(Function() As Boolean
                             If ec._started() OrElse Not ec.cb Is Nothing Then
                                 Return False
                             End If
                             inc_pends()
                             ec.cb = Me
                             Return True
                         End Function) AndAlso
               begin()
    End Function

    Private Function _waitfor(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return _waitfor(ec,
                        Function() As Boolean
                            Return begin(ec, timeout_ms)
                        End Function)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal ec As event_comb) As Boolean
        Return _waitfor(ec,
                        Function() As Boolean
                            Return begin(ec)
                        End Function)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor_or_null(ByVal ec As event_comb) As Boolean
        Return If(ec Is Nothing, True, _waitfor(ec))
    End Function

    Private Function _waitfor(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If isemptyarray(ecs) Then
            Return False
        End If
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If Not _waitfor(ecs(i)) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function _waitfor_or_null(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If isemptyarray(ecs) Then
            Return True
        End If
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If Not _waitfor_or_null(ecs(i)) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function _waitone(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If Not _waitfor(ecs) Then
            Return False
        End If
        assert(Not ecs Is Nothing AndAlso ecs.Length() > 0)
        For i As Int64 = 1 To ecs.Length() - 1
            dec_pends()
        Next
        Return True
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean),
                              ByVal w As Func(Of Func(Of Boolean), Action, Boolean)) As Boolean
        assert_in_lock()
        assert(Not w Is Nothing)
        If [try] Is Nothing Then
            Return False
        End If
        Return w([try], _wait())
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return _waitfor([try], Function(x, y) queue_runner.push(queue_runner.check(x, y, timeout_ms)))
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean),
                              ByVal try_result As ref(Of Boolean),
                              ByVal timeout_ms As Int64) As Boolean
        If try_result Is Nothing Then
            Return _waitfor([try], timeout_ms)
        End If
        If [try] Is Nothing Then
            Return False
        End If
        Return _waitfor(Function() As Boolean
                            assert(eva(try_result, [try]()))
                            Return +try_result
                        End Function,
                        timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal [try] As Func(Of Boolean)) As Boolean
        Return _waitfor([try], Function(x, y) queue_runner.push(queue_runner.check(x, y)))
    End Function

    Private Function _multiple_resume_wait() As Action
        assert_in_lock()
        inc_pends()
        Dim se As ref(Of singleentry) = Nothing
        se = New ref(Of singleentry)()
        Return Sub()
                   If se.mark_in_use() Then
                       '1, put it back to selected threadpool
                       '2, no matter how the _wait() called, it would be safe
                       thread_pool.push(Sub()
                                            [resume](Me)
                                        End Sub)
                   End If
               End Sub
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _wait() As Action
        assert_in_lock()
        inc_pends()
        Return Sub()
                   '1, put it back to selected threadpool
                   '2, no matter how the _wait() called, it would be safe
                   thread_pool.push(Sub()
                                        [resume](Me)
                                    End Sub)
               End Sub
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub _waitfor(ByVal d As Action, ByVal cb As Action)
        assert(Not d Is Nothing)
        assert(Not cb Is Nothing)
        managed_thread_pool.push(Sub()
                                     Try
                                         d()
                                     Finally
                                         cb()
                                     End Try
                                 End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal d As Action) As Boolean
        If d Is Nothing Then
            Return False
        End If
        _waitfor(d, _wait())
        Return True
    End Function

    Private Function _waitfor(ByVal d As Action, ByVal timeout_ms As Int64) As Boolean
        If d Is Nothing Then
            Return False
        End If
        If timeout_ms < 0 Then
            Return _waitfor(d)
        End If
        Dim cb As Action = Nothing
        cb = _multiple_resume_wait()
        assert(Not stopwatch.push(timeout_ms, cb.as_weak_action()) Is Nothing)
        _waitfor(d, cb)
        Return True
    End Function

    Private Shared Function _do_void(Of T)(ByVal d As Func(Of T),
                                           ByVal r As ref(Of T),
                                           ByRef v As Action) As Boolean
        If d Is Nothing Then
            Return False
        End If
        v = Sub()
                eva(r, d())
            End Sub
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(Of T)(ByVal d As Func(Of T), ByVal r As ref(Of T)) As Boolean
        Dim v As Action = Nothing
        If _do_void(d, r, v) Then
            Return _waitfor(v)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(Of T)(ByVal d As Func(Of T),
                                    ByVal r As ref(Of T),
                                    ByVal timeout_ms As Int64) As Boolean
        Dim v As Action = Nothing
        If _do_void(d, r, v) Then
            Return _waitfor(v, timeout_ms)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal e As WaitHandle, ByVal timeout_ms As Int64) As Boolean
        If e Is Nothing Then
            Return False
        End If
        Return _waitfor(Sub()
                            e.wait(timeout_ms)
                        End Sub)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal e As WaitHandle) As Boolean
        Return _waitfor(e, npos)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        assert_in_lock()
        Return begin(action, timeout_ms) AndAlso
               _waitfor(callback_action.action_check(action))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal action As callback_action) As Boolean
        assert_in_lock()
        Return begin(action) AndAlso
               _waitfor(callback_action.action_check(action))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal ms As Int64) As Boolean
        If ms < 0 Then
            Return False
        End If
        If ms = 0 Then
            Return _waitfor_yield()
        End If
        Return assert(Not stopwatch.push(ms, _wait()) Is Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal l As ref(Of event_comb_lock)) As Boolean
        If l Is Nothing Then
            Return False
        End If
        l.wait()
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal i As ref(Of singleentry)) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return _waitfor(AddressOf i.in_use)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal i As ref(Of singleentry), ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return _waitfor(AddressOf i.in_use, timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(Of T)(ByVal l As multilock(Of event_comb_lock), ByVal i As T) As Boolean
        If l Is Nothing Then
            Return False
        End If
        l.lock(i)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor_nap() As Boolean
        Return assert(queue_runner.once(_wait()))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor_yield() As Boolean
        _wait()()
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _waitfor(ByVal i As attachable_event) As Boolean
        If i Is Nothing Then
            Return False
        End If
        If i.marked() Then
            Return True
        End If
        Return assert(i.attach(_wait()))
    End Function

    Private Function _waitfor(ByVal i As attachable_event, ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        End If
        If i.marked() Then
            Return True
        End If
        Dim e As ref(Of stopwatch.event) = Nothing
        e = New ref(Of stopwatch.event)()
        Dim cb As Action = Nothing
        cb = _multiple_resume_wait()
        e.set(stopwatch.push(timeout_ms, cb))
        assert(Not e.empty())
        assert(i.attach(Sub()
                            cb()
                            assert(Not e.empty())
                            e.get().cancel()
                        End Sub))
        Return True
    End Function
End Class
