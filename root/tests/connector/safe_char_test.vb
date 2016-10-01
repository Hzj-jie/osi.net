
Imports osi.root.connector
Imports osi.root.utt

#If 0 Then
Public Class safe_char_test
    Inherits [case]

    Private Shared ReadOnly known_unsafe_chars() As Char

    Shared Sub New()
        known_unsafe_chars = {}
    End Sub

    Public Overrides Function run() As Boolean
        If isemptyarray(known_unsafe_chars) Then
            Return True
        End If

        For i As Int32 = 0 To array_size(known_unsafe_chars) - 1
            assert_false(safe_char.V(known_unsafe_chars(i)))
        Next
        Return True
    End Function
End Class
#End If
