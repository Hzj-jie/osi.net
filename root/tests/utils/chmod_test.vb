
Imports System.Diagnostics
Imports System.IO
Imports System.IO.Compression
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.procedure
Imports envs = osi.root.envs

Public Class chmod_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        If envs.mono Then
            Dim file_name As String = Nothing
            file_name = Path.Combine(temp_folder,
                                     strcat(guid_str(),
                                            filesystem.extension_prefix,
                                            filesystem.extensions.executable_file))
            If assertion.is_true(chmod_test_exe.sync_export(file_name)) AndAlso
               assertion.is_true(File.Exists(file_name)) AndAlso
               assertion.is_true(chmod_exe(file_name)) Then
                Dim p As shell_less_process = Nothing
                p = New shell_less_process()
                Dim s1 As String = Nothing
                s1 = guid_str()
                Dim s2 As String = Nothing
                s2 = guid_str()
                p.start_info().FileName() = file_name
                p.start_info().Arguments() = strcat(s1, character.blank, s2)
                Dim ex As Exception = Nothing
                assertion.is_true(p.start(ex))
                assertion.is_null(ex, If(ex Is Nothing, Nothing, ex.Message()))
                p.wait_for_exit()
                ' the output contains an \r\n
                assertion.equal(p.stdout_str(),
                             strcat(s1, newline.incode(), s2, newline.incode()))
                p.dispose()
            End If
        End If
        Return True
    End Function
End Class
