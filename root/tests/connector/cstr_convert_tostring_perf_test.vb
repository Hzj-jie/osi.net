
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class cstr_convert_tostring_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt64 = CULng(1024) * 1024 * 16

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf cstr_run),
                   New delegate_case(AddressOf convert_tostring_run),
                   New delegate_case(AddressOf implicit_convert_run))
    End Sub

    Protected Overrides Function max_rate_table() As Double(,)
        Return {{0, 1.1, 1.1},
                {-1, 0, -1},
                {1.1, 1.1, 0}}
    End Function

    Private Shared Sub cstr_run()
        Dim j As String = 0
        For i As UInt64 = 0 To CULng(size - 1)
            j = CStr(i)
        Next
    End Sub

    Private Shared Sub convert_tostring_run()
        Dim j As String = 0
        For i As UInt64 = 0 To CULng(size - 1)
            j = Convert.ToString(i)
        Next
    End Sub

    Private Shared Sub implicit_convert_run()
        Dim j As String = 0
        For i As UInt64 = 0 To CULng(size - 1)
            j = i
        Next
    End Sub
End Class
