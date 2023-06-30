
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports osi.root.constants

Public Module _sizeof
    Public ReadOnly sizeof_bool As UInt32 = type_info(Of Boolean).size_uint32()
    Public ReadOnly sizeof_sbyte As UInt32 = type_info(Of SByte).size_uint32()
    Public ReadOnly sizeof_byte As UInt32 = type_info(Of Byte).size_uint32()
    Public ReadOnly sizeof_int8 As UInt32 = type_info(Of SByte).size_uint32()
    Public ReadOnly sizeof_uint8 As UInt32 = type_info(Of Byte).size_uint32()
    Public ReadOnly sizeof_int16 As UInt32 = type_info(Of Int16).size_uint32()
    Public ReadOnly sizeof_uint16 As UInt32 = type_info(Of UInt16).size_uint32()
    Public ReadOnly sizeof_char As UInt32 = type_info(Of Char).size_uint32()
    Public ReadOnly sizeof_int32 As UInt32 = type_info(Of Int32).size_uint32()
    Public ReadOnly sizeof_uint32 As UInt32 = type_info(Of UInt32).size_uint32()
    Public ReadOnly sizeof_int64 As UInt32 = type_info(Of Int64).size_uint32()
    Public ReadOnly sizeof_uint64 As UInt32 = type_info(Of UInt64).size_uint32()
    Public ReadOnly sizeof_single As UInt32 = type_info(Of Single).size_uint32()
    Public ReadOnly sizeof_double As UInt32 = type_info(Of Double).size_uint32()

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function sizeof(Of T)() As Int32
        Return type_info(Of T).size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uint32_sizeof(Of T)() As UInt32
        Return type_info(Of T).size_uint32()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function sizeof(Of T)(ByVal obj As T) As Int32
        Try
            Return Marshal.SizeOf(obj)
        Catch
            Return npos
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function sizeof(ByVal t As Type) As Int32
        ' https://stackoverflow.com/questions/11699005/why-is-char-4-bytes-in-marshal-sizeof
        If t.Equals(GetType(Boolean)) Then
            Return 1
        End If
        If t.Equals(GetType(Char)) Then
            Return 2
        End If
        Try
            Return Marshal.SizeOf(t)
        Catch
            Return npos
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function sizeof(Of T)(ByVal i() As T) As Int32
        Return array_size_i(i)
    End Function
End Module
