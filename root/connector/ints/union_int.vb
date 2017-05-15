
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
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure union_int32
    <FieldOffset(0)>
    Public u_value As UInt32
    <FieldOffset(0)>
    Public s_value As Int32
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure union_int64
    <FieldOffset(0)>
    Public u_value As UInt64
    <FieldOffset(0)>
    Public s_value As Int64
End Structure
