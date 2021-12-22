
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class enum_to_string_behavior_test
    Private Enum E As Byte
        a
        b
        c
    End Enum

    <test>
    Private Shared Sub to_string_vs_convert_to_string()
        assertions.of(E.a.ToString()).Equals("a")
        assertions.of(Convert.ToString(E.a)).Equals("0")
    End Sub

    Private Sub New()
    End Sub
End Class
