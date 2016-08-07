
Imports System.IO
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.log_and_counter_services)>
Public Class file_error_writer
    Private Shared ReadOnly log_file As String
    Private Shared ReadOnly writer As log_writer

    Private Class log_writer
        Inherits application_info_writer

        Public Sub New()
            MyBase.New(log_folder, log_file, "log")
        End Sub

        Public Shadows Function writer() As StreamWriter
            Return MyBase.writer()
        End Function
    End Class

    Shared Sub New()
        If envs.env_bool(envs.env_keys("no", "file", "log")) Then
            static_constructor(Of colorful_console_error_writer).execute()
            error_writer_ignore_types(Of colorful_console_error_writer).value_all()
            Return
        End If

        If Not envs.env_value(envs.env_keys("log", "file"), log_file) Then
            log_file = Nothing
        End If
        writer = New log_writer()
        AddHandler error_event.R6,
                   Sub(err_type As error_type, err_type_char As Char, msg As String)
                       If error_writer_ignore_types(Of file_error_writer).valued(err_type, err_type_char) Then
                           Try
                               writer.writer().WriteLine(msg)
                           Catch ex As Exception
                               write_console_error_line("write log content ", msg, " failed, ex ", ex.Message())
                           End Try
                       End If
                   End Sub
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
