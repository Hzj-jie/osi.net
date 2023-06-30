
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class priority_activity_test
    Inherits [case]

    Private Shared ReadOnly expected_output As String =
        strcat("Finished global_init.execute(", CByte(global_init_level.foundamental), ")")
    Private Shared ReadOnly priority_activity_name As String
    Private Shared ReadOnly priority_activity_exe_full_path As String
    Private Shared ReadOnly priority_activity_pdb_full_path As String

    Shared Sub New()
        priority_activity_name = guid_str()
        'priority_activity uses global_init
        priority_activity_exe_full_path = Path.Combine(application_directory,
                                                       strcat(priority_activity_name,
                                                              filesystem.extension_prefix,
                                                              filesystem.extensions.executable_file))
        priority_activity_pdb_full_path = Path.Combine(application_directory,
                                                       strcat(priority_activity_name,
                                                              filesystem.extension_prefix,
                                                              filesystem.extensions.program_database))
        assert(priority_activity_exe.sync_export_exec(priority_activity_exe_full_path))
        assert(priority_activity_pdb.sync_export(priority_activity_pdb_full_path))
        application_lifetime.stopping_handle(Sub()
                                                 File.Delete(priority_activity_exe_full_path)
                                                 File.Delete(priority_activity_pdb_full_path)
                                             End Sub)
    End Sub

    Public Overrides Function run() As Boolean
        enum_def(Of ProcessPriorityClass).foreach(
            Sub(ByVal ppc As ProcessPriorityClass, ByVal s As String)
                Using p As New shell_less_process()
                    p.start_info().FileName() = priority_activity_exe_full_path
                    p.start_info().Arguments() = s
                    If Not assertion.is_true(p.start()) Then
                        Return
                    End If
                    managed_thread_pool.with_timeout(Sub()
                                                         While True
                                                             Dim l As String = p.stdout().ReadLine()
                                                             If strsame(l, expected_output) Then
                                                                 Exit While
                                                             End If
                                                         End While
                                                         assertion.happening(Function() As Boolean
                                                                                 Return (+p).PriorityClass() = ppc
                                                                             End Function)
                                                         p.stdin().WriteLine()
                                                         If Not assertion.is_true(p.wait_for_exit(
                                                                seconds_to_milliseconds(10))) Then
                                                             p.quit()
                                                         End If
                                                     End Sub,
                                                     seconds_to_milliseconds(20))
                End Using
            End Sub)
        Return True
    End Function
End Class
