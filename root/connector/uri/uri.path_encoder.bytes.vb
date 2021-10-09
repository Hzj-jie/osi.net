
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector.uri
Imports osi.root.constants

Namespace uri
    Partial Public NotInheritable Class path_encoder
        Public NotInheritable Class bytes
            Public NotInheritable Class shorten
                Private Const separator As Char = character.dot
                Private ReadOnly separators() As Char = {separator}
                Private Const escape As Char = character.dollar

                <global_init(global_init_level.runtime_checkers)>
                Private NotInheritable Class assertions
                    Private Shared Sub init()
                        assert(separator.unreserved())
                        assert(escape.unreserved())
                    End Sub

                    Private Sub New()
                    End Sub
                End Class

                ' Return false when o is nothing.
                Public Shared Function encode(ByVal b() As Byte, ByVal o As StringWriter) As Boolean
                    If o Is Nothing Then
                        Return False
                    End If

                    If isemptyarray(b) Then
                        Return True
                    End If

                    For i As Int32 = 0 To array_size_i(b) - 1
                        Dim c As Char = Nothing
                        c = Convert.ToChar(b(i))
                        If c.unreserved() AndAlso
                       c <> separator AndAlso
                       c <> escape Then
                            o.Write(c)
                        Else
                            o.Write(escape)
                            o.Write(b(i).hex())
                        End If
                    Next
                    Return True
                End Function

                ' Return false when o is nothing. Return true when s is nothing. Otherwise return false only when s is
                ' malformated.
                Public Shared Function decode(ByVal s As String, ByVal o As MemoryStream) As Boolean
                    If o Is Nothing Then
                        Return False
                    End If

                    If String.IsNullOrEmpty(s) Then
                        Return True
                    End If

                    For i As Int32 = 0 To strlen_i(s) - 1
                        assert(s(i) <> separator)
                        If s(i) = escape Then
                            Dim b As Byte = 0
                            If hex_byte(strmid(s, CUInt(i + 1), expected_hex_byte_length), b) Then
                                If Not o.write_byte(b) Then
                                    Return False
                                End If
                                i += expected_hex_byte_length
                            Else
                                Return False
                            End If
                        ElseIf s(i).unreserved() Then
                            Dim t As Int32 = 0
                            t = Convert.ToInt32(s(i))
                            assert(t >= 0 AndAlso t <= max_uint8)
                            If Not o.write_byte(CByte(t)) Then
                                Return False
                            End If
                        Else
                            Return False
                        End If
                    Next

                    Return True
                End Function

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class base64
                Public Shared Function encode(ByVal b() As Byte, ByVal o As StringWriter) As Boolean
                    If o Is Nothing Then
                        Return False
                    End If

                    If b Is Nothing Then
                        Return True
                    End If

                    o.Write(Convert.ToBase64String(b))
                    Return True
                End Function

                Public Shared Function decode(ByVal s As String, ByVal o As MemoryStream) As Boolean
                    If o Is Nothing Then
                        Return False
                    End If

                    If String.IsNullOrEmpty(s) Then
                        Return True
                    End If

                    Dim b() As Byte = Nothing
                    Try
                        b = Convert.FromBase64String(s)
                    Catch
                        Return False
                    End Try

                    Return o.write(b)
                End Function

                Private Sub New()
                End Sub
            End Class

            Private Sub New()
            End Sub
        End Class
    End Class
End Namespace
