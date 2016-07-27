
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.utt
Imports System.Math

Public Class nowadays_gap_consistency_test
    Inherits [case]

    Private Shared Sub current(ByRef l As Int64, ByRef n As Int64, ByRef h As Int64)
        l = nowadays.low_res_milliseconds()
        n = nowadays.normal_res_milliseconds()
        h = nowadays.high_res_milliseconds()
        h += nowadays.high_res_milliseconds()
        n += nowadays.normal_res_milliseconds()
        l += nowadays.low_res_milliseconds()
        l >>= 1
        n >>= 1
        h >>= 1
    End Sub

    Public Overrides Function run() As Boolean
        Const round As Int32 = 256
        Dim failed As Int32 = 0
        For i As Int32 = 0 To round - 1
            Dim l As Int64 = 0
            Dim n As Int64 = 0
            Dim h As Int64 = 0
            current(l, n, h)
            sleep((rnd_int(0, 4) + 1) * timeslice_length_ms)
            Dim l2 As Int64 = 0
            Dim n2 As Int64 = 0
            Dim h2 As Int64 = 0
            current(l2, n2, h2)
            l2 -= l
            n2 -= n
            h2 -= h
            Dim diff As Int64 = 0
            diff = Abs(l2 - n2) + Abs(n2 - h2) + Abs(l2 - h2)
            failed += If(diff >= timeslice_length_ms * 3, 1, 0)
            'assert_more_or_equal_and_less_or_equal(l2, n2 - timeslice_length_ms, n2 + timeslice_length_ms)
            'assert_more_or_equal_and_less_or_equal(l2, h2 - timeslice_length_ms, h2 + timeslice_length_ms)
            'assert_more_or_equal_and_less_or_equal(n2, h2 - timeslice_length_ms, h2 + timeslice_length_ms)
        Next
        assert_less_or_equal(failed, round * 0.1)
        Return True
    End Function
End Class
