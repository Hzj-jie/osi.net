
Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.constants.utt
Imports osi.root.connector

Friend Module failure_handle
    Public Sub utt_raise_error(ByVal ParamArray msg() As Object)
        If Not self_health_stage() Then
            raise_error(error_type.other, errortype_char, msg)
        End If
    End Sub

    Public Sub failed(ByVal ParamArray reason() As Object)
        utt_raise_error("failed to run utt cases, ", reason)
    End Sub
End Module
