
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class window_size_test_form
    Private ReadOnly cb As Action(Of window_size_test_form)

    Public Sub New(ByVal cb As Action(Of window_size_test_form))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        assert(Not cb Is Nothing)
        Me.cb = cb
    End Sub

    Private Sub window_size_test_form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        cb(Me)
    End Sub
End Class