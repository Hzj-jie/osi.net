
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class big_int
    Public Shared Function rnd_support_str(ByVal length As UInt32,
                                           Optional ByVal base As Byte = default_str_base) As String
        If length <= 1 OrElse rnd_bool() Then
            Return big_uint.rnd_support_str(length, base)
        Else
            Return negative_signal_mask + big_uint.rnd_support_str(length - uint32_1, base)
        End If
    End Function

    Public Shared Function rnd_support_base() As Byte
        Return big_uint.rnd_support_base()
    End Function

    Public Shared Function rnd_unsupport_str_char(Optional ByVal base As Byte = default_str_base) As Char
        Return big_uint.rnd_unsupport_str_char(base)
    End Function

    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_int
        Dim r As big_int = Nothing
        r = share(big_uint.random(length))
        If rnd_bool() Then
            r.set_negative()
        End If
        Return r
    End Function
End Class
