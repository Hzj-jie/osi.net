
Imports osi.root.constants.system_perf

Public Class integer_operator
    Private Shared Sub int8_run()
        Const inc As Byte = 7
        Dim j As Byte = 0
        For i As Int32 = 0 To (integer_operator_size >> 2) - 1
            j += inc
            j = 0
        Next
    End Sub

    Private Shared Sub int16_run()
        Const inc As Int16 = 7
        Dim j As Int16 = 0
        For i As Int32 = 0 To (integer_operator_size >> 1) - 1
            j += inc
            j = 0
        Next
    End Sub

    Private Shared Sub int32_run()
        Const inc As Int32 = 7
        Dim j As Int32 = 0
        For i As Int32 = 0 To integer_operator_size - 1
            j += inc
            j = 0
        Next
    End Sub

    Private Shared Sub int64_run()
        Const inc As Int64 = 7
        Dim j As Int64 = 0
        For i As Int32 = 0 To (integer_operator_size >> 1) - 1
            j += inc
            j = 0
        Next
    End Sub

    Public Shared Sub run()
        Dim j As Int32 = 0
        For i As Int32 = 0 To integer_operator_size - 1
            j += 7
        Next
    End Sub
End Class
