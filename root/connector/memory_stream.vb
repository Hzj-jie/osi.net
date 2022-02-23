
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _memory_stream
    <Extension()> Public Sub assert_valid(ByVal this As MemoryStream)
        assert(this IsNot Nothing)
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
        arrays.copy(o, i.GetBuffer(), CUInt(i.Length()))
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
        Return try_write(this, b, 0, array_size(b))
    End Function

    <Extension()> Public Function try_write(ByVal this As MemoryStream,
                                            ByVal b() As Byte,
                                            ByVal offset As UInt32,
                                            ByVal count As UInt32) As Boolean
        assert(this IsNot Nothing)
        If b Is Nothing Then
            Return False
        End If

        If offset > max_int32 OrElse count > max_int32 Then
            Return False
        End If

        If array_size(b) < offset + count Then
            Return False
        End If

        If count = 0 Then
            Return True
        End If

        Try
            this.Write(b, CInt(offset), CInt(count))
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function read(ByVal this As MemoryStream, ByVal o() As Byte) As Boolean
        this.assert_valid()
        If o Is Nothing Then
            Return False
        End If
        If isemptyarray(o) Then
            Return True
        End If
        Dim p As Int64 = 0
        p = this.Position()
        If this.Read(o, 0, array_size_i(o)) = array_size_i(o) Then
            Return True
        End If
        this.Position() = p
        Return False
    End Function

    <Extension()> Public Function read_to_end(ByVal this As MemoryStream) As Byte()
        this.assert_valid()
        Dim r() As Byte = Nothing
        ReDim r(CInt(this.unread_length() - 1))
        assert(this.read(r))
        Return r
    End Function

    <Extension()> Public Function write_byte(ByVal this As MemoryStream, ByVal b As Byte) As Boolean
        assert(this IsNot Nothing)
        Try
            this.WriteByte(b)
            Return True
        Catch
        End Try
        Return False
    End Function

    <Extension()> Public Function eos(ByVal this As MemoryStream) As Boolean
        assert(this IsNot Nothing)
        Return this.Position() = this.Length()
    End Function

    <Extension()> Public Function dump_to_file(ByVal this As MemoryStream, ByVal o As String) As Boolean
        assert(this IsNot Nothing)
        assert(Not memory_stream.is_wrapper(this))
        Try
            Using fs As New FileStream(o, FileMode.Create)
                this.WriteTo(fs)
            End Using
            Return True
        Catch ex As Exception
            raise_error(error_type.warning, "failed to write to ", o, ", ex ", ex)
        End Try
        Return False
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    <Extension()> Public Function zip_to_file(ByVal this As MemoryStream, ByVal o As String) As Boolean
        assert(this IsNot Nothing)
        Dim b() As Byte = Nothing
        b = this.ToArray()
        Try
            Using fs As FileStream = New FileStream(o, FileMode.Create),
                  gz As GZipStream = New GZipStream(fs, CompressionMode.Compress, True)
                gz.Write(b, 0, b.array_size_i())
            End Using
            Return True
        Catch ex As Exception
            raise_error(error_type.warning, "failed to write to ", o, ", ex ", ex)
        End Try
        Return False
    End Function

    <Extension()> Public Function read_from_file(ByVal this As MemoryStream, ByVal i As String) As Boolean
        assert(this IsNot Nothing)
        Try
            Using fs As New FileStream(i, FileMode.Open, FileAccess.Read)
                fs.CopyTo(this)
            End Using
            Return True
        Catch ex As Exception
            raise_error(error_type.warning, "failed to read from ", i, ", ex ", ex)
        End Try
        Return False
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    <Extension()> Public Function unzip_from_file(ByVal this As MemoryStream, ByVal i As String) As Boolean
        assert(this IsNot Nothing)
        Try
            Using fs As FileStream = New FileStream(i, FileMode.Open),
                  gz As GZipStream = New GZipStream(fs, CompressionMode.Decompress, True)
                gz.CopyTo(this)
            End Using
            Return True
        Catch ex As Exception
            raise_error(error_type.warning, "failed to read from ", i, ", ex ", ex)
        End Try
        Return False
    End Function

    <Extension()> Public Function trim_used(ByRef this As MemoryStream) As MemoryStream
        assert(this IsNot Nothing)
        If this.Position() = 0 Then
            Return this
        End If
        Dim r As MemoryStream = Nothing
        r = New MemoryStream()
        this.CopyTo(r)
        r.Position() = 0
        this = r
        Return r
    End Function

    <Extension()> Public Function trim(ByVal this As MemoryStream, ByVal length As UInt32) As MemoryStream
        assert(this IsNot Nothing)
        If this.Length() > length Then
            this.SetLength(length)
        End If
        Return this
    End Function

    <Extension()> Public Function clear(ByVal this As MemoryStream) As MemoryStream
        assert(this IsNot Nothing)
        this.Position() = 0
        this.SetLength(0)
        Return this
    End Function

    <Extension()> Public Function clone(ByVal this As MemoryStream) As MemoryStream
        this.assert_valid()
        Dim r As MemoryStream = Nothing
        r = New MemoryStream(this.ToArray())
        r.Position() = this.Position()
        Return r
    End Function

    <Extension()> Public Function unread_compare_to(ByVal this As MemoryStream, ByVal that As MemoryStream) As Int32
        If this Is Nothing AndAlso that Is Nothing Then
            Return 0
        End If
        If this Is Nothing Then
            Return -1
        End If
        If that Is Nothing Then
            Return 1
        End If
        If this.unread_length() < that.unread_length() Then
            Return -1
        End If
        If this.unread_length() > that.unread_length() Then
            Return 1
        End If
        Return memcmp(this.export(), that.export())
    End Function

    <Extension()> Public Function content_compare_to(ByVal this As MemoryStream, ByVal that As MemoryStream) As Int32
        If this Is Nothing AndAlso that Is Nothing Then
            Return 0
        End If
        If this Is Nothing Then
            Return -1
        End If
        If that Is Nothing Then
            Return 1
        End If
        If this.Length() < that.Length() Then
            Return -1
        End If
        If this.Length() > that.Length() Then
            Return 1
        End If
        Return memcmp(this.ToArray(), that.ToArray())
    End Function

    <Extension()> Public Function empty(ByVal this As MemoryStream) As Boolean
        assert(this IsNot Nothing)
        Return this.Length() = 0
    End Function
End Module

Public NotInheritable Class memory_stream
    Private Sub New()
    End Sub

    Public Shared Function [New](ByVal i() As Byte,
                                 ByVal offset As Int32,
                                 ByVal count As Int32,
                                 ByRef o As MemoryStream) As Boolean
        If i Is Nothing OrElse
           offset < 0 OrElse
           count < 0 OrElse
           array_size(i) < offset + count Then
            Return False
        End If
        ' The constructor of MemoryStream sets only _position but not Position(). So create a MemoryStream and
        ' manually set Position.
        o = New MemoryStream(i, 0, offset + count, True, True)
        o.Position() = offset
        Return True
    End Function

    Public Shared Function [New](ByVal i() As Byte,
                                 ByVal offset As UInt32,
                                 ByVal count As UInt32,
                                 ByRef o As MemoryStream) As Boolean
        If offset > max_int32 OrElse count > max_int32 Then
            Return False
        End If
        Return [New](i, CInt(offset), CInt(count), o)
    End Function

    Public Shared Function from_index(ByVal i() As Byte,
                                      ByVal offset As Int32,
                                      ByRef o As MemoryStream) As Boolean
        If offset > array_size_i(i) Then
            Return False
        End If
        Return [New](i, offset, array_size_i(i) - offset, o)
    End Function

    Public Shared Function from_index(ByVal i() As Byte,
                                      ByVal offset As UInt32,
                                      ByRef o As MemoryStream) As Boolean
        If offset > array_size(i) Then
            Return False
        End If
        Return [New](i, offset, array_size(i) - offset, o)
    End Function

    Public Shared Function [New](ByVal i() As Byte,
                                 ByVal count As Int32,
                                 ByRef o As MemoryStream) As Boolean
        Return [New](i, 0, count, o)
    End Function

    Public Shared Function [New](ByVal i() As Byte, ByRef o As MemoryStream) As Boolean
        Return [New](i, array_size_i(i), o)
    End Function

    Public Shared Function [of](ByVal i() As Byte,
                                ByVal offset As Int32,
                                ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, offset, count, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i() As Byte, ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, count, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i() As Byte) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, o))
        Return o
    End Function

    Public Shared Function [New](ByVal i As String,
                                 ByVal offset As Int32,
                                 ByVal count As Int32,
                                 ByVal enc As Text.Encoding,
                                 ByRef o As MemoryStream) As Boolean
        If offset < 0 OrElse
           count < 0 OrElse
           strlen(i) < offset + count Then
            Return False
        End If
        Dim b() As Byte = Nothing
        b = If(enc Is Nothing, default_encoding, enc).GetBytes(i, offset, count)
        Return assert([New](b, 0, array_size_i(b), o))
    End Function

    Public Shared Function [New](ByVal i As String,
                                 ByVal offset As Int32,
                                 ByVal count As Int32,
                                 ByRef o As MemoryStream) As Boolean
        Return [New](i, offset, count, Nothing, o)
    End Function

    Public Shared Function [New](ByVal i As String,
                                 ByVal count As Int32,
                                 ByVal enc As Text.Encoding,
                                 ByRef o As MemoryStream) As Boolean
        Return [New](i, 0, count, enc, o)
    End Function

    Public Shared Function [New](ByVal i As String,
                                 ByVal count As Int32,
                                 ByRef o As MemoryStream) As Boolean
        Return [New](i, 0, count, o)
    End Function

    Public Shared Function [New](ByVal i As String,
                                 ByVal enc As Text.Encoding,
                                 ByRef o As MemoryStream) As Boolean
        Return [New](i, 0, enc, o)
    End Function

    Public Shared Function [New](ByVal i As String, ByRef o As MemoryStream) As Boolean
        Return [New](i, strlen_i(i), o)
    End Function

    Public Shared Function [of](ByVal i As String,
                                ByVal offset As Int32,
                                ByVal count As Int32,
                                ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, offset, count, enc, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As String,
                                ByVal offset As Int32,
                                ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, offset, count, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As String,
                                ByVal count As Int32,
                                ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, count, enc, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As String, ByVal count As Int32) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, count, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As String, ByVal enc As Text.Encoding) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, enc, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As String) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert([New](i, o))
        Return o
    End Function

    Public Shared Function write_file_wrapped(ByVal i As String, ByRef o As MemoryStream) As Boolean
        Try
            o = wrap(New FileStream(i, FileMode.Create, FileAccess.Write))
            Return True
        Catch ex As Exception
            raise_error(error_type.warning, "failed to write to ", o, ", ex ", ex)
        End Try
        Return False
    End Function

    Public Shared Function write_file_wrapped(ByVal i As String) As MemoryStream
        Dim o As MemoryStream = Nothing
        assert(write_file_wrapped(i, o))
        Return o
    End Function

    ' Wraps a regular Stream into a MemoryStream which will not be backed by memory. This is purely for edge cases and
    ' should not be used commonly.
    Public Shared Function wrap(ByVal o As Stream) As MemoryStream
        Return New forwarder(o)
    End Function

    Public Shared Function is_wrapper(ByVal i As MemoryStream) As Boolean
        Return i IsNot Nothing AndAlso i.GetType().Equals(GetType(forwarder))
    End Function

    Private NotInheritable Class forwarder
        Inherits MemoryStream

        Private ReadOnly o As Stream

        Public Sub New(ByVal o As Stream)
            assert(o IsNot Nothing)
            Me.o = o
        End Sub

        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return o.CanRead()
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return o.CanSeek()
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return o.CanWrite()
            End Get
        End Property

        Public Overrides Property Capacity() As Int32
            Get
                Return CInt(o.Length())
            End Get
            Set(ByVal value As Int32)
                o.SetLength(value)
            End Set
        End Property

        Public Overrides ReadOnly Property Length() As Int64
            Get
                Return o.Length()
            End Get
        End Property

        Public Overrides Property Position() As Int64
            Get
                Return o.Position()
            End Get
            Set(ByVal value As Int64)
                o.Position() = value
            End Set
        End Property

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                o.Dispose()
            End If
        End Sub

        Public Overrides Sub Flush()
            o.Flush()
        End Sub

        Public Overrides Function GetBuffer() As Byte()
            Throw New UnauthorizedAccessException()
            Return Nothing
        End Function

        Public Overrides Function Read(ByVal buffer() As Byte, ByVal offset As Int32, ByVal count As Int32) As Int32
            Return o.Read(buffer, offset, count)
        End Function

        Public Overrides Function ReadByte() As Int32
            Return o.ReadByte()
        End Function

        Public Overrides Function Seek(ByVal offset As Int64, ByVal loc As SeekOrigin) As Int64
            Return o.Seek(offset, loc)
        End Function

        Public Overrides Sub SetLength(ByVal value As Int64)
            o.SetLength(value)
        End Sub

        Public Overrides Function ToArray() As Byte()
            assert(o.Length() <= max_int32)
            Dim m As New MemoryStream(CInt(o.Length()))
            WriteTo(m)
            Return m.ToArray()
        End Function

        Public Overrides Sub Write(ByVal buffer() As Byte, ByVal offset As Int32, ByVal count As Int32)
            o.Write(buffer, offset, count)
        End Sub

        Public Overrides Sub WriteByte(ByVal value As Byte)
            o.WriteByte(value)
        End Sub

        Public Overrides Sub WriteTo(ByVal stream As Stream)
            Dim p As Int64 = Position()
            o.Position() = 0
            o.CopyTo(stream)
            o.Position() = p
        End Sub
    End Class
End Class
