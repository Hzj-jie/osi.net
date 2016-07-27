
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports osi.root.connector
Imports osi.root.utils

Public Class exec_case
    Inherits [case]

    Protected Const default_arg As String = Nothing
    Protected Const default_ignore_output As Boolean = True
    Protected Const default_ignore_error As Boolean = False
    Protected Const default_expected_return As Int32 = 0
    Protected ReadOnly ignore_output As Boolean
    Protected ReadOnly ignore_error As Boolean
    Protected ReadOnly exec_file As String
    Protected ReadOnly exec_arg As String
    Protected ReadOnly expected_return As Int32
    Protected Event receive_output(ByVal s As String)
    Protected Event receive_error(ByVal s As String)

    Public Sub New(ByVal file As String,
                   Optional ByVal arg As String = default_arg,
                   Optional ByVal ignore_output As Boolean = default_ignore_output,
                   Optional ByVal ignore_error As Boolean = default_ignore_error,
                   Optional ByVal expected_return As Int32 = default_expected_return)
        assert(Not String.IsNullOrEmpty(file))
        Me.exec_file = file
        Me.exec_arg = arg
        Me.ignore_output = ignore_output
        Me.ignore_error = ignore_error
        Me.expected_return = expected_return
    End Sub

    Protected Overridable Function inputs() As IEnumerable(Of String)
        Return Nothing
    End Function

    Protected Sub attach_receive_output(ByVal v As receive_outputEventHandler)
        AddHandler receive_output, v
    End Sub

    Protected Sub attach_receive_error(ByVal v As receive_errorEventHandler)
        AddHandler receive_error, v
    End Sub

    Private Sub received(ByVal s As String, ByVal output As Boolean)
        If (output AndAlso Not ignore_output) OrElse
           (Not output AndAlso Not ignore_error) Then
            utt_raise_error("process ",
                            exec_file,
                            If(Not String.IsNullOrEmpty(exec_arg), strcat(" with argument ", exec_arg), Nothing),
                            " ",
                            If(output, "outputs", "outputs error"),
                            " { ",
                            s,
                            "}")
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

    Public NotOverridable Overrides Function run() As Boolean
        If File.Exists(exec_file) Then
            Using p As shell_less_process = New shell_less_process(True)
                p.start_info().FileName() = exec_file
                p.start_info().Arguments() = exec_arg
                AddHandler p.receive_output, AddressOf output_received
                AddHandler p.receive_error, AddressOf error_received
                Dim ex As Exception = Nothing
                If p.start(ex) Then
                    assert(ex Is Nothing)
                    If Not inputs() Is Nothing Then
                        For Each s In inputs()
                            p.stdin().Write(s)
                        Next
                    End If
                    p.wait_for_exit()
                    Return p.exit_code() = expected_return
                Else
                    If Not ex Is Nothing Then
                        assert_true(False, ex.Message())
                    End If
                    Return False
                End If
            End Using
        Else
            Return False
        End If
    End Function
End Class
