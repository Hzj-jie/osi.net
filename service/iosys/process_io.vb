
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.resource
Imports osi.service.streamer

Friend Class process_io_wrapper
    Public Shared ReadOnly process_io_wrapper_exe_full_path As String
    Public Shared ReadOnly process_io_wrapper_pdb_full_path As String
    Private Shared ReadOnly process_io_wrapper_name As String

    Private Sub New()
    End Sub

    Shared Sub New()
        ' process_io_wrapper need to work with osi.root.connector._c_escape.escape method
        process_io_wrapper_name = guid_str()
        process_io_wrapper_exe_full_path = Path.Combine(application_directory,
                                                        strcat(process_io_wrapper_name,
                                                               filesystem.extension_prefix,
                                                               filesystem.extensions.executable_file))
        process_io_wrapper_pdb_full_path = Path.Combine(application_directory,
                                                        strcat(process_io_wrapper_name,
                                                               filesystem.extension_prefix,
                                                               filesystem.extensions.program_database))
        assert(process_io_wrapper_exe.sync_export_exec(process_io_wrapper_exe_full_path))
        assert(process_io_wrapper_pdb.sync_export(process_io_wrapper_pdb_full_path))
        application_lifetime.stopping_handle(Sub()
                                                 File.Delete(process_io_wrapper_exe_full_path)
                                                 File.Delete(process_io_wrapper_pdb_full_path)
                                             End Sub)
    End Sub
End Class

Public Class process_io
    Inherits process_io(Of String, String)

    Public Const out_prefix As String = "O:"
    Public Const error_prefix As String = "E:"

    Protected NotOverridable Overrides Function input_to_string(ByVal input As String) As String
        Return input
    End Function

    Protected NotOverridable Overrides Function string_to_output(ByVal s As String,
                                                                 ByVal from_err As Boolean) As String
        Return strcat(If(from_err, error_prefix, out_prefix), s)
    End Function
End Class

Public MustInherit Class process_io(Of INPUT_T, OUTPUT_T)
    Implements IDisposable

    Public Event output(ByVal o As OUTPUT_T)
    Private ReadOnly process As shell_less_process

    Public Sub New()
        process = New shell_less_process(True)
        AddHandler process.receive_output, AddressOf receive_from_output
        AddHandler process.receive_error, AddressOf receive_from_error
    End Sub

    Public Function start_info() As ProcessStartInfo
        Return process.start_info()
    End Function

    Public Function start() As Boolean
        start_info().Arguments() = strcat(start_info().FileName(),
                                          character.blank,
                                          start_info().Arguments())
        start_info().FileName() = process_io_wrapper.process_io_wrapper_exe_full_path
        Dim ex As Exception = Nothing
        If process.start(ex) Then
            Return assert(ex Is Nothing)
        Else
            raise_error(error_type.warning,
                        "failed to start process ",
                        start_info().FileName(),
                        " with argument ",
                        start_info().Arguments(),
                        If(Not ex Is Nothing, strcat(", ex ", ex.Message()), Nothing))
            Return False
        End If
    End Function

    Protected MustOverride Function input_to_string(ByVal input As INPUT_T) As String

    Public Function input_received(ByVal input As INPUT_T) As Boolean
        Dim s As String = Nothing
        s = input_to_string(input)
        process.stdin().Write(s)
        Return True
    End Function

    Public Function quit(ByVal wait_ms As Int64) As Boolean
        Return process.quit(wait_ms)
    End Function

    Public Function quit() As Boolean
        Return process.quit()
    End Function

    Public Function queue_quit(ByVal wait_ms As Int64,
                               Optional ByVal quit_result As ref(Of Boolean) = Nothing) As Boolean
        Return process.queue_quit(wait_ms, quit_result)
    End Function

    Public Function queue_quit(Optional ByVal quit_result As ref(Of Boolean) = Nothing) As Boolean
        Return process.queue_quit(quit_result)
    End Function

    Protected Overridable Function string_to_output(ByVal s As String, ByVal from_err As Boolean) As OUTPUT_T
        assert(False)
        Return Nothing
    End Function

    Protected Overridable Sub receive_output(ByVal s As String, ByVal from_err As Boolean)
        Dim o As OUTPUT_T = Nothing
        o = string_to_output(s, from_err)
        RaiseEvent output(o)
    End Sub

    Private Sub receive(ByVal s As String, ByVal from_err As Boolean)
        receive_output(s.c_unescape(), from_err)
    End Sub

    Private Sub receive_from_output(ByVal s As String)
        receive(s, False)
    End Sub

    Private Sub receive_from_error(ByVal s As String)
        receive(s, True)
    End Sub

    Public Function process_exited() As Boolean
        Return process.exited()
    End Function

    Public Sub IDisposable_Dispose() Implements IDisposable.Dispose
        process.dispose()
    End Sub

    Public Sub dispose()
        process.dispose()
    End Sub
End Class
