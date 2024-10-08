
Imports Microsoft.VisualBasic

Public NotInheritable Class character
    Public Const ascii_lower_bound As Int32 = 0
    Public Const ascii_upper_bound As Int32 = max_int8
    Public Const ascii_extended_upper_bound As Int32 = max_uint8
    Public Shared ReadOnly unicode_lower_bound As Int32 = Convert.ToInt32(Char.MinValue)
    Public Shared ReadOnly unicode_upper_bound As Int32 = Convert.ToInt32(Char.MaxValue)

    Public Const null As Char = Chr(0)
    Public Const eot As Char = Chr(4)
    Public Const alert As Char = Chr(7)
    Public Const bell As Char = alert
    Public Const backspace As Char = Chr(8)
    Public Const tab As Char = Chr(9)
    Public Const newline As Char = Chr(10)
    Public Const vtab As Char = Chr(11)
    Public Const feed As Char = Chr(12)
    Public Const [return] As Char = Chr(13)
    Public Const _return As Char = [return]
    Public Const enter As Char = [return]
    Public Const [sub] As Char = Chr(26)
    Public Const esc As Char = Chr(27)
    Public Const semicolon As Char = ";"c
    Public Const left_mid_bracket As Char = "["c
    Public Const right_mid_bracket As Char = "]"c
    Public Const left_angle_bracket As Char = "<"c
    Public Const right_angle_bracket As Char = ">"c
    Public Const equal_sign As Char = "="c
    Public Const blank As Char = " "c
    Public Const sbcblank As Char = "　"c
    Public Const right_slash As Char = "\"c
    Public Const left_slash As Char = "/"c
    Public Const division_sign As Char = left_slash
    Public Const tilde As Char = "~"c
    Public Const a As Char = "a"c
    Public Const _A As Char = "A"c
    Public Const b As Char = "b"c
    Public Const _B As Char = "B"c
    Public Const _C As Char = "C"c
    Public Const d As Char = "d"c
    Public Const _D As Char = "D"c
    Public Const e As Char = "e"c
    Public Const _E As Char = "E"c
    Public Const f As Char = "f"c
    Public Const _F As Char = "F"c
    Public Const h As Char = "h"c
    Public Const _H As Char = "H"c
    Public Const i As Char = "i"c
    Public Const m As Char = "m"c
    Public Const n As Char = "n"c
    Public Const _N As Char = "N"c
    Public Const p As Char = "p"c
    Public Const r As Char = "r"c
    Public Const s As Char = "s"c
    Public Const t As Char = "t"c
    Public Const _T As Char = "T"c
    Public Const u As Char = "u"c
    Public Const _U As Char = "U"c
    Public Const v As Char = "v"c
    Public Const x As Char = "x"c
    Public Const z As Char = "z"c
    Public Const _Z As Char = "Z"c
    Public Const zero As Char = "0"c
    Public Const _0 As Char = zero
    Public Const sbc0 As Char = "０"c
    Public Const one As Char = "1"c
    Public Const _1 As Char = one
    Public Const nine As Char = "9"c
    Public Const _9 As Char = nine
    Public Const comma As Char = ","c
    Public Const minus_sign As Char = "-"c
    Public Const colon As Char = ":"c
    Public Const asterisk As Char = "*"c
    Public Const multiplication_sign As Char = asterisk
    Public Const question_mark As Char = "?"c
    Public Const sheffer As Char = "|"c
    Public Const single_quotation As Char = "'"c
    Public Const plus_sign As Char = "+"c
    Public Const left_bracket As Char = "("c
    Public Const right_bracket As Char = ")"c
    Public Const full_stop As Char = "."c
    Public Const dot As Char = full_stop
    Public Const decimal_point As Char = dot
    Public Const left_brace As Char = "{"c
    Public Const right_brace As Char = "}"c
    Public Const number_mark As Char = "#"c
    Public Const hash_mark As Char = number_mark
    Public Const at As Char = "@"c
    Public Const underline As Char = "_"c
    Public Const quote As Char = """"c
    Public Const quote_mark As Char = quote
    Public Const dollar As Char = "$"c
    Public Const and_mark As Char = "&"c
    Public Const ampersand As Char = and_mark
    Public Const exclamation_mark As Char = "!"c
    Public Const percent_sign As Char = "%"c
    Public Const percent_mark As Char = percent_sign
    Public Const caret As Char = "^"c
    Public Const involution_sign As Char = caret
    Public Const involution_mark As Char = involution_sign
    Public Const backquote As Char = "`"c

    Private Sub New()
    End Sub
End Class
