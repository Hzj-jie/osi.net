
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text

Namespace uri
    ' Encode a minimum set of characters for *different* uri protocols.
    ' System.Web.HttpServerUtility.UrlPathEncode
    Partial Public NotInheritable Class path_encoder
        Public NotInheritable Class [string]
            ' Return false when o is nothing.
            Public Shared Function encode(ByVal s As String,
                                          ByVal o As StringWriter,
                                          Optional ByVal e As Encoding = Nothing) As Boolean
                If o Is Nothing Then
                    Return False
                End If

                If s.null_or_empty() Then
                    Return True
                End If

                If e Is Nothing Then
                    e = default_encoding
                End If
                For i As Int32 = 0 To strlen_i(s) - 1
                    If s(i).unreserved() Then
                        o.Write(s(i))
                    Else
                        Dim bs() As Byte = Nothing
                        bs = e.GetBytes(s(i))
                        assert(Not isemptyarray(bs))
                        For j As Int32 = 0 To array_size_i(bs) - 1
                            o.Write(constants.uri.escape)
                            o.Write(bs(j).hex())
                        Next
                    End If
                Next
                Return True
            End Function

            ' Return false when o is nothing.
            Public Shared Function decode(ByVal s As String,
                                          ByVal o As StringWriter,
                                          Optional ByVal e As Encoding = Nothing) As Boolean
                If o Is Nothing Then
                    Return False
                End If

                If s.null_or_empty() Then
                    Return True
                End If

                If e Is Nothing Then
                    e = default_encoding
                End If

                Dim buff() As Byte = Nothing
                Dim last As Int32 = 0
                Dim append_last As Action = Nothing
                Dim append_with_last As Action(Of String) = Nothing
                ReDim buff((strlen_i(s) \ (expected_hex_byte_length + 1)) - 1)
                append_last = Sub()
                                  If last > 0 Then
                                      Dim t As String = Nothing
                                      If e.try_get_string(buff, 0, last, t) Then
                                          o.Write(t)
                                      Else
                                          For i As Int32 = 0 To last - 1
                                              o.Write(constants.uri.escape)
                                              o.Write(buff(i).hex())
                                          Next
                                      End If
                                      last = 0
                                  End If
                              End Sub
                append_with_last = Sub(x As String)
                                       append_last()
                                       o.Write(x)
                                   End Sub
                For i As Int32 = 0 To strlen_i(s) - 1
                    If s(i) = constants.uri.escape Then
                        Dim t As String = Nothing
                        t = strmid(s, CUInt(i + 1), expected_hex_byte_length)
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
                Return True
            End Function

            Private Sub New()
            End Sub
        End Class
    End Class
End Namespace