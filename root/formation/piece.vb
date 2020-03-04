
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

<global_init(global_init_level.functor)>
Public Module _piece
    <Extension()> Public Function null_or_empty(ByVal this As piece) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function

    <Extension()> Public Function export_or_null(ByVal this As piece) As Byte()
        Return If(this Is Nothing, Nothing, this.export())
    End Function

    <Extension()> Public Function size(ByVal this As piece) As UInt32
        Return If(this Is Nothing, uint32_0, this.count)
    End Function

    <Extension()> Public Function from_piece(Of T)(ByVal this As bytes_serializer(Of T),
                                                   ByVal i As piece,
                                                   ByRef o As T) As Boolean
        assert(Not this Is Nothing)
        If i Is Nothing OrElse i.buff Is Nothing Then
            Return False
        End If
        Using ms As MemoryStream = memory_stream.of(i.buff, CInt(i.offset), CInt(i.count))
            Return this.read_from(ms, o)
        End Using
    End Function

    <Extension()> Public Function from_bytes(Of T)(ByVal this As bytes_serializer(Of T),
                                                   ByVal i As piece,
                                                   ByRef o As T) As Boolean
        Return from_piece(this, i, o)
    End Function

    <Extension()> Public Function to_piece(Of T)(ByVal this As bytes_serializer(Of T), ByVal i As T) As piece
        assert(Not this Is Nothing)
        Return New piece(this.to_bytes(i))
    End Function

    Sub New()
        ' Do not convert from byte() to avoid performance impacts.
        bytes_serializer.byte_size.register(AddressOf size,
                                            Function(ByVal i As piece, ByVal o As MemoryStream) As Boolean
                                                If i.size() > 0 Then
                                                    o.Write(i.buff, CInt(i.offset), CInt(i.count))
                                                End If
                                                Return True
                                            End Function,
                                            Function(ByVal l As UInt32,
                                                     ByVal i As MemoryStream,
                                                     ByRef o As piece) As Boolean
                                                assert(Not i Is Nothing)
                                                If piece.create(i, l, o) Then
                                                    i.Seek(l, SeekOrigin.Current)
                                                    Return True
                                                End If
                                                Return False
                                            End Function)
    End Sub

    Private Sub init()
    End Sub
End Module

Public NotInheritable Class piece
    Implements ICloneable, ICloneable(Of piece), IComparable, IComparable(Of piece), IComparable(Of Byte())

    Public Shared ReadOnly null As piece
    Public Shared ReadOnly blank As piece
    Public ReadOnly buff() As Byte
    Public ReadOnly offset As UInt32
    Public ReadOnly count As UInt32

    Public Shared Function valid(ByVal buff() As Byte,
                                 ByVal offset As UInt32,
                                 ByVal count As UInt32) As Boolean
        Return array_size(buff) >= offset + count AndAlso offset <= max_int32 AndAlso count <= max_int32
    End Function

    Shared Sub New()
        null = Nothing
        blank = New piece(Nothing)
        assert(blank.empty())
    End Sub

    Private Sub assert_valid()
        assert(valid(buff, offset, count))
    End Sub

    Public Sub New(ByVal buff() As Byte,
                   ByVal offset As UInt32,
                   ByVal count As UInt32)
        Me.buff = buff
        Me.offset = offset
        Me.count = count
        assert_valid()
    End Sub

    Public Sub New(ByVal buff() As Byte,
                   ByVal count As UInt32)
        Me.New(buff, uint32_0, count)
    End Sub

    Public Sub New(ByVal buff() As Byte)
        Me.New(buff, array_size(buff))
    End Sub

    Public Function empty() As Boolean
        Return count = uint32_0
    End Function

    ' Consume at most count bytes, and copy the result into buff, from offset. o is the instance after consuming,
    ' consumed is the total count of bytes written into buff, which equals to Me.count - o.count.
    Public Function consume(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByRef o As piece,
                            Optional ByRef consumed As UInt32 = uint32_0) As Boolean
        If array_size(buff) < offset + count Then
            Return False
        End If
        consumed = min(Me.count, count)
        If consumed > uint32_0 Then
            memcpy(buff, offset, Me.buff, Me.offset, consumed)
        End If
        Return assert(consume(consumed, o))
    End Function

    Public Function consume(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            Optional ByRef consumed As UInt32 = uint32_0) As piece
        Dim o As piece = Nothing
        assert(consume(buff, offset, count, o, consumed))
        Return o
    End Function

    Public Function consume(ByVal buff() As Byte,
                            ByVal count As UInt32,
                            ByRef o As piece,
                            Optional ByRef consumed As UInt32 = uint32_0) As Boolean
        Return consume(buff, uint32_0, count, o, consumed)
    End Function

    Public Function consume(ByVal buff() As Byte,
                            ByRef o As piece,
                            Optional ByRef consumed As UInt32 = uint32_0) As Boolean
        Return consume(buff, array_size(buff), o, consumed)
    End Function

    Public Function consume(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        End If
        If size = uint32_0 Then
            o = Me
            Return True
        End If
        o = New piece(buff, offset + size, count - size)
        Return True
    End Function

    Public Function consume(ByVal size As UInt32) As piece
        Dim r As piece = Nothing
        assert(consume(size, r))
        Return r
    End Function

    Public Function keep(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        End If
        If size = count Then
            o = Me
            Return True
        End If
        o = New piece(buff, offset, size)
        Return True
    End Function

    Public Function keep(ByVal size As UInt32) As piece
        Dim o As piece = Nothing
        assert(keep(size, o))
        Return o
    End Function

    Public Function consume_back(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        End If
        If size = uint32_0 Then
            o = Me
            Return True
        End If
        o = New piece(buff, offset, count - size)
        Return True
    End Function

    Public Function consume_back(ByVal size As UInt32) As piece
        Dim r As piece = Nothing
        assert(consume_back(size, r))
        Return r
    End Function

    Public Function keep_back(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        End If
        If size = count Then
            o = Me
            Return True
        End If
        o = New piece(buff, offset + count - size, size)
        Return True
    End Function

    Public Function keep_back(ByVal size As UInt32) As piece
        Dim o As piece = Nothing
        assert(keep_back(size, o))
        Return o
    End Function

    ' Exports to a byte array, there is no guarantee bytes arrary returned is a new instance.
    Public Function export() As Byte()
        If offset = uint32_0 AndAlso count = array_size(buff) Then
            Return buff
        End If
        Dim r() As Byte = Nothing
        If count = uint32_0 Then
            ReDim r(npos)
            Return r
        End If
        ReDim r(CInt(count - uint32_1))
        memcpy(r, uint32_0, buff, offset, count)
        Return r
    End Function

    ' Exports to a byte array, the array size may be larger than buffer size. There is no guarantee bytes array returned
    ' is a new instance.
    Public Function export(ByRef count As UInt32) As Byte()
        If offset = uint32_0 Then
            count = Me.count
            Return Me.buff
        End If
        Dim r() As Byte = Nothing
        If Me.count = uint32_0 Then
            ReDim r(npos)
            count = uint32_0
            Return r
        End If
        ReDim r(CInt(Me.count - uint32_1))
        memcpy(r, uint32_0, buff, offset, Me.count)
        count = Me.count
        Return r
    End Function

    Public Shared Function create(ByVal buff() As Byte,
                                  ByVal offset As UInt32,
                                  ByVal count As UInt32,
                                  ByRef o As piece) As Boolean
        If valid(buff, offset, count) Then
            o = New piece(buff, offset, count)
            Return True
        End If
        Return False
    End Function

    Public Shared Function create(ByVal buff() As Byte,
                                  ByVal count As UInt32,
                                  ByRef o As piece) As Boolean
        Return create(buff, uint32_0, count, o)
    End Function

    Public Shared Function create(ByVal buff() As Byte,
                                  ByRef o As piece) As Boolean
        Return create(buff, array_size(buff), o)
    End Function

    Public Shared Function create(ByRef o As piece) As Boolean
        Return create([default](Of Byte()).null, o)
    End Function

    Public Shared Function create(ByVal ms As MemoryStream, ByRef o As piece) As Boolean
        If ms Is Nothing Then
            Return False
        End If
        Dim b() As Byte = Nothing
        Dim offset As UInt32 = 0
        Dim len As UInt32 = 0
        ms.get_buffer(b, offset, len)
        assert(len >= offset)
        Return assert(create(b, offset, len - offset, o))
    End Function

    Public Shared Function create(ByVal ms As MemoryStream, ByVal count As UInt32, ByRef o As piece) As Boolean
        If ms Is Nothing Then
            Return False
        End If
        Dim b() As Byte = Nothing
        Dim offset As UInt32 = 0
        ms.get_buffer(b, offset)
        Return create(b, offset, count, o)
    End Function

    Public Function CloneT() As piece Implements ICloneable(Of piece).Clone
        Return New piece(buff, offset, count)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shared Function compare(ByVal this As piece, ByVal that As piece) As Int32
        If this Is Nothing AndAlso that Is Nothing Then
            Return 0
        End If
        If this Is Nothing Then
            Return -1
        End If
        If that Is Nothing Then
            Return 1
        End If
        If this.count <> that.count Then
            Return this.count.CompareTo(that.count)
        End If
        Return memcmp(this.buff, this.offset, that.buff, that.offset, this.count)
    End Function

    Public Function compare(ByVal that As piece) As Int32
        Return compare(Me, that)
    End Function

    Public Function compare(ByVal that() As Byte, ByVal offset As UInt32, ByVal count As UInt32) As Int32
        Dim p As piece = Nothing
        If create(that, offset, count, p) Then
            Return compare(p)
        End If
        Return 1
    End Function

    Public Function compare(ByVal that() As Byte, ByVal count As UInt32) As Int32
        Return compare(that, uint32_0, count)
    End Function

    Public Function compare(ByVal that() As Byte) As Int32
        Return compare(that, array_size(that))
    End Function

    Public Function start_with(ByVal that As piece) As Boolean
        Return Not that Is Nothing AndAlso
               count >= that.count AndAlso
               memcmp(buff, offset, that.buff, that.offset, that.count) = 0
    End Function

    Public Function start_with(ByVal that() As Byte, ByVal offset As UInt32, ByVal count As UInt32) As Boolean
        Dim p As piece = Nothing
        Return create(that, offset, count, p) AndAlso start_with(p)
    End Function

    Public Function start_with(ByVal that() As Byte, ByVal count As UInt32) As Boolean
        Return start_with(that, uint32_0, count)
    End Function

    Public Function start_with(ByVal that() As Byte) As Boolean
        Return start_with(that, array_size(that))
    End Function

    Public Function end_with(ByVal that As piece) As Boolean
        Return Not that Is Nothing AndAlso
               count >= that.count AndAlso
               memcmp(buff, offset + count - that.count, that.buff, that.offset, that.count) = 0
    End Function

    Public Function end_with(ByVal that() As Byte, ByVal offset As UInt32, ByVal count As UInt32) As Boolean
        Dim p As piece = Nothing
        Return create(that, offset, count, p) AndAlso end_with(p)
    End Function

    Public Function end_with(ByVal that() As Byte, ByVal count As UInt32) As Boolean
        Return end_with(that, uint32_0, count)
    End Function

    Public Function end_with(ByVal that() As Byte) As Boolean
        Return end_with(that, array_size(that))
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim p As piece = Nothing
        If cast(obj, p) Then
            Return CompareTo(p)
        End If
        Dim b() As Byte = Nothing
        If cast(obj, b) Then
            Return CompareTo(b)
        End If
        Return compare(Me, Nothing)
    End Function

    Public Function CompareTo(ByVal other() As Byte) As Int32 Implements IComparable(Of Byte()).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As piece) As Int32 Implements IComparable(Of piece).CompareTo
        Return compare(other)
    End Function

    Public Shared Operator +(ByVal this As piece) As Byte()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.export()
    End Operator

    Public Shared Widening Operator CType(ByVal b() As Byte) As piece
        Return New piece(b)
    End Operator

    Public Shared Narrowing Operator CType(ByVal i As piece) As Byte()
        Return +i
    End Operator
End Class
