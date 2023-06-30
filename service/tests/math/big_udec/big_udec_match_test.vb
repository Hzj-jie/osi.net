
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
    Private Shared Sub match_digits(ByVal i As UInt32)
        raise_error(error_type.warning, "Match first ", i, " digits")
    End Sub

    Private Shared Sub match_more_digits(ByVal i As UInt32)
        raise_error(error_type.warning, "Match more than first ", i, " digits")
    End Sub

    <test>
    Private Shared Sub pi_fractional_str()
        Dim pi As String = Nothing
        pi = constants.pi_10m()
        Dim r As UInt32 = 0
        r = big_udec.parse_fraction(IO.File.ReadAllText("pi.txt")).str_match(pi)
        If r < pi.strlen() Then
            match_digits(r)
        Else
            match_more_digits(pi.strlen())
        End If
    End Sub

    <test>
    Private Shared Sub e_fractional_str()
        Dim e As String = Nothing
        e = constants.e_2m()
        Dim r As UInt32 = 0
        r = big_udec.parse_fraction(IO.File.ReadAllText("e.txt")).str_match(e)
        If r < e.strlen() Then
            match_digits(r)
        Else
            match_more_digits(e.strlen())
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
