
Option Explicit On
Option Infer Off
Option Strict Off

' This file contains expected implicit conversions.
Public NotInheritable Class implicit_conversions
    Public Shared Sub cint_convert_toint_perf_test_implicit_convert_run(ByRef o As Int32, ByVal i As UInt32)
        o = i
    End Sub

    Public Shared Sub cstr_convert_tostring_perf_test_implicit_convert_run(ByRef o As String, ByVal i As UInt64)
        o = i
    End Sub

    Private Sub New()
    End Sub
End Class
