
Imports System.Web
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.xml

Friend Module _generator
    Private Class rnd
        Private Shared ReadOnly tag_excepts() As Char
        Private Shared ReadOnly value_excepts() As Char

        Shared Sub New()
            value_excepts = {constants.value_leading,
                             constants.value_final,
                             constants.value_leading_2,
                             constants.value_final_2,
                             constants.tag_close_mark}
            tag_excepts = space_chars.c_str().append(value_excepts).append(constants.attribute_separator)
        End Sub

        Public Shared Function tag() As String
            Return strcat(rndenchar(),
                          rnd_ascii_display_chars(rnd_int(4, 10), tag_excepts))
        End Function

        Public Shared Function key() As String
            Return tag()
        End Function

        Public Shared Function value() As String
            Return strcat(rndenchar(),
                          rnd_ascii_display_chars(rnd_int(4, 10), value_excepts))
        End Function

        Public Shared Function text() As String
            Return rnd_ascii_display_chars(rnd_int(10, 50))
        End Function
    End Class

    Public Function rnd_start_tag(ByRef tag As String,
                                  ByRef attrs As vector(Of pair(Of String, String)),
                                  ByRef self_close As Boolean) As String
        tag = rnd.tag()
        attrs.renew()
        self_close = rnd_bool()
        For i As Int32 = 0 To rnd_int(0, 5) - 1
            attrs.emplace_back(emplace_make_pair(rnd.key(), rnd.value()))
        Next
        Dim r As String = Nothing
        r = create_start_tag(tag, self_close, attrs)
        tag = HttpUtility.HtmlEncode(tag)
        For i As Int32 = 0 To attrs.size() - 1
            attrs(i).first = HttpUtility.HtmlEncode(attrs(i).first)
            attrs(i).second = HttpUtility.HtmlEncode(attrs(i).second)
        Next
        Return r
    End Function

    Public Function rnd_start_tag(ByRef self_close As Boolean) As String
        Return rnd_start_tag(Nothing, Nothing, self_close)
    End Function

    Public Function rnd_start_tag() As String
        Return rnd_start_tag(Nothing, Nothing, Nothing)
    End Function

    Public Function rnd_end_tag(ByRef tag As String) As String
        tag = rnd.tag()
        Dim r As String = Nothing
        r = create_end_tag(tag)
        tag = HttpUtility.HtmlEncode(tag)
        Return r
    End Function

    Public Function rnd_end_tag() As String
        Return rnd_end_tag(Nothing)
    End Function

    Public Function rnd_loosen_comment() As String
        Return create_loosen_comment(rnd.text())
    End Function

    Public Function rnd_text() As String
        Return create_text(rnd.text())
    End Function

    Public Function rnd_loosen_cdata() As String
        Return create_loosen_cdata(rnd.text())
    End Function
End Module
