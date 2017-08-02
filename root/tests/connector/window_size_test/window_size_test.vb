
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class window_size_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New window_size_case())
    End Sub

    Private Class window_size_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Windows.Forms.Application.Run(New window_size_test_form(
                    Sub(f As window_size_test_form)
                        f.ControlBox() = False
                        f.FormBorderStyle() = Windows.Forms.FormBorderStyle.None
                        f.WindowState() = Windows.Forms.FormWindowState.Normal
                        f.ControlBox() = False
                        f.MinimizeBox() = False
                        f.MaximizeBox() = False
                        f.Width() = 0
                        f.Height() = 0
                        f.BeginInvoke(Sub()
                                          Console.WriteLine(strcat("width: ", f.Width(), ", height: ", f.Height()))
                                          f.Close()
                                      End Sub)
                    End Sub))
            Return True
        End Function
    End Class
End Class
