
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

'http://tools.ietf.org/html/rfc1738
Namespace uri
    Public Module _uri
        <Extension()> Public Function lowalpha(ByVal c As Char) As Boolean
            Return _character.lowalpha(c)
        End Function

        <Extension()> Public Function hialpha(ByVal c As Char) As Boolean
            Return _character.hialpha(c)
        End Function

        <Extension()> Public Function alpha(ByVal c As Char) As Boolean
            Return _character.alpha(c)
        End Function

        <Extension()> Public Function digit(ByVal c As Char) As Boolean
            Return _character.digit(c)
        End Function

        Private ReadOnly safes() As Char = {character.dollar,
                                            character.minus_sign,
                                            character.underline,
                                            character.dot,
                                            character.plus_sign}

        <Extension()> Public Function safe(ByVal c As Char) As Boolean
            Return safes.has(c)
        End Function

        Private ReadOnly extras() As Char = {character.exclamation_mark,
                                             character.asterisk,
                                             character.single_quotation,
                                             character.left_bracket,
                                             character.right_bracket,
                                             character.comma}

        <Extension()> Public Function extra(ByVal c As Char) As Boolean
            Return extras.has(c)
        End Function

        Private ReadOnly nationals() As Char = {character.left_brace,
                                                character.right_brace,
                                                character.sheffer,
                                                character.right_slash,
                                                character.caret,
                                                character.tilde,
                                                character.left_mid_bracket,
                                                character.right_mid_bracket,
                                                character.backquote}

        <Extension()> Public Function national(ByVal c As Char) As Boolean
            Return nationals.has(c)
        End Function

        Private ReadOnly punctuations() As Char = {character.left_angle_bracket,
                                                   character.right_angle_bracket,
                                                   character.hash_mark,
                                                   character.percent_mark,
                                                   character.quote_mark}

        <Extension()> Public Function punctuation(ByVal c As Char) As Boolean
            Return punctuations.has(c)
        End Function

        Private ReadOnly reserveds() As Char = {character.semicolon,
                                                character.left_slash,
                                                character.question_mark,
                                                character.colon,
                                                character.at,
                                                character.ampersand,
                                                character.equal_sign}

        <Extension()> Public Function reserved(ByVal c As Char) As Boolean
            Return reserveds.has(c)
        End Function

        <Extension()> Public Function hex(ByVal c As Char) As Boolean
            Return _character.hex(c)
        End Function

        <Extension()> Public Function unreserved(ByVal c As Char) As Boolean
            Return alpha(c) OrElse
                   digit(c) OrElse
                   safe(c) OrElse
                   extra(c)
        End Function
    End Module
End Namespace

Public Module _uri
    'a minimum set of characters for different uri protocols,
    'it's safe, but we may need to extra encode several characters
    <Extension()> Public Function uri_path_encode(ByVal s As String, Optional ByVal e As Encoding = Nothing) As String
        If String.IsNullOrEmpty(s) Then
            Return empty_string
        Else
            If e Is Nothing Then
                e = default_encoding
            End If
            Dim o As StringBuilder = Nothing
            o = New StringBuilder()
            For i As UInt32 = uint32_0 To strlen(s) - uint32_1
                If uri.unreserved(s(i)) Then
                    o.Append(s(i))
                Else
                    Dim bs() As Byte = Nothing
                    bs = e.GetBytes(s(i))
                    assert(Not isemptyarray(bs))
                    For j As UInt32 = uint32_0 To array_size(bs) - uint32_1
                        o.Append(constants.uri.escape)
                        o.Append(bs(j).hex())
                    Next
                End If
            Next
            Return Convert.ToString(o)
        End If
    End Function

    <Extension()> Public Function uri_path_decode(ByVal s As String, Optional ByVal e As Encoding = Nothing) As String
        If String.IsNullOrEmpty(s) Then
            Return empty_string
        Else
            If e Is Nothing Then
                e = default_encoding
            End If
            Dim o As StringBuilder = Nothing
            Dim buff() As Byte = Nothing
            Dim last As Int32 = 0
            Dim append_last As Action = Nothing
            Dim append_with_last As Action(Of String) = Nothing
            o = New StringBuilder()
            ReDim buff((strlen(s) \ (expected_hex_byte_length + 1)) - 1)
            append_last = Sub()
                              If last > 0 Then
                                  Dim t As String = Nothing
                                  If e.try_get_string(buff, 0, last, t) Then
                                      o.Append(t)
                                  Else
                                      For i As Int32 = 0 To last - 1
                                          o.Append(constants.uri.escape) _
                                           .Append(buff(i).hex())
                                      Next
                                  End If
                                  last = 0
                              End If
                          End Sub
            append_with_last = Sub(x As String)
                                   append_last()
                                   o.Append(x)
                               End Sub
            For i As UInt32 = uint32_0 To strlen(s) - uint32_1
                If s(i) = constants.uri.escape Then
                    Dim t As String = Nothing
                    t = strmid(s, i + 1, expected_hex_byte_length)
                    Dim b As Byte = 0
                    If hex_byte(t, b) Then
                        buff(last) = b
                        last += 1
                    Else
                        append_with_last(strcat(constants.uri.escape, t))
                    End If
                    i += expected_hex_byte_length
                Else
                    append_with_last(s(i))
                End If
            Next
            append_last()
            Return Convert.ToString(o)
        End If
    End Function
End Module
