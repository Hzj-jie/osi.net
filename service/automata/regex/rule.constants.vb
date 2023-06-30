
Imports osi.root.constants

Partial Class rlexer
    Partial Public Class rule
        Public Const define_separator As Char = character.blank
        Public Const mode_separator As Char = character.blank

        Public Const command_mode As String = "MODE"
        Public Const command_clear_define As String = "CLEAR_DEFINE"
        Public Const command_define As String = "DEFINE"
        Public Const command_clear_word As String = "CLEAR_WORD"

        Public Const mode_type_choice As String = "type_choice"
        Public Const mode_word_choice As String = "word_choice"
    End Class
End Class
