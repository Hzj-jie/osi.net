
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.utt

Public NotInheritable Class cint_convert_toint_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt64 = CULng(1024) * CULng(1024) * CULng(1024) * CULng(16)

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf cint_run),
                   New delegate_case(AddressOf convert_toint_run),
                   New delegate_case(AddressOf implicit_convert_run))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If os.windows_major <= os.windows_major_t._5 Then
            Return loosen_bound({448, 973, 447}, i, j)
        End If
        If os.windows_major <= os.windows_major_t._6 Then
            Return loosen_bound({306, 817, 310}, i, j)
        End If
        Return loosen_bound({150, 778, 173}, i, j)
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
            implicit_conversions.cint_convert_toint_perf_test_implicit_convert_run(k, i)
        Next
    End Sub
End Class
