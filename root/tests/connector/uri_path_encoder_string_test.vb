
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports uri = osi.root.connector.uri

Public Class uri_path_encoder_string_test
    Inherits [case]

    Private Shared Function basic_function() As Boolean
        Dim s As String = Nothing
        For i As Int32 = 0 To 128 - 1
            s = strcat(s, If(rnd_bool(), "H", character.sbc0))
        Next
        Dim s2 As String = Nothing
        s2 = uri.path_encoder.encode(s)
        s2 = uri.path_encoder.decode(s2)
        assertion.equal(s, s2)
        Return True
    End Function

    Private Shared Function invalid_hex() As Boolean
        Dim s As String = Nothing
        s = "abc%FG%FG%FGabc"
        Dim s2 As String = Nothing
        s2 = uri.path_encoder.decode(s)
        assertion.equal(s, s2)
        Return True
    End Function

    Private Shared Function invalid_encoding() As Boolean
        Dim s As String = Nothing
        For i As Int32 = 0 To 128 - 1
            strcat(s, If(rnd_bool(), "H", character.sbc0))
        Next
        s = uri.path_encoder.encode(s, Text.Encoding.Unicode())
        Dim s2 As String = Nothing
        s2 = uri.path_encoder.decode(s, New Text.UTF8Encoding(False, True))
        assertion.equal(s, s2)
        Return True
    End Function

    Private Shared Function not_ended() As Boolean
        Dim s As String = Nothing
        s = "abc%D"
        Dim s2 As String = Nothing
        s2 = uri.path_encoder.decode(s)
        assertion.equal(s, s2)

        s = "abc%%"
        s2 = uri.path_encoder.decode(s)
        assertion.equal(s, s2)
        Return True
    End Function

    Private Shared Function all_hex() As Boolean
        Dim s As String = Nothing
        For i As Int32 = 0 To 128 - 1
            s = strcat(s, If(rnd_bool(), character.sbc0, character.sbcblank))
        Next
        Dim s2 As String = Nothing
        s2 = uri.path_encoder.encode(s)
        s2 = uri.path_encoder.decode(s2)
        assertion.equal(s, s2)
        Return True
    End Function

    Private Shared Function special_cases() As Boolean
        assertion.equal(uri.path_encoder.encode("//"), "%2F%2F")
        assertion.equal(uri.path_encoder.decode("%2F%2F"), "//")
        assertion.equal(uri.path_encoder.decode("%2f%2f"), "//")
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return basic_function() AndAlso
               invalid_hex() AndAlso
               invalid_encoding() AndAlso
               not_ended() AndAlso
               all_hex() AndAlso
               special_cases()
    End Function
End Class
