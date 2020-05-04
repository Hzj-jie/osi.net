
Imports osi.root.connector
Imports osi.root.utt

Public Class byte_array_uint32_array_conversion_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 100
            Dim u() As UInt32 = Nothing
            u = rnd_uints(rnd_uint(10, 100))
            Dim b() As Byte = Nothing
            b = u.byte_array()
            Dim u2() As UInt32 = Nothing
            assertion.is_true(b.uint32_array(u2))
            assertion.array_equal(u, u2)
            Dim b2() As Byte = Nothing
            ReDim b2(array_size_i(u) * sizeof_uint32 - 1)
            For j As Int32 = 0 To array_size_i(u) - 1
                arrays.copy(b2, j * sizeof_uint32, u(j).bytes())
            Next
            assertion.array_equal(b, b2)
        Next
        Return True
    End Function
End Class
