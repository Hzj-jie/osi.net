
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Shared Function rnd_support_str(ByVal length As UInt32,
                                           Optional ByVal base As Byte = default_str_base) As String
        assert_support_base(base)
        If length = 0 Then
            Return Nothing
        End If
        Dim r As StringBuilder = Nothing
        r = New StringBuilder(CInt(length))
        For i As UInt32 = 0 To length - uint32_1
            r.Append(number_to_char(assert_which.of(rnd_int(If(i = 0, byte_1, byte_0), base)).can_cast_to_byte()))
        Next
        Return Convert.ToString(r)
    End Function

    Public Shared Function rnd_support_base() As Byte
        Dim r As Byte = 0
        r = assert_which.of(rnd_int(2, big_uint.support_str_base + 1)).can_cast_to_byte()
        assert_support_base(r)
        Return r
    End Function

    Public Shared Function rnd_unsupport_str_char(Optional ByVal base As Byte = default_str_base) As Char
        Dim c As Char = Nothing
        Do
            c = rnd_ascii_display_char()
        Loop While support_str_char(c, base) OrElse space_chars.Contains(c)
        Return c
    End Function

    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_uint
        If length = 0 Then
            length = rnd_uint(100, 1000)
        End If
        Return New big_uint(rnd_bytes(length))
    End Function
End Class
