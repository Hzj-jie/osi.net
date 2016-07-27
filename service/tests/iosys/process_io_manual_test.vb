
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.resource
Imports osi.service.iosys

Public Class process_io_manual_test
    Inherits commandline_specific_case_wrapper

    Private Shared ReadOnly process_io_exe_full_path As String

    Shared Sub New()
        process_io_exe_full_path = Path.Combine(temp_folder,
                                                strcat(guid_str(),
                                                       filesystem.extension_prefix,
                                                       filesystem.extensions.executable_file))
        assert(process_io_exe.sync_export_exec(process_io_exe_full_path))
    End Sub

    Public Sub New()
        MyBase.New(New process_io_manual_case())
    End Sub

    Private Class process_io_manual_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Using io As process_io = New process_io()
                AddHandler io.output, Sub(input As String)
                                          Console.WriteLine(input)
                                      End Sub
                io.start_info().FileName() = process_io_exe_full_path
                If rnd_bool() Then
                    io.start_info().Arguments() += guid_str()
                End If
                If assert_true(io.start()) Then
                    Dim s As String = Nothing
                    s = Console.ReadLine()
                    While Not s Is Nothing
                        assert_true(io.input_received(s))
                        s = Console.ReadLine()
                    End While
                End If
                assert_true(io.quit())
            End Using
            Return True
        End Function
    End Class
End Class
