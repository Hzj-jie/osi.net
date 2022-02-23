
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

Public Module _str_bytes
    Public ReadOnly default_encoding As Encoding = Encoding.UTF8()

    Private Function valid_input_parameters(ByVal s As String,
                                            ByVal start As UInt32,
                                            ByVal len As UInt32) As Boolean
        Return start + len <= strlen(s) AndAlso
               s IsNot Nothing
    End Function

    Private Function valid_input_parameters(ByVal b() As Byte,
                                            ByVal start As UInt32,
                                            ByVal len As UInt32) As Boolean
        Return start + len <= array_size(b) AndAlso
               b IsNot Nothing
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByVal d() As Byte,
                              ByRef offset As UInt32) As Boolean
        Dim l As UInt32 = 0
        If Not str_byte_count(s, start, len, l) Then
            Return False
        End If
        If l + offset > array_size(d) Then
            Return False
        End If
        If l > 0 Then
            assert(l = default_encoding.GetBytes(s, CInt(start), CInt(len), d, CInt(offset)))
            offset += l
        End If
        Return True
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByRef d() As Byte) As Boolean
        If Not valid_input_parameters(s, start, len) Then
            Return False
        End If
        d = default_encoding.GetBytes(s, CInt(start), CInt(len))
        Return True
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal d() As Byte,
                              ByRef offset As UInt32) As Boolean
        If strlen(s) < start Then
            Return False
        End If
        Return str_bytes(s, start, strlen(s) - start, d, offset)
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByRef d() As Byte) As Boolean
        If strlen(s) < start Then
            Return False
        End If
        Return str_bytes(s, start, strlen(s) - start, d)
    End Function

    Public Function str_bytes(ByVal s As String, ByVal d() As Byte, ByRef offset As UInt32) As Boolean
        Return str_bytes(s, uint32_0, strlen(s), d, offset)
    End Function

    Public Function str_bytes_ref(ByVal s As String, ByRef d() As Byte) As Boolean
        Return str_bytes(s, uint32_0, d)
    End Function

    Public Function str_bytes_val(ByVal s As String, ByVal d() As Byte) As Boolean
        Return str_bytes(s, d, uint32_0)
    End Function

    Public Function str_bytes(ByVal s As String, ByVal start As UInt32, ByVal len As UInt32) As Byte()
        If valid_input_parameters(s, start, len) Then
            Return default_encoding.GetBytes(s, CInt(start), CInt(len))
        End If
        Return Nothing
    End Function

    Public Function str_bytes(ByVal s As String, ByVal start As UInt32) As Byte()
        If strlen(s) < start Then
            Return Nothing
        End If
        Return str_bytes(s, start, strlen(s) - start)
    End Function

    Public Function str_bytes(ByVal s As String) As Byte()
        Return str_bytes(s, uint32_0, strlen(s))
    End Function

    Public Function str_byte_count(ByVal s As String,
                                   ByVal start As UInt32,
                                   ByVal len As UInt32,
                                   ByRef count As UInt32) As Boolean
        If Not valid_input_parameters(s, start, len) Then
            Return False
        End If
        Dim r As Int32 = 0
        r = default_encoding.GetByteCount(s, CInt(start), CInt(len))
        If r < 0 Then
            Return False
        End If
        count = CUInt(r)
        Return True
    End Function

    Public Function str_byte_count(ByVal s As String, ByVal start As UInt32, ByVal len As UInt32) As UInt32
        Dim c As UInt32 = uint32_0
        If str_byte_count(s, start, len, c) Then
            Return c
        End If
        Return uint32_0
    End Function

    Public Function str_byte_count(ByVal s As String, ByVal start As UInt32) As UInt32
        If strlen(s) < start Then
            Return uint32_0
        End If
        Return str_byte_count(s, start, strlen(s) - start)
    End Function

    Public Function str_byte_count(ByVal s As String) As UInt32
        Return str_byte_count(s, uint32_0, strlen(s))
    End Function

    Public Function bytes_char_count(ByVal b() As Byte, ByVal start As UInt32, ByVal len As UInt32) As UInt32
        If Not valid_input_parameters(b, start, len) Then
            Return uint32_0
        End If
        Dim r As Int32 = 0
        r = default_encoding.GetCharCount(b, CInt(start), CInt(len))
        If r >= 0 Then
            Return CUInt(r)
        End If
        Return uint32_0
    End Function

    Public Function bytes_char_count(ByVal b() As Byte, ByVal start As UInt32) As UInt32
        If array_size(b) < start Then
            Return uint32_0
        End If
        Return bytes_char_count(b, start, array_size(b) - start)
    End Function

    Public Function bytes_char_count(ByVal b() As Byte) As UInt32
        Return bytes_char_count(b, uint32_0, array_size(b))
    End Function

    Public Function bytes_str(ByVal b() As Byte,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByRef o As String) As Boolean
        If Not valid_input_parameters(b, start, len) Then
            Return False
        End If
        o = default_encoding.GetString(b, CInt(start), CInt(len))
        Return True
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32, ByVal len As UInt32) As String
        Dim o As String = Nothing
        If bytes_str(b, start, len, o) Then
            Return o
        End If
        Return Nothing
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32, ByRef o As String) As Boolean
        If array_size(b) < start Then
            Return False
        End If
        assert(bytes_str(b, start, array_size(b) - start, o))
        Return True
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32) As String
        Dim o As String = Nothing
        If bytes_str(b, start, o) Then
            Return o
        End If
        Return Nothing
    End Function

    Public Function bytes_str(ByVal b() As Byte) As String
        Return bytes_str(b, uint32_0, array_size(b))
    End Function
End Module
