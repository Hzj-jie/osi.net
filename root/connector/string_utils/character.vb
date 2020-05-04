
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _character
    <Extension()> Public Function lowalpha(ByVal c As Char) As Boolean
        Return (c >= character.a AndAlso c <= character.z)
    End Function

    <Extension()> Public Function hialpha(ByVal c As Char) As Boolean
        Return (c >= character._A AndAlso c <= character._Z)
    End Function

    <Extension()> Public Function alpha(ByVal c As Char) As Boolean
        Return lowalpha(c) OrElse hialpha(c)
    End Function

    <Extension()> Public Function digit(ByVal c As Char) As Boolean
        Return c >= character._0 AndAlso c <= character._9
    End Function

    <Extension()> Public Function hex(ByVal c As Char) As Boolean
        Return digit(c) OrElse
               (c >= character.a AndAlso c <= character.f) OrElse
               (c >= character._A AndAlso c <= character._F)
    End Function

    <Extension()> Public Function hex_value(ByVal c As Char, ByRef v As Byte) As Boolean
        If Not hex(c) Then
            Return False
        End If
        v = CByte(If(digit(c),
                     Convert.ToInt32(c) - Convert.ToInt32(character._0),
                     If(lowalpha(c),
                        Convert.ToInt32(c) - Convert.ToInt32(character.a),
                        Convert.ToInt32(c) - Convert.ToInt32(character._A)) + 10))
        Return True
    End Function

    <Extension()> Public Function hex_value(ByVal c As Char) As Byte
        Dim v As Byte = 0
        assert(hex_value(c, v))
        Return v
    End Function

    <Extension()> Public Function utf8_supported(ByVal c As Char) As Boolean
        Return utf8_char.V(c)
    End Function

#If 0 Then
    <Extension()> Public Function safe_char_supported(ByVal c As Char) As Boolean
        Return safe_char.V(c)
    End Function
#End If

    <Extension()> Public Function space(ByVal c As Char) As Boolean
        Return Char.IsWhiteSpace(c)
    End Function

    <Extension()> Public Function visible(ByVal c As Char) As Boolean
        Return Not Char.IsControl(c)
    End Function

    <Extension()> Public Function _7_bit(ByVal c As Char) As Boolean
        Return c.ascii()
    End Function

    <Extension()> Public Function ascii(ByVal c As Char) As Boolean
        Return extended_ascii(c) AndAlso Convert.ToByte(c) <= character.ascii_upper_bound
    End Function

    <Extension()> Public Function _8_bit(ByVal c As Char) As Boolean
        Return c.extended_ascii()
    End Function

    <Extension()> Public Function extended_ascii(ByVal c As Char) As Boolean
        Return c.big_endian_bytes()(0) = 0
    End Function

    <Extension()> Public Function cjk(ByVal c As Char) As Boolean
        Dim v As UInt16 = 0
        v = char_uint16(c)
        Return v >= &H4E00 AndAlso v <= &H9FFF
    End Function

    Public Function uint16_char(ByVal i As UInt16) As Char
        Return Convert.ToChar(i)
    End Function

    Public Function char_uint16(ByVal i As Char) As UInt16
        Return Convert.ToUInt16(i)
    End Function
End Module
