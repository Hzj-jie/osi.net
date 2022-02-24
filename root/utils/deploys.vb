
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

<global_init(global_init_level.foundamental)>
Public Module deploys
    Public Const temp_folder_name As String = "temp"
    Public ReadOnly temp_folder As String = Path.Combine(deploys_folder, temp_folder_name, application_name, guid_str())

    Private Sub init()
        Try
            assert(Not Directory.CreateDirectory(temp_folder) Is Nothing)
        Catch ex As Exception
            assert(False, ex.Message())
        End Try
        application_lifetime.stopping_handle(Sub()
                                                 Directory.Delete(temp_folder, True)
                                             End Sub)
        assert(set_env("temp_folder", temp_folder))
    End Sub

    <global_init(global_init_level.other)>
    Private NotInheritable Class report
        Private Shared Sub init()
            If report_deploys_folder Then
                raise_error("temp_folder ", temp_folder)
            End If
        End Sub

        Private Sub New()
        End Sub
    End Class
End Module
