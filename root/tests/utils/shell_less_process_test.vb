
Imports System.ComponentModel
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.threadpool
Imports osi.root.utt

Public Class shell_less_process_test
    Inherits [case]

    Private Shared ReadOnly shell_less_process_test_exe_full_path As String
    Private Shared ReadOnly shell_less_process_test_pdb_full_path As String
    Private Shared ReadOnly shell_less_process_test_name As String

    Shared Sub New()
        shell_less_process_test_name = guid_str()
        shell_less_process_test_exe_full_path = Path.Combine(temp_folder,
                                                             strcat(shell_less_process_test_name,
                                                                    filesystem.extension_prefix,
                                                                    filesystem.extensions.executable_file))
        shell_less_process_test_pdb_full_path = Path.Combine(temp_folder,
                                                             strcat(shell_less_process_test_name,
                                                                    filesystem.extension_prefix,
                                                                    filesystem.extensions.program_database))
        assert(shell_less_process_test_exe.sync_export_exec(shell_less_process_test_exe_full_path))
        assert(shell_less_process_test_pdb.sync_export(shell_less_process_test_pdb_full_path))
    End Sub

    Private Shared Function raise_on_invoke_case() As Boolean
        Dim t As slimqless2_runner = Nothing
        t = New slimqless2_runner()
        Dim p As shell_less_process = Nothing
        p = New shell_less_process(
                    True,
                    implementation_of(Of ISynchronizeInvoke).from_instance(New slimqless2_runner_synchronize_invoke(t)))
        p.start_info().FileName() = shell_less_process_test_exe_full_path
        AddHandler p.receive_error, Sub()
                                        assertion.is_true(t.running_in_current_thread())
                                    End Sub
        AddHandler p.receive_output, Sub()
                                         assertion.is_true(t.running_in_current_thread())
                                     End Sub
        AddHandler p.process_exit, Sub()
                                       assertion.is_true(t.running_in_current_thread())
                                   End Sub
        If assertion.is_true(p.start()) Then
            For i As Int32 = 0 To 100
                p.stdin().WriteLine(Convert.ToString(i))
            Next
            assertion.is_true(p.queue_quit())
            p.wait_for_exit()
            ' wait_for_exit() won't guarantee the process_exit() event has been raised, since it may run in another
            ' thread.
            assertion.is_true(lazy_sleep_wait_until(Function() p.exited(), second_milli))
        End If
        assert(t.stop())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return raise_on_invoke_case()
    End Function
End Class
