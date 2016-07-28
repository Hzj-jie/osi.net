
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.secure

#If 0 Then
Public NotInheritable Class token2_test
    Inherits chained_case_wrapper

    Private Shared Function C(ByVal s As signer) As [case]
        Return New token2_case(s)
    End Function

    Public Sub New()
        ' ring and xor definitely won't work with token2, they are too simple and easily to conflict.
        MyBase.New(C(sign.hash_algorithm_merge_hasher(Of sign.hash_algorithm_merge_hasher.sha1_new).concat),
                   C(sign.keyed_hasher(Of sign.keyed_hasher.sha512_new).instance),
                   C(sign.keyed_hasher(Of sign.keyed_hasher.sha384_new).instance))
    End Sub

    Private Class token2_case
        Inherits token_test

        Public Sub New(ByVal s As signer)
            MyBase.New(AddressOf token2_defender.[New](Of mock_ppt, mock_conn),
                       AddressOf token2_challenger.[New](Of mock_ppt, mock_conn),
                       Function(x, y, z) New info(x, y, z, s),
                       False)
        End Sub
    End Class
End Class
#End If
