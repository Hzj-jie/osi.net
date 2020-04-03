
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector._raise_error
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_e_test
    <test>
    Private Shared Sub calculate_e_power_1000()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        s.add(big_udec.fraction(CUInt(1), CUInt(1000)))
        s.power(CUInt(1000))
        assertion.equal(s.str(), "2.7169239322358924573830881219475771889643150188365728037223547748")
        '                         2.71828182845904523536028747135266249775724709369995957
    End Sub

    <test>
    Private Shared Sub calculate_e_power_100000()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        s.add(big_udec.fraction(CUInt(1), CUInt(100000)))
        s.power(CUInt(100000))
        assertion.equal(s.str(), "2.7182682371744896680350648244260464479744469326778228630091644198")
        '                         2.71828182845904523536028747135266249775724709369995957
    End Sub

    <test>
    Private Shared Sub calculate_e_factorial_100()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        Dim c As big_udec = Nothing
        c = big_udec.one()
        For i As UInt32 = 1 To 100
            c.assert_divide(New big_udec(i))
            s.add(c)
        Next
        assertion.equal(s.str(), "2.7182818284590452353602874713526624977572470936999595749669676277")
        '                         2.71828182845904523536028747135266249775724709369995957
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_e_factorial_1000()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        Dim c As big_udec = Nothing
        c = big_udec.one()
        For i As UInt32 = 1 To 1000
            c.assert_divide(New big_udec(i))
            s.add(c)
        Next
        assertion.equal(s.str(), "2.7182818284590452353602874713526624977572470936999595749669676277")
        '                         2.71828182845904523536028747135266249775724709369995957
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub calculate_e_factorial_max_uint32_progressively()
        Dim s As big_udec = Nothing
        s = big_udec.one()
        Dim c As big_udec = Nothing
        c = big_udec.one()
        For i As UInt32 = 1 To max_uint32 - uint32_1
            c.assert_divide(New big_udec(i))
            s.add(c)

            If (i Mod 1000) = 0 Then
                s.fully_reduce_fraction()
                raise_error(error_type.warning, "@ ", i, " -> ", s.fractional_str())
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
