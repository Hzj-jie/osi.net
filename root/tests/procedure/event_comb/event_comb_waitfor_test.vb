
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt

Public NotInheritable Class event_comb_waitfor_test
    Inherits [case]

    Private Shared Function waitfor_test(ByVal w As Func(Of Int64, Int64, Boolean)) As Boolean
        assert(Not w Is Nothing)
        Dim i As Int32 = 0
        Const target As Int32 = -1
        Const wait_ms As Int64 = 10000
        assert(i <> target)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        assertion.is_true(async_sync(New event_comb(Function() As Boolean
                                                        Return w(start_ms, wait_ms) AndAlso
                                                         goto_next()
                                                    End Function,
                                                    Function() As Boolean
                                                        i = target
                                                        Return goto_end()
                                                    End Function),
                                     wait_ms << 1))
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, wait_ms)
        assertion.equal(i, target)
        Return True
    End Function

    Private Shared Function waitfor_time_ms() As Boolean
        Return waitfor_test(Function(x, y) waitfor(y))
    End Function

    Private Shared Function waitfor_try_result() As Boolean
        Dim r As pointer(Of Boolean) = Nothing
        If Not waitfor_test(Function(x, y) waitfor(Function() nowadays.milliseconds() - x >= y, r, y)) Then
            Return False
        End If
        r = New pointer(Of Boolean)()
        If Not waitfor_test(Function(x, y) waitfor(Function() nowadays.milliseconds() - x >= y, r, y)) Then
            Return False
        End If
        assertion.is_true(+r)
        If Not waitfor_test(Function(x, y) waitfor(Function() False, r, y)) Then
            Return False
        End If
        assertion.is_false(+r)
        Return True
    End Function

    Private Shared Function waitfor_try() As Boolean
        Return waitfor_test(Function(x, y) waitfor(Function() nowadays.milliseconds() - x >= y)) AndAlso
               waitfor_test(Function(x, y) waitfor(Function() False, y))
    End Function

    Private Shared Function waitfor_void() As Boolean
        Return waitfor_test(Function(x, y) waitfor(Sub() sleep(y))) AndAlso
               waitfor_test(Function(x, y) waitfor(Sub() sleep(max_int32), y))
    End Function

    Private Shared Function waitfor_do1() As Boolean
        Const v As Int32 = 1000
        Dim r As pointer(Of Int32) = Nothing
        r = New pointer(Of Int32)()
        assert(+r <> v)
        If waitfor_test(Function(x, y) waitfor(Function() As Int32
                                                   sleep(y)
                                                   Return v
                                               End Function,
                                               r)) Then
            assertion.equal(+r, v)
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function waitfor_do2() As Boolean
        Const v As Int32 = 1000
        Dim r As pointer(Of Int32) = Nothing
        r = New pointer(Of Int32)()
        assert(+r <> v)
        If waitfor_test(Function(x, y) waitfor(Function() As Int32
                                                   sleep(max_int32)
                                                   Return v
                                               End Function,
                                               r,
                                               y)) Then
            assertion.not_equal(+r, v)
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function waitfor_do() As Boolean
        Return waitfor_do1() AndAlso
               waitfor_do2()
    End Function

    Private Shared Function waitfor_event_comb() As Boolean
        Return waitfor_test(Function(x, y) waitfor(New event_comb(Function() As Boolean
                                                                      Return waitfor(y) AndAlso
                                                                             goto_end()
                                                                  End Function)))
    End Function

    Private Shared Function waitfor_signal_event() As Boolean
        Dim se As signal_event = Nothing
        se = New signal_event()
        Dim se2 As signal_event = Nothing
        se2 = New signal_event()
        Dim se3 As signal_event = Nothing
        se3 = New signal_event()
        Dim [step] As Int32 = 0
        Dim ec As event_comb = Nothing
        ec = New event_comb(Function() As Boolean
                                [step] = 0
                                Return waitfor(se) AndAlso
                                       goto_next()
                            End Function,
                            Function() As Boolean
                                [step] = 1
                                Return waitfor(se2, 1) AndAlso
                                       goto_next()
                            End Function,
                            Function() As Boolean
                                [step] = 2
                                Return waitfor(se3) AndAlso
                                       goto_next()
                            End Function,
                            Function() As Boolean
                                [step] = 3
                                Return False
                            End Function)
        [step] = -1
        assert_begin(ec)
        assertion.is_true(timeslice_sleep_wait_until(Function() [step] = 0, seconds_to_milliseconds(1)))
        assertion.is_false(timeslice_sleep_wait_when(Function() [step] = 0, seconds_to_milliseconds(1)))
        se.mark()
        assertion.is_true(timeslice_sleep_wait_until(Function() [step] = 2, seconds_to_milliseconds(1)))
        assertion.is_false(timeslice_sleep_wait_when(Function() [step] = 2, seconds_to_milliseconds(1)))
        se3.mark()
        assertion.is_true(timeslice_sleep_wait_until(Function() [step] = 3, seconds_to_milliseconds(1)))
        assertion.is_false(timeslice_sleep_wait_when(Function() [step] = 3, seconds_to_milliseconds(1)))
        assertion.is_true(timeslice_sleep_wait_until(Function() ec.end(), seconds_to_milliseconds(1)))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return waitfor_time_ms() AndAlso
               waitfor_try() AndAlso
               waitfor_try_result() AndAlso
               waitfor_void() AndAlso
               waitfor_do() AndAlso
               waitfor_event_comb() AndAlso
               waitfor_signal_event()
    End Function
End Class
