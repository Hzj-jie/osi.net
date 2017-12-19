
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

Public Module _str_bytes
    Public ReadOnly default_encoding As Encoding

    Sub New()
        default_encoding = Encoding.UTF8()
    End Sub

    Private Function valid_input_parameters(ByVal s As String,
                                            ByVal start As UInt32,
                                            ByVal len As UInt32) As Boolean
        Return start + len <= strlen(s) AndAlso
               Not s Is Nothing
    End Function

    Private Function valid_input_parameters(ByVal b() As Byte,
                                            ByVal start As UInt32,
                                            ByVal len As UInt32) As Boolean
        Return start + len <= array_size(b) AndAlso
               Not b Is Nothing
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByVal d() As Byte,
                              ByRef offset As UInt32) As Boolean
        Dim l As UInt32 = 0
        If str_byte_count(s, start, len, l) Then
            If l + offset <= array_size(d) Then
                If l > 0 Then
                    assert(l = default_encoding.GetBytes(s, CInt(start), CInt(len), d, CInt(offset)))
                    offset += l
                    Return True
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByRef d() As Byte) As Boolean
        If valid_input_parameters(s, start, len) Then
            d = default_encoding.GetBytes(s, CInt(start), CInt(len))
            Return True
        Else
            Return False
        End If
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByVal d() As Byte,
                              ByRef offset As UInt32) As Boolean
        If strlen(s) < start Then
            Return False
        Else
            Return str_bytes(s, start, strlen(s) - start, d, offset)
        End If
    End Function

    Public Function str_bytes(ByVal s As String,
                              ByVal start As UInt32,
                              ByRef d() As Byte) As Boolean
        If strlen(s) < start Then
            Return False
        Else
            Return str_bytes(s, start, strlen(s) - start, d)
        End If
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
        Else
            Return Nothing
        End If
    End Function

    Public Function str_bytes(ByVal s As String, ByVal start As UInt32) As Byte()
        If strlen(s) < start Then
            Return Nothing
        Else
            Return str_bytes(s, start, strlen(s) - start)
        End If
    End Function

    Public Function str_bytes(ByVal s As String) As Byte()
        Return str_bytes(s, uint32_0, strlen(s))
    End Function

    Public Function str_byte_count(ByVal s As String,
                                   ByVal start As UInt32,
                                   ByVal len As UInt32,
                                   ByRef count As UInt32) As Boolean
        If valid_input_parameters(s, start, len) Then
            Dim r As Int32 = 0
            r = default_encoding.GetByteCount(s, CInt(start), CInt(len))
            If r >= 0 Then
                count = CUInt(r)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function str_byte_count(ByVal s As String, ByVal start As UInt32, ByVal len As UInt32) As UInt32
        Dim c As UInt32 = uint32_0
        If str_byte_count(s, start, len, c) Then
            Return c
        Else
            Return uint32_0
        End If
    End Function

    Public Function str_byte_count(ByVal s As String, ByVal start As UInt32) As UInt32
        If strlen(s) < start Then
            Return uint32_0
        Else
            Return str_byte_count(s, start, strlen(s) - start)
        End If
    End Function

    Public Function str_byte_count(ByVal s As String) As UInt32
        Return str_byte_count(s, uint32_0, strlen(s))
    End Function

    Public Function bytes_char_count(ByVal b() As Byte, ByVal start As UInt32, ByVal len As UInt32) As UInt32
        If valid_input_parameters(b, start, len) Then
            Dim r As Int32 = 0
            r = default_encoding.GetCharCount(b, CInt(start), CInt(len))
            If r >= 0 Then
                Return CUInt(r)
            Else
                Return uint32_0
            End If
        Else
            Return uint32_0
        End If
    End Function

    Public Function bytes_char_count(ByVal b() As Byte, ByVal start As UInt32) As UInt32
        If array_size(b) < start Then
            Return uint32_0
        Else
            Return bytes_char_count(b, start, array_size(b) - start)
        End If
    End Function

    Public Function bytes_char_count(ByVal b() As Byte) As UInt32
        Return bytes_char_count(b, uint32_0, array_size(b))
    End Function

    Public Function bytes_str(ByVal b() As Byte,
                              ByVal start As UInt32,
                              ByVal len As UInt32,
                              ByRef o As String) As Boolean
        If valid_input_parameters(b, start, len) Then
            o = default_encoding.GetString(b, CInt(start), CInt(len))
            Return True
        Else
            Return False
        End If
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32, ByVal len As UInt32) As String
        Dim o As String = Nothing
        If bytes_str(b, start, len, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32, ByRef o As String) As Boolean
        If array_size(b) < start Then
            Return False
        Else
            assert(bytes_str(b, start, array_size(b) - start, o))
            Return True
        End If
    End Function

    Public Function bytes_str(ByVal b() As Byte, ByVal start As UInt32) As String
        Dim o As String = Nothing
        If bytes_str(b, start, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Function bytes_str(ByVal b() As Byte) As String
        Return bytes_str(b, uint32_0, array_size(b))
    End Function
End Module
