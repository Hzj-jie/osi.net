
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Public NotInheritable Class failure_case
    Inherits [case]

#If TODO Then
    Public Const expected_failure_count As Int32 = 28
#Else
    Public Const expected_failure_count As Int32 = 25
#End If

    ' TODO: Test expectation.
    Private Shared Sub report_self_health_failure(ByVal exp As Int64, ByVal cmd As String)
        If assertion.failure_count() <> exp Then
            utt_raise_error("failure count is not same as expected value ", exp, " after command ", cmd)
        End If
    End Sub

    Public Overrides Function run() As Boolean
        If self_health_stage() Then
            assertion.is_true(False)
            report_self_health_failure(1, "assertion.is_true(False)")
            assertion.is_null("")
            report_self_health_failure(2, "assertion.is_null("")")
            assertion.is_not_null(CStr(Nothing))  ' Convert.ToString conflict
            report_self_health_failure(3, "assertion.is_not_null(CStr(Nothing))")
            assertion.is_false(True)
            report_self_health_failure(4, "assertion.is_false(True)")
            assertion.reference_equal("", CStr(Nothing))  ' Convert.ToString conflict
            report_self_health_failure(5, "assertion.reference_equal("", CStr(Nothing))")
            assertion.not_reference_equal("", "")
            report_self_health_failure(6, "assertion.not_reference_equal("", "")")
            assertion.not_equal(0, 0)
            report_self_health_failure(7, "assertion.not_equal(0, 0)")
            assertion.equal(0, 1)
            report_self_health_failure(8, "assertion.equal(0, 1)")
            assertion.less(1, 0)
            report_self_health_failure(9, "assertion.less(1, 0)")
            assertion.less(0, 0)
            report_self_health_failure(10, "assertion.less(0, 0)")
            assertion.less_or_equal(1, 0)
            report_self_health_failure(11, "assertion.less_or_equal(1, 0)")
            assertion.more(0, 0)
            report_self_health_failure(12, "assertion.more(0, 0)")
            assertion.more(0, 1)
            report_self_health_failure(13, "assertion.more(0, 1)")
            assertion.more_or_equal(0, 1)
            report_self_health_failure(14, "assertion.more_or_equal(0, 1)")
            assertion.is_int(0.1)
            report_self_health_failure(15, "assertion.is_int(0.1)")
            assertion.is_not_int(1.0)
            report_self_health_failure(16, "assertion.is_not_int(1.0)")
            assertion.now_in_time_range(Now().milliseconds() + 1000, Now().milliseconds() + 1001)
            report_self_health_failure(
                17,
                "assertion.now_in_time_range(Now().milliseconds() + 1000, Now().milliseconds() + 1001)")
            assertion.now_in_time_range(Now().milliseconds() - 1001, Now().milliseconds() - 1000)
            report_self_health_failure(
                18,
                "assertion.now_in_time_range(Now().milliseconds() - 1001, Now().milliseconds() - 1000)")
            Using assertion.timelimited_operation(0, 1)
                Dim ma As IDisposable = Nothing
                ma = assertion.timelimited_operation(0, 1)
                measure_sleep(CInt(2 * timeslice_length_ms))
                ma.Dispose()
                report_self_health_failure(19, "manually assertion.timelimited_operation.Dispose()")
            End Using
            report_self_health_failure(20, "automatically assertion.timelimited_operation.Dispose()")

            ' tirgger 2 failures, one from exec_case, one from assertion.is_true
            assertion.is_true(host.execute_case(New exec_failure_case_1.exec_failure_case()))
            report_self_health_failure(22,
                                       "assertion.is_true(host.execute_case(New exec_failure_case_1.exec_failure_case()))")

#If TODO Then
            ' trigger 5 failures,
            ' one from exec_case,
            ' one from assertion.is_true
            ' one from err_rec,
            ' one from out_rec
            ' one from error_received
            assertion.is_true(host.execute_case(New exec_failure_case_2.exec_failure_case()))
            report_self_health_failure(27,
                                       "assertion.is_true(host.execute_case(New exec_failure_case_2.exec_failure_case()))")
#End If
            assertion.happening_in(Function() False, 1)
            assertion.not_happening_in(Function() True, 1)
        End If

        assertion.is_true(True)
        assertion.is_null(CStr(Nothing))  ' Convert.ToString conflict
        assertion.is_not_null("")
        assertion.is_false(False)
        assertion.reference_equal("", "")
        assertion.not_reference_equal("", CStr(Nothing))  ' Convert.ToString conflict
        assertion.not_equal(0, 1)
        assertion.equal(0, 0)
        assertion.less(0, 1)
        assertion.less_or_equal(0, 1)
        assertion.less_or_equal(0, 0)
        assertion.more(1, 0)
        assertion.more_or_equal(0, 0)
        assertion.more_or_equal(1, 0)
        assertion.is_int(1.0)
        assertion.is_not_int(0.1)
        assertion.now_in_time_range(Now().milliseconds(), Now().milliseconds() + 1000)
        assertion.now_in_time_range(Now().milliseconds() - 1000, Now().milliseconds() + 2 * timeslice_length_ms)
        Using assertion.timelimited_operation(0, 2 * timeslice_length_ms)
            Dim ma As IDisposable = Nothing
            ma = assertion.timelimited_operation(0, 2 * timeslice_length_ms)
            ma.Dispose()
        End Using
        assertion.happening_in(Function() True, 1)
        assertion.not_happening_in(Function() False, 1)

        Return Not self_health_stage()
    End Function
End Class
