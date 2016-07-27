
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_power_divide_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_power_divide_case(), round_scale * 128 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function power_divide_case(ByVal i As UInt64, ByVal j As UInt64) As Boolean
        assert(i > 0)
        Dim b As big_uint = Nothing
        b = New big_uint(i)
        b.power(j)
        For k As UInt64 = 0 To j - 1
            Dim r As big_uint = Nothing
            b.divide(i, r)
            assert_true(r.is_zero())
        Next
        assert_true(b.is_one())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return power_divide_case(rnd_uint64(max_uint32, max_uint64),
                                 rnd_uint(3, 80))
    End Function
End Class
