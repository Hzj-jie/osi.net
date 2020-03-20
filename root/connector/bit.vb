
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _bit
    Private ReadOnly jump_size As Byte
    Private ReadOnly jump1() As UInt64
    Private ReadOnly jump0() As UInt64

    Private ReadOnly rjump1() As UInt64
    Private ReadOnly rjump0() As UInt64

    Private ReadOnly bit_count_in_int8 As Byte
    Private ReadOnly bit_count_in_int16 As Byte
    Private ReadOnly bit_count_in_int32 As Byte
    Private ReadOnly bit_count_in_int64 As Byte

    Private ReadOnly int8_offset As Int32
    Private ReadOnly int16_offset As Int32
    Private ReadOnly int32_offset As Int32
    Private ReadOnly int64_offset As Int32

    Sub New()
        jump_size = CByte(bit_count_in_byte * sizeof_uint64)
        ReDim jump1(jump_size - 1)
        ReDim jump0(jump_size - 1)
        set_jump1()
        set_jump0()

        rjump1 = jump1.reverse()
        rjump0 = jump0.reverse()

        bit_count_in_int8 = CByte(bit_count_in_byte * sizeof_int8)
        bit_count_in_int16 = CByte(bit_count_in_byte * sizeof_int16)
        bit_count_in_int32 = CByte(bit_count_in_byte * sizeof_int32)
        bit_count_in_int64 = CByte(bit_count_in_byte * sizeof_int64)

        int8_offset = bit_count_in_int64 - bit_count_in_int8
        int16_offset = bit_count_in_int64 - bit_count_in_int16
        int32_offset = bit_count_in_int64 - bit_count_in_int32
        int64_offset = bit_count_in_int64 - bit_count_in_int64
    End Sub

    Private Sub set_jump1()
        Dim t As UInt64 = 0
        t = int64_uint64(min_int64)
        For i As Int32 = 0 To jump_size - 1
            jump1(i) = t
            t >>= 1
        Next
        assert(t = 0)
    End Sub

    Private Sub set_jump0()
        For i As Int32 = 0 To jump_size - 1
            jump0(i) = Not jump1(i)
        Next
    End Sub

    Private Sub assert_index_int8(ByVal i As Byte)
        assert(i < bit_count_in_int8)
    End Sub

    <Extension()> Public Function getbit(ByVal b As Byte, ByVal index As Byte) As Boolean
        assert_index_int8(index)
        Return (b And jump1(index + int8_offset)) <> byte_0
    End Function

    <Extension()> Public Function setbit(ByRef b As Byte,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As Byte
        assert_index_int8(index)
        If value Then
            b = CByte((b Or jump1(index + int8_offset)))
        Else
            b = CByte((b And jump0(index + int8_offset)))
        End If
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As Byte, ByVal index As Byte) As Boolean
        assert_index_int8(index)
        Return (b And rjump1(index)) <> byte_0
    End Function

    <Extension()> Public Function setrbit(ByRef b As Byte,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As Byte
        assert_index_int8(index)
        If value Then
            b = CByte((b Or rjump1(index)))
        Else
            b = CByte((b And rjump0(index)))
        End If
        Return b
    End Function

    <Extension()> Public Function getbit(ByVal b As SByte, ByVal index As Byte) As Boolean
        Return getbit(int8_uint8(b), index)
    End Function

    <Extension()> Public Function setbit(ByRef b As SByte,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As SByte
        Dim c As Byte = 0
        c = int8_uint8(b)
        setbit(c, index, value)
        b = uint8_int8(c)
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As SByte, ByVal index As Byte) As Boolean
        Return getrbit(int8_uint8(b), index)
    End Function

    <Extension()> Public Function setrbit(ByRef b As SByte,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As SByte
        Dim c As Byte = 0
        c = int8_uint8(b)
        setrbit(c, index, value)
        b = uint8_int8(c)
        Return b
    End Function

    Private Sub assert_index_int16(ByVal i As Byte)
        assert(i < bit_count_in_int16)
    End Sub

    <Extension()> Public Function getbit(ByVal b As UInt16, ByVal index As Byte) As Boolean
        assert_index_int16(index)
        Return (b And jump1(index + int16_offset)) <> uint16_0
    End Function

    <Extension()> Public Function setbit(ByRef b As UInt16,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As UInt16
        assert_index_int16(index)
        If value Then
            b = CUShort((b Or jump1(index + int16_offset)))
        Else
            b = CUShort((b And jump0(index + int16_offset)))
        End If
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As UInt16, ByVal index As Byte) As Boolean
        assert_index_int16(index)
        Return (b And rjump1(index)) <> uint16_0
    End Function

    <Extension()> Public Function setrbit(ByRef b As UInt16,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As UInt16
        assert_index_int16(index)
        If value Then
            b = CUShort((b Or rjump1(index)))
        Else
            b = CUShort((b And rjump0(index)))
        End If
        Return b
    End Function

    <Extension()> Public Function getbit(ByVal b As Int16, ByVal index As Byte) As Boolean
        Return getbit(int16_uint16(b), index)
    End Function

    <Extension()> Public Function setbit(ByRef b As Int16,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As Int16
        Dim c As UInt16 = 0
        c = int16_uint16(b)
        setbit(c, index, value)
        b = uint16_int16(c)
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As Int16, ByVal index As Byte) As Boolean
        Return getrbit(int16_uint16(b), index)
    End Function

    <Extension()> Public Function setrbit(ByRef b As Int16,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As Int16
        Dim c As UInt16 = 0
        c = int16_uint16(b)
        setrbit(c, index, value)
        b = uint16_int16(c)
        Return b
    End Function

    Private Sub assert_index_int32(ByVal i As Byte)
        assert(i < bit_count_in_int32)
    End Sub

    <Extension()> Public Function getbit(ByVal b As UInt32, ByVal index As Byte) As Boolean
        assert_index_int32(index)
        Return (b And jump1(index + int32_offset)) <> uint32_0
    End Function

    <Extension()> Public Function setbit(ByRef b As UInt32,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As UInt32
        assert_index_int32(index)
        If value Then
            b = CUInt((b Or jump1(index + int32_offset)))
        Else
            b = CUInt((b And jump0(index + int32_offset)))
        End If
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As UInt32, ByVal index As Byte) As Boolean
        assert_index_int32(index)
        Return (b And rjump1(index)) <> uint32_0
    End Function

    <Extension()> Public Function setrbit(ByRef b As UInt32,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As UInt32
        assert_index_int32(index)
        If value Then
            b = CUInt((b Or rjump1(index)))
        Else
            b = CUInt((b And rjump0(index)))
        End If
        Return b
    End Function

    <Extension()> Public Function getbit(ByVal b As Int32, ByVal index As Byte) As Boolean
        Return getbit(int32_uint32(b), index)
    End Function

    <Extension()> Public Function setbit(ByRef b As Int32,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As Int32
        Dim c As UInt32 = 0
        c = int32_uint32(b)
        setbit(c, index, value)
        b = uint32_int32(c)
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As Int32, ByVal index As Byte) As Boolean
        Return getrbit(int32_uint32(b), index)
    End Function

    <Extension()> Public Function setrbit(ByRef b As Int32,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As Int32
        Dim c As UInt32 = 0
        c = int32_uint32(b)
        setrbit(c, index, value)
        b = uint32_int32(c)
        Return b
    End Function

    Private Sub assert_index_int64(ByVal i As Byte)
        assert(i < bit_count_in_int64)
    End Sub

    <Extension()> Public Function getbit(ByVal b As UInt64, ByVal index As Byte) As Boolean
        assert_index_int64(index)
        Return (b And jump1(index + int64_offset)) <> uint64_0
    End Function

    <Extension()> Public Function setbit(ByRef b As UInt64,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As UInt64
        assert_index_int64(index)
        If value Then
            b = (b Or jump1(index + int64_offset))
        Else
            b = (b And jump0(index + int64_offset))
        End If
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As UInt64, ByVal index As Byte) As Boolean
        assert_index_int64(index)
        Return (b And rjump1(index)) <> uint64_0
    End Function

    <Extension()> Public Function setrbit(ByRef b As UInt64,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As UInt64
        assert_index_int64(index)
        If value Then
            b = (b Or rjump1(index))
        Else
            b = (b And rjump0(index))
        End If
        Return b
    End Function

    <Extension()> Public Function getbit(ByVal b As Int64, ByVal index As Byte) As Boolean
        Return getbit(int64_uint64(b), index)
    End Function

    <Extension()> Public Function setbit(ByRef b As Int64,
                                         ByVal index As Byte,
                                         Optional ByVal value As Boolean = True) As Int64
        Dim c As UInt64 = 0
        c = int64_uint64(b)
        setbit(c, index, value)
        b = uint64_int64(c)
        Return b
    End Function

    <Extension()> Public Function getrbit(ByVal b As Int64, ByVal index As Byte) As Boolean
        Return getrbit(int64_uint64(b), index)
    End Function

    <Extension()> Public Function setrbit(ByRef b As Int64,
                                          ByVal index As Byte,
                                          Optional ByVal value As Boolean = True) As Int64
        Dim c As UInt64 = 0
        c = int64_uint64(b)
        setrbit(c, index, value)
        b = uint64_int64(c)
        Return b
    End Function

    Private Function bit_byte(ByVal c As Char, ByRef v As Boolean) As Boolean
        Select Case c
            Case character._0
                v = False
                Return True
            Case character._1
                v = True
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function bit_byte(ByVal s As String, ByVal start As Int32, ByRef b As Byte) As Boolean
        If strlen(s) < start + bit_count_in_byte Then
            Return False
        End If
        b = 0
        For i As Int32 = start To start + bit_count_in_byte - 1
            Dim v As Boolean = False
            If bit_byte(s(i), v) Then
                setbit(b, CByte(i - start), v)
            Else
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function bit_bytes(ByVal s As String,
                                            ByVal buff() As Byte,
                                            Optional ByVal start As Int32 = 0) As Boolean
        If strlen(s) Mod bit_count_in_byte <> 0 OrElse
           array_size(buff) < start + strlen(s) \ bit_count_in_byte Then
            Return False
        Else
            Dim j As Int32 = 0
            j = start
            For i As Int32 = 0 To strlen_i(s) - 1 Step bit_count_in_byte
                If bit_byte(s, i, buff(j)) Then
                    j += 1
                Else
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    <Extension()> Public Function bit_int8(ByVal s As String, ByRef r As SByte) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int8 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_int8(b, r)
    End Function

    <Extension()> Public Function bit_uint8(ByVal s As String, ByRef r As Byte) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int8 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_uint8(b, r)
    End Function

    <Extension()> Public Function bit_int16(ByVal s As String, ByRef r As Int16) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int16 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_int16(b, r)
    End Function

    <Extension()> Public Function bit_uint16(ByVal s As String, ByRef r As UInt16) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int16 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_uint16(b, r)
    End Function

    <Extension()> Public Function bit_int32(ByVal s As String, ByRef r As Int32) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int32 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_int32(b, r)
    End Function

    <Extension()> Public Function bit_uint32(ByVal s As String, ByRef r As UInt32) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int32 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_uint32(b, r)
    End Function

    <Extension()> Public Function bit_int64(ByVal s As String, ByRef r As Int64) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int64 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_int64(b, r)
    End Function

    <Extension()> Public Function bit_uint64(ByVal s As String, ByRef r As UInt64) As Boolean
        Dim b() As Byte = Nothing
        ReDim b(CInt(sizeof_int64 - 1))
        Return bit_bytes(s, b) AndAlso
               bytes_uint64(b, r)
    End Function

    'bit_count is the minmum bit count to hold the value, say (7)10 = (111)2, so 7.bit_count() = 3
    <Extension()> Public Function bit_count(ByVal v As Byte) As Byte
        Dim r As Byte = 0
        While v > 0
            r += uint8_1
            v >>= 1
        End While
        Return r
#If 0 Then
        'this solution will trigger UInt64.CompareTo, so it's slow
        For i As Byte = 0 To jump_size - 1
            If rjump1(i) > v Then
                Return i
            End If
        Next
        assert(False)
        Return 0
#End If
    End Function

    <Extension()> Public Function bit_count(ByVal v As SByte) As Byte
        Return bit_count(sbyte_byte(v))
    End Function

    <Extension()> Public Function bit_count(ByVal v As UInt16) As Byte
        Dim r As Byte = 0
        While v > 0
            r += uint8_1
            v >>= 1
        End While
        Return r
#If 0 Then
        For i As Byte = 0 To jump_size - 1
            If rjump1(i) > v Then
                Return i
            End If
        Next
        assert(False)
        Return 0
#End If
    End Function

    <Extension()> Public Function bit_count(ByVal v As Int16) As Byte
        Return bit_count(int16_uint16(v))
    End Function

    <Extension()> Public Function bit_count(ByVal v As UInt32) As Byte
        Dim r As Byte = 0
        While v > 0
            r += uint8_1
            v >>= 1
        End While
        Return r
#If 0 Then
        For i As Byte = 0 To jump_size - 1
            If rjump1(i) > v Then
                Return i
            End If
        Next
        assert(False)
        Return 0
#End If
    End Function

    <Extension()> Public Function bit_count(ByVal v As Int32) As Byte
        Return bit_count(int32_uint32(v))
    End Function

    <Extension()> Public Function bit_count(ByVal v As UInt64) As Byte
        For i As Byte = 0 To jump_size - uint8_1
            If rjump1(i) > v Then
                Return i
            End If
        Next
        Return jump_size
    End Function

    <Extension()> Public Function bit_count(ByVal v As Int64) As Byte
        Return bit_count(int64_uint64(v))
    End Function
End Module
