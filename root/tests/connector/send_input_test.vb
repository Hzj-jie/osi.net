
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt

Public Class send_input_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New send_input_case())
    End Sub

    Private Class send_input_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            While True
                Dim code As UInt16 = 0
                Dim s As String = Nothing
                s = Console.ReadLine()
                If Not UInt16.TryParse(s, code) Then
                    Return True
                End If

                Dim e As send_input.keyboard_input = Nothing
                e = send_input.keyboard_input.from_scan_code(code)
                assert_equal(send_input.keyboard(e), uint32_1)
            End While

            assert(False)
            Return True
        End Function
    End Class
End Class
