
Option Strict On

Imports osi.root.constants

Public Module _non_integer_overflow
    Public Function uint8_int8(ByVal i As Byte) As SByte
        Return byte_sbyte(i)
    End Function

    Public Function int8_uint8(ByVal i As SByte) As Byte
        Return sbyte_byte(i)
    End Function

    Public Function byte_int8(ByVal i As Byte) As SByte
        Return byte_sbyte(i)
    End Function

    Public Function byte_uint8(ByVal i As Byte) As Byte
        Return i
    End Function

    Public Function int8_byte(ByVal i As SByte) As Byte
        Return sbyte_byte(i)
    End Function

    Public Function uint8_byte(ByVal i As Byte) As Byte
        Return i
    End Function

    Public Function byte_sbyte(ByVal i As Byte) As SByte
        Dim x As union_byte
        x.u_value = i
        Return x.s_value
    End Function

    Public Function sbyte_byte(ByVal i As SByte) As Byte
        Dim x As union_byte
        x.s_value = i
        Return x.u_value
    End Function

    Public Function uint16_int16(ByVal i As UInt16) As Int16
        Dim x As union_int16
        x.u_value = i
        Return x.s_value
    End Function

    Public Function int16_uint16(ByVal i As Int16) As UInt16
        Dim x As union_int16
        x.s_value = i
        Return x.u_value
    End Function

    Public Function uint32_int32(ByVal i As UInt32) As Int32
        Dim x As union_int32
        x.u_value = i
        Return x.s_value
    End Function

    Public Function int32_uint32(ByVal i As Int32) As UInt32
        Dim x As union_int32
        x.s_value = i
        Return x.u_value
    End Function

    Public Function uint64_int64(ByVal i As UInt64) As Int64
        Dim x As union_int64
        x.u_value = i
        Return x.s_value
    End Function

    Public Function int64_uint64(ByVal i As Int64) As UInt64
        Dim x As union_int64
        x.s_value = i
        Return x.u_value
    End Function
End Module
