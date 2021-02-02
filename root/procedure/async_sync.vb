
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.threadpool

<global_init(global_init_level.runtime_checkers)>
Public Module _async_sync
    Private Sub init()
        assert(npos < 0)
    End Sub

    Private Function async_sync(ByVal waitfor As Func(Of Boolean),
                                ByVal end_result As Func(Of Boolean)) As Boolean
        assert(Not waitfor Is Nothing)
        assert(Not end_result Is Nothing)
        Dim w As AutoResetEvent = Nothing
        w = New AutoResetEvent(False)
        Dim r As Boolean = False
        assert_begin(New event_comb(Function() As Boolean
                                        r = waitfor()
                                        Return goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If r Then
                                            r = end_result()
                                        End If
                                        assert(w.Set())
                                        Return goto_end()
                                    End Function))
        If threadpool.threadpool.in_restricted_threadpool_thread() Then
            While Not w.WaitOne(0)
                If Not thread_pool().execute() Then
                    thread_pool().wait(envs.two_timeslice_length_ms)
                End If
            End While
        Else
            assert(w.WaitOne())
        End If
        w.Close()
        Return r
    End Function

    Public Function async_sync(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return async_sync(Function() As Boolean
                              Return waitfor(ec, timeout_ms)
                          End Function,
                          Function() As Boolean
                              Return assert(Not ec Is Nothing) AndAlso
                                     ec.end_result()
                          End Function)
    End Function

    Public Function async_sync(ByVal ec As event_comb) As Boolean
        Return async_sync(ec, npos)
    End Function

    Public Sub assert_async_sync(ByVal ec As event_comb, ByVal timeout_ms As Int64)
        assert(async_sync(ec, timeout_ms))
    End Sub

    Public Sub assert_async_sync(ByVal ec As event_comb)
        assert(async_sync(ec))
    End Sub

    Public Function async_sync(ByVal cb As callback_action, ByVal timeout_ms As Int64) As Boolean
        Return async_sync(Function() As Boolean
                              Return waitfor(cb, timeout_ms)
                          End Function,
                          Function() As Boolean
                              Return assert(Not cb Is Nothing) AndAlso
                                     cb.success_finished()
                          End Function)
    End Function

    Public Function async_sync(ByVal cb As callback_action) As Boolean
        Return async_sync(cb, npos)
    End Function

    Public Sub assert_async_sync(ByVal cb As callback_action, ByVal timeout_ms As Int64)
        assert(async_sync(cb, timeout_ms))
    End Sub

    Public Sub assert_async_sync(ByVal cb As callback_action)
        assert(async_sync(cb))
    End Sub
End Module
