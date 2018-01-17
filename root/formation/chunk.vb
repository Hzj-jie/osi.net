
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

' A container of byte() with more serious restrict of the boundaries to build safer serialized data for transportation.
Public NotInheritable Class chunk
    Implements ICloneable(Of chunk), ICloneable, IComparable(Of chunk), IComparable

    Private Shared ReadOnly checksum As UInt32
    Private ReadOnly v As vector(Of Byte())

    Shared Sub New()
        assert(bytes_serializer.from_bytes(Text.Encoding.Unicode().GetBytes("HH"), checksum))
    End Sub

    Public Shared Function [New](ByVal b() As Byte, ByRef o As chunk) As Boolean
        o = New chunk()
        Return o.import(b)
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

    ' Return false only when ms is nothing or not writable / large enough.
    Public Function export(ByVal ms As MemoryStream) As Boolean
        If ms Is Nothing Then
            Return False
        End If

        Dim i As UInt32 = 0
        While i < v.size()
            bytes_serializer.append_to(array_size(v(i)), ms)
            bytes_serializer.append_to(array_size(v(i)) Xor checksum, ms)
            If Not v(i) Is Nothing Then
                If Not ms.write(v(i)) Then
                    Return False
                End If
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
        Using ms As MemoryStream = memory_stream.create(b)
            Return import(ms)
        End Using
    End Function

    ' Return false if ms is nothing or not readable / large enough or the data is malformatted.
    Public Function import(ByVal ms As MemoryStream) As Boolean
        While Not ms.eos()
            Dim l As UInt32 = 0
            Dim c As UInt32 = 0
            If Not bytes_serializer.consume_from(ms, l) OrElse
               Not bytes_serializer.consume_from(ms, c) Then
                Return False
            End If
            If c <> (l Xor checksum) Then
                Return False
            End If

            Dim b() As Byte = Nothing
            If l > 0 Then
                ReDim b(CInt(l - uint32_1))
                If Not ms.read(b) Then
                    Return False
                End If
            End If

            v.emplace_back(b)
        End While

        Return True
    End Function

    Public Function CloneT() As chunk Implements ICloneable(Of chunk).Clone
        Return New chunk(copy_no_error(v))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CompareTo(ByVal other As chunk) As Int32 Implements IComparable(Of chunk).CompareTo
        Dim cmp As Int32 = 0
        cmp = object_compare(Me, other)
        If cmp <> object_compare_undetermined Then
            Return cmp
        End If
        Return compare(v, other.v)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of chunk)(obj))
    End Function
End Class
