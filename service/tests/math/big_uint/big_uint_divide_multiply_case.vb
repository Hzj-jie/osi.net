
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_divide_multiply_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_divide_multiply_case(), round_scale * 1024 * 256 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function divide_case(ByVal i As UInt64, ByVal j As UInt64) As Boolean
        Dim u As big_uint = Nothing
        Dim r As big_uint = Nothing
        u = New big_uint(i)
        u.multiply(j)
        u.divide(j, r)
        assertion.is_true(u.equal(i))
        assertion.is_true(r.is_zero())

        Dim exp_remainder As UInt64 = 0
        exp_remainder = rnd_uint64(1, j)
        u.multiply(j)
        u.add(exp_remainder)
        u.divide(j, r)
        assertion.is_true(u.equal(i))
        assertion.is_true(r.equal(exp_remainder))

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return divide_case(rnd_uint64(max_uint32, max_uint64),
                           rnd_uint64(max_int8, max_uint16))
    End Function
End Class
