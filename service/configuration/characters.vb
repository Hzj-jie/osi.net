
Imports osi.root.constants
Imports osi.root.connector

Public Class characters
    'default is
    '; a comment line
    'ev:v1&e2:v2$some_filter=filter_type
    '[e1:v1&e2:v2$section@d1:v3&d2:v4]
    'e1:v1&e2:v2$key@d1:v3&d2:v4=value
    Public ReadOnly remark As String = character.semicolon
    Public ReadOnly section_left As String = character.left_mid_bracket
    Public ReadOnly section_right As String = character.right_mid_bracket
    Public ReadOnly key_value_separator As String = character.equal_sign
    Public ReadOnly static_filter_mark As String = character.dollar
    Public ReadOnly dynamic_filter_mark As String = character.at
    Public ReadOnly filter_separator As String = character.and_mark
    Public ReadOnly filter_key_value_separator As String = character.colon

    Private Sub New()
    End Sub

    Public Sub New(ByVal remark As String,
                   ByVal section_left As String,
                   ByVal section_right As String,
                   ByVal key_value_separator As String,
                   ByVal static_filter_mark As String,
                   ByVal dynamic_filter_mark As String,
                   ByVal filter_separator As String,
                   ByVal filter_key_value_separator As String)
        Me.remark = remark
        Me.section_left = section_left
        Me.section_right = section_right
        Me.key_value_separator = key_value_separator
        Me.static_filter_mark = static_filter_mark
        Me.dynamic_filter_mark = dynamic_filter_mark
        Me.filter_separator = filter_separator
        Me.filter_key_value_separator = filter_key_value_separator
    End Sub

    Public Function preserved_str(ByVal s As String) As Boolean
        Return strsame(s, remark) OrElse
               strsame(s, section_left) OrElse
               strsame(s, section_right) OrElse
               strsame(s, key_value_separator) OrElse
               strsame(s, static_filter_mark) OrElse
               strsame(s, dynamic_filter_mark) OrElse
               strsame(s, filter_separator) OrElse
               strsame(s, filter_key_value_separator)
    End Function

    Public Shared ReadOnly [default] As characters

    Shared Sub New()
        [default] = New characters()
    End Sub
End Class
