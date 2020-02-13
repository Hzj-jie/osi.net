
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class upure_dec
    Private Const default_str_base As Byte = 10

    Public Function str(Optional ByVal base_byte As Byte = default_str_base) As String
        Return strcat(character.zero, str_without_leading_zero(base_byte))
    End Function

    Public Function str_without_leading_zero(Optional ByVal base_byte As Byte = default_str_base) As String
        Dim base As big_uint = Nothing
        base = New big_uint(base_byte)
        Dim l As UInt32 = 0
        Dim n As big_uint = Nothing
        n = New big_uint(Me.n)
        While n.uint32_size() <= (d.uint32_size() << 1)
            l += uint32_1
            n.multiply(base)
        End While
        n.divide(d)
        Dim r As String = Nothing
        r = n.str(base_byte)
        assert(l >= strlen(r))
        Return strcat(strncat(character.dot, character.zero, l - strlen(r)), r).TrimEnd(character.zero)
    End Function
End Class
