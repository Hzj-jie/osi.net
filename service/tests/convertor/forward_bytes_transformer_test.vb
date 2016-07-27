
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.convertor

Public Class forward_bytes_transformer_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim t As bytes_transformer = Nothing
        t = New forward_bytes_transformer()
        Dim i() As Byte = Nothing
        Dim o() As Byte = Nothing
        i = rndbytes()

        assert_true(t.transform(i, o))
        assert_reference_equal(i, o)

        assert_true(t.transform(i, 1, array_size(i) - 1, o))
        assert_not_reference_equal(i, o)
        assert_equal(array_size(o), array_size(i) - 1)
        assert_equal(memcmp(i, 1, o, 0, array_size(o)), 0)

        assert_true(t.transform(i, array_size(i) - 1, o))
        assert_not_reference_equal(i, o)
        assert_equal(array_size(o), array_size(i) - 1)
        assert_equal(memcmp(i, o, array_size(o)), 0)

        assert_false(t.transform(Nothing, 0, 1, Nothing))

        assert_true(t.transform(i, array_size(i), 0, o))
        assert_not_nothing(o)
        assert_equal(array_size(o), uint32_0)

        assert_false(t.transform(i, array_size(i), 1, Nothing))

        assert_false(t.transform(i, 0, array_size(i) + 1, Nothing))

        Return True
    End Function
End Class
