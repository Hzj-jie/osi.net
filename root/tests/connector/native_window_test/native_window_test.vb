
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports Screen = System.Windows.Forms.Screen

Public Class native_window_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New window_size_case())
    End Sub

    Private Class window_size_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            assertion.is_true(dpi_awareness.set_aware())
            Windows.Forms.Application.Run(New native_window_test_form(
                    Sub(f As native_window_test_form)
                        f.ControlBox() = True
                        f.FormBorderStyle() = Windows.Forms.FormBorderStyle.Sizable
                        f.WindowState() = Windows.Forms.FormWindowState.Maximized
                        f.ControlBox() = True
                        f.MinimizeBox() = True
                        f.MaximizeBox() = True
                        f.Width() = 100
                        f.Height() = 100
                        f.WindowState() = Windows.Forms.FormWindowState.Maximized
                        Dim process_id As UInt32 = 0
                        Dim thread_id As UInt32 = 0
                        assertion.is_true(native_window.get_thread_process_id(f, process_id, thread_id))
                        assertion.equal(process_id, CUInt(osi.root.envs.current_process.Id()))
                        assertion.equal(thread_id, CUInt(osi.root.envs.current_process_thread_id()))
                        Console.WriteLine(strcat("Native API: process id: ", process_id, ", thread id: ", thread_id))
                        Dim rect As native_window.rect = Nothing
                        assertion.is_true(native_window.get_rect(f, rect))
                        Console.WriteLine(strcat("Native API: window rect width: ",
                                                 rect.width(),
                                                 ", height: ",
                                                 rect.height(),
                                                 ", left: ",
                                                 rect.left,
                                                 ", top: ",
                                                 rect.top))
                        assertion.is_true(native_window.get_client_rect(f, rect))
                        Console.WriteLine(strcat("Native API: client area width: ",
                                                 rect.width(),
                                                 ", height: ",
                                                 rect.height(),
                                                 ", left: ",
                                                 rect.left,
                                                 ", top: ",
                                                 rect.top))
                        Dim window As IntPtr = Nothing
                        assertion.is_true(native_window.get_window(f, native_window.get_window_command.hwnd_first, window))
                        Console.WriteLine(strcat("Native API: first window: ", window))
                        assertion.is_true(native_window.get_window(f, native_window.get_window_command.hwnd_last, window))
                        Console.WriteLine(strcat("Native API: last window: ", window))
                        assertion.is_true(native_window.get_window(f, native_window.get_window_command.hwnd_prev, window))
                        Console.WriteLine(strcat("Native API: previous window: ", window))
                        assertion.is_true(native_window.get_window(f, native_window.get_window_command.hwnd_next, window))
                        Console.WriteLine(strcat("Native API: next window: ", window))
                        Console.WriteLine(strcat("WinForms: width: ",
                                                 f.Width(),
                                                 ", height: ",
                                                 f.Height()))
                        Console.WriteLine(strcat("WinForms: current window: ", f.Handle()))
                        assertion.is_true(native_window.get_ancestor(f, native_window.get_ancestor_flag.root_owner, window))
                        Console.WriteLine(strcat("Native API: root-owner of current window: ", f.Handle()))
                        Dim screens() As Screen = Nothing
                        screens = Screen.AllScreens()
                        For i As Int32 = 0 To array_size_i(screens) - 1
                            Console.WriteLine(strcat("WinForms: screen rectangle: ", screens(i).Bounds()))
                        Next
                        Console.ReadKey()
                    End Sub))
            Return True
        End Function
    End Class
End Class
