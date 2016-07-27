
Imports System.Text

Public Module _string
    Public Const empty_string As String = ""
    Public Const newline_chars As String = character.newline +
                                           character.return
    Public ReadOnly space_chars As String
    Public Const left_brackets As String = "（｛［《＜〔〈【({[<"
    Public Const right_brackets As String = "）｝］》＞〕〉】)}]>"
    Public Const brackets As String = left_brackets + right_brackets
    Public Const marks As String = "`~!@#$%^&*-_=+\|;':"",./?" +
                                   "｀～！·＃￥％……—＊、｜；‘’：""，。／？" +
                                   brackets
    Public Const dbc_digits As String = "0123456789"
    Public Const sbc_digits As String = "０１２３４５６７８９"
    Public Const digits As String = dbc_digits + sbc_digits
    Public Const hexdigits As String = digits + "abcdefABCDEFａｂｃｄｅｆＡＢＣＤＥＦ"
    Public Const upper_dbc_hex_digits As String = dbc_digits + "ABCDEF"
    Public Const hex_digits_char_count As Int32 = 16
    Public Const lower_english_characters As String = "abcdefghijklmnopqrstuvwxyz"
    Public Const upper_english_characters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Public Const english_characters As String = lower_english_characters + upper_english_characters

    Public ReadOnly stringbuilder_default_capacity As UInt32

    Public Const npos As Int32 = -1

    Sub New()
        For i As Int32 = Convert.ToInt32(Char.MinValue) To Convert.ToInt32(Char.MaxValue)
            If Char.IsWhiteSpace(Convert.ToChar(i)) Then
                space_chars += Convert.ToChar(i)
            End If
        Next

        stringbuilder_default_capacity = (New StringBuilder()).Capacity()
    End Sub
End Module

Namespace newline
    Public Module _newline
        Private ReadOnly _incode As String = Console.Out().NewLine()

        Public Function incode() As String
            Return _incode
        End Function
    End Module
End Namespace
