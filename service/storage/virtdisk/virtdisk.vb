﻿
Imports System.IO
Imports osi.root.procedure
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.lock
Imports osi.service.device

Partial Public Class virtdisk
    Implements IDisposable
    Private Const default_buff_size As Int32 = 4096
    Private ReadOnly s As stream_flow_adapter
    Private ReadOnly fn As String
    Private ReadOnly c As capinfo
    Private ReadOnly l As ref(Of event_comb_lock)
    Private closed As singleentry

    Private Sub New(ByVal s As Stream)
        l = New ref(Of event_comb_lock)()
        'do not assert, since the valid will help to make sure it's safe
        Me.s = New stream_flow_adapter(s)
        AddHandler Me.s.dispose_exception,
                   Sub(ex As Exception)
                       raise_error(error_type.warning,
                                   "failed to close underlaid stream ",
                                   file_name(),
                                   ", ex ",
                                   ex.Message())
                   End Sub
    End Sub

    Private Shared Function create_file_stream(ByVal filename As String,
                                               ByVal buff_size As Int32) As Stream
        If buff_size <= 0 Then
            buff_size = default_buff_size
        End If
        create_directory(filename)
        Try
            Return New FileStream(filename,
                                  FileMode.OpenOrCreate,
                                  FileAccess.ReadWrite,
                                  FileShare.Read,
                                  buff_size,
                                  FileOptions.Asynchronous Or FileOptions.RandomAccess)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to open file ",
                        filename,
                        " for virtdisk, ex ",
                        ex.Message(),
                        ", the virtdisk object will stay in invalid status.")
            Return Nothing
        End Try
    End Function

    Public Sub New(ByVal filename As String,
                   Optional ByVal buff_size As Int32 = npos)
        Me.New(create_file_stream(filename, buff_size))
        copy(fn, filename)
        c = New capinfo(s.stream())
    End Sub

    'mainly for test only
    Private Sub New(ByVal s As MemoryStream)
        Me.New(DirectCast(s, Stream))
        fn = "NOT-TRACED"
        c = New capinfo(s)
    End Sub

    'test purpose
    Public Shared Function memory_virtdisk() As virtdisk
        Return New virtdisk(New MemoryStream())
    End Function

    Private Shared Sub create_directory(ByVal filename As String)
        Dim dir As String = Nothing
        Try
            dir = Path.GetDirectoryName(filename)
            Directory.CreateDirectory(dir)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to create directory ",
                        dir,
                        " for virtdisk file ",
                        filename,
                        ", ex ",
                        ex.Message())
        End Try
    End Sub

    Private Function stream() As Stream
        assert(Not s Is Nothing)
        Return s.stream()
    End Function

    Private Function pos(ByVal p As Int64) As Boolean
        Try
            stream().Position() = p
            Return True
        Catch ex As IOException
            'do not support windows 98 or earlier, so use fill function before
            raise_error(error_type.warning, "failed to set position of the stream, ex ", ex.Message())
            Return False
        Catch
            'other exception should be caught by valid check
            Return assert(False)
        End Try
    End Function

    Public Function file_name() As String
        Return fn
    End Function

    Public Function capacity() As Int64
        Return (+c)
    End Function

    Public Function valid() As Boolean
        Return Not stream() Is Nothing AndAlso
               Not closed.in_use() AndAlso
               stream().CanRead() AndAlso
               stream().CanWrite() AndAlso
               stream().CanSeek()
    End Function

    Public Function size() As Int64
        Return If(valid(), stream().Length(), npos)
    End Function

    Public Function read(ByVal start As Int64,
                         ByVal len As Int32,
                         ByVal buff() As Byte,
                         Optional ByVal offset As Int32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not valid() Then
                                      Return False
                                  ElseIf len = 0 Then
                                      'for perf concern, do not set position if not needed
                                      Return goto_end()
                                  ElseIf start < 0 OrElse
                                         len < 0 OrElse
                                         offset < 0 OrElse
                                         array_size(buff) < offset + len OrElse
                                         Not valid() OrElse
                                         stream().Length() < start + len Then
                                      Return False
                                  Else
                                      Return waitfor(l) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If pos(start) Then
                                      ec = s.receive(buff, offset, len)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      l.release()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  l.release()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read(ByVal start As Int64,
                         ByVal buff() As Byte,
                         Optional ByVal offset As Int32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not valid() OrElse
                                     offset < 0 OrElse
                                     offset >= array_size(buff) Then
                                      Return False
                                  Else
                                      ec = read(start, array_size(buff) - offset, buff, offset)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read(ByVal start As Int64,
                         ByVal len As Int32,
                         ByVal output As pointer(Of Byte())) As event_comb
        Dim r() As Byte = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If len < 0 Then
                                      Return False
                                  ElseIf len = 0 Then
                                      Return goto_end()
                                  Else
                                      ReDim r(len - 1)
                                      ec = read(start, len, r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(output, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    'looks like this function is the only one which is not thread-safe
    Private Function len(ByVal l As Int64) As Boolean
        If valid() Then
            Try
                stream().SetLength(l)
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to set the length of virtdisk to ", l, ", ex ",
                            ex.Message())
                Return False
            End Try
            If stream().Length() = l Then
                Return True
            Else
                raise_error(error_type.warning,
                            "failed to set the length of virtdisk to ", l, ", undefined error")
                Return False
            End If
        Else
            Return False
        End If
    End Function

    'make sure the stream has at least such length
    Public Function fill(ByVal l As Int64) As event_comb
        Return Me.l.locked(Function() As Boolean
                               Return unlocked_fill(l)
                           End Function)
    End Function

    Private Function unlocked_fill(ByVal l As Int64) As Boolean
        Return valid() AndAlso (stream().Length() >= l OrElse len(l))
    End Function

    'drop the following data after l as the position, so l is the new length of the stream
    'return false if underlaid stream is not accessable or the length of the stream is shorter than l
    Public Function drop(ByVal l As Int64) As event_comb
        Return Me.l.locked(Function() As Boolean
                               Return unlocked_drop(l)
                           End Function)
    End Function

    Private Function unlocked_drop(ByVal l As Int64) As Boolean
        Return valid() AndAlso (stream().Length() >= l AndAlso len(l))
    End Function

    Public Function write(ByVal start As Int64,
                          ByVal count As Int32,
                          ByVal buff() As Byte,
                          Optional ByVal offset As Int32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not valid() Then
                                      Return False
                                  ElseIf count = 0 Then
                                      'for perf concern, do not set position if not needed
                                      Return goto_end()
                                  ElseIf start < 0 OrElse
                                         count < 0 OrElse
                                         offset < 0 OrElse
                                         array_size(buff) < offset + count OrElse
                                         Not valid() Then
                                      Return False
                                  Else
                                      Return waitfor(l) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If unlocked_fill(start + count) AndAlso
                                     pos(start) Then
                                      ec = s.send(buff, offset, count)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      l.release()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  l.release()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function write(ByVal start As Int64,
                          ByVal buff() As Byte,
                          Optional ByVal offset As Int32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If offset < 0 OrElse
                                     offset >= array_size(buff) Then
                                      Return False
                                  Else
                                      ec = write(start, array_size(buff) - offset, buff, offset)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Sub flush()
        If valid() Then
            Try
                stream().Flush()
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to flush data into the underlaid stream ",
                            file_name(),
                            ", ex ",
                            ex.Message())
            End Try
        End If
    End Sub

    Public Sub close()
        Dispose()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        s.Dispose()
    End Sub

    Protected NotOverridable Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
