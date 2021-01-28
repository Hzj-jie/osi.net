
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _non_integer_overflow
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint8_int8(ByVal i As Byte) As SByte
        Return byte_sbyte(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function int8_uint8(ByVal i As SByte) As Byte
        Return sbyte_byte(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function byte_int8(ByVal i As Byte) As SByte
        Return byte_sbyte(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function byte_uint8(ByVal i As Byte) As Byte
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function int8_byte(ByVal i As SByte) As Byte
        Return sbyte_byte(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint8_byte(ByVal i As Byte) As Byte
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function byte_sbyte(ByVal i As Byte) As SByte
        Return New union_byte() With {.u_value = i}.s_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function sbyte_byte(ByVal i As SByte) As Byte
        Return New union_byte() With {.s_value = i}.u_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint16_int16(ByVal i As UInt16) As Int16
        Return New union_int16() With {.u_value = i}.s_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function int16_uint16(ByVal i As Int16) As UInt16
        Return New union_int16() With {.s_value = i}.u_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint32_int32(ByVal i As UInt32) As Int32
        Return New union_int32() With {.u_value = i}.s_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function int32_uint32(ByVal i As Int32) As UInt32
        Return New union_int32() With {.s_value = i}.u_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint64_int64(ByVal i As UInt64) As Int64
        Return New union_int64() With {.u_value = i}.s_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function int64_uint64(ByVal i As Int64) As UInt64
        Return New union_int64() With {.s_value = i}.u_value
    End Function
End Module
