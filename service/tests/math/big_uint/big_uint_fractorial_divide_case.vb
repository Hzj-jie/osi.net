
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_factorial_divide_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_factorial_divide_case(), round_scale * 256 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function run() As Boolean
        Dim l As big_uint = Nothing
        While l Is Nothing
            l = big_uint.random(rnd_uint(1, 2))
            If l.is_zero_or_one() Then
                l = Nothing
            End If
        End While
        Dim r As big_uint = Nothing
        r = l.CloneT()
        r.factorial()
        While Not l.is_one()
            Dim remainder As big_uint = Nothing
            r.assert_divide(l, remainder)
            assertion.is_true(remainder.is_zero())
            l.assert_sub(uint32_1)
        End While
        assertion.is_true(r.is_one())
        Return True
    End Function
End Class
