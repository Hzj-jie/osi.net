
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_udec_dec_str_perf
    <repeat(2)>
    <test>
    Private Shared Sub run()
        big_udec.random(100000).as_str().with_upure_length(100000).ToString()
    End Sub

    Private Sub New()
    End Sub
End Class
