
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class get_thread_process_id_perf
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(performance(New get_thread_process_id_case()))
    End Sub

    Private Class get_thread_process_id_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Windows.Forms.Application.Run(New native_window_test_form(
                Sub(ByVal f As native_window_test_form)
                    For i As Int32 = 0 To 10000000
                        Dim process_id As UInt32 = 0
                        Dim thread_id As UInt32 = 0
                        assertion.is_true(native_window.get_thread_process_id(f, process_id, thread_id))
                    Next
                End Sub))
            Return True
        End Function
    End Class
End Class
