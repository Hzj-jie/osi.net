
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices

Partial Public Class bytes_serializer(Of T)
    Public Function to_bytes(ByVal i As T) As Byte()
        Using ms As MemoryStream = New MemoryStream()
            assert(write_to(i, ms))
            Return ms.ToArray()
        End Using
    End Function

    Public Function append_to(ByVal i As T, ByVal o() As Byte, ByRef offset As UInt32) As Boolean
        Dim ms As MemoryStream = Nothing
        If Not memory_stream.from_index(o, offset, ms) Then
            Return False
        End If

        assert(Not ms Is Nothing)
        Using ms
            If Not append_to(i, ms) Then
                Return False
            End If

            ms.assert_valid()
            offset = CUInt(ms.Position())
            Return True
        End Using
    End Function

    Public Function from_bytes(ByVal b() As Byte, ByRef o As T) As Boolean
        If b Is Nothing Then
            Return False
        End If

        Using ms As MemoryStream = memory_stream.of(b)
            Return read_from(ms, o)
        End Using
    End Function

    Public Function from_bytes(ByVal b() As Byte) As T
        Dim r As T = Nothing
        assert(from_bytes(b, r))
        Return r
    End Function

    Public Function consume_from(ByVal b() As Byte, ByRef offset As UInt32, ByRef o As T) As Boolean
        Dim ms As MemoryStream = Nothing
        If memory_stream.from_index(b, offset, ms) Then
            assert(Not ms Is Nothing)
            Using ms
                If consume_from(ms, o) Then
                    ms.assert_valid()
                    offset = CUInt(ms.Position())
                    Return True
                Else
                    Return False
                End If
            End Using
        Else
            Return False
        End If
    End Function

    Public Function from_container(Of CONTAINER, ELEMENT)(ByVal i As CONTAINER, ByRef o As T) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim it As container_operator(Of CONTAINER, ELEMENT).enumerator = Nothing
        it = container_operator(Of CONTAINER, ELEMENT).default.enumerate(i)
        If o Is Nothing Then
            o = alloc(Of T)()
        Else
            container_operator.clear(Of T, Byte())(o)
        End If
        If it Is Nothing Then
            Return True
        End If

        While Not it.end()
            container_operator.emplace(o, bytes_serializer.to_bytes(it.current()))
            it.next()
        End While
        Return True
    End Function

    Public Function to_container(Of CONTAINER, ELEMENT)(ByVal i As T, ByRef o As CONTAINER) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim it As container_operator(Of T, Byte()).enumerator = Nothing
        it = container_operator(Of T, Byte()).default.enumerate(i)
        If o Is Nothing Then
            o = alloc(Of CONTAINER)()
        Else
            container_operator.clear(Of CONTAINER, ELEMENT)(o)
        End If
        If it Is Nothing Then
            Return True
        End If

        While Not it.end()
            Dim r As ELEMENT = Nothing
            If Not bytes_serializer.from_bytes(it.current(), r) Then
                Return False
            End If

            container_operator.emplace(o, r)
            it.next()
        End While
        Return True
    End Function
End Class

Public Module _bytes_serializer_shortcuts
    <Extension()> Public Function [to](Of T)(ByVal i() As Byte, ByRef o As T) As Boolean
        Return bytes_serializer.from_bytes(i, o)
    End Function

    <Extension()> Public Function [from](Of T)(ByRef o() As Byte, ByVal i As T) As Byte()
        o = bytes_serializer.to_bytes(i)
        Return o
    End Function

    <Extension()> Public Function bytes(Of T)(ByVal i As T) As Byte()
        Return bytes_serializer.to_bytes(i)
    End Function
End Module
