
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public Class cstr_convert_tostring_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt64 = CULng(1024) * CULng(1024) * CULng(16)

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf cstr_run),
                   New delegate_case(AddressOf convert_tostring_run),
                   New delegate_case(AddressOf implicit_convert_run))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({344, 340, 340}, i, j)
    End Function

    Private Shared Sub cstr_run()
        Dim j As String = Nothing
        For i As UInt64 = 0 To CULng(size - 1)
            j = CStr(i)
        Next
    End Sub

    Private Shared Sub convert_tostring_run()
        Dim j As String = Nothing
        For i As UInt64 = 0 To CULng(size - 1)
            j = Convert.ToString(i)
        Next
    End Sub

    Private Shared Sub implicit_convert_run()
        Dim j As String = Nothing
        For i As UInt64 = 0 To CULng(size - 1)
            implicit_conversions.cstr_convert_tostring_perf_test_implicit_convert_run(j, i)
        Next
    End Sub
End Class
