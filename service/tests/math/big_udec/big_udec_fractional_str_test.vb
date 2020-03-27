
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_fractional_str_test
    <test>
    <repeat(100)>
    Private Shared Sub run()
        Dim d As big_udec = Nothing
        d = big_udec.random()
        Dim i As Byte = 0
        i = 2
        While big_udec.support_base(i)
            Dim o As big_udec = Nothing
            assertion.is_true(big_udec.parse_fraction(d.fractional_str(i), o, i))
            assertion.equal(d, o)
            i += byte_1
        End While
    End Sub

    Private Sub New()
    End Sub
End Class
