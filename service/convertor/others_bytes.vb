
' TODO: Remove
Option Strict On
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _others_bytes
    <Extension()> Public Function to_bytes(ByVal i As Boolean) As Byte()
        Return bool_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Byte) As Byte()
        Return byte_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As SByte) As Byte()
        Return sbyte_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Int16) As Byte()
        Return int16_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As UInt16) As Byte()
        Return uint16_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Int32) As Byte()
        Return int32_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As UInt32) As Byte()
        Return uint32_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Int64) As Byte()
        Return int64_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As UInt64) As Byte()
        Return uint64_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Char) As Byte()
        Return char_bytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Single) As Byte()
        Return BitConverter.GetBytes(i)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Double) As Byte()
        Return BitConverter.GetBytes(i)
    End Function
End Module
