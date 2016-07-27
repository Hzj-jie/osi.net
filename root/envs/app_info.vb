
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

Public Module _app_info
    Public ReadOnly application_directory As String = Nothing
    Public ReadOnly application_name As String = Nothing
    Public ReadOnly application_version As String = Nothing
    Public ReadOnly application_short_version As String = Nothing
    Public ReadOnly application_sign As String = Nothing

    Sub New()
        application_directory = Path.GetDirectoryName(current_process.MainModule().FileName())
        assert(Not String.IsNullOrEmpty(application_directory))
        application_name = current_assembly.GetName().Name()
        application_version = Convert.ToString(current_assembly.GetName().Version())
        application_short_version = strcat(Convert.ToString(current_assembly.GetName().Version().Major()),
                                           character.dot,
                                           Convert.ToString(current_assembly.GetName().Version().Minor()))
        application_sign = strcat(application_name, character.minus_sign, application_version)
    End Sub
End Module
