
Imports System.IO
Imports osi.root.connector

Public Class mock_stream
    Inherits Stream

    Private ReadOnly receive_buff As MemoryStream
    Private ReadOnly send_buff As MemoryStream
    Private ReadOnly random_zero As Boolean
    Private ReadOnly random_failure As Boolean

    Public Sub New(ByVal receive_buff() As Byte,
                   ByVal random_zero As Boolean,
                   ByVal random_failure As Boolean)
        Me.receive_buff = New MemoryStream(receive_buff)
        Me.send_buff = New MemoryStream()
        Me.random_zero = random_zero
        Me.random_failure = random_failure
    End Sub

    Public Function receive_consistent(ByVal receive_buff() As Byte) As Boolean
        Return memcmp(Me.receive_buff.fit_buffer(), receive_buff) = 0
    End Function

    Public Function send_consistent(ByVal send_buff() As Byte) As Boolean
        Return memcmp(Me.send_buff.fit_buffer(), send_buff) = 0
    End Function

    Public NotOverridable Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return receive_buff.CanRead()
        End Get
    End Property

    Public NotOverridable Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return receive_buff.CanSeek()
        End Get
    End Property

    Public NotOverridable Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return send_buff.CanWrite()
        End Get
    End Property

    Public NotOverridable Overrides Sub Flush()
        send_buff.Flush()
    End Sub

    Public NotOverridable Overrides ReadOnly Property Length() As Int64
        Get
            Return receive_buff.Length()
        End Get
    End Property

    Public NotOverridable Overrides Property Position() As Int64
        Get
            Return receive_buff.Position()
        End Get
        Set(ByVal value As Int64)
            receive_buff.Position() = value
        End Set
    End Property

    Public NotOverridable Overrides Function Read(ByVal buffer() As Byte,
                                                  ByVal offset As Int32,
                                                  ByVal count As Int32) As Int32
        If random_zero AndAlso rnd_bool(3) Then
            Return 0
        ElseIf random_failure AndAlso rnd_bool(3) Then
            Throw New IOException()
        Else
            Return receive_buff.Read(buffer, offset, count)
        End If
    End Function

    Public NotOverridable Overrides Function Seek(ByVal offset As Int64, ByVal origin As SeekOrigin) As Int64
        Return receive_buff.Seek(offset, origin)
    End Function

    Public NotOverridable Overrides Sub SetLength(ByVal value As Int64)
        receive_buff.SetLength(value)
    End Sub

    Public NotOverridable Overrides Sub Write(ByVal buffer() As Byte, ByVal offset As Int32, ByVal count As Int32)
        If random_failure AndAlso rnd_bool(3) Then
            Throw New IOException()
        Else
            send_buff.Write(buffer, offset, count)
        End If
    End Sub
End Class
