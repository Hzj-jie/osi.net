
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
        s = big_udec.parse_fraction(IO.File.ReadAllText("input.txt")).as_str().with_upure_length(102400)
        Dim till As Int32 = 0
        till = min(s.Length(), constants.pi_1m().Length())
        For i As Int32 = 0 To till - 1
            If constants.pi_1m()(i) <> s(i) Then
                Console.WriteLine(strcat("Match first ", i, " digits"))
                Return
            End If
        Next
        Console.WriteLine(strcat("Match first ", till, " digits"))
    End Sub

    <test>
    Private Shared Sub e_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(IO.File.ReadAllText("input.txt")).as_str().with_upure_length(1024000)
        Dim till As Int32 = 0
        till = min(s.Length(), constants.e_1m().Length())
        For i As Int32 = 0 To till - 1
            If constants.e_1m()(i) <> s(i) Then
                Console.WriteLine(strcat("Match first ", i, " digits"))
                Return
            End If
        Next
        Console.WriteLine(strcat("Match first ", till, " digits"))
    End Sub

    Private Sub New()
    End Sub
End Class
