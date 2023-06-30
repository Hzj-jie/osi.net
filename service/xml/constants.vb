
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Namespace constants
    Public Module _constants
        Public Const tag_leading As Char = character.left_angle_bracket
        Public Const tag_leading_str As String = tag_leading
        Public ReadOnly tag_leading_len As UInt32 = strlen(tag_leading_str)
        Public Const tag_final As Char = character.right_angle_bracket
        Public Const tag_final_str As String = tag_final
        Public ReadOnly tag_final_len As UInt32 = strlen(tag_final_str)
        Public Const tag_close_mark As Char = character.left_slash
        Public Const tag_close_mark_str As String = tag_close_mark
        Public ReadOnly tag_close_mark_len As UInt32 = strlen(tag_close_mark_str)
        Public Const attributes_separator As Char = character.blank
        Public Const attributes_separator_str As String = attributes_separator
        Public ReadOnly attributes_separator_len As UInt32 = strlen(attributes_separator_str)
        Public Const attribute_separator As Char = character.equal_sign
        Public Const attribute_separator_str As String = attribute_separator
        Public ReadOnly attribute_separator_len As UInt32 = strlen(attribute_separator_str)
        Public Const value_leading As Char = character.quote_mark
        Public Const value_leading_str As String = value_leading
        Public ReadOnly value_leading_len As UInt32 = strlen(value_leading_str)
        Public Const value_leading_2 As Char = character.single_quotation
        Public Const value_leading_2_str As String = value_leading_2
        Public ReadOnly value_leading_2_len As UInt32 = strlen(value_leading_2_str)
        Public Const value_final As Char = character.quote_mark
        Public Const value_final_str As String = value_final
        Public ReadOnly value_final_len As UInt32 = strlen(value_final_str)
        Public Const value_final_2 As Char = character.single_quotation
        Public Const value_final_2_str As String = value_final_2
        Public ReadOnly value_final_2_len As UInt32 = strlen(value_final_2_str)
        Public Const declaration_leading As String = tag_leading + character.question_mark
        Public ReadOnly declaration_leading_len As UInt32 = strlen(declaration_leading)
        Public Const declaration_final As String = character.question_mark + tag_final
        Public ReadOnly declaration_final_len As UInt32 = strlen(declaration_final)
        Public Const declaration_leading_2 As String = tag_leading + character.exclamation_mark
        Public ReadOnly declaration_leading_2_len As UInt32 = strlen(declaration_leading_2)
        Public Const declaration_final_2 As Char = tag_final
        Public Const declaration_final_2_str As String = declaration_final_2
        Public ReadOnly declaration_final_2_len As UInt32 = strlen(declaration_final_2_str)
        Public Const invalid_comment_text As String = character.minus_sign + character.minus_sign
        Public ReadOnly invalid_comment_text_len As UInt32 = strlen(invalid_comment_text)
        Public Const invalid_comment_text_replacement As String = character.minus_sign +
                                                                  character.blank +
                                                                  character.minus_sign +
                                                                  character.blank
        Public ReadOnly invalid_comment_text_replacement_len As UInt32 = strlen(invalid_comment_text_replacement)
        Public Const comment_leading As String = tag_leading +
                                                 character.exclamation_mark +
                                                 invalid_comment_text
        Public ReadOnly comment_leading_len As UInt32 = strlen(comment_leading)
        Public Const comment_final As String = invalid_comment_text +
                                               tag_final
        Public ReadOnly comment_final_len As UInt32 = strlen(comment_final)
        Public Const invalid_cdata_text As String = cdata_final
        Public ReadOnly invalid_cdata_text_len As UInt32 = strlen(invalid_cdata_text)
        Public Const invalid_cdata_text_replacement As String = character.right_mid_bracket +
                                                                character.blank +
                                                                character.right_mid_bracket +
                                                                tag_final
        Public ReadOnly invalid_cdata_text_replacement_len As UInt32 = strlen(invalid_cdata_text_replacement)
        Public Const cdata_leading As String = tag_leading +
                                               character.exclamation_mark +
                                               character.left_mid_bracket +
                                               "CDATA" +
                                               character.left_mid_bracket
        Public ReadOnly cdata_leading_len As UInt32 = strlen(cdata_leading)
        Public Const cdata_final As String = character.right_mid_bracket +
                                             character.right_mid_bracket +
                                             tag_final
        Public ReadOnly cdata_final_len As UInt32 = strlen(cdata_final)

        Sub New()
            assert(Not strsame(invalid_comment_text, invalid_comment_text_replacement))
            assert(Not strsame(invalid_cdata_text, invalid_cdata_text_replacement))
        End Sub
    End Module
End Namespace
