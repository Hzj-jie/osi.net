
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Private Shared ReadOnly escapes() As pair(Of String, String) = {
        make_escape(characters.matcher_separator),
        make_escape(characters.group_separator),
        make_escape(characters.group_start),
        make_escape(characters.group_end),
        make_escape(characters.optional_suffix),
        make_escape(characters._0_or_more_suffix),
        make_escape(characters._1_or_more_suffix),
        make_escape(character.right_slash)
    }

    Private Shared Function make_escape(ByVal c As Char) As pair(Of String, String)
        Return pair.emplace_of(character.right_slash + c, character.right_slash + "x" + Convert.ToByte(c).hex())
    End Function

    Public Shared Function escape(ByVal s As String) As String
        assert(Not s Is Nothing)
        If s.null_or_whitespace() Then
            Return s
        End If
        For i As Int32 = 0 To array_size_i(escapes) - 1
            s = s.Replace(escapes(i).first, escapes(i).second)
        Next
        Return s
    End Function

    Public Shared Function unescape(ByVal s As String) As String
        assert(Not s Is Nothing)
        If s.null_or_whitespace() Then
            Return s
        End If
        For i As Int32 = 0 To array_size_i(escapes) - 1
            s = s.Replace(escapes(i).second, escapes(i).first)
        Next
        Return s
    End Function
End Class
