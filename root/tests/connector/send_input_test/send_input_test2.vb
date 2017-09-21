
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public Class send_input_test2
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New case2())
    End Sub

    Private Class case2
        Inherits [case]

        <STAThread()>
        Public Overrides Function run() As Boolean
            Windows.Forms.Application.Run(New send_input_test_form())
            Return True
        End Function
    End Class
End Class
