
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.envs
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class sleep_test
    Inherits [case]

    Private Shared Function succ(ByVal w1 As Func(Of Func(Of Boolean), Int64, Boolean),
                                 ByVal w2 As _do_val_ref_val(Of _do(Of Int32, Boolean), Int32, Int64, Boolean),
                                 ByVal d As _do(Of Int32, Boolean),
                                 ByVal exp As Int32) As Boolean
        assert(w1 IsNot Nothing)
        assert(w2 IsNot Nothing)
        assert(d IsNot Nothing)
        Dim i As Int32 = 0
        assertion.is_true(w1(Function() d(i), 0))
        assertion.equal(i, exp)
        i = 0
        assertion.is_true(w2(Function(ByRef x) d(x), i, 0))
        assertion.equal(i, exp)
        Return True
    End Function

    Private Shared Function succ(ByVal w1 As Action(Of Func(Of Boolean), Int64),
                                 ByVal w2 As void_val_ref_val(Of _do(Of Int32, Boolean), Int32, Int64),
                                 ByVal d As _do(Of Int32, Boolean),
                                 ByVal exp As Int32) As Boolean
        assert(w1 IsNot Nothing)
        assert(w2 IsNot Nothing)
        Return succ(Function(v As Func(Of Boolean), ms As Int64) As Boolean
                        w1(v, ms)
                        Return True
                    End Function,
                    Function(v As _do(Of Int32, Boolean), ByRef i As Int32, ms As Int64) As Boolean
                        w2(v, i, ms)
                        Return True
                    End Function,
                    d,
                    exp)
    End Function

    Private Shared Function succ_wait_until(ByVal d As _do(Of Int32, Boolean),
                                            ByVal exp As Int32) As Boolean
        Return succ(AddressOf sleep_wait_until,
                    AddressOf sleep_wait_until(Of Int32),
                    d,
                    exp)
    End Function

    Private Shared Function succ_wait_until_timeout(ByVal d As _do(Of Int32, Boolean),
                                                    ByVal exp As Int32) As Boolean
        Return succ(Function(v As Func(Of Boolean), ms As Int64) As Boolean
                        Return sleep_wait_until(v, ms, max_int64)
                    End Function,
                    Function(v As _do(Of Int32, Boolean), ByRef i As Int32, ms As Int64) As Boolean
                        Return sleep_wait_until(v, i, ms, max_int64)
                    End Function,
                    d,
                    exp)
    End Function

    Private Shared Function succ_wait_when(ByVal d As _do(Of Int32, Boolean),
                                           ByVal exp As Int32) As Boolean
        Return succ(AddressOf sleep_wait_when,
                    AddressOf sleep_wait_when(Of Int32),
                    d,
                    exp)
    End Function

    Private Shared Function succ_wait_when_timeout(ByVal d As _do(Of Int32, Boolean),
                                                   ByVal exp As Int32) As Boolean
        Return succ(Function(v As Func(Of Boolean), ms As Int64) As Boolean
                        Return sleep_wait_when(v, ms, max_int64)
                    End Function,
                    Function(v As _do(Of Int32, Boolean), ByRef i As Int32, ms As Int64) As Boolean
                        Return sleep_wait_when(v, i, ms, max_int64)
                    End Function,
                    d,
                    exp)
    End Function

    Private Shared Function succ(ByVal u As Func(Of _do(Of Int32, Boolean), Int32, Boolean),
                                 ByVal w As Func(Of _do(Of Int32, Boolean), Int32, Boolean)) As Boolean
        assert(u IsNot Nothing)
        assert(w IsNot Nothing)
        Const size As Int32 = 10000
        Return u(Function(ByRef x) _inc(x) = size, size) AndAlso
               u(Function(ByRef x) _inc(x) > size, size + 1) AndAlso
               u(Function(ByRef x) _inc(x) < size, 1) AndAlso
               w(Function(ByRef x) _inc(x) = size, 1) AndAlso
               w(Function(ByRef x) _inc(x) > size, 1) AndAlso
               w(Function(ByRef x) _inc(x) < size, size)
    End Function

    Private Shared Function succ_no_timeout() As Boolean
        Return succ(AddressOf succ_wait_until, AddressOf succ_wait_when)
    End Function

    Private Shared Function succ_timeout() As Boolean
        Return succ(AddressOf succ_wait_until_timeout, AddressOf succ_wait_when_timeout)
    End Function

    Private Shared Function timeout(ByVal d As Func(Of Int64, Boolean)) As Boolean
        assert(d IsNot Nothing)
        Const timeout_ms As Int64 = second_milli
        Using New boost()
            Using assertion.timelimited_operation(timeout_ms, timeout_ms + 4 * timeslice_length_ms)
                assertion.is_false(d(timeout_ms))
            End Using
        End Using
        Return True
    End Function

    Private Shared Function timeout() As Boolean
        Return timeout(Function(timeout_ms) sleep_wait_until(Function(ByRef x) False, 0, 0, timeout_ms)) AndAlso
               timeout(Function(timeout_ms) sleep_wait_until(Function() False, 0, timeout_ms)) AndAlso
               timeout(Function(timeout_ms) sleep_wait_when(Function(ByRef x) True, 0, 0, timeout_ms)) AndAlso
               timeout(Function(timeout_ms) sleep_wait_when(Function() True, 0, timeout_ms))
    End Function

    Public Overrides Function run() As Boolean
        Return succ_no_timeout() AndAlso
               succ_timeout() AndAlso
               timeout()
    End Function
End Class
