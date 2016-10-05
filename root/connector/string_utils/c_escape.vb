
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _c_escape
    Private Const escape_char As Char = character.right_slash

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal str_len As UInt32) As String
        Dim o As String = Nothing
        assert(c_escape(s, str_start, str_len, o))
        Return o
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32) As String
        assert(strlen(s) >= str_start)
        Return c_escape(s, str_start, strlen(s) - str_start)
    End Function

    <Extension()> Public Function c_escape(ByVal s As String) As String
        Return c_escape(s, uint32_0, strlen(s))
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal str_len As UInt32,
                                           ByRef o As String) As Boolean
        Dim buff_size As UInt32 = 0
        buff_size = (str_len << 1)
        Dim b As StringBuilder = Nothing
        b = New StringBuilder(CInt(buff_size))
        b.Length() = b.Capacity()
        Dim buff_start As Int32 = 0
        If c_escape(s, str_start, str_len, b, buff_start, buff_size) Then
            o = b.ToString(0, buff_start)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByRef o As String) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_escape(s, str_start, strlen(s) - str_start, o)
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByRef o As String) As Boolean
        Return c_escape(s, uint32_0, strlen(s), o)
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal str_len As UInt32,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32,
                                           ByVal buff_len As UInt32) As Boolean
        If str_len = 0 Then
            Return True
        ElseIf strlen(s) < str_start + str_len OrElse
               strlen(buff) < buff_start + buff_len Then
            Return False
        Else
            Dim buff_end As UInt32 = 0
            buff_end = buff_start + buff_len
            For i As UInt32 = str_start To str_start + str_len - uint32_1
                Dim c As Char = Nothing
                Select Case s(i)
                    Case character.alert
                        c = character.a
                    Case character.feed
                        c = character.f
                    Case character.vtab
                        c = character.v
                    Case character.single_quotation
                        c = character.single_quotation
                    Case character.quote
                        c = character.quote
                    Case character.question_mark
                        c = character.question_mark
                    Case escape_char
                        c = escape_char
                    Case character.newline
                        c = character.n
                    Case character.return
                        c = character.r
                    Case character.tab
                        c = character.t
                    Case character.backspace
                        c = character.b
                    Case character.null
                        c = character._0
                    Case Else
                        c = Nothing
                End Select
                If buff_start = buff_end Then
                    Return False
                End If
                If c = Nothing Then
                    buff(buff_start) = s(i)
                    buff_start += 1
                Else
                    buff(buff_start) = escape_char
                    buff_start += 1
                    If buff_start = buff_end Then
                        Return False
                    End If
                    buff(buff_start) = c
                    buff_start += 1
                End If
            Next
            Return True
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal str_len As UInt32,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32) As Boolean
        If strlen(buff) < buff_start Then
            Return False
        Else
            Return c_escape(s, str_start, str_len, buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32,
                                           ByVal buff_len As UInt32) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_escape(s, str_start, strlen(s) - str_start, buff, buff_start, buff_len)
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_escape(s, str_start, strlen(s) - str_start, buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal str_len As UInt32,
                                           ByVal buff As StringBuilder) As Boolean
        Return c_escape(s, str_start, str_len, buff, uint32_0, strlen(buff))
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32,
                                           ByVal buff_len As UInt32) As Boolean
        Return c_escape(s, uint32_0, strlen(s), buff, buff_start, buff_len)
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal str_start As UInt32,
                                           ByVal buff As StringBuilder) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_escape(s, str_start, strlen(s) - str_start, buff, uint32_0, strlen(buff))
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal buff As StringBuilder,
                                           ByRef buff_start As UInt32) As Boolean
        If strlen(buff) < buff_start Then
            Return False
        Else
            Return c_escape(s, uint32_0, strlen(s), buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_escape(ByVal s As String,
                                           ByVal buff As StringBuilder) As Boolean
        Return c_escape(s, uint32_0, strlen(s), buff, 0, strlen(buff))
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal str_len As UInt32,
                                             ByRef o As String) As Boolean
        Dim buff_size As UInt32 = 0
        buff_size = str_len
        Dim b As StringBuilder = Nothing
        b = New StringBuilder(CInt(buff_size))
        b.Length() = b.Capacity()
        Dim buff_start As UInt32 = 0
        If c_unescape(s, str_start, str_len, b, buff_start, buff_size) Then
            o = b.ToString(0, CInt(buff_start))
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByRef o As String) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_unescape(s, str_start, strlen(s) - str_start, o)
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByRef o As String) As Boolean
        Return c_unescape(s, uint32_0, strlen(s), o)
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal str_len As UInt32) As String
        Dim o As String = Nothing
        assert(c_unescape(s, str_start, str_len, o))
        Return o
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32) As String
        assert(strlen(s) >= str_start)
        Return c_unescape(s, str_start, strlen(s) - str_start)
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String) As String
        Return c_unescape(s, 0, strlen(s))
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal str_len As UInt32,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32,
                                             ByVal buff_len As UInt32) As Boolean
        If str_len = 0 Then
            Return True
        ElseIf strlen(s) < str_start + str_len OrElse
               strlen(buff) < buff_start + buff_len Then
            Return False
        Else
            Dim buff_end As Int32 = 0
            buff_end = buff_start + buff_len
            Dim b() As Byte = Nothing
            ReDim b(4 - 1)
            For i As UInt32 = str_start To str_start + str_len - uint32_1
                Dim c As Char = Nothing
                If s(i) = escape_char Then
                    i += 1
                    'should not end with a \
                    If i = str_start + str_len Then
                        Return False
                    Else
                        Select Case s(i)
                            Case character.a
                                c = character.alert
                            Case character.f
                                c = character.feed
                            Case character.v
                                c = character.vtab
                            Case character.single_quotation
                                c = character.single_quotation
                            Case character.quote
                                c = character.quote
                            Case character.question_mark
                                c = character.question_mark
                            Case escape_char
                                c = escape_char
                            Case character.n
                                c = character.newline
                            Case character.r
                                c = character.return
                            Case character.t
                                c = character.tab
                            Case character.b
                                c = character.backspace
                            Case character.u
                                Const u_size As Int32 = 4
                                assert(s.hex_bytes_len(i + 1, u_size) <= array_size(b))
                                If Not s.hex_bytes(i + 1, u_size, b) OrElse
                                   Not big_endian_bytes_char(b, c) Then
                                    Return False
                                End If
                                i += u_size
                            Case character._U
                                'this is not the same as c++11, since the char in .net is unicode 16 bits
                                Const U_size As Int32 = 8
                                assert(s.hex_bytes_len(i + 1, U_size) <= array_size(b))
                                If Not s.hex_bytes(i + 1, U_size, b) OrElse
                                   Not big_endian_bytes_char(b, c) OrElse
                                   Not big_endian_bytes_char(b, c, (array_size(b) >> 1)) Then
                                    Return False
                                End If
                                i += U_size
                            Case character.x
                                Const x_size As Int32 = 2
                                assert(s.hex_bytes_len(i + 1, x_size) <= array_size(b))
                                If Not s.hex_bytes(i + 1, x_size, b) Then
                                    Return False
                                End If
                                c = Convert.ToChar(b(0))
                                i += x_size
                            Case Else
                                If s(i).digit() Then
                                    'the first digit should not be 0, otherwise the number contains only 1 digit
                                    'it's not the correct solution, but can help to handle \01 issue
                                    'original string is <NULL>1
                                    'escaped string is \01
                                    'unescaped string is still <NULL>1
                                    'since we only escape <NULL> to \0
                                    If s(i) = character._0 Then
                                        c = Convert.ToChar(0)
                                    Else
                                        Dim u As UInt32 = 0
                                        While i < str_start + str_len
                                            If s(i).digit() Then
                                                Dim cur As Byte = 0
                                                'this is not the same as c / c++,
                                                'we accept \65535 to be a char instead of {\65}+{"535"}
                                                cur = Convert.ToInt32(s(i)) - Convert.ToInt32(character._0)
                                                If u * 10 + cur > Convert.ToInt32(Char.MaxValue) Then
                                                    Exit While
                                                Else
                                                    u *= 10
                                                    u += cur
                                                    i += 1
                                                End If
                                            Else
                                                Exit While
                                            End If
                                        End While
                                        c = Convert.ToChar(u)
                                        i -= 1
                                    End If
                                Else
                                    Return False
                                End If
                        End Select
                    End If
                Else
                    c = s(i)
                End If
                'out of buff
                If buff_start = buff_end Then
                    Return False
                End If
                buff(buff_start) = c
                buff_start += 1
            Next
            Return True
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal str_len As UInt32,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32) As Boolean
        If strlen(buff) < buff_start Then
            Return False
        Else
            Return c_unescape(s, str_start, str_len, buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32,
                                             ByVal buff_len As UInt32) As Boolean
        If strlen(s) < str_start Then
            Return False
        Else
            Return c_unescape(s, str_start, strlen(s) - str_start, buff, buff_start, buff_len)
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32) As Boolean
        If strlen(s) < str_start OrElse strlen(buff) < buff_start Then
            Return False
        Else
            Return c_unescape(s, str_start, strlen(s) - str_start, buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal str_len As UInt32,
                                             ByVal buff As StringBuilder) As Boolean
        Return c_unescape(s, str_start, str_len, buff, 0, strlen(buff))
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32,
                                             ByVal buff_len As UInt32) As Boolean
        Return c_unescape(s, 0, strlen(s), buff, buff_start, buff_len)
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal str_start As UInt32,
                                             ByVal buff As StringBuilder) As Boolean
        Return c_unescape(s, str_start, strlen(s), buff, 0, strlen(buff))
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal buff As StringBuilder,
                                             ByRef buff_start As UInt32) As Boolean
        If strlen(buff) < buff_start Then
            Return False
        Else
            Return c_unescape(s, 0, strlen(s), buff, buff_start, strlen(buff) - buff_start)
        End If
    End Function

    <Extension()> Public Function c_unescape(ByVal s As String,
                                             ByVal buff As StringBuilder) As Boolean
        Return c_unescape(s, 0, strlen(s), buff, 0, strlen(buff))
    End Function
End Module
