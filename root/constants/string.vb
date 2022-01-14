
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text

Public Module _string
    ' TODO: Remove
    Public Const empty_string As String = ""
    Public Const newline_chars As String = character.newline +
                                           character.return
    Public ReadOnly space_chars As String = calculate_space_chars()
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

    Public ReadOnly stringbuilder_default_capacity As UInt32 = CUInt((New StringBuilder()).Capacity())

    Public Const npos As Int32 = -1

    Private Function calculate_space_chars() As String
        Dim space_chars As String = Nothing
        For i As Int32 = Convert.ToInt32(Char.MinValue) To Convert.ToInt32(Char.MaxValue)
            If Char.IsWhiteSpace(Convert.ToChar(i)) Then
                space_chars += Convert.ToChar(i)
            End If
        Next
        Return space_chars
    End Function
End Module

Public NotInheritable Class newline
    Private Shared ReadOnly _incode As String = Console.Out().NewLine()

    Public Shared Function incode() As String
        Return _incode
    End Function

    Private Sub New()
    End Sub
End Class
