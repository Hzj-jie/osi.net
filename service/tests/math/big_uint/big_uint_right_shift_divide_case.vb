
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_right_shift_divide_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_right_shift_divide_case(), round_scale * 512 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function right_shift_divide_case(ByVal x As UInt64) As Boolean
        Const round As Int32 = 32
        Dim sr As big_uint = Nothing
        Dim mr As big_uint = Nothing
        sr = New big_uint()
        mr = New big_uint()
        For i As Int32 = 0 To round - 1
            sr.left_shift(bit_count_in_byte * sizeof_int32)
            mr.left_shift(bit_count_in_byte * sizeof_int32)
            sr.add(x)
            mr.add(x)
        Next
        For i As Int32 = 0 To round - 1
            Dim t As Int32 = 0
            t = rnd_int(1, bit_count_in_byte * sizeof_int32)
            sr.right_shift(t)
            mr.divide(uint32_1 << t)
            assert_true(sr.equal(mr))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return right_shift_divide_case(rnd_uint64(min_uint64, max_uint64))
    End Function
End Class
