
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
        i = rnd_bytes()

        assertion.is_true(t.transform(i, o))
        assertion.reference_equal(i, o)

        assertion.is_true(t.transform(i, 1, array_size(i) - 1, o))
        assertion.not_reference_equal(i, o)
        assertion.equal(array_size(o), array_size(i) - 1)
        assertion.equal(memcmp(i, 1, o, 0, array_size(o)), 0)

        assertion.is_true(t.transform(i, array_size(i) - 1, o))
        assertion.not_reference_equal(i, o)
        assertion.equal(array_size(o), array_size(i) - 1)
        assertion.equal(memcmp(i, o, array_size(o)), 0)

        assertion.is_false(t.transform(Nothing, 0, 1, Nothing))

        assertion.is_true(t.transform(i, array_size(i), 0, o))
        assertion.is_not_null(o)
        assertion.equal(array_size(o), uint32_0)

        assertion.is_false(t.transform(i, array_size(i), 1, Nothing))

        assertion.is_false(t.transform(i, 0, array_size(i) + 1, Nothing))

        Return True
    End Function
End Class
