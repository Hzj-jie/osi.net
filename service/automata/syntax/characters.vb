
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class characters
        Public Const matching_group_start As Char = character.left_mid_bracket
        Public Const matching_group_end As Char = character.right_mid_bracket
        Public Const optional_matching As Char = character.question_mark
        Public Const any_matching As Char = character.asterisk
        Public Const multi_matching As Char = character.plus_sign
        Public Const matching_group_separator As Char = character.comma
        Public Shared ReadOnly matching_group_separators() As String = {Convert.ToString(matching_group_separator)}
        Public Shared ReadOnly matching_separators As String = space_chars
        Public Shared ReadOnly matching_separators_array() As Char = matching_separators.ToCharArray()
        Public Shared ReadOnly reserved_characters() As Char = array_concat({matching_group_start,
                                                                             matching_group_end,
                                                                             optional_matching,
                                                                             any_matching,
                                                                             multi_matching,
                                                                             matching_group_separator},
                                                                            matching_separators_array)

        Public Shared Function valid_type_str(ByVal s As String) As Boolean
            Return Not s.null_or_whitespace() AndAlso
                   Not s.contains_any(reserved_characters)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
