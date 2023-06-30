
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_udec
        If length = 0 Then
            length = rnd_uint(100, 1000)
        End If
        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        n = big_uint.random(CUInt(length * rnd_double(0.9, 1.5)))
        Do
            d = big_uint.random(length)
        Loop While d.is_zero()
        Return New big_udec(n, d)
    End Function

    Public Shared Function rnd_support_str(ByVal length As UInt32,
                                           Optional ByVal base As Byte = constants.str_base) As String
        Dim s As String = Nothing
        s = big_uint.rnd_support_str(length, base)
        Return s.strrplc(rnd_uint(0, length), character.dot)
    End Function

    Public Shared Function rnd_support_base() As Byte
        Return big_uint.rnd_support_base()
    End Function

    Public Shared Function rnd_unsupport_str_char(Optional ByVal base As Byte = constants.str_base) As Char
        Return big_uint.rnd_unsupport_str_char(base)
    End Function
End Class
