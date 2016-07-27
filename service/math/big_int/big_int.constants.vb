
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class big_int
    Private Const default_str_base As Byte = 10
    Private Const positive_signal_mask As Char = character.plus_sign
    Private Const negative_signal_mask As Char = character.minus_sign

    Shared Sub New()
        assert(strlen(positive_signal_mask) = 1)
        assert(strlen(negative_signal_mask) = 1)
    End Sub
End Class
