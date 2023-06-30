
Imports System.Security.Cryptography
Imports osi.root.constants
Imports osi.root.template 

Namespace sign
    <global_init(global_init_level.services)>
    Public NotInheritable Class hash_algorithm_merge_hasher
        Shared Sub New()
            hash_algorithm_merge_hasher(Of md5_new).register("md5_2")
            hash_algorithm_merge_hasher(Of ripemd160_new).register("ripemd160")
            hash_algorithm_merge_hasher(Of sha1_new).register("sha1")
            hash_algorithm_merge_hasher(Of sha256_new).register("sha256")
            hash_algorithm_merge_hasher(Of sha384_new).register("sha384")
            hash_algorithm_merge_hasher(Of sha512_new).register("sha512")
            hash_algorithm_merge_hasher(Of sha512_new).register("sha")
        End Sub

        Public Class md5_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return MD5.Create()
            End Function
        End Class

        Public Class ripemd160_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return RIPEMD160.Create()
            End Function
        End Class

        Public Class sha1_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return SHA1.Create()
            End Function
        End Class

        Public Class sha256_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return SHA256.Create()
            End Function
        End Class

        Public Class sha384_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return SHA384.Create()
            End Function
        End Class

        Public Class sha512_new
            Inherits __do(Of HashAlgorithm)

            Protected Overrides Function at() As HashAlgorithm
                Return SHA512.Create()
            End Function
        End Class

        Private Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
