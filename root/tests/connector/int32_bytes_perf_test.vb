
Imports System.Runtime.InteropServices
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class int32_bytes_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt32 = 1024 * 1024 * 128

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf bitconverter_getbytes), New delegate_case(AddressOf marshal_write))
    End Sub

    Protected Overrides Function max_rate_table() As Double(,)
        Return {{-1, 0.5},
                {-1, -1}}
    End Function

    Private Shared Sub bitconverter_getbytes()
        Dim d() As Byte = Nothing
        ReDim d(sizeof_int32 - 1)
        Dim t As Int32 = 0
        t = rnd_int()
        For i As UInt32 = 0 To size - 1
            Dim x() As Byte = Nothing
            x = BitConverter.GetBytes(t)
            memcpy(d, x)
        Next
    End Sub

    Private Shared Sub marshal_write()
        Dim d() As Byte = Nothing
        ReDim d(sizeof_int32 - 1)
        Dim t As Int32 = 0
        t = rnd_int()
        For i As UInt32 = 0 To size - 1
            Marshal.WriteInt32(d, 0, t)
        Next
    End Sub
End Class
