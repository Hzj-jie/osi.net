
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector.uri

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
