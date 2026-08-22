
Imports System.Security.Cryptography
Imports osi.root.constants
Imports osi.root.template

Namespace sign
    <global_init(global_init_level.services)>
    Public NotInheritable Class keyed_hasher
        Shared Sub New()
#If Not NET8_0_OR_GREATER Then
            keyed_hasher(Of mac_triple_des_new).register("mac_triple_des")
#End If
            keyed_hasher(Of md5_new).register("hmac_md5")
#If Not NET8_0_OR_GREATER Then
            keyed_hasher(Of ripemd160_new).register("hmac_ripemd160")
#End If
            keyed_hasher(Of sha1_new).register("hmac_sha1")
            keyed_hasher(Of sha256_new).register("hmac_sha256")
            keyed_hasher(Of sha384_new).register("hmac_sha384")
            keyed_hasher(Of sha512_new).register("hmac_sha512")
            keyed_hasher(Of sha512_new).register("hmac_sha")
        End Sub

#If Not NET8_0_OR_GREATER Then
        Public Class mac_triple_des_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return MACTripleDES.Create()
            End Function
        End Class
#End If

        Public Class md5_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACMD5()
            End Function
        End Class

#If Not NET8_0_OR_GREATER Then
        Public Class ripemd160_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACRIPEMD160()
            End Function
        End Class
#End If

        Public Class sha1_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACSHA1()
            End Function
        End Class

        Public Class sha256_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACSHA256()
            End Function
        End Class

        Public Class sha384_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACSHA384()
            End Function
        End Class

        Public Class sha512_new
            Inherits __do(Of KeyedHashAlgorithm)

            Protected Overrides Function at() As KeyedHashAlgorithm
                Return New HMACSHA512()
            End Function
        End Class

        Private Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
