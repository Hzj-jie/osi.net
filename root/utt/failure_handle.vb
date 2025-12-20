
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.utt

Friend Module failure_handle
    Private Sub utt_raise_error(ByVal msg() As Object, ByVal additional_jump As Int32)
        If Not selfhealth.in_stage() Then
            raise_error(error_type.other,
                        errortype_char,
                        additional_jump + 1,
                        If(current_case.is_null(), Nothing, {"[", current_case.[of]().full_name, "] - "}),
                        msg)
        End If
    End Sub

    Public Sub utt_raise_error(ByVal ParamArray msg() As Object)
        utt_raise_error(msg, 1)
    End Sub

    Public Sub failed(ByVal ParamArray reason() As Object)
        utt_raise_error({"failed to run utt cases, ", reason}, 1)
    End Sub
End Module
