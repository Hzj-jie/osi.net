
Option Strict On
Imports osi.root.constants

Public NotInheritable Class endian
    Public Shared Function to_little_endian(ByVal i As SByte) As SByte
        Return i
    End Function

    Public Shared Function to_little_endian(ByVal i As Byte) As Byte
        Return i
    End Function

    Public Shared Function to_little_endian(ByVal i As Int16) As Int16
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As UInt16) As UInt16
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As Int32) As Int32
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As UInt32) As UInt32
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As Int64) As Int64
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As UInt64) As UInt64
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As Single) As Single
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function to_little_endian(ByVal i As Double) As Double
        If BitConverter.IsLittleEndian Then
            Return i
        End If
        Return reverse(i)
    End Function

    Public Shared Function reverse(ByVal i As SByte) As SByte
        Return i
    End Function

    Public Shared Function reverse(ByVal i As Byte) As Byte
        Return i
    End Function

    Public Shared Function reverse(ByVal i As Int16) As Int16
        Dim r As Int16 = 0
        For j As Int32 = 0 To CInt(sizeof_int16) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As UInt16) As UInt16
        Dim r As UInt16 = 0
        For j As Int32 = 0 To CInt(sizeof_uint16) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As Int32) As Int32
        Dim r As Int32 = 0
        For j As Int32 = 0 To CInt(sizeof_int32) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As UInt32) As UInt32
        Dim r As UInt32 = 0
        For j As Int32 = 0 To CInt(sizeof_uint32) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As Int64) As Int64
        Dim r As Int64 = 0
        For j As Int32 = 0 To CInt(sizeof_int64) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As UInt64) As UInt64
        Dim r As UInt64 = 0
        For j As Int32 = 0 To CInt(sizeof_uint64) - 1
            r <<= bit_count_in_byte
            r += (i And max_uint8)
            i >>= bit_count_in_byte
        Next
        Return r
    End Function

    Public Shared Function reverse(ByVal i As Single) As Single
        Dim j As Int32 = 0
        j = reverse(BitConverter.ToInt32(BitConverter.GetBytes(i), 0))
        Return BitConverter.ToSingle(BitConverter.GetBytes(j), 0)
    End Function

    Public Shared Function reverse(ByVal i As Double) As Double
        Return BitConverter.Int64BitsToDouble(reverse(BitConverter.DoubleToInt64Bits(i)))
    End Function

    Private Sub New()
    End Sub
End Class
