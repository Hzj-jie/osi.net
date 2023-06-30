
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text

Namespace uri
    Partial Public NotInheritable Class path_encoder
        Public Shared Function encode(ByVal b() As Byte, ByVal o As StringWriter) As Boolean
            Return bytes.shorten.encode(b, o)
        End Function

        Public Shared Function encode(ByVal b() As Byte) As String
            Using r As StringWriter = New StringWriter()
                assert(encode(b, r))
                Return Convert.ToString(r)
            End Using
        End Function

        Public Shared Function decode(ByVal s As String, ByVal o As MemoryStream) As Boolean
            Return bytes.shorten.decode(s, o)
        End Function

        Public Shared Function decode(ByVal s As String, ByRef o() As Byte) As Boolean
            Using r As MemoryStream = New MemoryStream()
                If Not decode(s, r) Then
                    Return False
                End If
                o = r.fit_buffer()
                Return True
            End Using
        End Function

        Public Shared Function encode(ByVal s As String,
                                      ByVal o As StringWriter,
                                      Optional ByVal encoder As Encoding = Nothing) As Boolean
            Return [string].encode(s, o, encoder)
        End Function

        Public Shared Function encode(ByVal s As String, Optional ByVal encoder As Encoding = Nothing) As String
            Using r As StringWriter = New StringWriter()
                If Not encode(s, r, encoder) Then
                    Return Nothing
                End If
                Return Convert.ToString(r)
            End Using
        End Function

        Public Shared Function decode(ByVal s As String,
                                      ByVal o As StringWriter,
                                      Optional ByVal encoder As Encoding = Nothing) As Boolean
            Return [string].decode(s, o, encoder)
        End Function

        Public Shared Function decode(ByVal s As String, Optional ByVal encoder As Encoding = Nothing) As String
            Using r As StringWriter = New StringWriter()
                If Not decode(s, r, encoder) Then
                    Return Nothing
                End If
                Return Convert.ToString(r)
            End Using
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
