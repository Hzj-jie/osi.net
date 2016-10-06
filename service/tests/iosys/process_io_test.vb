
Imports System.IO
Imports System.Threading
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.resource
Imports osi.service.iosys

Public Class process_io_test
    Inherits [case]

    Private Const output_times As Int32 = 1024 * 8
    Private Shared ReadOnly process_io_exe_full_path As String

    Shared Sub New()
        process_io_exe_full_path = Path.Combine(temp_folder,
                                                strcat(guid_str(),
                                                       filesystem.extension_prefix,
                                                       filesystem.extensions.executable_file))
        assert(process_io_exe.sync_export_exec(process_io_exe_full_path))
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
            If assert_true(io.start()) Then
                For i As Int32 = 0 To output_times - 1
                    atomic.eva(last_input, rnd_en_chars(rnd_int(10, 100)))
                    assert_true(io.input_received(last_input))
                    assert_true(yield_wait(are, 10000))
                Next
            End If
            assert_true(io.quit())
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
        If assert_false(String.IsNullOrEmpty(last_input), input) AndAlso
           assert_equal(input, strcat(If(from_err.in_use(),
                                         process_io.error_prefix,
                                         process_io.out_prefix),
                                      strleft(last_input, 1))) Then
            last_input = strmid(last_input, 1)
            If String.IsNullOrEmpty(last_input) Then
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
        assert_equal(received_times, output_times << 1)
        Return MyBase.finish()
    End Function
End Class
