
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class cint_convert_toint_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt64 = CULng(1024) * 1024 * 1024 * 16

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf cint_run),
                   New delegate_case(AddressOf convert_toint_run),
                   New delegate_case(AddressOf implicit_convert_run))
    End Sub

    Protected Overrides Function max_rate_table() As Double(,)
        Return {{0, 0.3, 1.1},
                {-1, 0, -1},
                {1.1, 0.3, 0}}
    End Function

    Private Shared Sub cint_run()
        Dim i As UInt32 = 0
        i = rnd_uint(0, max_int32)
        Dim k As Int32 = 0
        For j As UInt64 = 0 To CULng(size - 1)
            k = CInt(i)
        Next
    End Sub

    Private Shared Sub convert_toint_run()
        Dim i As UInt32 = 0
        i = rnd_uint(0, max_int32)
        Dim k As Int32 = 0
        For j As UInt64 = 0 To CULng(size - 1)
            k = Convert.ToInt32(i)
        Next
    End Sub

    Private Shared Sub implicit_convert_run()
        Dim i As UInt32 = 0
        i = rnd_uint(0, max_int32)
        Dim k As Int32 = 0
        For j As UInt64 = 0 To CULng(size - 1)
            k = i
        Next
    End Sub
End Class
