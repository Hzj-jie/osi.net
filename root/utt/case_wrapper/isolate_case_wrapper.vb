
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
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
    Private case_started As Boolean
    Private case_finished As Boolean

    Private Sub New(ByVal c As [case], ByVal timeout_ms As Int64)
        MyBase.New(envs.application_full_path,
                   strcat("""", assert_which.of(c).is_not_null().full_name, """"),
                   ignore_error:=True,
                   timeout_ms:=timeout_ms,
                   expected_return:=If(envs.os.windows_major = envs.os.windows_major_t._5, 128, 0))
        ' TODO: Why is the return value of osi.root.utt.exe 128 on Windows 2003 with .Net 4.0?
        ' On Windows 2003 with .Net 4.0 (?), the return value of osi.root.utt is 128. The reason is still unknown.
        Me.c = c
        assert(host.case_type_restriction.accepted_case_type(c.GetType()))
        _assertion_failures = New atomic_int()
        _unsatisfied_expectations = New atomic_int()
    End Sub

    Public Sub New(ByVal c As commandline_specified_case_wrapper, Optional ByVal timeout_ms As Int64 = -1)
        Me.New(DirectCast(c, [case]), timeout_ms)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return c.reserved_processors()
    End Function

    Public Overrides Function prepare() As Boolean
        If Not MyBase.prepare() Then
            Return False
        End If
        case_started = False
        case_finished = False
        _assertion_failures.set(0)
        _unsatisfied_expectations.set(0)
        Return True
    End Function

    Private Sub received(ByVal s As String)
        If Not s.StartsWith("u, ") Then
            Return
        End If
        If s.Contains(strcat(", start running ", c.full_name)) Then
            case_started = True
        ElseIf s.Contains(strcat(", finish running ", c.full_name)) Then
            case_finished = True
        ElseIf s.Contains(", assertion failure, ") Then
            utt_raise_error("isolate_case_wrapper received assertion failure: ", s)
            _assertion_failures.increment()
        ElseIf s.Contains(", unsatisfied expectation, ") Then
            utt_raise_error("isolate_case_wrapper received unsatisfied expectation: ", s)
            _unsatisfied_expectations.increment()
        End If
    End Sub

    Protected Overrides Function init_process(ByVal p As shell_less_process) As Boolean
        p.start_info().EnvironmentVariables().Add("no_file_log", "1")
        p.start_info().EnvironmentVariables().Add("utt_report_case_name", "1")
        attach_receive_output(AddressOf received)
        attach_receive_error(AddressOf received)
        Return MyBase.init_process(p)
    End Function

    Protected Overrides Function inputs() As IEnumerable(Of String)
        Return const_array.elements(newline.incode(), newline.incode()).as_array()
    End Function

    Public Overrides Function finish() As Boolean
        Return assertion.is_true(case_started) AndAlso
               assertion.is_true(case_finished) AndAlso
               _assertion_failures.get() = 0 AndAlso
               MyBase.finish()
    End Function

    Public Function assertion_failures() As UInt32
        Return CUInt(_assertion_failures.get())
    End Function

    Public Function unsatisfied_expectations() As UInt32
        Return CUInt(_unsatisfied_expectations.get())
    End Function
End Class
