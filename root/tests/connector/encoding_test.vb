
Imports System.Text
Imports osi.root.connector
Imports osi.root.utt

Public Class encoding_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim encs() As EncodingInfo = Nothing
        encs = Encoding.GetEncodings()
        If assert_false(isemptyarray(encs)) Then
            For i As Int32 = 0 To array_size(encs) - 1
                If assert_not_nothing(encs(i)) Then
                    Dim n As String = Nothing
                    n = strcat(encs(i).Name(), " [", encs(i).DisplayName(), "] - ", encs(i).CodePage())
                    Dim e As Encoding = Nothing
                    If assert_true(try_get_encoding(encs(i).Name(), e),
                                   "failed to get encoding from name, ",
                                   n) Then
                        assert_not_nothing(e, n)
                    End If
                    If assert_true(try_get_encoding(encs(i).CodePage(), e),
                                   "failed to get encoding from codepage, ",
                                   n) Then
                        assert_not_nothing(e, n)
                    End If
                End If
            Next
        End If
        Return True
    End Function
End Class
