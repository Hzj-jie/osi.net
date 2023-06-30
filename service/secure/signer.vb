
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template

Public Interface signer
    Function sign(ByVal key() As Byte,
                  ByVal i() As Byte,
                  ByVal offset As UInt32,
                  ByVal count As UInt32,
                  ByRef o() As Byte) As Boolean
End Interface

Namespace sign
    Public Module _signer
        Private Function merged(Of T As __do(Of HashAlgorithm))() As signer()
            Return {
                hash_algorithm_merge_hasher(Of T).concat,
                hash_algorithm_merge_hasher(Of T).ring,
                hash_algorithm_merge_hasher(Of T).xor
            }
        End Function

        Private Function keyed(Of T As __do(Of KeyedHashAlgorithm))() As signer
            Return keyed_hasher(Of T).instance
        End Function

        Public Function all_signers() As signer()
            Return array_concat(Of signer)({
                                               secure.sign.bypass.instance,
                                               bypass2.instance,
                                               ring.instance,
                                               [xor].instance
                                           },
                                           {
                                               md5_merge_hasher.concat,
                                               md5_merge_hasher.ring,
                                               md5_merge_hasher.xor
                                           },
                                           merged(Of hash_algorithm_merge_hasher.md5_new)(),
                                           merged(Of hash_algorithm_merge_hasher.ripemd160_new)(),
                                           merged(Of hash_algorithm_merge_hasher.sha1_new)(),
                                           merged(Of hash_algorithm_merge_hasher.sha256_new)(),
                                           merged(Of hash_algorithm_merge_hasher.sha384_new)(),
                                           merged(Of hash_algorithm_merge_hasher.sha512_new)(),
                                           {
                                               keyed(Of keyed_hasher.mac_triple_des_new)(),
                                               keyed(Of keyed_hasher.md5_new)(),
                                               keyed(Of keyed_hasher.ripemd160_new)(),
                                               keyed(Of keyed_hasher.sha1_new)(),
                                               keyed(Of keyed_hasher.sha256_new)(),
                                               keyed(Of keyed_hasher.sha384_new)(),
                                               keyed(Of keyed_hasher.sha512_new)()
                                           })
        End Function

        <Extension()> Public Function bypass(ByVal e As signer) As Boolean
            Return assert(Not e Is Nothing) AndAlso
                   object_compare(e, secure.sign.bypass.instance) = 0
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal p As piece,
                                           ByRef o() As Byte) As Boolean
            Return Not e Is Nothing AndAlso
                   Not p Is Nothing AndAlso
                   e.sign(key, p.buff, p.offset, p.count, o)
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal i() As Byte,
                                           ByVal count As UInt32,
                                           ByRef o() As Byte) As Boolean
            Return Not e Is Nothing AndAlso
                   e.sign(key, i, uint32_0, count, o)
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal i() As Byte,
                                           ByRef o() As Byte) As Boolean
            Return Not e Is Nothing AndAlso
                   e.sign(key, i, array_size(i), o)
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal i() As Byte,
                                           ByVal offset As UInt32,
                                           ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(Not e Is Nothing)
            assert(e.sign(key, i, offset, count, o))
            Return o
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal i() As Byte,
                                           ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(Not e Is Nothing)
            assert(e.sign(key, i, count, o))
            Return o
        End Function

        <Extension()> Public Function sign(ByVal e As signer,
                                           ByVal key() As Byte,
                                           ByVal i() As Byte) As Byte()
            Dim o() As Byte = Nothing
            assert(Not e Is Nothing)
            assert(e.sign(key, i, o))
            Return o
        End Function
    End Module
End Namespace
