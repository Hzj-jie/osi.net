
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _bytes_others
    <Extension()> Public Function to_string(ByVal b() As Byte,
                                            Optional ByVal default_value As String = Nothing) As String
        If isemptyarray(b) Then
            Return default_value
        Else
            Return bytes_str(b)
        End If
    End Function

    <Extension()> Public Function to_bytes(ByVal b() As Byte,
                                           Optional ByVal default_value() As Byte = Nothing) As Byte()
        Dim o() As Byte = Nothing
        If bytes_bytes_ref(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_bool(ByVal b() As Byte,
                                          Optional ByVal default_value As Boolean = False) As Boolean
        Dim o As Boolean = False
        If entire_bytes_bool(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_sbyte(ByVal b() As Byte,
                                           Optional ByVal default_value As SByte = 0) As SByte
        Dim o As SByte = 0
        If entire_bytes_sbyte(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int16(ByVal b() As Byte,
                                           Optional ByVal default_value As Int16 = 0) As Int16
        Dim o As Int16 = 0
        If entire_bytes_int16(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int32(ByVal b() As Byte,
                                           Optional ByVal default_value As Int32 = 0) As Int32
        Dim o As Int32 = 0
        If entire_bytes_int32(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int64(ByVal b() As Byte,
                                           Optional ByVal default_value As Int64 = 0) As Int64
        Dim o As Int64 = 0
        If entire_bytes_int64(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_byte(ByVal b() As Byte,
                                          Optional ByVal default_value As Byte = 0) As Byte
        Dim o As Byte = 0
        If entire_bytes_byte(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint16(ByVal b() As Byte,
                                            Optional ByVal default_value As UInt16 = 0) As UInt16
        Dim o As UInt16 = 0
        If entire_bytes_uint16(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint32(ByVal b() As Byte,
                                            Optional ByVal default_value As UInt32 = 0) As UInt32
        Dim o As UInt32 = 0
        If entire_bytes_uint32(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint64(ByVal b() As Byte,
                                            Optional ByVal default_value As UInt64 = 0) As UInt64
        Dim o As UInt64 = 0
        If entire_bytes_uint64(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_single(ByVal b() As Byte,
                                            Optional ByVal default_value As Single = 0) As Single
        Dim o As Single = 0
        If entire_bytes_single(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_double(ByVal b() As Byte,
                                            Optional ByVal default_value As Double = 0) As Double
        Dim o As Double = 0
        If entire_bytes_double(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_char(ByVal b() As Byte,
                                          Optional ByVal default_value As Char = Nothing) As Char
        Dim o As Char = Nothing
        If entire_bytes_char(b, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function
End Module
