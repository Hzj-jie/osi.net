
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class chunk
    Public Shared ReadOnly head_size As UInt32 = (sizeof_uint32 << 1)
    Private Shared ReadOnly checksum As UInt32 = calculate_checksum()

    Private Shared Function calculate_checksum() As UInt32
        Dim checksum As UInt32
        assert(bytes_serializer.from_bytes(Text.Encoding.Unicode().GetBytes("HH"), checksum))
        Return checksum
    End Function

    Public Shared Function append_to(ByVal v() As Byte, ByVal ms As MemoryStream) As Boolean
        If Not bytes_serializer.append_to(array_size(v), ms) OrElse
           Not bytes_serializer.append_to(array_size(v) Xor checksum, ms) Then
            Return False
        End If

        If Not v Is Nothing AndAlso Not ms.write(v) Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function append_to(ByVal v As piece, ByVal ms As MemoryStream) As Boolean
        If Not bytes_serializer.append_to(v.size(), ms) OrElse
           Not bytes_serializer.append_to(v.size() Xor checksum, ms) Then
            Return False
        End If

        If v Is Nothing Then
            Return True
        End If
        Return ms.try_write(v.buff, v.offset, v.count)
    End Function

    Public Shared Function from_bytes(ByVal b() As Byte) As Byte()
        Return from_bytes(New piece(b))
    End Function

    Public Shared Function from_bytes(ByVal b() As Byte, ByRef o() As Byte) As Boolean
        Return from_bytes(New piece(b), o)
    End Function

    Public Shared Function from_bytes(ByVal v As piece) As Byte()
        Dim o() As Byte = Nothing
        assert(from_bytes(v, o))
        Return o
    End Function

    Public Shared Function from_bytes(ByVal v As piece, ByRef o() As Byte) As Boolean
        Using r As MemoryStream = New MemoryStream()
            If Not append_to(v, r) Then
                Return False
            End If
            o = r.ToArray()
            Return True
        End Using
    End Function

    Public Shared Function consume_from(ByVal ms As MemoryStream, ByRef o() As Byte) As Boolean
        Dim b() As Byte = Nothing
        Dim offset As UInt32 = 0
        ms.get_buffer(b, offset)
        If Not parse_head(b, offset, o) Then
            Return False
        End If
        ms.Position() = offset
        If o Is Nothing Then
            Return True
        End If
        Return ms.read(o)
    End Function

    Public Shared Function head() As Byte()
        Dim r() As Byte = Nothing
        ReDim r(CInt(head_size) - 1)
        Return r
    End Function

    Public Shared Function parse_head(ByVal head() As Byte, ByRef o() As Byte) As Boolean
        Return parse_head(head, 0, o)
    End Function

    Private Shared Function parse_head(ByVal buff() As Byte, ByRef offset As UInt32, ByRef o() As Byte) As Boolean
        Dim l As UInt32 = 0
        Dim c As UInt32 = 0
        If Not bytes_serializer.consume_from(buff, offset, l) OrElse
           Not bytes_serializer.consume_from(buff, offset, c) Then
            Return False
        End If
        If c <> (l Xor checksum) Then
            Return False
        End If

        If l > 0 Then
            ReDim o(CInt(l - uint32_1))
        Else
            o = Nothing
        End If
        Return True
    End Function

    Private Sub New()
    End Sub
End Class

' A container of byte() with more serious restrict of the boundaries to build safer serialized data for transportation.
Public NotInheritable Class chunks
    Implements ICloneable(Of chunks), ICloneable, IComparable(Of chunks), IComparable

    Private ReadOnly v As vector(Of Byte())

    Public Shared Function [New](ByVal b() As Byte, ByRef o As chunks) As Boolean
        o = New chunks()
        Return o.import(b)
    End Function

    Public Shared Function parse(ByVal b() As Byte, ByRef o As vector(Of Byte())) As Boolean
        Dim c As chunks = Nothing
        If Not [New](b, c) Then
            Return False
        End If
        o = c.raw_data()
        Return True
    End Function

    Public Shared Function parse_or_null(ByVal b() As Byte) As vector(Of Byte())
        Dim r As vector(Of Byte()) = Nothing
        If parse(b, r) Then
            Return r
        End If
        Return Nothing
    End Function

    Public Shared Function contains(ByVal b() As Byte, ByVal p() As Byte) As Boolean
        Dim c As chunks = Nothing
        If Not [New](b, c) Then
            Return False
        End If
        Return c.find(p) <> npos
    End Function

    Public Sub New(ByVal b() As Byte)
        Me.New()
        assert(import(b))
    End Sub

    Public Sub New()
        Me.New(New vector(Of Byte())())
    End Sub

    <copy_constructor>
    Protected Sub New(ByVal v As vector(Of Byte()))
        assert(Not v Is Nothing)
        Me.v = v
    End Sub

    Public Sub insert(Of T)(ByVal i As T)
        v.emplace_back(bytes_serializer.to_bytes(i))
    End Sub

    ' Avoid copying.
    Public Sub emplace(ByVal b() As Byte)
        v.emplace_back(b)
    End Sub

    Public Function size() As UInt32
        Return v.size()
    End Function

    Public Function read(Of T)(ByVal i As UInt32, ByRef o As T) As Boolean
        Dim b() As Byte = Nothing
        Return read(i, b) AndAlso
               bytes_serializer.from_bytes(b, o)
    End Function

    Public Function read(Of T)(ByVal i As UInt32) As T
        Dim r As T = Nothing
        assert(read(i, r))
        Return r
    End Function

    Public Function read(ByVal i As UInt32, ByRef o() As Byte) As Boolean
        If i >= v.size() Then
            Return False
        End If

        o = v(i)
        Return True
    End Function

    Public Function read(ByVal i As UInt32) As Byte()
        Dim r() As Byte = Nothing
        assert(read(i, r))
        Return r
    End Function

    Public Function find(ByVal i() As Byte) As Int32
        Return v.find(i)
    End Function

    ' Return false only when ms is nothing or not writable / large enough.
    Public Function export(ByVal ms As MemoryStream) As Boolean
        If ms Is Nothing Then
            Return False
        End If

        Dim i As UInt32 = 0
        While i < v.size()
            If Not chunk.append_to(v(i), ms) Then
                Return False
            End If
            i += uint32_1
        End While
        Return True
    End Function

    Public Function export() As Byte()
        Using ms As MemoryStream = New MemoryStream()
            assert(export(ms))
            Return ms.ToArray()
        End Using
    End Function

    Public Function import(ByVal b() As Byte) As Boolean
        If b Is Nothing Then
            Return False
        End If
        Using ms As MemoryStream = memory_stream.of(b)
            Return import(ms)
        End Using
    End Function

    ' Return false if ms is nothing or not readable / large enough or the data is malformatted.
    Public Function import(ByVal ms As MemoryStream) As Boolean
        While Not ms.eos()
            Dim b() As Byte = Nothing
            If Not chunk.consume_from(ms, b) Then
                Return False
            End If
            v.emplace_back(b)
        End While

        Return True
    End Function

    Public Function raw_data() As vector(Of Byte())
        Return v
    End Function

    Public Function CloneT() As chunks Implements ICloneable(Of chunks).Clone
        Return New chunks(copy_no_error(v))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CompareTo(ByVal other As chunks) As Int32 Implements IComparable(Of chunks).CompareTo
        Dim cmp As Int32 = 0
        cmp = object_compare(Me, other)
        If cmp <> object_compare_undetermined Then
            Return cmp
        End If
        Return compare(v, other.v)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of chunks)(obj))
    End Function
End Class
