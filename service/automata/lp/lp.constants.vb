
Imports osi.root.template
Imports osi.root.constants

Partial Public Class lp(Of MAX_TYPE As _int64, RESULT_T)
    'for initialize
    Private Const separator_type_name As String = "separator"
    Private Const unknown_type_name As String = "unknown"
    Private Const end_type_name As String = "end"
    Private Const start_status_name As String = "start"
    Private Const end_status_name As String = "end"

    'for input
    Private Const value_start As String = character.tab
    Private Const value_start2 As String = character.blank
    Private Const comment_start As String = character.number_mark
    Private Const type_ignore_mask As String = character.minus_sign

    Private Const use_default_separators_mask As String = "use"
    Private Const ignore_separators_mask As String = "ignore"

    Private Shared ReadOnly default_separators_section_separators() As Char = {character.blank,
                                                                               character.tab}
    Private Shared ReadOnly section_name_separators() As Char = {character.blank,
                                                                 character.tab,
                                                                 character.minus_sign}
    Private Shared ReadOnly value_separators() As Char = {character.tab}

    Private Const method_type_section_name As String = "method_type"
    Private Const key_words_section_name As String = "key_words"
    Private Const transitions_section_name As String = "transitions"
    Private Const default_separators_section_name As String = "default_separators"
End Class
