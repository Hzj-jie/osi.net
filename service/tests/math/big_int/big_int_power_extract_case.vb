
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.math

Friend Class big_int_power_extract_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_int_power_extract_case(), round_scale * 128 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
    End Sub

    Private Shared Function power_extract_case(ByVal i As UInt64, ByVal j As UInt64) As Boolean
        assert(i > 0)
        Dim b As big_int = Nothing
        b = New big_int(i)
        b.power(j)
        Dim r As big_int = Nothing
        b.extract(j, r)
        assertion.is_not_null(r)
        assertion.is_true(r.is_zero())
        assertion.is_true(b.equal(New big_int(i)))

        Dim t As UInt64 = 0
        t = rnd_uint64(2, i)
        b.power(j)
        b.add(t)
        b.extract(j, r)
        assertion.is_not_null(r)
        assertion.is_true(b.equal(New big_int(i)))
        assertion.is_true(r.equal(New big_int(t)))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return power_extract_case(rnd_uint64(max_uint32, max_uint64),
                                  rnd_uint(3, 80))
    End Function
End Class
