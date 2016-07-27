
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_int_left_shift_multiply_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_int_left_shift_multiply_case(), round_scale * 1024 * 64 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function left_shift_multiply_case(ByVal x As UInt64) As Boolean
        Dim sr As big_int = Nothing
        Dim mr As big_int = Nothing
        sr = New big_int(x)
        mr = New big_int(x)
        For i As Int32 = 0 To 32 - 1
            Dim t As Int32 = 0
            t = rnd_int(1, bit_count_in_byte * sizeof_int32)
            sr.left_shift(t)
            mr.multiply(uint32_1 << t)
            assert_true(sr.equal(mr))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return left_shift_multiply_case(rnd_uint64(min_uint64, max_uint64))
    End Function
End Class
