
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.foundamental)>
Public Module _app_info
    Public ReadOnly application_full_path As String =
        If(mono, Reflection.Assembly.GetEntryAssembly().Location(), this_process.ref.MainModule().FileName())
    Public ReadOnly application_directory As String = Path.GetDirectoryName(application_full_path)
    Public ReadOnly application_file_name As String = Path.GetFileName(application_full_path)
    Public ReadOnly application_name As String = current_assembly.GetName().Name()
    Public ReadOnly application_version As String = Convert.ToString(current_assembly.GetName().Version())
    Public ReadOnly application_short_version As String =
        strcat(current_assembly.GetName().Version().Major(),
               character.dot,
               current_assembly.GetName().Version().Minor())
    Public ReadOnly application_sign As String = strcat(application_name, character.minus_sign, application_version)

    Private Sub init()
        assert(Not application_full_path.null_or_empty())
        assert(Not application_directory.null_or_empty())
        assert(Not application_file_name.null_or_empty())
    End Sub
End Module
