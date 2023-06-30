
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class rlexer
    Public Class characters
        Public Const matching_group_start As Char = character.left_mid_bracket
        Public Const matching_group_end As Char = character.right_mid_bracket
        Public Const string_matches_separator As Char = character.comma
        Public Const any_matching As Char = character.asterisk
        Public Const multi_matching As Char = character.plus_sign
        Public Const optional_matching As Char = character.question_mark
        Public Const reverse_matching As Char = character.exclamation_mark
        Public Const unmatched_matching As Char = character.minus_sign
        Public Const macro_escape As Char = character.right_slash
    End Class
End Class
