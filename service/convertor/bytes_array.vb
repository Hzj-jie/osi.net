
' TODO Remove
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

'mainly for serialization and deserialization
'writing a generic serialization / deserialization in .net is almost impossible,
'so just leave the to_bytes logic in each type,
'but this static class is only focusing on assembly vector<byte()> to byte() and deassembly it
Public Module _bytes_array
    Public ReadOnly preamble_length As UInt32
    Private ReadOnly preamble_length_shift As UInt32
    Private ReadOnly checksum As Int32

    Sub New()
        preamble_length_shift = 3
        preamble_length = (1 << preamble_length_shift)
        assert(preamble_length = (sizeof_int32 << 1))
        Dim cs() As Byte = Nothing
        cs = Text.Encoding.Unicode().GetBytes("HH")
        assert(array_size(cs) = sizeof_int32)
        checksum = bytes_int32(cs)
    End Sub

    Private Sub write_preamble(ByVal d() As Byte,
                               ByVal offset As UInt32,
                               ByVal count As UInt32,
                               ByVal r() As Byte,
                               ByRef index As UInt32)
        assert(array_size(r) >= preamble_length + index)
        assert(uint32_bytes(count, r, index))
        assert(uint32_bytes(count Xor checksum, r, index))
    End Sub

    Private Sub write_preamble(ByVal d() As Byte,
                               ByVal r() As Byte,
                               ByRef index As UInt32)
        write_preamble(d, uint32_0, array_size(d), r, index)
    End Sub

    Private Sub write_data(ByVal d() As Byte,
                           ByVal offset As UInt32,
                           ByVal count As UInt32,
                           ByVal r() As Byte,
                           ByRef index As UInt32)
        assert(array_size(d) >= offset + count)
        assert(array_size(r) >= count + index)
        memcpy(r, index, d, offset, count)
        index += array_size(d)
    End Sub

    Private Sub write_data(ByVal d() As Byte,
                           ByVal r() As Byte,
                           ByRef index As UInt32)
        write_data(d, 0, array_size(d), r, index)
    End Sub

    Private Function read_preamble(ByVal d() As Byte, ByRef l As UInt32, ByRef index As UInt32) As Boolean
        If array_size(d) - index >= preamble_length Then
            assert(bytes_int32(d, l, index))
            If l >= 0 Then
                Dim c As UInt32 = 0
                assert(bytes_uint32(d, c, index))
                Return (l Xor checksum) = c
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function parse_as_preamble(ByVal b() As Byte, ByRef l As UInt32) As Boolean
        Return read_preamble(b, l, uint32_0)
    End Function

    <Extension()> Public Function from_chunk(ByVal b() As Byte,
                                             ByVal offset As UInt32,
                                             ByVal count As UInt32,
                                             ByRef o() As Byte) As Boolean
        Dim index As UInt32 = uint32_0
        index = offset
        Return read_chunk(b, index, o) AndAlso index - offset = count
    End Function

    <Extension()> Public Function from_chunk(ByVal b() As Byte,
                                             ByVal count As UInt32,
                                             ByRef o() As Byte) As Boolean
        Return from_chunk(b, uint32_0, count, o)
    End Function

    <Extension()> Public Function from_chunk(ByVal b() As Byte, ByRef o() As Byte) As Boolean
        Return from_chunk(b, array_size(b), o)
    End Function

    <Extension()> Public Function to_chunk(ByVal b() As Byte,
                                           ByVal offset As UInt32,
                                           ByVal count As UInt32,
                                           ByRef o() As Byte) As Boolean
        If array_size(b) < offset + count Then
            Return False
        Else
            ReDim o(preamble_length + count - 1)
            Dim index As UInt32 = 0
            write_preamble(b, offset, count, o, index)
            write_data(b, offset, count, o, index)
            Return True
        End If
    End Function

    <Extension()> Public Function to_chunk(ByVal b() As Byte,
                                           ByVal offset As UInt32,
                                           ByVal count As UInt32) As Byte()
        Dim r() As Byte = Nothing
        assert(b.to_chunk(offset, count, r))
        Return r
    End Function

    <Extension()> Public Function to_chunk(ByVal b() As Byte) As Byte()
        Return b.to_chunk(0, array_size(b))
    End Function

    Private Function chunk_length(ByVal a As vector(Of Byte())) As Int64
        assert(Not a Is Nothing)
        If a.empty() Then
            Return 0
        Else
            Dim l As UInt64 = 0
            For i As Int32 = 0 To a.size() - 1
                l += array_size(a(i))
            Next
            Return l + (a.size() << preamble_length_shift)
        End If
    End Function

    <Extension()> Public Function vector_bytes_bytes_ref(ByVal a As vector(Of Byte()),
                                                         ByRef o() As Byte) As Boolean
        If a Is Nothing Then
            Return False
        Else
            Dim l As UInt64 = 0
            l = chunk_length(a)
            ReDim o(l - 1)
            If Not a.empty() Then
                Dim p As UInt32 = 0
                For i As UInt32 = 0 To a.size() - uint32_1
                    write_preamble(a(i), o, p)
                    write_data(a(i), o, p)
                Next
                assert(p = l)
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function vector_bytes_bytes_val(ByVal a As vector(Of Byte()),
                                                         ByVal o() As Byte,
                                                         Optional ByRef offset As UInt32 = 0) As Boolean
        If a Is Nothing Then
            Return False
        Else
            Dim l As UInt64 = 0
            l = chunk_length(a)
            If array_size(o) < offset + l Then
                Return False
            Else
                If a.empty() Then
                    assert(l = 0)
                Else
                    Dim p As UInt32 = 0
                    p = offset
                    For i As UInt32 = 0 To a.size() - uint32_1
                        write_preamble(a(i), o, p)
                        write_data(a(i), o, p)
                    Next
                    assert(p = offset + l)
                    offset = p
                End If
                Return True
            End If
        End If
    End Function

    <Extension()> Public Function to_bytes(ByVal a As vector(Of Byte()),
                                           Optional ByVal default_value() As Byte = Nothing) As Byte()
        Dim o() As Byte = Nothing
        If vector_bytes_bytes_ref(a, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    Private Function read_chunk(ByVal b() As Byte, ByRef index As UInt32, ByRef r() As Byte) As Boolean
        Dim l As UInt32 = 0
        If read_preamble(b, l, index) AndAlso array_size(b) >= index + l Then
            If l = 0 Then
                r = Nothing
            Else
                ReDim r(l - 1)
                memcpy(r, uint32_0, b, index, l)
                index += l
            End If
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function bytes_vector_bytes(ByVal b() As Byte,
                                                     ByVal ii As UInt32,
                                                     ByVal il As UInt32,
                                                     ByRef o As vector(Of Byte())) As Boolean
        If array_size(b) < ii + il Then
            Return False
        ElseIf il = uint32_0 Then
            o.renew()
            Return True
        Else
            Dim p As UInt32 = 0
            p = ii
            Dim c() As Byte = Nothing
            o.renew()
            While read_chunk(b, p, c)
                o.emplace_back(c)
                If p - ii >= il Then
                    Exit While
                End If
            End While
            Return p - ii = il
        End If
    End Function

    <Extension()> Public Function bytes_vector_bytes(ByVal b() As Byte,
                                                     ByRef o As vector(Of Byte()),
                                                     Optional ByRef offset As UInt32 = 0) As Boolean
        If array_size(b) < offset Then
            Return False
        ElseIf array_size(b) = offset Then
            o.renew()
            Return True
        Else
            Dim p As UInt32 = 0
            p = offset
            o.renew()
            Dim c() As Byte = Nothing
            While read_chunk(b, p, c)
                o.emplace_back(c)
            End While
            If p = array_size(b) Then
                offset = p
                Return True
            Else
                Return False
            End If
        End If
    End Function

    <Extension()> Public Function entire_bytes_vector_bytes(ByVal b() As Byte,
                                                            ByRef o As vector(Of Byte())) As Boolean
        Dim p As UInt32 = 0
        Return bytes_vector_bytes(b, o, p) AndAlso
               p = array_size(b)
    End Function

    <Extension()> Public Function to_vector_bytes(ByVal b() As Byte,
                                                  Optional ByVal default_value _
                                                                    As vector(Of Byte()) = Nothing) _
                                                 As vector(Of Byte())
        Dim o As vector(Of Byte()) = Nothing
        If bytes_vector_bytes(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function contains_chunk(ByVal b() As Byte,
                                                 ByVal search() As Byte) As Boolean
        If isemptyarray(b) Then
            Return False
        ElseIf isemptyarray(search) Then
            Return True
        Else
            Dim p As UInt32 = 0
            Dim c() As Byte = Nothing
            While read_chunk(b, p, c)
                If memcmp(c, search) = 0 Then
                    Return True
                End If
            End While
            Return False
        End If
    End Function
End Module
