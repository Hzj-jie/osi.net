
Imports osi.root.constants

' Seems the issue has been resolved

#If 0 Then
' Some invalid characters make troubles to .net String.Contains and String.IndexOf functions. So this class detects
' these characters to help build 'safe strings' for tests.
Public NotInheritable Class safe_char
    Private Shared ReadOnly b() As Boolean

    Private Shared Function test_char(ByVal c As Char) As Boolean
        If c = "H"c Then
            Return True
        End If

        Dim s As String = Nothing
        s = "HH" + c
        If Not s.Contains(c) Then
            Return False
        End If

        If s.IndexOf(c) <> 2 Then
            Return False
        End If

        If Not s.EndsWith(c) Then
            Return False
        End If

        If String.Compare(s, "HH" + c) <> 0 Then
            Return False
        End If

        If s.CompareTo("HH" + c) <> 0 Then
            Return False
        End If

        s = c + "HH"
        If Not s.Contains(c) Then
            Return False
        End If

        If s.IndexOf(c) <> 0 Then
            Return False
        End If

        If Not s.StartsWith(c) Then
            Return False
        End If

        If String.Compare(s, c + "HH") <> 0 Then
            Return False
        End If

        If s.CompareTo(c + "HH") <> 0 Then
            Return False
        End If

        s = "H" + c + "H"
        If Not s.Contains(c) Then
            Return False
        End If

        If s.IndexOf(c) <> 1 Then
            Return False
        End If

        If String.Compare(s, "H" + c + "H") <> 0 Then
            Return False
        End If

        If s.CompareTo("H" + c + "H") <> 0 Then
            Return False
        End If

        Return True
    End Function

    Shared Sub New()
        ReDim b(character.unicode_upper_bound - character.unicode_lower_bound)

        For i As Int32 = character.unicode_lower_bound To character.unicode_upper_bound
            b(i - character.unicode_lower_bound) = test_char(Convert.ToChar(i))
        Next
    End Sub

    Public Shared Function V(ByVal c As Char) As Boolean
        Return b(Convert.ToInt32(c))
    End Function

    Private Sub New()
    End Sub
End Class
#End If
