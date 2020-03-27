
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt.attributes
Imports osi.service.math

<command_line_specified>
<test>
Public NotInheritable Class big_udec_match_test
    <test>
    Private Shared Sub pi_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(Console.ReadLine()).as_str().with_upure_length(512)
        For i As Int32 = 1 To s.Length()
            If Not constants.pi_1m().StartsWith(s.Substring(0, i)) OrElse i = s.Length() Then
                Console.WriteLine(strcat("Match first ", i, " digits"))
                Return
            End If
        Next
    End Sub

    <test>
    Private Shared Sub e_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(Console.ReadLine()).as_str().with_upure_length(102400)
        For i As Int32 = 1 To s.Length()
            If Not constants.e_1m().StartsWith(s.Substring(0, i)) OrElse i = s.Length() Then
                Console.WriteLine(strcat("Match first ", i, " digits"))
                Return
            End If
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
