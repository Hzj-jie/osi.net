
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class div_rem_uint64_uint32_test
    Private Shared Sub run_case(ByVal dividend As UInt64, ByVal divisor As UInt32)
        Dim result As UInt64 = 0
        Dim remainder As UInt32 = 0
        result = dividend.div_rem(divisor, remainder)
        assertion.equal(dividend, result * divisor + remainder, dividend, " / ", divisor)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub all()
        For i As UInt64 = 0 To max_uint64 - uint64_1
            For j As UInt32 = 0 To max_uint32 - uint32_1
                run_case(i + uint64_1, j + uint32_1)
            Next
        Next
    End Sub

    <test>
    Private Shared Sub predefined()
        run_case(max_uint64, max_uint32)
        run_case(max_uint64 - uint64_1, max_uint32)
        run_case(max_uint64, 2)
        run_case(max_uint64 - uint64_1, 2)
    End Sub

    <test>
    <repeat(10000000)>
    Private Shared Sub random()
        run_case(rnd_uint64(), rnd_uint(uint32_1, max_uint32))
    End Sub

    Private Sub New()
    End Sub
End Class
