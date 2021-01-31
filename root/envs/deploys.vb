
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
    Public ReadOnly deploys_root As String = calculate_deploys_root()
    Public ReadOnly service_name As String = Path.GetFileName(application_directory)
    Public ReadOnly report_deploys_folder As Boolean = env_bool(env_keys("report", "deploys", "folder"))

    Public ReadOnly deploys_folder As String = deploys_root
    Public ReadOnly app_folder As String = Path.Combine(deploys_folder, app_folder_name)
    Public ReadOnly counter_folder As String = Path.Combine(deploys_folder, counter_folder_name)
    Public ReadOnly data_folder As String = Path.Combine(deploys_folder, data_folder_name)
    Public ReadOnly log_folder As String = Path.Combine(deploys_folder, log_folder_name)
    Public ReadOnly service_data_folder As String = Path.Combine(data_folder, service_name)

    <global_init(global_init_level.foundamental)>
    Private NotInheritable Class set_envs
        Private Shared Sub init()
            assert(Not strendwith(application_directory, Path.DirectorySeparatorChar, False))
            assert(Not String.IsNullOrEmpty(service_name))

            assert(set_env("deploys_root", deploys_root))
            assert(set_env("deploys_folder", deploys_folder))
            assert(set_env("service_name", service_name))
            assert(set_env("app_folder", app_folder))
            assert(set_env("counter_folder", counter_folder))
            assert(set_env("data_folder", data_folder))
            assert(set_env("log_folder", log_folder))
            assert(set_env("service_data_folder", service_data_folder))
        End Sub

        Private Sub New()
        End Sub
    End Class

    <global_init(global_init_level.other)>
    Private NotInheritable Class report_deploys_folder_delegator
        Private Shared Sub init()
            If report_deploys_folder Then
                raise_error("deploys_root ", deploys_root)
                raise_error("deploys_folder ", deploys_folder)
                raise_error("app_folder ", app_folder)
                raise_error("counter_folder ", counter_folder)
                raise_error("data_folder ", data_folder)
                raise_error("log_folder ", log_folder)
            End If
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Function calculate_deploys_root() As String
        Dim deploys_root As String
        If strsame(Path.GetFileName(Path.GetDirectoryName(application_directory)), app_folder_name, False) Then
            deploys_root = Path.GetDirectoryName(Path.GetDirectoryName(application_directory))
        Else
            deploys_root = Path.GetPathRoot(application_directory)
        End If
        assert(Not String.IsNullOrEmpty(deploys_root))
        Return deploys_root
    End Function

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
                                   this_process.ref.Id()))
    End Function
End Module
