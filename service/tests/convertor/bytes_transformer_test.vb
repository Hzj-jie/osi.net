
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.convertor

Public Class bytes_transformer_test
    Inherits [case]

    Private Shared Function transform(ByVal i() As Byte,
                                      ByVal offset As Int32,
                                      ByVal count As Int32,
                                      ByRef o() As Byte) As Boolean
        assert(Not isemptyarray(i))
        If offset < 0 OrElse count < 0 OrElse array_size(i) < offset + count Then
            Return False
        Else
            ReDim o(count + sizeof_int32 * 2 - 1)
            assert(int32_bytes(offset, o))
            assert(int32_bytes(count, o, sizeof_int32))
            arrays.copy(o, sizeof_int32 * 2, i, 0, count)
            Return True
        End If
    End Function

    Public Overrides Function run() As Boolean
        Dim t As bytes_transformer = Nothing
        t = New bytes_transformer_wrapper(AddressOf transform)
        Dim i() As Byte = Nothing
        Dim o() As Byte = Nothing
        Dim o2() As Byte = Nothing
        i = rnd_bytes()
        assert(Not isemptyarray(i))
        assert(transform(i, 0, array_size(i), o2))

        assertion.is_true(t.transform(i, o))
        assertion.array_equal(o, o2)

        assertion.is_true(t.transform(i, array_size(i), o))
        assertion.array_equal(o, o2)

        Dim i2() As Byte = Nothing
        Dim offset As Int32 = 0
        Dim count As Int32 = 0
        i2 = i
        offset = 0
        count = array_size(i)
        assertion.is_true(t.transform(i2, offset, count))
        assertion.equal(offset, 0)
        assertion.equal(CUInt(count), array_size(i2))
        assertion.array_equal(i2, o2)

        Return True
    End Function
End Class
