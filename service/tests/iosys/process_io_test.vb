
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.resource
Imports osi.service.iosys

Public NotInheritable Class process_io_test
    Inherits [case]

    Public Shared ReadOnly process_io_exe_full_path As String
    Public Shared ReadOnly process_io_pdb_full_path As String
    Private Const output_times As Int32 = 1024 * 8

    Shared Sub New()
        Dim filename As String = Nothing
        filename = guid_str()
        process_io_exe_full_path = Path.Combine(temp_folder,
                                                strcat(filename,
                                                       filesystem.extension_prefix,
                                                       filesystem.extensions.executable_file))
        process_io_pdb_full_path = Path.Combine(temp_folder,
                                                strcat(filename,
                                                       filesystem.extension_prefix,
                                                       filesystem.extensions.program_database))
        assert(process_io_exe.sync_export_exec(process_io_exe_full_path))
        assert(process_io_pdb.sync_export(process_io_pdb_full_path))
    End Sub

    ' Let process_io_test to dump binary files.
    Public Shared Sub dump()
    End Sub

    Private ReadOnly are As AutoResetEvent
    Private from_err As singleentry
    Private last_input As String
    Private received_times As Int32

    Public Sub New()
        are = New AutoResetEvent(False)
    End Sub

    Private Function run_case() As Boolean
        Using io As process_io = New process_io()
            AddHandler io.output, AddressOf receive_output
            io.start_info().FileName() = process_io_exe_full_path
            If from_err.in_use() Then
                io.start_info().Arguments() += guid_str()
            End If
            If assertion.is_true(io.start()) Then
                For i As Int32 = 0 To output_times - 1
                    atomic.eva(last_input, rnd_en_chars(rnd_int(10, 100)))
                    assertion.is_true(io.input_received(last_input))
                    assertion.is_true(yield_wait(are, 100000))
                Next
            End If
            assertion.is_true(io.quit())
        End Using
        Return True
    End Function

    Public Overrides Function run() As Boolean
        from_err.mark_not_in_use()
        If Not run_case() Then
            Return False
        End If
        from_err.mark_in_use()
        If Not run_case() Then
            Return False
        End If
        Return True
    End Function

    Private Sub assert_output(ByVal input As String)
        If assertion.is_false(last_input.null_or_empty(), input) AndAlso
           assertion.equal(input, strcat(If(from_err.in_use(),
                                         process_io.error_prefix,
                                         process_io.out_prefix),
                                      strleft(last_input, 1))) Then
            last_input = strmid(last_input, 1)
            If last_input.null_or_empty() Then
                received_times += 1
                assert(are.Set())
            End If
        End If
    End Sub

    Private Sub receive_output(ByVal output As String)
        assert_output(output)
    End Sub

    Public Overrides Function finish() As Boolean
        'both from_err and from_out
        assertion.equal(received_times, output_times << 1)
        Return MyBase.finish()
    End Function
End Class
