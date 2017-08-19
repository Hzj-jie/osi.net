
Imports osi.root.connector
Imports osi.root.utils

Public Class isolate_case_wrapper
    Inherits exec_case

    Private ReadOnly c As [case]
    Private case_started As Boolean
    Private case_finished As Boolean
    Private assert_detected As Boolean

    Private Sub New(ByVal c As [case], ByVal timeout_ms As Int64)
        MyBase.New(envs.application_full_path,
                   strcat("""", assert_not_nothing_return(c).assembly_qualified_name, """"),
                   ignore_error:=True,
                   timeout_ms:=timeout_ms,
                   expected_return:=If(envs.os.windows_major = envs.os.windows_major_t._5, 128, 0))
        ' TODO: Why is the return value of osi.root.utt.exe 128 on Windows 2003 with .Net 4.0?
        ' On Windows 2003 with .Net 4.0 (?), the return value of osi.root.utt is 128. The reason is still unknown.
        Me.c = c
        assert(host.case_type_restriction.accept(c.GetType()))
    End Sub

    Public Sub New(ByVal c As commandline_specific_case_wrapper, Optional ByVal timeout_ms As Int64 = -1)
        Me.New(DirectCast(c, [case]), timeout_ms)
    End Sub

    Public Sub New(ByVal c As commandline_specific_event_comb_case_wrapper, Optional ByVal timeout_ms As Int64 = -1)
        Me.New(DirectCast(c, [case]), timeout_ms)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return c.reserved_processors()
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            case_started = False
            case_finished = False
            assert_detected = False
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub received(ByVal s As String)
        If strcontains(s, strcat("start running ", c.full_name)) Then
            case_started = True
        ElseIf strcontains(s, strcat("finish running ", c.full_name)) Then
            case_finished = True
        ElseIf strcontains(s, "assert failed, ") Then
            assert_detected = True
        End If
    End Sub

    Protected Overrides Function init_process(ByVal p As shell_less_process) As Boolean
        p.start_info().EnvironmentVariables().Add("no_file_log", "1")
        p.start_info().EnvironmentVariables().Add("utt_report_case_name", "1")
        attach_receive_output(AddressOf received)
        attach_receive_error(AddressOf received)
        Return MyBase.init_process(p)
    End Function

    Public Overrides Function finish() As Boolean
        Return assert_true(case_started) AndAlso
               assert_true(case_finished) AndAlso
               Not assert_detected AndAlso
               MyBase.finish()
    End Function
End Class
