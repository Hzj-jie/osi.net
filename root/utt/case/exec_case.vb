
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports osi.root.connector
Imports osi.root.utils

Public Class exec_case
    Inherits [case]

    Protected Const default_arg As String = Nothing
    Protected Const default_ignore_output As Boolean = True
    Protected Const default_ignore_error As Boolean = False
    Protected Const default_expected_return As Int32 = 0
    Protected Const default_timeout_ms As Int64 = -1
    Protected ReadOnly ignore_output As Boolean
    Protected ReadOnly ignore_error As Boolean
    Protected ReadOnly exec_file As String
    Protected ReadOnly exec_arg As String
    Protected ReadOnly process_name As String
    Protected ReadOnly _expected_return As Int32
    Protected ReadOnly timeout_ms As Int64
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Protected Event receive_output(ByVal s As String)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Protected Event receive_error(ByVal s As String)

    ' TODO: Remove constructor parameters, use overridable functions instead.
    Public Sub New(ByVal file As String,
                   Optional ByVal arg As String = default_arg,
                   Optional ByVal ignore_output As Boolean = default_ignore_output,
                   Optional ByVal ignore_error As Boolean = default_ignore_error,
                   Optional ByVal expected_return As Int32 = default_expected_return,
                   Optional ByVal timeout_ms As Int64 = default_timeout_ms)
        assert(Not file.null_or_empty())
        Me.exec_file = file
        Me.exec_arg = arg
        Me.ignore_output = ignore_output
        Me.ignore_error = ignore_error
        Me._expected_return = expected_return
        Me.timeout_ms = timeout_ms
        Me.process_name = strcat("process ",
                                 exec_file,
                                 If(Not exec_arg.null_or_empty(),
                                    strcat(" with argument [", exec_arg, "]"),
                                    Nothing))
    End Sub

    Protected Overridable Function inputs() As IEnumerable(Of String)
        Return Nothing
    End Function

    Protected Overridable Function init_process(ByVal p As shell_less_process) As Boolean
        Return True
    End Function

    Private Sub received(ByVal s As String, ByVal output As Boolean)
        If (output AndAlso Not ignore_output) OrElse
           (Not output AndAlso Not ignore_error) Then
            utt_raise_error(process_name,
                            " ",
                            If(output, "outputs", "outputs error"),
                            " { ",
                            s,
                            " }")
        End If
        If output Then
            RaiseEvent receive_output(s)
        Else
            RaiseEvent receive_error(s)
        End If
    End Sub

    Private Sub output_received(ByVal s As String)
        received(s, True)
    End Sub

    Private Sub error_received(ByVal s As String)
        received(s, False)
    End Sub

    Protected Overridable Function expected_return() As Int32
        Return _expected_return
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        If Not File.Exists(exec_file) Then
            Return False
        End If
        Using p As shell_less_process = New shell_less_process(True)
            p.start_info().FileName() = exec_file
            p.start_info().Arguments() = exec_arg
            If Not init_process(p) Then
                Return False
            End If
            AddHandler p.receive_output, AddressOf output_received
            AddHandler p.receive_error, AddressOf error_received
            Dim ex As Exception = Nothing
            If Not p.start(ex) Then
                If Not ex Is Nothing Then
                    assertion.is_true(False, ex.Message())
                End If
                Return False
            End If
            assert(ex Is Nothing)
            If Not inputs() Is Nothing Then
                For Each s As String In inputs()
                    p.stdin().Write(s)
                Next
            End If
            If Not assertion.is_true(p.wait_for_exit(timeout_ms),
                                     process_name,
                                     " cannot finish in ",
                                     timeout_ms,
                                     " milliseconds") Then
                If Not p.quit(0) Then
                    utt_raise_error("failed to stop ", process_name)
                End If
            End If
            assertion.equal(p.exit_code(), expected_return())
            Return True
        End Using
    End Function
End Class
