
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class native_window_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New window_size_case())
    End Sub

    Private Class window_size_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Windows.Forms.Application.Run(New native_window_test_form(
                    Sub(f As native_window_test_form)
                        f.ControlBox() = False
                        f.FormBorderStyle() = Windows.Forms.FormBorderStyle.None
                        f.WindowState() = Windows.Forms.FormWindowState.Normal
                        f.ControlBox() = False
                        f.MinimizeBox() = False
                        f.MaximizeBox() = False
                        f.Width() = 0
                        f.Height() = 0
                        f.WindowState() = Windows.Forms.FormWindowState.Minimized
                        Dim process_id As UInt32 = 0
                        Dim thread_id As UInt32 = 0
                        assert_true(native_window.get_thread_process_id(f, process_id, thread_id))
                        assert_equal(process_id, CUInt(osi.root.envs.current_process.Id()))
                        assert_equal(thread_id, CUInt(osi.root.envs.current_process_thread_id()))
                        Console.WriteLine(strcat("Native API: process id: ", process_id, ", thread id: ", thread_id))
                        Dim rect As native_window.rect = Nothing
                        assert_true(native_window.get_rect(f, rect))
                        Console.WriteLine(strcat("Native API: width: ",
                                                 rect.width(),
                                                 ", height: ",
                                                 rect.height(),
                                                 ", left: ",
                                                 rect.left,
                                                 ", top: ",
                                                 rect.top))
                        f.BeginInvoke(Sub()
                                          Console.WriteLine(strcat("WinForms: width: ",
                                                                   f.Width(),
                                                                   ", height: ",
                                                                   f.Height()))
                                      End Sub)
                    End Sub))
            Return True
        End Function
    End Class
End Class
