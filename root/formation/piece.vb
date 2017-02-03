
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

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
End Module

Public NotInheritable Class piece
    Implements ICloneable, IComparable, IComparable(Of piece), IComparable(Of Byte())

    Public Shared ReadOnly null As piece
    Public Shared ReadOnly blank As piece
    Public ReadOnly buff() As Byte
    Public ReadOnly offset As UInt32
    Public ReadOnly count As UInt32

    Public Shared Function valid(ByVal buff() As Byte,
                                 ByVal offset As UInt32,
                                 ByVal count As UInt32) As Boolean
        Return array_size(buff) >= offset + count
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
        Else
            consumed = min(Me.count, count)
            If consumed > uint32_0 Then
                memcpy(buff, offset, Me.buff, Me.offset, consumed)
            End If
            Return assert(consume(consumed, o))
        End If
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
        ElseIf size = uint32_0 Then
            o = Me
            Return True
        Else
            o = New piece(buff, offset + size, count - size)
            Return True
        End If
    End Function

    Public Function consume(ByVal size As UInt32) As piece
        Dim r As piece = Nothing
        assert(consume(size, r))
        Return r
    End Function

    Public Function keep(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        ElseIf size = count Then
            o = Me
            Return True
        Else
            o = New piece(buff, offset, size)
            Return True
        End If
    End Function

    Public Function keep(ByVal size As UInt32) As piece
        Dim o As piece = Nothing
        assert(keep(size, o))
        Return o
    End Function

    Public Function consume_back(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        ElseIf size = uint32_0 Then
            o = Me
            Return True
        Else
            o = New piece(buff, offset, count - size)
            Return True
        End If
    End Function

    Public Function consume_back(ByVal size As UInt32) As piece
        Dim r As piece = Nothing
        assert(consume_back(size, r))
        Return r
    End Function

    Public Function keep_back(ByVal size As UInt32, ByRef o As piece) As Boolean
        If size > count Then
            Return False
        ElseIf size = count Then
            o = Me
            Return True
        Else
            o = New piece(buff, offset + count - size, size)
            Return True
        End If
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
        ElseIf count = uint32_0 Then
            Dim r() As Byte = Nothing
            ReDim r(npos)
            Return r
        Else
            Dim r() As Byte = Nothing
            ReDim r(CInt(count - uint32_1))
            memcpy(r, uint32_0, buff, offset, count)
            Return r
        End If
    End Function

    ' Exports to a byte array, the array size may be larger than buffer size. There is no guarantee bytes array returned
    ' is a new instance.
    Public Function export(ByRef count As UInt32) As Byte()
        If offset = uint32_0 Then
            count = Me.count
            Return Me.buff
        ElseIf Me.count = uint32_0 Then
            Dim r() As Byte = Nothing
            ReDim r(npos)
            count = uint32_0
            Return r
        Else
            Dim r() As Byte = Nothing
            ReDim r(CInt(Me.count - uint32_1))
            memcpy(r, uint32_0, buff, offset, Me.count)
            count = Me.count
            Return r
        End If
    End Function

    Public Shared Function create(ByVal buff() As Byte,
                                  ByVal offset As UInt32,
                                  ByVal count As UInt32,
                                  ByRef o As piece) As Boolean
        If valid(buff, offset, count) Then
            o = New piece(buff, offset, count)
            Return True
        Else
            Return False
        End If
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
        Return create(Nothing, o)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New piece(buff, offset, count)
    End Function

    Public Shared Function compare(ByVal this As piece, ByVal that As piece) As Int32
        If this Is Nothing AndAlso that Is Nothing Then
            Return 0
        ElseIf this Is Nothing Then
            Return -1
        ElseIf that Is Nothing Then
            Return 1
        ElseIf this.count <> that.count Then
            Return this.count.CompareTo(that.count)
        Else
            Return memcmp(this.buff, this.offset, that.buff, that.offset, this.count)
        End If
    End Function

    Public Function compare(ByVal that As piece) As Int32
        Return compare(Me, that)
    End Function

    Public Function compare(ByVal that() As Byte, ByVal offset As UInt32, ByVal count As UInt32) As Int32
        Dim p As piece = Nothing
        If create(that, offset, count, p) Then
            Return compare(p)
        Else
            Return 1
        End If
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
        Else
            Dim b() As Byte = Nothing
            If cast(obj, b) Then
                Return CompareTo(b)
            Else
                Return compare(Me, Nothing)
            End If
        End If
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
        Else
            Return this.export()
        End If
    End Operator

    Public Shared Widening Operator CType(ByVal b() As Byte) As piece
        Return New piece(b)
    End Operator

    Public Shared Widening Operator CType(ByVal p As piece) As Byte()
        Return +p
    End Operator
End Class
