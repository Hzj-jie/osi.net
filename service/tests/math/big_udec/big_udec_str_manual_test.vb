
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.math

<command_line_specified>
<test>
Public NotInheritable Class big_udec_str_manual_test
    <test>
    Private Shared Sub fraction_to_decimal()
        Console.WriteLine(big_udec.parse_fraction(Console.ReadLine()).str())
    End Sub

    <test>
    Private Shared Sub fraction_to_decimal_512()
        Console.WriteLine(big_udec.parse_fraction(Console.ReadLine()).as_str().with_upure_length(512).ToString())
    End Sub

    <test>
    Private Shared Sub decimal_to_fraction()
        Console.WriteLine(big_udec.parse(Console.ReadLine()).fractional_str())
    End Sub

    Private Sub New()
    End Sub
End Class
