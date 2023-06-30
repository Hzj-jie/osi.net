
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.secure

Public Class token2_case
    Inherits token_test

    Public Sub New(ByVal s As signer)
        MyBase.New(AddressOf token2_defender.[New](Of mock_ppt, mock_conn),
                   AddressOf token2_challenger.[New](Of mock_ppt, mock_conn),
                   Function(x, y, z) New info(x, y, z, s),
                   False)
    End Sub
End Class

' ring and xor definitely won't work with token2, they are too simple and easily to conflict.
Public NotInheritable Class token2_sha1_test
    Inherits token2_case

    Public Sub New()
        MyBase.New(sign.hash_algorithm_merge_hasher(Of sign.hash_algorithm_merge_hasher.sha1_new).concat)
    End Sub
End Class

Public NotInheritable Class token2_sha384_test
    Inherits token2_case

    Public Sub New()
        MyBase.New(sign.keyed_hasher(Of sign.keyed_hasher.sha384_new).instance)
    End Sub
End Class

Public NotInheritable Class token2_sha512_test
    Inherits token2_case

    Public Sub New()
        MyBase.New(sign.keyed_hasher(Of sign.keyed_hasher.sha512_new).instance)
    End Sub
End Class

Public NotInheritable Class token2_md5_test
    Inherits token2_case

    Public Sub New()
        MyBase.New(sign.md5_merge_hasher.concat)
    End Sub
End Class

Public NotInheritable Class token2_md5_2_test
    Inherits token2_case

    Public Sub New()
        MyBase.New(sign.hash_algorithm_merge_hasher(Of sign.hash_algorithm_merge_hasher.md5_new).concat)
    End Sub
End Class
