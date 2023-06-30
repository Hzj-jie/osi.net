
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_int_divide_multiply_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_int_divide_multiply_case(), round_scale * 1024 * 256 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function divide_case(ByVal i As UInt64, ByVal j As Int64) As Boolean
        If j = 0 Then
            j = If(rnd_bool(), -1, 1)
        End If
        Dim u As big_int = Nothing
        Dim r As big_int = Nothing
        u = New big_int(i)
        u.multiply(j)
        u.divide(j, r)
        assertion.is_true(u.equal(i))
        assertion.is_true(r.is_zero())

        Dim exp_remainder As Int64 = 0
        If j.abs() > 1 Then
            If j > 0 Then
                exp_remainder = rnd_uint64(1, j)
            Else
                exp_remainder = -rnd_uint64(1, -j)
            End If
        End If
        u.multiply(j)
        u.add(exp_remainder)
        u.divide(j, r)
        assertion.is_true(u.equal(i))
        assertion.is_true(r.equal(exp_remainder))

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return divide_case(rnd_uint64(max_uint32, max_uint64),
                           rnd_int64(min_uint16, max_uint16))
    End Function
End Class
