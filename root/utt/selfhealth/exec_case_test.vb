
Option Explicit On
Option Infer Off
Option Strict On

Public Class exec_case_test
    Inherits exec_export_case

    Private out_rec As Boolean
    Private err_rec As Boolean

    Public Sub New(ByVal arg As String)
        MyBase.New(exec_case_exe, arg)
        attach_receive_output(AddressOf output_received)
        attach_receive_error(AddressOf error_received)
        out_rec = False
        err_rec = False
    End Sub

    Public Sub New()
        Me.New("true")
    End Sub

    Private Sub output_received(ByVal s As String)
        assert_equal(s, "output")
        out_rec = True
    End Sub

    Private Sub error_received(ByVal s As String)
        assert_equal(s, "output")
        err_rec = True
    End Sub

    Public NotOverridable Overrides Function finish() As Boolean
        assert_true(out_rec)
        assert_false(err_rec)
        Return MyBase.finish()
    End Function
End Class

Public Class exec_case_test2
    Inherits exec_export_case

    Private out_rec As Boolean
    Private err_rec As Boolean

    Public Sub New()
        MyBase.New(exec_case_exe, "false", expected_return:=-1)
        attach_receive_output(AddressOf output_received)
        attach_receive_error(AddressOf error_received)
        out_rec = False
        err_rec = False
    End Sub

    Private Sub output_received(ByVal s As String)
        assert_equal(s, "output")
        out_rec = True
    End Sub

    Private Sub error_received(ByVal s As String)
        assert_equal(s, "error")
        err_rec = True
    End Sub

    Public NotOverridable Overrides Function finish() As Boolean
        assert_false(out_rec)
        assert_true(err_rec)
        Return MyBase.finish()
    End Function
End Class
