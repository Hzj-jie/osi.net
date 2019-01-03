
Imports System.Text
Imports osi.root.connector
Imports osi.root.utt

Public Class encoding_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim encs() As EncodingInfo = Nothing
        encs = Encoding.GetEncodings()
        If assertion.is_false(isemptyarray(encs)) Then
            For i As Int32 = 0 To array_size(encs) - 1
                If assertion.is_not_null(encs(i)) Then
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
                End If
            Next
        End If
        Return True
    End Function
End Class
