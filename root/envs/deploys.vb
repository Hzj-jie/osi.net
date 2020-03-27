
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.connector

Public Module deploys
    Public Const deploys_folder_name As String = "deploys"
    Public Const app_folder_name As String = "apps"
    Public Const counter_folder_name As String = "counter"
    Public Const data_folder_name As String = "data"
    Public Const log_folder_name As String = "log"
    Public ReadOnly deploys_root As String = Nothing
    Public ReadOnly service_name As String = Nothing
    Public ReadOnly report_deploys_folder As Boolean = False

    Public ReadOnly deploys_folder As String = Nothing
    Public ReadOnly app_folder As String = Nothing
    Public ReadOnly counter_folder As String = Nothing
    Public ReadOnly data_folder As String = Nothing
    Public ReadOnly log_folder As String = Nothing
    Public ReadOnly service_data_folder As String = Nothing

    Sub New()
        assert(Not strendwith(application_directory, Path.DirectorySeparatorChar, False))
        service_name = Path.GetFileName(application_directory)
        assert(Not String.IsNullOrEmpty(service_name))
        If strsame(Path.GetFileName(Path.GetDirectoryName(application_directory)), app_folder_name, False) Then
            deploys_root = Path.GetDirectoryName(Path.GetDirectoryName(application_directory))
        Else
            deploys_root = Path.GetPathRoot(application_directory)
        End If
        assert(Not String.IsNullOrEmpty(deploys_root))

        copy(deploys_folder, deploys_root)
        app_folder = Path.Combine(deploys_folder, app_folder_name)
        counter_folder = Path.Combine(deploys_folder, counter_folder_name)
        data_folder = Path.Combine(deploys_folder, data_folder_name)
        log_folder = Path.Combine(deploys_folder, log_folder_name)
        service_data_folder = Path.Combine(data_folder, service_name)

        assert(set_env("deploys_root", deploys_root))
        assert(set_env("deploys_folder", deploys_folder))
        assert(set_env("service_name", service_name))
        assert(set_env("app_folder", app_folder))
        assert(set_env("counter_folder", counter_folder))
        assert(set_env("data_folder", data_folder))
        assert(set_env("log_folder", log_folder))
        assert(set_env("service_data_folder", service_data_folder))

        report_deploys_folder = env_bool(env_keys("report", "deploys", "folder"))

        If report_deploys_folder Then
            raise_error("deploys_root ", deploys_root)
            raise_error("deploys_folder ", deploys_folder)
            raise_error("app_folder ", app_folder)
            raise_error("counter_folder ", counter_folder)
            raise_error("data_folder ", data_folder)
            raise_error("log_folder ", log_folder)
        End If
    End Sub

    Public Function application_info_output_filename() As String
        Return Path.Combine(application_name,
                            strcat(application_sign,
                                   character.underline,
                                   source_control.current.id,
                                   character.underline,
                                   service_name,
                                   character.underline,
                                   short_time(character.underline, character.minus_sign, character.minus_sign),
                                   character.underline,
                                   current_process.Id()))
    End Function
End Module
