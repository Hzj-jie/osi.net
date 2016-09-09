
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt

Public Class npos_uint_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const round As Int32 = 1024 * 1024
    Private Shared ReadOnly rnd_ints() As Int32

    Shared Sub New()
        ReDim rnd_ints(1024 - 1)
        For i As Int32 = 0 To CInt(array_size(rnd_ints)) - 1
            rnd_ints(i) = rnd_int()
        Next
    End Sub

    Public Sub New()
        MyBase.New(repeat(New delegate_case(AddressOf npos_uint_case), round),
                   repeat(New delegate_case(AddressOf int_case), round))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        If os.windows_major = os.windows_major_t._5 Then
            Return {{0, 25},
                    {0.08, 0}}
        Else
            Return {{0, 12},
                    {0.15, 0}}
        End If
    End Function

    Private Shared Sub npos_uint_case()
        Dim x As npos_uint = Nothing
        Dim y As UInt32 = 0
        Dim b As Boolean = False
        For i As UInt32 = 0 To array_size(rnd_ints) - uint32_1
            x = rnd_ints(i)
            b = (x > i)
            b = (x > rnd_ints(i))
            b = (x < i)
            b = (x < rnd_ints(i))
            b = (x = i)
            b = (x = rnd_ints(i))
            If Not x.npos() Then
                y = x.value()
            End If
            If Not x.npos() Then
                y = x.value()
            End If
        Next
    End Sub

    Private Shared Sub int_case()
        Dim x As Int32 = 0
        Dim y As UInt32 = 0
        Dim b As Boolean = False
        For i As UInt32 = 0 To array_size(rnd_ints) - uint32_1
            x = rnd_ints(i)
            b = (x > i)
            b = (x > rnd_ints(i))
            b = (x < i)
            b = (x < rnd_ints(i))
            b = (x = i)
            b = (x = rnd_ints(i))
            If x >= 0 Then
                y = CUInt(x)
            End If
            If x >= 0 Then
                y = CUInt(x)
            End If
        Next
    End Sub
End Class
