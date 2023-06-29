
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.log_and_counter_services)>
Public NotInheritable Class file_error_writer
    Private Shared ReadOnly writer As log_writer = New log_writer()

    Private NotInheritable Class log_writer
        Inherits application_info_writer

        Public Sub New()
            MyBase.New(envs.log_folder,
                       Function() As String
                           Dim log_file As String = Nothing
                           If Not envs.env_value(envs.env_keys("log", "file"), log_file) Then
                               Return Nothing
                           End If
                           Return log_file
                       End Function(),
                       "log")
        End Sub

        Public Shadows Function writer() As StreamWriter
            Return MyBase.writer()
        End Function
    End Class

    Private Shared Sub init()
        If envs.env_bool(envs.env_keys("no", "file", "log")) Then
            static_constructor(Of colorful_console_error_writer).execute()
            error_writer_ignore_types(Of colorful_console_error_writer).value_all()
            Return
        End If

        AddHandler error_event.r6,
                   Sub(ByVal err_type As error_type, ByVal err_type_char As Char, ByVal msg As String)
                       If error_writer_ignore_types(Of file_error_writer).valued(err_type, err_type_char) AndAlso
                          Not msg.null_or_empty() Then
                           msg = msg.Replace(character.newline, character.null) _
                                    .Replace(character.return, character.null)
                           Try
                               writer.writer().WriteLine(msg)
                           Catch ex As Exception
                               write_console_error_line("write log content ", msg, " failed, ex ", ex.details())
                           End Try
                       End If
                   End Sub
    End Sub

    Private Sub New()
    End Sub
End Class
