
Imports System.Threading
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.envs
Imports osi.root.threadpool

Partial Public Class event_comb
    Private Function _waitfor(ByVal ec As event_comb, ByVal begin As Func(Of Boolean)) As Boolean
        assert(Not begin Is Nothing)
        assert_in_lock()
        If Not ec Is Nothing AndAlso
           object_compare(Me, ec) <> 0 Then
            Return ec.reenterable_locked(Function() As Boolean
                                             If ec.not_started() AndAlso
                                                ec.cb Is Nothing Then
                                                 inc_pends()
                                                 ec.cb = Me
                                                 Return begin()
                                             Else
                                                 Return False
                                             End If
                                         End Function)
        Else
            Return False
        End If
    End Function

    Private Function _waitfor(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return _waitfor(ec,
                        Function() As Boolean
                            Return begin(ec, timeout_ms)
                        End Function)
    End Function

    Private Function _waitfor(ByVal ec As event_comb) As Boolean
        Return _waitfor(ec,
                        Function() As Boolean
                            Return begin(ec)
                        End Function)
    End Function

    Private Function _waitfor_or_null(ByVal ec As event_comb) As Boolean
        Return If(ec Is Nothing, True, _waitfor(ec))
    End Function

    Private Function _waitfor(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If isemptyarray(ecs) Then
            Return False
        Else
            For i As Int32 = 0 To array_size(ecs) - 1
                If Not _waitfor(ecs(i)) Then
                    Return False
                End If
            Next

            Return True
        End If
    End Function

    Private Function _waitfor_or_null(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If isemptyarray(ecs) Then
            Return True
        Else
            For i As Int32 = 0 To array_size(ecs) - 1
                If Not _waitfor_or_null(ecs(i)) Then
                    Return False
                End If
            Next

            Return True
        End If
    End Function

    Private Function _waitone(ByVal ecs() As event_comb) As Boolean
        assert_in_lock()
        If _waitfor(ecs) Then
            assert(Not ecs Is Nothing AndAlso ecs.Length() > 0)
            For i As Int64 = 1 To ecs.Length() - 1
                dec_pends()
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean),
                              ByVal w As Func(Of Func(Of Boolean), Action, Boolean)) As Boolean
        assert_in_lock()
        assert(Not w Is Nothing)
        If [try] Is Nothing Then
            Return False
        Else
            Return w([try], _wait())
        End If
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return _waitfor([try], Function(x, y) queue_runner.push(queue_runner.check(x, y, timeout_ms)))
    End Function

    Private Function _waitfor(ByVal [try] As Func(Of Boolean),
                              ByVal try_result As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As Boolean
        If try_result Is Nothing Then
            Return _waitfor([try], timeout_ms)
        ElseIf [try] Is Nothing Then
            Return False
        Else
            Return _waitfor(Function() As Boolean
                                assert(eva(try_result, [try]()))
                                Return +try_result
                            End Function,
                            timeout_ms)
        End If
    End Function

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
                       thread_pool().queue_job(Sub()
                                                   [resume](Me)
                                               End Sub)
                   End If
               End Sub
    End Function

    Private Function _wait() As Action
        assert_in_lock()
        inc_pends()
        Return Sub()
                   '1, put it back to selected threadpool
                   '2, no matter how the _wait() called, it would be safe
                   thread_pool().queue_job(Sub()
                                               [resume](Me)
                                           End Sub)
               End Sub
    End Function

    Private Shared Sub _waitfor(ByVal d As Action, ByVal cb As action)
        assert(Not d Is Nothing)
        assert(Not cb Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        Try
                                            d()
                                        Finally
                                            cb()
                                        End Try
                                    End Sub)
    End Sub

    Private Function _waitfor(ByVal d As Action) As Boolean
        If d Is Nothing Then
            Return False
        Else
            _waitfor(d, _wait())
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal d As Action, ByVal timeout_ms As Int64) As Boolean
        If d Is Nothing Then
            Return False
        ElseIf timeout_ms < 0 Then
            Return _waitfor(d)
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            _waitfor(d, cb)
            Return True
        End If
    End Function

    Private Shared Function _do_void(Of T)(ByVal d As Func(Of T),
                                           ByVal r As pointer(Of T),
                                           ByRef v As Action) As Boolean
        If d Is Nothing Then
            Return False
        Else
            v = Sub()
                    eva(r, d())
                End Sub
            Return True
        End If
    End Function

    Private Function _waitfor(Of T)(ByVal d As Func(Of T), ByVal r As pointer(Of T)) As Boolean
        Dim v As Action = Nothing
        If _do_void(d, r, v) Then
            Return _waitfor(v)
        Else
            Return False
        End If
    End Function

    Private Function _waitfor(Of T)(ByVal d As Func(Of T),
                                    ByVal r As pointer(Of T),
                                    ByVal timeout_ms As Int64) As Boolean
        Dim v As Action = Nothing
        If _do_void(d, r, v) Then
            Return _waitfor(v, timeout_ms)
        Else
            Return False
        End If
    End Function

    Private Function _waitfor(ByVal e As WaitHandle, ByVal timeout_ms As Int64) As Boolean
        If e Is Nothing Then
            Return False
        Else
            Return _waitfor(Sub()
                                e.wait(timeout_ms)
                            End Sub)
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal e As WaitHandle) As Boolean
        Return _waitfor(e, npos)
    End Function

    Private Function _waitfor(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        assert_in_lock()
        Return begin(action, timeout_ms) AndAlso
               _waitfor(callback_action.action_check(action))
    End Function

    Private Function _waitfor(ByVal action As callback_action) As Boolean
        assert_in_lock()
        Return begin(action) AndAlso
               _waitfor(callback_action.action_check(action))
    End Function

    Private Function _waitfor(ByVal ms As Int64) As Boolean
        If ms < 0 Then
            Return False
        ElseIf ms = 0 Then
            Return _waitfor_yield()
        Else
            Return assert(Not stopwatch.push(ms, _wait()) Is Nothing)
        End If
    End Function

    Private Function _waitfor(ByVal l As ref(Of event_comb_lock)) As Boolean
        If l Is Nothing Then
            Return False
        Else
            l.wait()
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal i As ref(Of singleentry)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return _waitfor(AddressOf i.in_use)
        End If
    End Function

    Private Function _waitfor(ByVal i As ref(Of singleentry), ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return _waitfor(AddressOf i.in_use, timeout_ms)
        End If
    End Function

    Private Function _waitfor(Of T)(ByVal l As multilock(Of event_comb_lock), ByVal i As T) As Boolean
        If l Is Nothing Then
            Return False
        Else
            l.lock(i)
            Return True
        End If
    End Function

    Private Function _waitfor_nap() As Boolean
        Return assert(queue_runner.once(_wait()))
    End Function

    Private Function _waitfor_yield() As Boolean
        _wait()()
        Return True
    End Function

    Private Function _waitfor(ByVal i As count_event) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return assert(i.attach(_wait()))
        End If
    End Function

    Private Function _waitfor(ByVal i As count_event, ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(i.attach(cb))
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal i As weak_count_event) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return assert(i.attach(_wait()))
        End If
    End Function

    Private Function _waitfor(ByVal i As weak_count_event, ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(i.attach(cb))
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal i As signal_event) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return assert(i.attach(_wait()))
        End If
    End Function

    Private Function _waitfor(ByVal i As signal_event, ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(i.attach(cb))
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal i As weak_signal_event) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return assert(i.attach(_wait()))
        End If
    End Function

    Private Function _waitfor(ByVal i As weak_signal_event, ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(i.attach(cb))
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            Return True
        End If
    End Function

    Private Function _waitfor(ByVal i As concurrency_event(Of _false)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return assert(i.attach(_wait()))
        End If
    End Function

    Private Function _waitfor(ByVal i As concurrency_event(Of _false), ByVal timeout_ms As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim cb As Action = Nothing
            cb = _multiple_resume_wait()
            assert(i.attach(cb))
            assert(Not stopwatch.push(timeout_ms, cb) Is Nothing)
            Return True
        End If
    End Function
End Class
