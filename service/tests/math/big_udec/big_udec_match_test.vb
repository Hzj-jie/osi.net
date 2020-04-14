
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

    Private Shared Function pi_str() As String
        Return constants.pi_10m()
    End Function

    Private Shared Function e_str() As String
        Return constants.e_2m()
    End Function

    <test>
    Private Shared Sub pi_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(IO.File.ReadAllText("pi.txt")).
                as_str().
                with_upure_length(pi_str().strlen())
        For i As Int32 = 0 To s.Length() - 1
            If pi_str()(i) <> s(i) Then
                match_digits(i)
                Return
            End If
        Next
        match_more_digits(pi_str().Length())
    End Sub

    <test>
    Private Shared Sub e_fractional_str()
        Dim s As String = Nothing
        s = big_udec.parse_fraction(IO.File.ReadAllText("e.txt")).
                as_str().
                with_upure_length(e_str().strlen())
        For i As Int32 = 0 To s.Length() - 1
            If e_str()(i) <> s(i) Then
                match_digits(i)
                Return
            End If
        Next
        match_more_digits(e_str().Length())
    End Sub

    Private Sub New()
    End Sub
End Class
