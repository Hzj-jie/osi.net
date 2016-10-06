
Option Strict On
Imports osi.root.constants

Public NotInheritable Class endian
    Public Shared Function reverse(ByVal i As UInt16) As UInt16
        Return ((i And max_uint8) << bit_count_in_byte) + (i >> bit_count_in_byte)
    End Function

    Private Sub New()
    End Sub
End Class
