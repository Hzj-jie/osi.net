﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.delegates
Imports osi.root.formation

Public Class event_comb_waitfor_test
    Inherits [case]

    Private Shared Function waitfor_test(ByVal w As Func(Of Int64, Int64, Boolean)) As Boolean
        assert(Not w Is Nothing)
        Dim i As Int32 = 0
        Const target As Int32 = -1
        Const wait_ms As Int64 = 1000
        assert(i <> target)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        assert_true(async_sync(New event_comb(Function() As Boolean
                                                  Return w(start_ms, wait_ms) AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  i = target
                                                  Return goto_end()
                                              End Function),
                               wait_ms << 1))
        assert_more_or_equal(nowadays.milliseconds() - start_ms, wait_ms)
        assert_equal(i, target)
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
        assert_true(+r)
        If Not waitfor_test(Function(x, y) waitfor(Function() False, r, y)) Then
            Return False
        End If
        assert_false(+r)
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
            assert_equal(+r, v)
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
            assert_not_equal(+r, v)
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

    Public Overrides Function run() As Boolean
        Return waitfor_time_ms() AndAlso
               waitfor_try() AndAlso
               waitfor_try_result() AndAlso
               waitfor_void() AndAlso
               waitfor_do() AndAlso
               waitfor_event_comb()
    End Function
End Class
