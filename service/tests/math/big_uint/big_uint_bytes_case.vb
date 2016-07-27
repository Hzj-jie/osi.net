
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_bytes_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_bytes_case(), round_scale * 102400 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function run() As Boolean
        Dim r As UInt64 = 0
        r = rnd_uint64()
        Dim b1 As big_uint = Nothing
        b1 = New big_uint(uint64_bytes(r))
        Dim b2 As big_uint = Nothing
        b2 = New big_uint(r)
        assert_equal(b1, b2)

        b1 = big_uint.random()
        b2 = New big_uint(b1.as_bytes())
        assert_equal(b1, b2)
        Return True
    End Function
End Class
