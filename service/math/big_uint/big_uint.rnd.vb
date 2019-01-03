
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class big_uint
    Public Shared Function rnd_support_str(ByVal length As UInt32,
                                           Optional ByVal base As Byte = default_str_base) As String
        assert_support_base(base)
        If length = 0 Then
            Return Nothing
        Else
            Dim r As StringBuilder = Nothing
            r = New StringBuilder(CInt(length))
            For i As UInt32 = 0 To length - uint32_1
                r.Append(number_to_char(rnd_int(If(i = 0, 1, 0), base)))
            Next
            Return Convert.ToString(r)
        End If
    End Function

    Public Shared Function rnd_support_base() As Byte
        Dim r As Byte = 0
        r = rnd_int(2, big_uint.support_str_base + 1)
        assert_support_base(r)
        Return r
    End Function

    Public Shared Function rnd_unsupport_str_char(Optional ByVal base As Byte = default_str_base) As Char
        Dim c As Char = Nothing
        Do
            c = rnd_ascii_display_char()
        Loop While support_str_char(c, base)
        Return c
    End Function

    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_uint
        If length = 0 Then
            length = rnd_uint(100, 1000)
        End If
        Return New big_uint(rnd_bytes(length))
    End Function
End Class
