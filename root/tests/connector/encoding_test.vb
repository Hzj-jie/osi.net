
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class encoding_test
    Inherits [case]

    Private Shared Sub try_get_encoding_test()
        Dim encs() As EncodingInfo = Encoding.GetEncodings()
        assertion.is_false(encs.isemptyarray())
        For i As Int32 = 0 To encs.array_size_i() - 1
            If Not assertion.is_not_null(encs(i)) Then
                Continue For
            End If
            Dim n As String = Nothing
            n = strcat(encs(i).Name(), " [", encs(i).DisplayName(), "] - ", encs(i).CodePage())
            Dim e As Encoding = Nothing
            If assertion.is_true(try_get_encoding(encs(i).Name(), e),
                                 "failed to get encoding from name, ",
                                 n) Then
                assertion.is_not_null(e, n)
            End If
            If assertion.is_true(try_get_encoding(encs(i).CodePage(), e),
                                 "failed to get encoding from codepage, ",
                                 n) Then
                assertion.is_not_null(e, n)
            End If
        Next
    End Sub

    Private Shared Sub guess_encoding_test()
        assertion.equal(memory_stream.of(utf8).guess_encoding(), encodings.utf8_nobom, utf8.hex_str())

        Dim t() As pair(Of Byte(), Encoding) = {
            pair.of(utf8_2, encodings.utf8_nobom),
            pair.of(unicode_be, Encoding.BigEndianUnicode),
            pair.of(unicode, Encoding.Unicode),
            pair.of(utf8, encodings.utf8_nobom),
            pair.of(utf8_bom, Encoding.UTF8),
            pair.of(gbk, encodings.gbk)
        }
        For i As Int32 = 0 To t.array_size_i() - 1
            assertion.equal(memory_stream.of(t(i).first).guess_encoding(), t(i).second, t(i).first.hex_str())
        Next
    End Sub

    Public Overrides Function run() As Boolean
        try_get_encoding_test()
        guess_encoding_test()
        Return True
    End Function
End Class
