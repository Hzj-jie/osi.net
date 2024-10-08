
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports os = osi.root.envs.os

Friend Module inlinable_extension_method
    <Extension()> Public Sub ext_method(ByRef j As Int32, ByVal r1 As Int32, ByVal r2 As Int32)
        j = r1 + r2
        j = r1 - r2
    End Sub
End Module

' On Windows XP / 2003 or maybe earlier, extension method is at least 2x slower than a regular function.
' On Windows 7 or upper, maybe Vista, extension method is ~30% slower than a regular function.
Public NotInheritable Class inlinable_perf
    Inherits performance_comparison_case_wrapper

    Private Const size As UInt64 = 1024 * 1024 * 1024
    Private Shared ReadOnly r1 As Int32
    Private Shared ReadOnly r2 As Int32

    Shared Sub New()
        r1 = rnd_int16()
        r2 = rnd_int16()
    End Sub

    Public Sub New()
        MyBase.New(New delegate_case(AddressOf internal_instructions),
                   New delegate_case(AddressOf just_method),
                   New delegate_case(AddressOf extension_method))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        If os.windows_major <= os.windows_major_t._5 Then
            Return {{-1, 1.2, 1.2},
                    {2.5, -1, 1.1},
                    {2.5, 1.1, -1}}
        End If
        ' Targeting _6 or upper
        Return Nothing
    End Function

    Protected Overrides Function max_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({8690, 8508, 8895}, i, j)
    End Function

    Private Shared Sub internal_instructions()
        For i As UInt64 = 0 To size - uint64_1
            Dim j As Int32 = 0
            j = r1 + r2
            j = r1 - r2
        Next
    End Sub

    Private Shared Sub this_method(ByRef j As Int32, ByVal r1 As Int32, ByVal r2 As Int32)
        j = r1 + r2
        j = r1 - r2
    End Sub

    Private Shared Sub just_method()
        For i As UInt64 = 0 To size - uint64_1
            Dim j As Int32 = 0
            this_method(j, r1, r2)
        Next
    End Sub

    Private Shared Sub extension_method()
        For i As UInt64 = 0 To size - uint64_1
            Dim j As Int32 = 0
            j.ext_method(r1, r2)
        Next
    End Sub
End Class
