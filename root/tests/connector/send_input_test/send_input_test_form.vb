
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Windows.Forms
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt

Public Class send_input_test_form
    Private Sub send_input_test2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        textbox.AppendText(strcat("code ",
                                  e.KeyCode(),
                                  ", data ",
                                  e.KeyData(),
                                  ", value ",
                                  e.KeyValue(),
                                  newline.incode()))
    End Sub

    Private Sub textbox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles textbox.Click
        Dim s As String = Nothing
        s = Microsoft.VisualBasic.Interaction.InputBox("Input a scan code")
        Dim code As UInt16 = 0
        If UInt16.TryParse(s, code) Then
            assertion.equal(send_input.keyboard(send_input.keyboard_input.from_scan_code(code)), uint32_2)
        End If
    End Sub

    Private Sub textbox_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles textbox.KeyDown
        send_input_test2_KeyDown(sender, e)
    End Sub
End Class