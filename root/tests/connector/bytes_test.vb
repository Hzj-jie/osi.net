
Imports osi.root.connector
Imports osi.root.utt

Public Class bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 10000
            Dim v As Int64 = 0
            v = rnd_int64()
            Dim be() As Byte = Nothing
            Dim le() As Byte = Nothing
            Dim a() As Byte = Nothing
            be = v.big_endian_bytes()
            le = v.little_endian_bytes()
            a = v.bytes()
            If BitConverter.IsLittleEndian Then
                assertion.array_equal(a, le)
            Else
                assertion.array_equal(a, be)
            End If
            assertion.array_equal(be.emplace_reverse(), le)
            assertion.equal(v, a.as_int64())
            assertion.equal(v, be.as_big_endian_int64())
            assertion.equal(v, le.as_little_endian_int64())
            assertion.equal(endian.reverse(v), be.as_little_endian_int64())
            assertion.equal(endian.reverse(v), le.as_big_endian_int64())
            If BitConverter.IsLittleEndian Then
                assertion.equal(endian.reverse(v), a.as_big_endian_int64())
                assertion.equal(v, le.as_int64())
                assertion.equal(endian.reverse(v), be.as_int64())
            Else
                assertion.equal(endian.reverse(v), a.as_little_endian_int64())
                assertion.equal(v, be.as_int64())
                assertion.equal(endian.reverse(v), le.as_int64())
            End If
        Next
        Return True
    End Function
End Class
