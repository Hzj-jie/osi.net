
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Public NotInheritable Class uri_serializer
    Public Shared Sub register(Of T)(ByVal to_str As Action(Of T, StringWriter))
        uri_serializer(Of T).register(to_str)
    End Sub

    Public Shared Sub register(Of T)(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        uri_serializer(Of T).register(from_str)
    End Sub

    Public Shared Function from_str(Of T)(ByVal i As StringReader, ByRef o As T) As Boolean
        Return uri_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return uri_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByVal o As StringWriter) As Boolean
        Return uri_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByRef o As String) As Boolean
        Return uri_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T) As String
        Return uri_serializer(Of T).default.to_str(i)
    End Function

    Private Sub New()
    End Sub
End Class

' From or to URI safe strings. Types are expected to implement bytes_serializer or json_serializer instead of
' implementing uri_serializer directly.
Public Class uri_serializer(Of T)
    Inherits string_serializer(Of T, uri_serializer(Of T))

    Public Shared Shadows ReadOnly [default] As uri_serializer(Of T)

    Shared Sub New()
        [default] = New uri_serializer(Of T)()
    End Sub

    Public Shared Shadows Operator +(ByVal this As uri_serializer(Of T)) As uri_serializer(Of T)
        If this Is Nothing Then
            Return [default]
        End If
        Return this
    End Operator

    Protected Overrides Function to_str() As Action(Of T, StringWriter)
        Return Sub(ByVal i As T, ByVal o As StringWriter)
                   assert(Not i Is Nothing)
                   assert(Not o Is Nothing)
                   Dim b() As Byte = Nothing
                   assert(bytes_serializer.to_bytes(i, b))
                   o.Write(constants.uri.path_separator)
                   o.Write(uri.path_encoder.encode(b))
               End Sub
    End Function

    Protected Overrides Function from_str() As _do_val_ref(Of StringReader, T, Boolean)
        Return Function(ByVal i As StringReader, ByRef o As T) As Boolean
                   assert(Not i Is Nothing)
                   If i.Read() <> Convert.ToInt32(constants.uri.path_separator) Then
                       Return False
                   End If

                   Dim b() As Byte = Nothing
                   Return uri.path_encoder.decode(i.ReadToEnd(), b) AndAlso
                          bytes_serializer.from_bytes(b, o)
               End Function
    End Function

    Protected Sub New()
    End Sub
End Class
