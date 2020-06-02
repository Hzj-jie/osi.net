
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utt
Imports tp = osi.root.threadpool.threadpool

Public NotInheritable Class async_sync_test
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim count As Int32 = 0
        ' A very large count may trigger stack overflow, as the following event_comb instances will only execute an
        ' async_sync.
        count = CInt(thread_pool().thread_count() << 1)
        Dim c As atomic_int = Nothing
        c = New atomic_int()
        Dim w As AutoResetEvent = Nothing
        w = New AutoResetEvent(False)
        For i As Int32 = 0 To count - 1
            assert_begin(New event_comb(Function() As Boolean
                                            Return async_sync(New event_comb(
                                                       Function() As Boolean
                                                           Return waitfor(seconds_to_milliseconds(1)) AndAlso
                                                                  goto_end()
                                                       End Function)) AndAlso
                                                   goto_next()
                                        End Function,
                                        Function() As Boolean
                                            If c.increment() = count Then
                                                assert(w.Set())
                                            End If
                                            Return goto_end()
                                        End Function))
        Next
        If tp.in_restricted_threadpool_thread() Then
            assertion.is_true(w.WaitOne(CInt(seconds_to_milliseconds(10))))
        Else
            assertion.is_true(w.WaitOne(CInt(seconds_to_milliseconds(60))))
        End If
        Return True
    End Function

    Private Shared Function async_sync_with_huge_timeout() As Boolean
        assertion.is_true(async_sync(New event_comb(Function() As Boolean
                                                        Return goto_end()
                                                    End Function),
                                     max_int64))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               async_sync_with_huge_timeout()
    End Function
End Class
