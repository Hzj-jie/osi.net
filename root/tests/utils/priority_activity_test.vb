
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.IO
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public Class priority_activity_test
    Inherits [case]

    Private Shared ReadOnly priority_activity_exe_full_path As String
    Private Shared ReadOnly priority_activity_pdb_full_path As String
    Private Shared ReadOnly priority_activity_name As String

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
        assert(enum_traversal(Of ProcessPriorityClass)(
                   Sub(ppc As ProcessPriorityClass, s As String)
                       Using p As shell_less_process = New shell_less_process()
                           p.start_info().FileName() = priority_activity_exe_full_path
                           p.start_info().Arguments() = s
                           If assert_true(p.start()) Then
                               assert_true(execute_in_managed_threadpool(
                                               Sub()
                                                   While True
                                                       Dim l As String = Nothing
                                                       l = p.stdout().ReadLine()
                                                       If strsame(l, "Finished global_init.execute(0)") Then
                                                           Exit While
                                                       End If
                                                   End While
                                                   assert_equal((+p).PriorityClass(), ppc)
                                                   p.stdin().WriteLine()
                                                   If Not assert_true(p.wait_for_exit(
                                                              seconds_to_milliseconds(10))) Then
                                                       p.quit()
                                                   End If
                                               End Sub,
                                               seconds_to_milliseconds(10)))
                           End If
                           p.dispose()
                       End Using
                   End Sub))
        Return True
    End Function
End Class
