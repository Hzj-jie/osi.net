
#If Not NET8_0_OR_GREATER Then
Imports osi.root.utt

Public Class string_null_or_whitespace_perf
    Inherits exec_export_case

    Public Sub New()
        MyBase.New(string_null_or_whitespace_perf_exe)
    End Sub
End Class
#End If
