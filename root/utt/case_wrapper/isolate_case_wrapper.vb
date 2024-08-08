
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils

Public Class isolate_case_wrapper
    Inherits exec_case

    Private ReadOnly c As [case]
    Private ReadOnly _assertion_failures As atomic_int
    Private ReadOnly _unsatisfied_expectations As atomic_int
    Private ReadOnly _assert_death_msg As StringBuilder
    Private _case_started As Boolean
    Private _case_finished As Boolean

    Private Sub New(ByVal c As [case], ByVal timeout_ms As Int64)
        MyBase.New(envs.application_full_path,
                   String.Concat("""", assert_which.of(c).is_not_null().full_name, """"),
                   ignore_error:=True,
                   timeout_ms:=timeout_ms,
                   expected_return:=If(envs.os.windows_major = envs.os.windows_major_t._5, 128, 0))
        ' TODO: Why the return value of osi.root.utt.exe is 128 on Windows 2003 with .Net 4.0?
        Me.c = c
        assert(host.case_type_restriction.accepted_case_type(c.GetType()))
        _assertion_failures = New atomic_int()
        _unsatisfied_expectations = New atomic_int()
        _assert_death_msg = New StringBuilder()
    End Sub

    Public Sub New(ByVal c As commandline_specified_case_wrapper, Optional ByVal timeout_ms As Int64 = -1)
        Me.New(DirectCast(c, [case]), timeout_ms)
    End Sub

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return c.reserved_processors()
    End Function

    Public NotOverridable Overrides Function prepare() As Boolean
        If Not MyBase.prepare() Then
            Return False
        End If
        _case_started = False
        _case_finished = False
        _assertion_failures.set(0)
        _unsatisfied_expectations.set(0)
        Return True
    End Function

    Private Sub received(ByVal s As String)
        If commandline.specified(Me) Then
            raise_error("isolate_case_wrapper received: ", s)
        End If
        If Not _assert_death_msg.empty() Then
            _assert_death_msg.AppendLine(s)
        End If
        If error_message.is_message_line(s, error_type.critical, character.null) Then
            _assert_death_msg.AppendLine(s)
            Return
        End If
        If Not error_message.is_message_line(s, error_type.other, constants.utt.errortype_char) Then
            Return
        End If
        If s.Contains(String.Concat(", start running ", c.full_name)) Then
            _case_started = True
        ElseIf s.Contains(String.Concat(", finish running ", c.full_name)) Then
            _case_finished = True
        ElseIf s.Contains(", assertion failure, ") Then
            _assertion_failures.increment()
        ElseIf s.Contains(", unsatisfied expectation, ") Then
            _unsatisfied_expectations.increment()
        End If
    End Sub

    Protected NotOverridable Overrides Function init_process(ByVal p As shell_less_process) As Boolean
        p.start_info().EnvironmentVariables().Add("no_file_log", "1")
        p.start_info().EnvironmentVariables().Add("utt_report_case_name", "1")
        p.start_info().EnvironmentVariables().Add("strong_assert", "1")
        AddHandler receive_output, AddressOf received
        AddHandler receive_error, AddressOf received
        Return MyBase.init_process(p)
    End Function

    Protected NotOverridable Overrides Function inputs() As IEnumerable(Of String)
        Return const_array.elements(newline.incode(), newline.incode()).as_array()
    End Function

    Protected Overridable Function check_results() As Boolean
        ' Note, assertion failures from sub process may be expected.
        Return assertion.is_true(_case_started, "case_started") And
               assertion.is_true(_case_finished, "case_ended") And
               _assertion_failures.get() = 0
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        Return check_results() AndAlso MyBase.finish()
    End Function

    Public Function case_started() As Boolean
        Return _case_started
    End Function

    Public Function case_finished() As Boolean
        Return _case_finished
    End Function

    Public Function assertion_failures() As UInt32
        Return CUInt(_assertion_failures.get())
    End Function

    Public Function unsatisfied_expectations() As UInt32
        Return CUInt(_unsatisfied_expectations.get())
    End Function

    Public Function assert_death_msg() As String
        Return _assert_death_msg.ToString()
    End Function
End Class
