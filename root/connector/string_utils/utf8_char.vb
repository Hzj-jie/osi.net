﻿
Imports System.Text
Imports osi.root.constants

Public NotInheritable Class utf8_char
    Private Shared ReadOnly b() As Boolean

    Shared Sub New()
        ReDim b(character.unicode_upper_bound - character.unicode_lower_bound)

        For i As Int32 = character.unicode_lower_bound To character.unicode_upper_bound
            b(i - character.unicode_lower_bound) =
                (Encoding.UTF8().GetCharCount(Encoding.UTF8().GetBytes(Convert.ToChar(i))) = 1) AndAlso
                (Encoding.UTF8().GetChars(Encoding.UTF8().GetBytes(Convert.ToChar(i)))(0) = Convert.ToChar(i))
        Next
    End Sub

    Public Shared Function V(ByVal c As Char) As Boolean
        Return b(Convert.ToInt32(c))
    End Function

    Private Sub New()
    End Sub
End Class
