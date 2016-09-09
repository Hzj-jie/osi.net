﻿
Imports System.DateTime
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector

Public Class failure_case
    Inherits [case]

    Public Const expected_failure_count As Int32 = 28

    Public Overrides Function run() As Boolean
        If self_health_stage() Then
            assert_true(False)
            assert_nothing(empty_string)
            assert_not_nothing(CStr(Nothing))  ' Convert.ToString conflict
            assert_false(True)
            assert_reference_equal(empty_string, CStr(Nothing))  ' Convert.ToString conflict
            assert_not_reference_equal(empty_string, empty_string)
            assert_not_equal(0, 0)
            assert_equal(0, 1)
            assert_less(1, 0)
            assert_less(0, 0)
            assert_less_or_equal(1, 0)
            assert_more(0, 0)
            assert_more(0, 1)
            assert_more_or_equal(0, 1)
            assert_int(0.1)
            assert_not_int(1.0)
            assert_now_in_time_range(Now().milliseconds() + 1000, Now().milliseconds() + 1001)
            assert_now_in_time_range(Now().milliseconds() - 1001, Now().milliseconds() - 1000)
            Using New auto_assert_timelimited_operation(0, 1)
                Dim ma As manual_assert_timelimited_operation = New manual_assert_timelimited_operation(0, 1)
                measure_sleep(two_timeslice_length_ms)
                ma.finish()
            End Using

            ' tirgger 2 failures, one from exec_case, one from assert_true
            assert_true(host.execute_case(New exec_failure_case_1.exec_failure_case()))
            ' trigger 5 failures,
            ' one from exec_case,
            ' one from assert_true
            ' one from err_rec,
            ' one from out_rec
            ' one from error_received
            assert_true(host.execute_case(New exec_failure_case_2.exec_failure_case()))
        End If

        assert_true(True)
        assert_nothing(CStr(Nothing))  ' Convert.ToString conflict
        assert_not_nothing(empty_string)
        assert_false(False)
        assert_reference_equal(empty_string, empty_string)
        assert_not_reference_equal(empty_string, CStr(Nothing))  ' Convert.ToString conflict
        assert_not_equal(0, 1)
        assert_equal(0, 0)
        assert_less(0, 1)
        assert_less_or_equal(0, 1)
        assert_less_or_equal(0, 0)
        assert_more(1, 0)
        assert_more_or_equal(0, 0)
        assert_more_or_equal(1, 0)
        assert_int(1.0)
        assert_not_int(0.1)
        assert_now_in_time_range(Now().milliseconds(), Now().milliseconds() + 1000)
        assert_now_in_time_range(Now().milliseconds() - 1000, Now().milliseconds() + two_timeslice_length_ms)
        Using New auto_assert_timelimited_operation(0, two_timeslice_length_ms)
            Dim ma As manual_assert_timelimited_operation = _
                      New manual_assert_timelimited_operation(0, two_timeslice_length_ms)
            ma.finish()
        End Using

        Return Not self_health_stage()
    End Function
End Class
