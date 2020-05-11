
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public NotInheritable Class string_serializer
    Public Shared Sub register(Of T)(ByVal to_str As Action(Of T, StringWriter))
        string_serializer(Of T).register(to_str)
    End Sub

    Public Shared Sub register(Of T)(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        string_serializer(Of T).register(from_str)
    End Sub

    Public Shared Sub register(Of T)(ByVal to_str As Action(Of T, StringWriter),
                                     ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        register(to_str)
        register(from_str)
    End Sub

    Public Shared Function from_str(Of T)(ByVal i As StringReader, ByRef o As T) As Boolean
        Return string_serializer(Of T).r.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return string_serializer(Of T).r.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String) As T
        Return string_serializer(Of T).r.from_str(i)
    End Function

    Public Shared Function from_str_or_default(Of T)(ByVal i As String, ByVal [default] As T) As T
        Return string_serializer(Of T).r.from_str_or_default(i, [default])
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByVal o As StringWriter) As Boolean
        Return string_serializer(Of T).r.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByRef o As String) As Boolean
        Return string_serializer(Of T).r.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T) As String
        Return string_serializer(Of T).r.to_str(i)
    End Function

    Public Shared Function to_str_or_default(Of T)(ByVal i As T, ByVal [default] As String) As String
        Return string_serializer(Of T).r.to_str_or_default(i, [default])
    End Function

    Public Shared Function to_str_or_null(Of T)(ByVal i As T) As String
        Return string_serializer(Of T).r.to_str_or_null(i)
    End Function
End Class
