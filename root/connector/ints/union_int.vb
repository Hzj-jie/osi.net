
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Explicit)>
Public Structure union_byte
    <FieldOffset(0)>
    Public u_value As Byte
    <FieldOffset(0)>
    Public s_value As SByte
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure union_int16
    <FieldOffset(0)>
    Public u_value As UInt16
    <FieldOffset(0)>
    Public s_value As Int16
    <FieldOffset(0)>
    Public first_u8 As Byte
    <FieldOffset(0)>
    Public first_s8 As Byte
    <FieldOffset(1)>
    Public second_u8 As Byte
    <FieldOffset(1)>
    Public second_s8 As Byte
End Structure

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId:="second_u16")>
<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId:="second_s16")>
<StructLayout(LayoutKind.Explicit)>
Public Structure union_int32
    <FieldOffset(0)>
    Public u_value As UInt32
    <FieldOffset(0)>
    Public s_value As Int32
    <FieldOffset(0)>
    Public first_u16 As UInt16
    <FieldOffset(0)>
    Public first_s16 As Int16
    <FieldOffset(1)>
    Public second_u16 As UInt16
    <FieldOffset(1)>
    Public second_s16 As Int16
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure union_int64
    <FieldOffset(0)>
    Public u_value As UInt64
    <FieldOffset(0)>
    Public s_value As Int64
    <FieldOffset(0)>
    Public first_u32 As UInt32
    <FieldOffset(0)>
    Public first_s32 As Int32
    <FieldOffset(4)>
    Public second_u32 As UInt32
    <FieldOffset(4)>
    Public second_s32 As Int32
End Structure
