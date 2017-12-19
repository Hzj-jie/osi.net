
Imports System.IO
Imports System.Runtime.CompilerServices

Public Module _memory_stream
    <Extension()> Public Sub shrink_to_fit(ByVal i As MemoryStream)
        assert(Not i Is Nothing)
        i.Capacity() = i.Length()
    End Sub

    <Extension()> Public Function fit_buffer(ByVal i As MemoryStream) As Byte()
        assert(Not i Is Nothing)
        i.shrink_to_fit()
        Return i.GetBuffer()
    End Function

    <Extension()> Public Function export(ByVal i As MemoryStream) As Byte()
        assert(Not i Is Nothing)
        Dim o() As Byte = Nothing
        ReDim o(i.Length() - 1)
        memcpy(o, i.GetBuffer(), i.Length())
        Return o
    End Function
End Module

Public NotInheritable Class memory_stream
    Private Sub New()
    End Sub

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal offset As Int32,
                                  ByVal count As Int32,
                                  ByRef o As MemoryStream) As Boolean
        If i Is Nothing OrElse
           offset < 0 OrElse
           count < 0 OrElse
           array_size(i) < offset + count Then
            Return False
        Else
            o = New MemoryStream(i, offset, count)
            Return True
        End If
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal count As Int32,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, 0, count, o)
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, array_size(i), o)
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal offset As Int32,
                                  ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, offset, count, o))
        Return o
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, count, o))
        Return o
    End Function

    Public Shared Function create(ByVal i() As Byte) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal offset As Int32,
                                  ByVal count As Int32,
                                  ByVal enc As Text.Encoding,
                                  ByRef o As MemoryStream) As Boolean
        If offset < 0 OrElse
           count < 0 OrElse
           strlen(i) < offset + count Then
            Return False
        Else
            Dim b() As Byte = Nothing
            b = If(enc Is Nothing, default_encoding, enc).GetBytes(i, offset, count)
            Return assert(create(b, 0, array_size(b), o))
        End If
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal offset As Int32,
                                  ByVal count As Int32,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, offset, count, Nothing, o)
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal count As Int32,
                                  ByVal enc As Text.Encoding,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, 0, count, enc, o)
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal count As Int32,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, 0, count, o)
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal enc As Text.Encoding,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, 0, enc, o)
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, strlen(i), o)
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal offset As Int32,
                                  ByVal count As Int32,
                                  ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, offset, count, enc, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal offset As Int32,
                                  ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, offset, count, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal count As Int32,
                                  ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, count, enc, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, count, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String,
                                  ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, enc, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, o))
        Return o
    End Function
End Class
