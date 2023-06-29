
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

Public NotInheritable Class utf8_char
    Private Shared ReadOnly b() As Boolean =
        Function() As Boolean()
            Dim b(character.unicode_upper_bound - character.unicode_lower_bound) As Boolean
            For i As Int32 = character.unicode_lower_bound To character.unicode_upper_bound
                b(i - character.unicode_lower_bound) =
                    (Encoding.UTF8().GetCharCount(Encoding.UTF8().GetBytes(Convert.ToChar(i))) = 1) AndAlso
                    (Encoding.UTF8().GetChars(Encoding.UTF8().GetBytes(Convert.ToChar(i)))(0) = Convert.ToChar(i))
            Next
            Return b
        End Function()

    Public Shared Function v(ByVal c As Char) As Boolean
        Return b(Convert.ToInt32(c))
    End Function

    Private Sub New()
    End Sub
End Class
