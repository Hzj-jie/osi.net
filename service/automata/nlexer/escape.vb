
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Private NotInheritable Class escapes
        Public Shared ReadOnly v As vector(Of pair(Of String, String))

        Private Shared Function escape_of(ByVal c As Char) As pair(Of String, String)
            Return pair.emplace_of(characters.escape_char + c, characters.escape_char + c.c_hex_escape())
        End Function

        Shared Sub New()
            v = New vector(Of pair(Of String, String))()
            For Each c As Char In characters.all
                v.emplace_back(escape_of(c))
            Next
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function escape(ByVal s As String) As String
        assert(s IsNot Nothing)
        If s.null_or_whitespace() Then
            Return s
        End If
        Dim i As UInt32 = 0
        While i < escapes.v.size()
            s = s.Replace(escapes.v(i).first, escapes.v(i).second)
            i += uint32_1
        End While
        Return s
    End Function

    Public Shared Function unescape(ByVal s As String) As String
        assert(s IsNot Nothing)
        If s.null_or_whitespace() Then
            Return s
        End If
        Dim i As UInt32 = 0
        While i < escapes.v.size()
            s = s.Replace(escapes.v(i).second, strmid(escapes.v(i).first, strlen(characters.escape_char)))
            i += uint32_1
        End While
        Return s
    End Function
End Class
