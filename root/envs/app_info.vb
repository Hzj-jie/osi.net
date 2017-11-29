
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

Public Module _app_info
    Public ReadOnly application_full_path As String
    Public ReadOnly application_directory As String
    Public ReadOnly application_file_name As String
    Public ReadOnly application_name As String
    Public ReadOnly application_version As String
    Public ReadOnly application_short_version As String
    Public ReadOnly application_sign As String

    Sub New()
        If mono Then
            application_full_path = Reflection.Assembly.GetEntryAssembly().Location()
        Else
            application_full_path = current_process.MainModule().FileName()
        End If
        application_directory = Path.GetDirectoryName(application_full_path)
        application_file_name = Path.GetFileName(application_full_path)
        assert(Not String.IsNullOrEmpty(application_full_path))
        assert(Not String.IsNullOrEmpty(application_directory))
        assert(Not String.IsNullOrEmpty(application_file_name))
        application_name = current_assembly.GetName().Name()
        application_version = Convert.ToString(current_assembly.GetName().Version())
        application_short_version = strcat(Convert.ToString(current_assembly.GetName().Version().Major()),
                                           character.dot,
                                           Convert.ToString(current_assembly.GetName().Version().Minor()))
        application_sign = strcat(application_name, character.minus_sign, application_version)
    End Sub
End Module
