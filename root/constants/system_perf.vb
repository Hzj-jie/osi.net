
Public Class system_perf
    Private Const ir As Double = 1.5
    Private Const irl2 As Double = 1
    Public Const ratio As Int64 = 58982400 / 43 * 48 / 34 * 30 / 8 * 7
    Public Const atomic_operator_size As Int32 = 32768 \ ir
    Public Const fibonacci_size As Int32 = 24 - irl2
#If 0 Then
    Public Const thread_static_size As Int32 = 32768 \ ir
#End If
    Public Const memory_access_size As Int32 = 64 \ ir
    Public Const memory_access_memory_size As Int32 = 16777216
    Public Const integer_operator_size As Int32 = 16777216 \ ir
    Public Const float_operator_size As Int32 = 65536 \ ir
End Class
