
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

' A functor to implement T to bytes and bytes to T operations.
Partial Public Class bytes_serializer(Of T)
    Public Shared ReadOnly [default] As bytes_serializer(Of T)

    Shared Sub New()
        [default] = New bytes_serializer(Of T)()
    End Sub

    ' Write i into a MemoryStream.
    Private Function to_bytes(ByVal i As T, ByVal o As MemoryStream, ByVal append As Boolean) As Boolean
        If o Is Nothing Then
            Return False
        End If

        Dim f As Func(Of T, MemoryStream, Boolean) = Nothing
        If append Then
            f = append_to()
        Else
            f = write_to()
        End If
        If Not f Is Nothing Then
            Return f(i, o)
        End If

        Dim s As String = Nothing
        s = json_serializer.to_str(i)
        If append Then
            Return bytes_serializer.append_to(s, o)
        End If
        Return bytes_serializer.write_to(s, o)
    End Function

    Public Function append_to(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return to_bytes(i, o, True)
    End Function

    Public Function write_to(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return to_bytes(i, o, False)
    End Function

    Private Function do_from_bytes(ByVal i As MemoryStream, ByRef o As T, ByVal consume As Boolean) As Boolean
        assert(Not i Is Nothing)

        Dim f As _do_val_ref(Of MemoryStream, T, Boolean) = Nothing
        If consume Then
            f = consume_from()
        Else
            f = read_from()
        End If
        If Not f Is Nothing Then
            Return f(i, o)
        End If

        Dim s As String = Nothing
        If consume Then
            If Not bytes_serializer.consume_from(i, s) Then
                Return False
            End If
        Else
            If Not bytes_serializer.read_from(i, s) Then
                Return False
            End If
        End If
        Return json_serializer.from_str(s, o)
    End Function

    ' Read enough bytes from i and construct o.
    Private Function from_bytes(ByVal i As MemoryStream, ByRef o As T, ByVal consume As Boolean) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim p As UInt32 = 0
        i.assert_valid()
        p = CUInt(i.Position())
        If do_from_bytes(i, o, consume) Then
            If consume Then
                Return True
            End If
            Return i.eos()
        End If
        i.Position() = p
        Return False
    End Function

    Public Function consume_from(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return from_bytes(i, o, True)
    End Function

    Public Function read_from(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return from_bytes(i, o, False)
    End Function

    Public Shared Operator +(ByVal this As bytes_serializer(Of T)) As bytes_serializer(Of T)
        If this Is Nothing Then
            Return [default]
        Else
            Return this
        End If
    End Operator

    Protected Sub New()
    End Sub
End Class
