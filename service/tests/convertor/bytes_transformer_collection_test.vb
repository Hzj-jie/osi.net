
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.convertor

Public Class bytes_transformer_collection_test
    Inherits [case]

    Private Shared Function trans(ByVal i() As Byte,
                                  ByVal offset As Int32,
                                  ByVal count As Int32,
                                  ByVal append As Byte,
                                  ByRef o() As Byte) As Boolean
        If offset < 0 OrElse
           count < 0 OrElse
           array_size(i) < offset + count Then
            Return False
        Else
            ReDim o(count)
            memcpy(o, i, count)
            o(count) = append
            Return True
        End If
    End Function

    Private Shared Function trans0(ByVal i() As Byte,
                                   ByVal offset As Int32,
                                   ByVal count As Int32,
                                   ByRef o() As Byte) As Boolean
        Return trans(i, offset, count, 0, o)
    End Function

    Private Shared Function trans1(ByVal i() As Byte,
                                   ByVal offset As Int32,
                                   ByVal count As Int32,
                                   ByRef o() As Byte) As Boolean
        Return trans(i, offset, count, 1, o)
    End Function

    Private Shared Function trans2(ByVal i() As Byte,
                                   ByVal offset As Int32,
                                   ByVal count As Int32,
                                   ByRef o() As Byte) As Boolean
        Return trans(i, offset, count, 2, o)
    End Function

    Private Shared Function trans3(ByVal i() As Byte,
                                   ByVal offset As Int32,
                                   ByVal count As Int32,
                                   ByRef o() As Byte) As Boolean
        Return trans(i, offset, count, 3, o)
    End Function

    Public Overrides Function run() As Boolean
        Dim bt As bytes_transformer_collection = Nothing
        bt = New bytes_transformer_collection()
        assertion.is_true(bt.chain(New bytes_transformer_wrapper(AddressOf trans0),
                             New bytes_transformer_wrapper(AddressOf trans1),
                             New bytes_transformer_wrapper(AddressOf trans2),
                             New bytes_transformer_wrapper(AddressOf trans3)))
        Dim i() As Byte = Nothing
        Dim o() As Byte = Nothing
        i = rnd_bytes()
        assert(Not isemptyarray(i))

        assertion.is_true(bt.transform_forward(i, o))
        If assertion.equal(array_size(o), array_size(i) + 4) Then
            For j As Int32 = 0 To 3
                assertion.equal(o(array_size(i) + j), j)
            Next
        End If

        assertion.is_true(bt.transform_backward(i, o))
        If assertion.equal(array_size(o), array_size(i) + 4) Then
            For j As Int32 = 0 To 3
                assertion.equal(o(array_size(i) + j), 3 - j)
            Next
        End If

        Return True
    End Function
End Class
