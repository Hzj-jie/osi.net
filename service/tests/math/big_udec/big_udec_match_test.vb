
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt.attributes
Imports osi.service.math
Imports constants = osi.service.resources.math

<command_line_specified>
<test>
Public NotInheritable Class big_udec_match_test
    Private Shared Sub match_digits(ByVal i As Int32)
        raise_error(error_type.warning, "Match first ", i, " digits")
    End Sub

    Private Shared Sub match_more_digits(ByVal i As Int32)
        raise_error(error_type.warning, "Match more than first ", i, " digits")
    End Sub

    <test>
    Private Shared Sub pi_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(IO.File.ReadAllText("pi.txt")).
                as_str().
                with_upure_length(constants.pi_1m().strlen())
        For i As Int32 = 0 To s.Length() - 1
            If constants.pi_1m()(i) <> s(i) Then
                match_digits(i)
                Return
            End If
        Next
        match_more_digits(s.Length())
    End Sub

    <test>
    Private Shared Sub e_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(IO.File.ReadAllText("e.txt")).
                as_str().
                with_upure_length(constants.e_2m().strlen())
        For i As Int32 = 0 To s.Length() - 1
            If constants.e_2m()(i) <> s(i) Then
                match_digits(i)
                Return
            End If
        Next
        match_more_digits(constants.e_2m().Length())
    End Sub

    Private Sub New()
    End Sub
End Class
