
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_parse_str_test
    <test>
    <repeat(100)>
    Private Shared Sub run()
        Dim u As big_udec = Nothing
        u = big_udec.random()
        Dim i As Byte = 0
        i = 2
        While big_udec.support_base(i)
            Dim o As big_udec = Nothing
            assertion.is_true(big_udec.parse(u.as_str().with_str_base(i).increase_upure_length(uint32_2), o, i))
            assertion.equal(u.str(i), o.str(i))
            i += byte_1
        End While
    End Sub

    Private Sub New()
    End Sub
End Class
