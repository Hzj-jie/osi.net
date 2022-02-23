
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.iosys

Public Class process_io_manual_test
    Inherits commandline_specified_case_wrapper

    Shared Sub New()
        process_io_test.dump()
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
                io.start_info().FileName() = process_io_test.process_io_exe_full_path
                If rnd_bool() Then
                    io.start_info().Arguments() += guid_str()
                End If
                If assertion.is_true(io.start()) Then
                    Dim s As String = Nothing
                    s = Console.ReadLine()
                    While s IsNot Nothing
                        assertion.is_true(io.input_received(s))
                        s = Console.ReadLine()
                    End While
                End If
                assertion.is_true(io.quit())
            End Using
            Return True
        End Function
    End Class
End Class
