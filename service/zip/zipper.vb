
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.convertor

Public Interface zipper
    Function zip(ByVal i() As Byte, ByVal offset As UInt32, ByVal count As UInt32, ByRef o() As Byte) As Boolean
    Function unzip(ByVal i() As Byte, ByVal offset As UInt32, ByVal count As UInt32, ByRef o() As Byte) As Boolean
End Interface

Public Module _zipper
    <Extension()> Public Function as_zip_bytes_transformer(ByVal z As zipper) As bytes_transformer
        assert(z IsNot Nothing)
        Return New bytes_transformer_wrapper(AddressOf z.zip)
    End Function

    <Extension()> Public Function as_unzip_bytes_transformer(ByVal z As zipper) As bytes_transformer
        assert(z IsNot Nothing)
        Return New bytes_transformer_wrapper(AddressOf z.unzip)
    End Function

    <Extension()> Public Function bypass(ByVal z As zipper) As Boolean
        Return assert(z IsNot Nothing) AndAlso
               object_compare(z, osi.service.zip.bypass.instance) = 0
    End Function

    <Extension()> Public Function bypass2(ByVal z As zipper) As Boolean
        Return assert(z IsNot Nothing) AndAlso
               object_compare(z, osi.service.zip.bypass2.instance) = 0
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal p As piece,
                                      ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               p IsNot Nothing AndAlso
               z.zip(p.buff, p.offset, p.count, o)
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i() As Byte,
                                      ByVal count As UInt32,
                                      ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.zip(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i() As Byte,
                                      ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.zip(i, array_size(i), o)
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i() As Byte,
                                      ByVal offset As UInt32,
                                      ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.zip(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i() As Byte,
                                      ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.zip(i, count, o))
        Return o
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.zip(i, o))
        Return o
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByVal count As UInt32,
                                        ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.unzip(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.unzip(i, array_size(i), o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByVal offset As UInt32,
                                        ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.unzip(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.unzip(i, count, o))
        Return o
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(z IsNot Nothing)
        assert(z.unzip(i, o))
        Return o
    End Function

    'accept empty string
    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i As String,
                                      ByVal offset As UInt32,
                                      ByVal count As UInt32,
                                      ByRef o() As Byte) As Boolean
        If z Is Nothing OrElse
           strlen(i) < offset + count Then
            Return False
        Else
            Return z.zip(str_bytes(i, offset, count), o)
        End If
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i As String,
                                      ByVal count As UInt32,
                                      ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.zip(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function zip(ByVal z As zipper,
                                      ByVal i As String,
                                      ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               z.zip(i, strlen(i), o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal p As piece,
                                        ByRef o() As Byte) As Boolean
        Return z IsNot Nothing AndAlso
               p IsNot Nothing AndAlso
               z.unzip(p.buff, p.offset, p.count, o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByVal offset As UInt32,
                                        ByVal count As UInt32,
                                        ByRef o As String) As Boolean
        Dim b() As Byte = Nothing
        Return z IsNot Nothing AndAlso
               z.unzip(i, offset, count, b) AndAlso
               bytes_str(b, uint32_0, array_size(b), o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByVal count As UInt32,
                                        ByRef o As String) As Boolean
        Return z IsNot Nothing AndAlso
               z.unzip(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function unzip(ByVal z As zipper,
                                        ByVal i() As Byte,
                                        ByRef o As String) As Boolean
        Return z IsNot Nothing AndAlso
               z.unzip(i, array_size(i), o)
    End Function
End Module
