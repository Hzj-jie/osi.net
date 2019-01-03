
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_shift_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_shift_case(), round_scale * 1024 * 64 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function shift_case(ByVal i As UInt64, ByVal j As UInt64) As Boolean
        Dim l As big_uint = Nothing
        Dim r As big_uint = Nothing
        l = New big_uint(i)
        r = New big_uint(i)
        For k As Int32 = 0 To 32 - 1
            Dim t As UInt64 = 0
            t = rnd_uint64(1, j)
            l.left_shift(t)
            r.left_shift(j).right_shift(j - t)
            assertion.is_true(l.equal(r))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return shift_case(rnd_uint64(min_uint64, max_uint64),
                          rnd_uint64(max_uint8, max_uint8 << 2))
    End Function
End Class
