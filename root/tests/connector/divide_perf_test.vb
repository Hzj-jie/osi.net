
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class divide_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt32 = 1024 * 1024 * 16
    Private Shared ReadOnly a As Int32
    Private Shared ReadOnly b As Int32
    Private Shared ReadOnly c As Int32

    Shared Sub New()
        a = rnd_int()
        While b = 0
            b = rnd_int()
        End While
        While c = 0 OrElse
              (CLng(a) - c < min_int32) OrElse
              (CLng(c) - a < min_int32) OrElse
              (CLng(a) - c > max_int32) OrElse
              (CLng(c) - a > max_int32)
            c = rnd_int()
        End While
    End Sub

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf divide),
                   New delegate_case(AddressOf minus),
                   New delegate_case(AddressOf empty))
    End Sub

    Private Shared Sub divide()
        For i As UInt32 = 0 To size - uint32_1
            Dim c As Int32 = 0
            c = a Mod b
            do_nothing(c = 0)
            c = a \ b
            do_nothing(c = 0)
        Next
    End Sub

    Private Shared Sub minus()
        For i As UInt32 = 0 To size - uint32_1
            Dim d As Int32 = 0
            d = a - c
            do_nothing(d = 0)
            d = c - a
            do_nothing(d = 0)
        Next
    End Sub

    Private Shared Sub empty()
        For i As UInt32 = 0 To size - uint32_1
            Dim d As Int32 = 0
            do_nothing(d = 0)
            do_nothing(d = 0)
        Next
    End Sub
End Class
