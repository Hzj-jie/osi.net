
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class characters
        Public Const matcher_separator As Char = character.comma
        Public Const group_separator As Char = character.sheffer
        Public Const group_start As Char = character.left_mid_bracket
        Public Const group_end As Char = character.right_mid_bracket
        Public Const optional_suffix As Char = character.question_mark
        Public Const _0_or_more_suffix As Char = character.asterisk
        Public Const _1_or_more_suffix As Char = character.plus_sign
        Public Const escape_char As Char = character.right_slash

        Public Shared ReadOnly all() As Char = {
            matcher_separator,
            group_separator,
            group_start,
            group_end,
            optional_suffix,
            _0_or_more_suffix,
            _1_or_more_suffix
        }

        Private Sub New()
        End Sub
    End Class
End Class
