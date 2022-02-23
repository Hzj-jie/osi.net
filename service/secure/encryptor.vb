
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

Public Interface encryptor
    Function encrypt(ByVal key() As Byte,
                     ByVal i() As Byte,
                     ByVal offset As UInt32,
                     ByVal count As UInt32,
                     ByRef o() As Byte) As Boolean
    Function decrypt(ByVal key() As Byte,
                     ByVal i() As Byte,
                     ByVal offset As UInt32,
                     ByVal count As UInt32,
                     ByRef o() As Byte) As Boolean
End Interface

Namespace encrypt
    Public Module _encryptor
        <Extension()> Public Function as_encrypt_bytes_transformer(ByVal e As encryptor,
                                                                   ByVal key() As Byte) As bytes_transformer
            assert(e IsNot Nothing)
            Return New bytes_transformer_wrapper(Function(i() As Byte,
                                                          offset As UInt32,
                                                          count As UInt32,
                                                          ByRef o() As Byte) As Boolean
                                                     Return e.encrypt(key, i, offset, count, o)
                                                 End Function)
        End Function

        <Extension()> Public Function as_decrypt_bytes_transformer(ByVal e As encryptor,
                                                                   ByVal key() As Byte) As bytes_transformer
            assert(e IsNot Nothing)
            Return New bytes_transformer_wrapper(Function(i() As Byte,
                                                          offset As UInt32,
                                                          count As UInt32,
                                                          ByRef o() As Byte) As Boolean
                                                     Return e.decrypt(key, i, offset, count, o)
                                                 End Function)
        End Function

        <Extension()> Public Function bypass(ByVal e As encryptor) As Boolean
            Return assert(e IsNot Nothing) AndAlso
                   object_compare(e, osi.service.secure.encrypt.bypass.instance) = 0
        End Function

        <Extension()> Public Function bypass2(ByVal e As encryptor) As Boolean
            Return assert(e IsNot Nothing) AndAlso
                   object_compare(e, osi.service.secure.encrypt.bypass2.instance) = 0
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal p As piece,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   p IsNot Nothing AndAlso
                   e.encrypt(key, p.buff, p.offset, p.count, o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.encrypt(key, i, uint32_0, count, o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.encrypt(key, i, array_size(i), o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, offset, count, o))
            Return o
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, count, o))
            Return o
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal p As piece,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   p IsNot Nothing AndAlso
                   e.decrypt(key, p.buff, p.offset, p.count, o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.decrypt(key, i, uint32_0, count, o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.decrypt(key, i, array_size(i), o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, offset, count, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, count, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key() As Byte,
                                              ByVal i() As Byte) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, o))
            Return o
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.encrypt(str_bytes(key), i, offset, count, o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.encrypt(str_bytes(key), i, count, o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.encrypt(str_bytes(key), i, o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.decrypt(str_bytes(key), i, offset, count, o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.decrypt(str_bytes(key), i, count, o)
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByRef o() As Byte) As Boolean
            Return e IsNot Nothing AndAlso
                   e.decrypt(str_bytes(key), i, o)
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, offset, count, o))
            Return o
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, count, o))
            Return o
        End Function

        <Extension()> Public Function encrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.encrypt(key, i, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal offset As UInt32,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, offset, count, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte,
                                              ByVal count As UInt32) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, count, o))
            Return o
        End Function

        <Extension()> Public Function decrypt(ByVal e As encryptor,
                                              ByVal key As String,
                                              ByVal i() As Byte) As Byte()
            Dim o() As Byte = Nothing
            assert(e IsNot Nothing)
            assert(e.decrypt(key, i, o))
            Return o
        End Function
    End Module
End Namespace
