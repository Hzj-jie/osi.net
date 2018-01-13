﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Public Module _memory_stream
    <Extension()> Public Sub assert_valid(ByVal this As MemoryStream)
        assert(Not this Is Nothing)
        assert(this.Length() >= 0, "Length of a MemoryStream instance is less than 0.")
        assert(this.Length() <= max_int32, "Length of a MemoryStream instance is over max_int32.")
        assert(this.Position() >= 0, "Position of a MemoryStream instance is less than 0.")
        assert(this.Position() <= max_int32, "Position of a MemoryStream instance is over max_int32.")
        assert(this.Position() <= this.Length(), "Position of a MemoryStream instance is over its Length().")
        assert(array_size_i(this.GetBuffer()) >= this.Length(),
               "Length of a MemoryStream instance is over the capability of its buffer.")
    End Sub

    <Extension()> Public Sub shrink_to_fit(ByVal i As MemoryStream)
        i.assert_valid()
        i.Capacity() = CInt(i.Length())
    End Sub

    <Extension()> Public Function fit_buffer(ByVal i As MemoryStream) As Byte()
        i.shrink_to_fit()
        Return i.GetBuffer()
    End Function

    <Extension()> Public Function unread_length(ByVal i As MemoryStream) As UInt32
        i.assert_valid()
        Return CUInt(i.Length() - i.Position())
    End Function

    <Extension()> Public Function export(ByVal i As MemoryStream) As Byte()
        i.assert_valid()
        Dim o() As Byte = Nothing
        ReDim o(CInt(i.Length()) - 1)
        memcpy(o, i.GetBuffer(), CUInt(i.Length()))
        Return o
    End Function

    <Extension()> Public Sub get_buffer(ByVal this As MemoryStream,
                                        ByRef o() As Byte,
                                        ByRef offset As UInt32,
                                        Optional ByRef len As UInt32 = 0)
        this.assert_valid()
        o = this.GetBuffer()
        offset = CUInt(this.Position())
        len = CUInt(this.Length())
    End Sub

    <Extension()> Public Function write(ByVal this As MemoryStream, ByVal b() As Byte) As Boolean
        assert(Not this Is Nothing)
        If b Is Nothing Then
            Return False
        End If
        Try
            this.Write(b, 0, array_size_i(b))
            Return True
        Catch
        End Try
        Return False
    End Function

    <Extension()> Public Function read(ByVal this As MemoryStream, ByVal o() As Byte) As Boolean
        this.assert_valid()
        If o Is Nothing Then
            Return False
        ElseIf isemptyarray(o) Then
            Return True
        Else
            Dim p As Int64 = 0
            p = this.Position()
            If this.Read(o, 0, array_size_i(o)) = array_size_i(o) Then
                Return True
            Else
                this.Position() = p
                Return False
            End If
        End If
    End Function

    <Extension()> Public Function write_byte(ByVal this As MemoryStream, ByVal b As Byte) As Boolean
        assert(Not this Is Nothing)
        Try
            this.WriteByte(b)
            Return True
        Catch
        End Try
        Return False
    End Function

    <Extension()> Public Function eos(ByVal this As MemoryStream) As Boolean
        assert(Not this Is Nothing)
        Return this.Position() = this.Length()
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
            o = New MemoryStream(i, offset, count, True, True)
            Return True
        End If
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal offset As UInt32,
                                  ByVal count As UInt32,
                                  ByRef o As MemoryStream) As Boolean
        If offset > max_int32 OrElse count > max_int32 Then
            Return False
        Else
            Return create(i, CInt(offset), CInt(count), o)
        End If
    End Function

    Public Shared Function create_from_index(ByVal i() As Byte,
                                             ByVal offset As Int32,
                                             ByRef o As MemoryStream) As Boolean
        If offset > array_size_i(i) Then
            Return False
        Else
            Return create(i, offset, array_size_i(i) - offset, o)
        End If
    End Function

    Public Shared Function create_from_index(ByVal i() As Byte,
                                             ByVal offset As UInt32,
                                             ByRef o As MemoryStream) As Boolean
        If offset > array_size(i) Then
            Return False
        Else
            Return create(i, offset, array_size(i) - offset, o)
        End If
    End Function

    Public Shared Function create(ByVal i() As Byte,
                                  ByVal count As Int32,
                                  ByRef o As MemoryStream) As Boolean
        Return create(i, 0, count, o)
    End Function

    Public Shared Function create(ByVal i() As Byte, ByRef o As MemoryStream) As Boolean
        Return create(i, array_size_i(i), o)
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
            Return assert(create(b, 0, array_size_i(b), o))
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

    Public Shared Function create(ByVal i As String, ByRef o As MemoryStream) As Boolean
        Return create(i, strlen_i(i), o)
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

    Public Shared Function create(ByVal i As String, ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(create(i, count, o))
        Return o
    End Function

    Public Shared Function create(ByVal i As String, ByVal enc As Text.Encoding) As MemoryStream
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
