
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class native_window_test_form
    Private ReadOnly cb As Action(Of native_window_test_form)

    Public Sub New(ByVal cb As Action(Of native_window_test_form))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        assert(Not cb Is Nothing)
        Me.cb = cb
    End Sub

    Private Sub native_window_test_form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cb(Me)
        Close()
    End Sub
End Class