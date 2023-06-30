
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _compress
    Private Function compress(ByVal i() As Byte,
                              ByRef o() As Byte,
                              ByVal offset As UInt32,
                              ByVal count As UInt32,
                              ByVal ctor As Func(Of MemoryStream, Stream)) As Boolean
        assert(Not ctor Is Nothing)
        If array_size(i) < offset + count Then
            Return False
        Else
            Using os As MemoryStream = New MemoryStream()
                Try
                    Using gs As Stream = ctor(os)
                        assert(Not gs Is Nothing)
                        gs.Write(i, offset, count)
                    End Using
                Catch ex As Exception
                    raise_error(error_type.warning,
                                  "failed to compress data, ex ",
                                  ex.Message())
                    Return False
                End Try
                o = os.fit_buffer()
            End Using
            Return True
        End If
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32,
                                       ByRef o() As Byte) As Boolean
        Return compress(i,
                        o,
                        offset,
                        count,
                        Function(os As MemoryStream) As Stream
                            Return New GZipStream(os, CompressionMode.Compress, True)
                        End Function)
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte,
                                       ByVal count As UInt32,
                                       ByRef o() As Byte) As Boolean
        Return gzip(i, 0, count, o)
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte,
                                       ByRef o() As Byte) As Boolean
        Return gzip(i, array_size(i), o)
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32,
                                          ByRef o() As Byte) As Boolean
        Return compress(i,
                        o,
                        offset,
                        count,
                        Function(os As MemoryStream) As Stream
                            Return New DeflateStream(os, CompressionMode.Compress, True)
                        End Function)
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte,
                                          ByVal count As UInt32,
                                          ByRef o() As Byte) As Boolean
        Return deflate(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte,
                                          ByRef o() As Byte) As Boolean
        Return deflate(i, array_size(i), o)
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(gzip(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte,
                                       ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(gzip(i, count, o))
        Return o
    End Function

    <Extension()> Public Function gzip(ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(gzip(i, o))
        Return o
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(deflate(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte,
                                          ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(deflate(i, count, o))
        Return o
    End Function

    <Extension()> Public Function deflate(ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(deflate(i, o))
        Return o
    End Function

    Private Function decompress(ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte,
                                ByVal ctor As Func(Of MemoryStream, Stream)) As Boolean
        assert(Not ctor Is Nothing)
        Dim ms As MemoryStream = Nothing
        If memory_stream.[New](i, offset, count, ms) Then
            assert(Not ms Is Nothing)
            Using ms
                Using os As MemoryStream = New MemoryStream()
                    Try
                        Using gs As Stream = ctor(ms)
                            assert(Not gs Is Nothing)
                            Const buff_size As Int32 = 1024
                            Dim buff() As Byte = Nothing
                            ReDim buff(buff_size - 1)
                            Dim rc As Int32 = 0
                            While True
                                rc = gs.Read(buff, 0, buff_size)
                                If rc = 0 Then
                                    Exit While
                                Else
                                    os.Write(buff, 0, rc)
                                End If
                            End While
                        End Using
                    Catch ex As Exception
                        raise_error(error_type.warning,
                                      "failed to decompress the data, ex ",
                                      ex.Message())
                        Return False
                    End Try
                    o = os.fit_buffer()
                End Using
            End Using
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte,
                                         ByVal offset As UInt32,
                                         ByVal count As UInt32,
                                         ByRef o() As Byte) As Boolean
        Return decompress(i,
                          offset,
                          count,
                          o,
                          Function(s As MemoryStream) As Stream
                              Return New GZipStream(s, CompressionMode.Decompress, False)
                          End Function)
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte,
                                         ByVal count As UInt32,
                                         ByRef o() As Byte) As Boolean
        Return ungzip(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte,
                                         ByRef o() As Byte) As Boolean
        Return ungzip(i, array_size(i), o)
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte,
                                            ByVal offset As UInt32,
                                            ByVal count As UInt32,
                                            ByRef o() As Byte) As Boolean
        Return decompress(i,
                          offset,
                          count,
                          o,
                          Function(s As MemoryStream) As Stream
                              Return New DeflateStream(s, CompressionMode.Decompress, False)
                          End Function)
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte,
                                            ByVal count As UInt32,
                                            ByRef o() As Byte) As Boolean
        Return undeflate(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte,
                                            ByRef o() As Byte) As Boolean
        Return undeflate(i, array_size(i), o)
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte,
                                         ByVal offset As UInt32,
                                         ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(ungzip(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte,
                                         ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(ungzip(i, count, o))
        Return o
    End Function

    <Extension()> Public Function ungzip(ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(ungzip(i, o))
        Return o
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte,
                                            ByVal offset As UInt32,
                                            ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(undeflate(i, offset, count, o))
        Return o
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte,
                                            ByVal count As UInt32) As Byte()
        Dim o() As Byte = Nothing
        assert(undeflate(i, count, o))
        Return o
    End Function

    <Extension()> Public Function undeflate(ByVal i() As Byte) As Byte()
        Dim o() As Byte = Nothing
        assert(undeflate(i, o))
        Return o
    End Function
End Module
