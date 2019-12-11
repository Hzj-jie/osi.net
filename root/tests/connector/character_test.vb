
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class character_test
    <test>
    Private Shared Sub run()
        assert(min_uint16 >= character.ascii_lower_bound)
        For i As UInt32 = min_uint16 To max_uint16
            Dim c As Char = Nothing
            c = Convert.ToChar(CUShort(i))
            If i <= character.ascii_upper_bound Then
                assertion.is_true(c.ascii())
                assertion.is_true(c.extended_ascii())
            ElseIf i <= character.ascii_extended_upper_bound Then
                assertion.is_false(c.ascii())
                assertion.is_true(c.extended_ascii())
            Else
                assertion.is_false(c.ascii())
                assertion.is_false(c.extended_ascii())
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
