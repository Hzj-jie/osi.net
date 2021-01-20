
Option Explicit On
Option Infer Off
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

    Public Shared Function to_little_endian(Of T)(ByVal i As T) As T
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

    Public Shared Function reverse(ByVal i As Char) As Char
        Return uint16_char(reverse(char_uint16(i)))
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

    Public Shared Function reverse(Of T)(ByVal i As T) As T
        Return reverse_cache(Of T).f(i)
    End Function

    Public Shared Function supported(Of T)() As Boolean
        Return reverse_supported_cache(Of T).v
    End Function

    Private NotInheritable Class reverse_supported_cache(Of T)
        Public Shared ReadOnly v As Boolean =
            type_info(Of T).is_number AndAlso Not type_info(Of T, type_info_operators.equal, Decimal).v

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class reverse_cache(Of T)
        Public Shared ReadOnly f As Func(Of T, T) = calculate_f()

        Private Shared Function calculate_f() As Func(Of T, T)
            Dim f As Func(Of T, T) = Nothing
            assert(supported(Of T)())
            If type_info(Of T, type_info_operators.equal, SByte).v Then
                f = c(Of SByte)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Byte).v Then
                f = c(Of Byte)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Int16).v Then
                f = c(Of Int16)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, UInt16).v Then
                f = c(Of UInt16)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Int32).v Then
                f = c(Of Int32)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, UInt32).v Then
                f = c(Of UInt32)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Int64).v Then
                f = c(Of Int64)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, UInt64).v Then
                f = c(Of UInt64)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Single).v Then
                f = c(Of Single)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Double).v Then
                f = c(Of Double)(AddressOf reverse)
            ElseIf type_info(Of T, type_info_operators.equal, Char).v Then
                f = c(Of Char)(AddressOf reverse)
            Else
                assert(False)
            End If
            assert(Not f Is Nothing)
            Return f
        End Function

        Private Shared Function c(Of VT)(ByVal f As Func(Of VT, VT)) As Func(Of T, T)
            assert(Not f Is Nothing)
            Return Function(ByVal i As T) As T
                       Return direct_cast(Of T)(f(direct_cast(Of VT)(i)))
                   End Function
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
