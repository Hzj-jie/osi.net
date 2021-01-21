
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with positive_size_t_32.vbp ----------
'so change positive_size_t_32.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with positive_npos_uint32.vbp ----------
'so change positive_npos_uint32.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with positive_npos_uint.vbp ----------
'so change positive_npos_uint.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint.vbp ----------
'so change npos_uint.vbp instead of this file



Imports System.IO
Imports osi.root.constants
Imports constants = osi.root.constants

Partial Public Structure positive_size_t_32
    Implements IComparable, IComparable(Of positive_size_t_32), ICloneable, ICloneable(Of positive_size_t_32)

    Public Shared ReadOnly inf As positive_size_t_32 = calculate_inf()
    Public Shared ReadOnly zero As positive_size_t_32 = calculate_zero()
    Public Shared ReadOnly sizeof_value As UInt32 = CUInt(sizeof(Of UInt32)())

    Private Shared Function calculate_inf() As positive_size_t_32
        Dim inf As positive_size_t_32 = New positive_size_t_32(constants.npos)
        assert(inf.npos())
        Return inf
    End Function

    Private Shared Function calculate_zero() As positive_size_t_32
        Dim zero As positive_size_t_32 = New positive_size_t_32(uint32_0)
        If False Then
            assert(Not zero.npos())
        Else
            assert(zero.npos())
        End If
        Return zero
    End Function

    Shared Sub New()
        assert(constants.npos < 0)
        Dim x As positive_size_t_32 = Nothing
        assert(Not x.npos())
        assert(x.raw_value() = uint32_0)

        bytes_serializer.fixed.register(
                Function(ByVal i As positive_size_t_32, ByVal o As MemoryStream) As Boolean
                    #If Not False Then
                        If i.npos() Then
                            Return bytes_serializer.append_to(uint32_0, o)
                        Else
                            Return bytes_serializer.append_to(i.raw_value(), o)
                        End If
                    #ElseIf Not True Then
                        If i.npos() Then
                            Return bytes_serializer.append_to(max_uint32, o)
                        Else
                            Return bytes_serializer.append_to(i.raw_value(), o)
                        End If
                    #Else
                        If i.npos() Then
                            Return bytes_serializer.append_to(True, o)
                        Else
                            Return bytes_serializer.append_to(False, o) AndAlso
                                   bytes_serializer.append_to(i.raw_value(), o)
                        End If
                    #End If
                End Function,
                Function(ByVal i As MemoryStream, ByRef o As positive_size_t_32) As Boolean
                    #If Not False OrElse Not True Then
                        Dim u As UInt32 = uint32_0
                        If Not bytes_serializer.consume_from(i, u) Then
                            Return False
                        End If
                        o = New positive_size_t_32(u)
                        Return True
                    #Else
                        Dim n As Boolean = False
                        If Not bytes_serializer.consume_from(i, n) Then
                            Return False
                        End If
                        If n Then
                            o = inf
                            Return True
                        End If
                        Dim u As UInt32 = uint32_0
                        If Not bytes_serializer.consume_from(i, u) Then
                            Return False
                        End If
                        o = New positive_size_t_32(u)
                        Return True
                    #End If
                End Function)
    End Sub

    Private ReadOnly i As UInt32
    Private ReadOnly n As Boolean

#If "Int32" <> "SByte" Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As SByte)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "SByte" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If True Then
    Public Sub New(ByVal i As Byte)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "SByte" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As SByte) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As Byte) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int32" <> "Int16" AndAlso "Int32" <> "Short" Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As Int16)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int16" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If True Then
    Public Sub New(ByVal i As UInt16)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int16" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int16) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt16) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int32" <> "Int32" AndAlso "Int32" <> "Integer" Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As Int32)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int32" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If True Then
    Public Sub New(ByVal i As UInt32)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int32" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int32) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt32) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int32" <> "Int64" AndAlso "Int32" <> "Long" Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As Int64)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int64" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If True Then
    Public Sub New(ByVal i As UInt64)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int64" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int64) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt64) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int32" <> "Decimal" Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As Decimal)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Decimal" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If False Then
    Public Sub New(ByVal i As UDecimal)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Decimal" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Decimal) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If False Then
    Public Shared Widening Operator CType(ByVal this As UDecimal) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_ctor.vbp ----------
'so change npos_uint_ctor.vbp instead of this file



    Public Sub New(ByVal i As Int32)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int32" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CUInt(i)
        End If
    End Sub

#If True Then
    Public Sub New(ByVal i As UInt32)
#If Not False Then
        If i = 0 Then
            n = True
            Return
        End If
#End If
#If "Int32" <> "Int32" Then
        If i > max_uint32 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint32 Then
            n = True
            Return
        End If
#End If
        Me.i = CUInt(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int32) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt32) As positive_size_t_32
        Return New positive_size_t_32(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------

    Public Sub New(ByVal that As positive_size_t_32)
        i = that.i
        n = that.n
    End Sub

#If True Then
    Public Function value() As UInt32
#If Not DEBUG Then
        assert(Not npos())
#End If
        Return raw_value()
    End Function
#End If

    Private Function raw_value() As UInt32
#If DEBUG Then
        assert(Not npos())
#End If
        Return i
    End Function

#If True Then
    Public Function npos() As Boolean
#Else
    Private Function npos() As Boolean
#End If
        Return n
    End Function

#If True Then
    Public Function infinite() As Boolean
        Return npos()
    End Function
#End If

    Public Shared Operator =(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        If this.npos() = that.npos() Then
            If this.npos() Then
                Return True
            Else
                Return this.raw_value() = that.raw_value()
            End If
        Else
            Return False
        End If
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        If that < 0 AndAlso this.npos() Then
            Return True
        Else
            Return this.raw_value() = CUInt(that)
        End If
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that <> this
    End Operator

    Public Shared Operator =(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        If this.npos() Then
            Return False
        Else
            Return this.raw_value() = that
        End If
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that <> this
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        If this.npos() Then
            Return False
        ElseIf that.npos() Then
            Return True
        Else
            Return this.raw_value() < that.raw_value()
        End If
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        If that.npos() Then
            Return False
        ElseIf this.npos() Then
            Return True
        Else
            Return this.raw_value() > that.raw_value()
        End If
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        If that <= 0 OrElse
           this.npos() OrElse
           this.raw_value() >= max_int32 Then
            Return False
        Else
            Return this.raw_value() < CUInt(that)
        End If
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        If that < 0 Then
            Return False
        ElseIf this.npos() OrElse this.raw_value() > max_int32 Then
            Return True
        Else
            Return this.raw_value() > CUInt(that)
        End If
    End Operator

    Public Shared Operator <(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that < this
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        If this.npos() Then
            Return False
        Else
            Return this.raw_value() < that
        End If
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        If this.npos() Then
            Return True
        Else
            Return this.raw_value() > that
        End If
    End Operator

    Public Shared Operator <(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that < this
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        If that.npos() Then
            Return True
        ElseIf this.npos() Then
            Return False
        Else
            Return this.raw_value() <= that.raw_value()
        End If
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_32, ByVal that As positive_size_t_32) As Boolean
        If this.npos() Then
            Return True
        ElseIf that.npos() Then
            Return False
        Else
            Return this.raw_value() >= that.raw_value()
        End If
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        If that < 0 OrElse
           this.npos() OrElse
           this.raw_value() > max_int32 Then
            Return False
        Else
            Return this.raw_value() <= CUInt(that)
        End If
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_32, ByVal that As Int32) As Boolean
        If this.npos() Then
            Return True
        ElseIf that < 0 Then
            Return False
        ElseIf this.raw_value() >= max_int32 Then
            Return True
        Else
            Return this.raw_value() >= CUInt(that)
        End If
    End Operator

    Public Shared Operator <=(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that >= this
    End Operator

    Public Shared Operator >=(ByVal this As Int32, ByVal that As positive_size_t_32) As Boolean
        Return that <= this
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        If this.npos() Then
            Return False
        Else
            Return this.raw_value() <= that
        End If
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_32, ByVal that As UInt32) As Boolean
        If this.npos() Then
            Return True
        Else
            Return this.raw_value() >= that
        End If
    End Operator

    Public Shared Operator <=(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that >= this
    End Operator

    Public Shared Operator >=(ByVal this As UInt32, ByVal that As positive_size_t_32) As Boolean
        Return that <= this
    End Operator

    Public Shared Widening Operator CType(ByVal this As positive_size_t_32) As Int32
        If this.npos() Then
            Return constants.npos
        Else
            assert(this.raw_value() <= max_int32)
            Return CInt(this.raw_value())
        End If
    End Operator

    Public Shared Widening Operator CType(ByVal this As positive_size_t_32) As UInt32
        assert(Not this.npos())
        Return this.raw_value()
    End Operator

#If True Then
    Public Shared Operator +(Byval this As positive_size_t_32) As UInt32
        Return this.raw_value()
    End Operator    
#End If

    Public Overrides Function ToString() As String
        If npos() Then
            Return Convert.ToString(constants.npos)
        Else
            Return Convert.ToString(raw_value())
        End If
    End Function

    Public Function CompareTo(ByVal that As positive_size_t_32) As Int32 Implements IComparable(Of positive_size_t_32).CompareTo
        If npos() = that.npos() Then
            Return 0
        ElseIf npos() Then
            Return 1
        ElseIf that.npos() Then
            Return -1
        Else
            Return raw_value().CompareTo(that.raw_value())
        End If
    End Function

    Public Function CompareTo(ByVal that As Object) As Int32 Implements IComparable.CompareTo
        Dim other As positive_size_t_32 = Nothing
        If cast(Of positive_size_t_32)(that, other) Then
            Return CompareTo(other)
        Else
            Return CompareTo(zero)
        End If
    End Function

    Public Function CloneT() As positive_size_t_32 Implements ICloneable(Of positive_size_t_32).Clone
        Return New positive_size_t_32(Me)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function
End Structure
'finish npos_uint.vbp --------
'finish positive_npos_uint.vbp --------
'finish positive_npos_uint32.vbp --------
'finish positive_size_t_32.vbp --------
