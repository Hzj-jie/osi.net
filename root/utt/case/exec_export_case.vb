
Imports System.IO
Imports osi.root.envs
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.connector
Imports osi.root.procedure

Public Class exec_export_case
    Inherits exec_case

    Private ReadOnly b() As Byte

    Public Sub New(ByVal b() As Byte,
                   Optional ByVal arg As String = default_arg,
                   Optional ByVal ignore_output As Boolean = default_ignore_output,
                   Optional ByVal ignore_error As Boolean = default_ignore_error,
                   Optional ByVal expected_return As Int32 = default_expected_return,
                   Optional ByVal export_to_temp_folder As Boolean = True)
        MyBase.New(Path.Combine(If(export_to_temp_folder, temp_folder, app_folder),
                                strcat(guid_str(),
                                       filesystem.extension_prefix,
                                       filesystem.extensions.executable_file)),
                   arg,
                   ignore_output,
                   ignore_error,
                   expected_return)
        Me.b = b
    End Sub

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso
               b.sync_export_exec(exec_file)
    End Function

    Public Overrides Function finish() As Boolean
        Return do_(Function() As Boolean
                       File.Delete(exec_file)
                       Return True
                   End Function,
                   False) AndAlso
               MyBase.finish()
    End Function
End Class
