
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with positive_size_t_64.vbp ----------
'so change positive_size_t_64.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with positive_npos_uint64.vbp ----------
'so change positive_npos_uint64.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint64.vbp ----------
'so change npos_uint64.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint.vbp ----------
'so change npos_uint.vbp instead of this file



Imports System.IO
Imports osi.root.constants
Imports constants = osi.root.constants

Partial Public Structure positive_size_t_64
    Implements IComparable, IComparable(Of positive_size_t_64), ICloneable, ICloneable(Of positive_size_t_64)

    Public Shared ReadOnly inf As positive_size_t_64 =
        Function() As positive_size_t_64
            Dim inf As positive_size_t_64 = New positive_size_t_64(constants.npos)
            assert(inf.npos())
            Return inf
        End Function()
    Public Shared ReadOnly zero As positive_size_t_64 =
        Function() As positive_size_t_64
            Dim zero As positive_size_t_64 = New positive_size_t_64(uint64_0)
            If False Then
                assert(Not zero.npos())
            Else
                assert(zero.npos())
            End If
            Return zero
        End Function()
    Public Shared ReadOnly sizeof_value As UInt32 = CUInt(sizeof(Of UInt64)())

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline")>
    Shared Sub New()
        assert(constants.npos < 0)
        Dim x As positive_size_t_64 = Nothing
        assert(Not x.npos())
        assert(x.raw_value() = uint64_0)

        bytes_serializer.fixed.register(
                Function(ByVal i As positive_size_t_64, ByVal o As MemoryStream) As Boolean
                    #If Not False Then
                        If i.npos() Then
                            Return bytes_serializer.append_to(uint64_0, o)
                        End If
                        Return bytes_serializer.append_to(i.raw_value(), o)
                    #ElseIf Not True Then
                        If i.npos() Then
                            Return bytes_serializer.append_to(max_uint64, o)
                        End If
                        Return bytes_serializer.append_to(i.raw_value(), o)
                    #Else
                        If i.npos() Then
                            Return bytes_serializer.append_to(True, o)
                        End If
                        Return bytes_serializer.append_to(False, o) AndAlso
                               bytes_serializer.append_to(i.raw_value(), o)
                    #End If
                End Function,
                Function(ByVal i As MemoryStream, ByRef o As positive_size_t_64) As Boolean
                    #If Not False OrElse Not True Then
                        Dim u As UInt64 = uint64_0
                        If Not bytes_serializer.consume_from(i, u) Then
                            Return False
                        End If
                        o = New positive_size_t_64(u)
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
                        Dim u As UInt64 = uint64_0
                        If Not bytes_serializer.consume_from(i, u) Then
                            Return False
                        End If
                        o = New positive_size_t_64(u)
                        Return True
                    #End If
                End Function)
    End Sub

    Private ReadOnly i As UInt64
    Private ReadOnly n As Boolean

#If "Int64" <> "SByte" Then

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
#If "SByte" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "SByte" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As SByte) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As Byte) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int64" <> "Int16" AndAlso "Int64" <> "Short" Then

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
#If "Int16" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "Int16" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int16) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt16) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int64" <> "Int32" AndAlso "Int64" <> "Integer" Then

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
#If "Int32" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "Int32" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int32) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt32) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int64" <> "Int64" AndAlso "Int64" <> "Long" Then

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
#If "Int64" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "Int64" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int64) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt64) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If

#If "Int64" <> "Decimal" Then

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
#If "Decimal" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "Decimal" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Decimal) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If False Then
    Public Shared Widening Operator CType(ByVal this As UDecimal) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------
#End If


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
#If "Int64" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If

        If i < 0 Then
            n = True
        Else
            Me.i = CULng(i)
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
#If "Int64" <> "Int64" Then
        If i > max_uint64 Then
            n = True
            Return
        End If
#End If
#If Not True Then
        If i = max_uint64 Then
            n = True
            Return
        End If
#End If
        Me.i = CULng(i)
    End Sub
#End If

    Public Shared Widening Operator CType(ByVal this As Int64) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator

#If True Then
    Public Shared Widening Operator CType(ByVal this As UInt64) As positive_size_t_64
        Return New positive_size_t_64(this)
    End Operator
#End If

'finish npos_uint_ctor.vbp --------

    Public Sub New(ByVal that As positive_size_t_64)
        i = that.i
        n = that.n
    End Sub

#If True Then
    Public Function value() As UInt64
#If Not DEBUG Then
        assert(Not npos())
#End If
        Return raw_value()
    End Function
#End If

    Private Function raw_value() As UInt64
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

    Public Shared Operator =(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        If this.npos() = that.npos() Then
            If this.npos() Then
                Return True
            End If
            Return this.raw_value() = that.raw_value()
        End If
        Return False
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        If that < 0 AndAlso this.npos() Then
            Return True
        End If
        Return this.raw_value() = CULng(that)
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that <> this
    End Operator

    Public Shared Operator =(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        If this.npos() Then
            Return False
        End If
        Return this.raw_value() = that
    End Operator

    Public Shared Operator <>(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that <> this
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        If this.npos() Then
            Return False
        End If
        If that.npos() Then
            Return True
        End If
        Return this.raw_value() < that.raw_value()
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        If that.npos() Then
            Return False
        End If
        If this.npos() Then
            Return True
        End If
        Return this.raw_value() > that.raw_value()
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        If that <= 0 OrElse
           this.npos() OrElse
           this.raw_value() >= max_int32 Then
            Return False
        End If
        Return this.raw_value() < CULng(that)
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        If that < 0 Then
            Return False
        End If
        If this.npos() OrElse this.raw_value() > max_int32 Then
            Return True
        End If
        Return this.raw_value() > CULng(that)
    End Operator

    Public Shared Operator <(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that < this
    End Operator

    Public Shared Operator <(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        If this.npos() Then
            Return False
        End If
        Return this.raw_value() < that
    End Operator

    Public Shared Operator >(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        If this.npos() Then
            Return True
        End If
        Return this.raw_value() > that
    End Operator

    Public Shared Operator <(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that < this
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        If that.npos() Then
            Return True
        End If
        If this.npos() Then
            Return False
        End If
        Return this.raw_value() <= that.raw_value()
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_64, ByVal that As positive_size_t_64) As Boolean
        If this.npos() Then
            Return True
        End If
        If that.npos() Then
            Return False
        End If
        Return this.raw_value() >= that.raw_value()
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        If that < 0 OrElse
           this.npos() OrElse
           this.raw_value() > max_int32 Then
            Return False
        End If
        Return this.raw_value() <= CULng(that)
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_64, ByVal that As Int64) As Boolean
        If this.npos() Then
            Return True
        End If
        If that < 0 Then
            Return False
        End If
        If this.raw_value() >= max_int32 Then
            Return True
        End If
        Return this.raw_value() >= CULng(that)
    End Operator

    Public Shared Operator <=(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that >= this
    End Operator

    Public Shared Operator >=(ByVal this As Int64, ByVal that As positive_size_t_64) As Boolean
        Return that <= this
    End Operator

    Public Shared Operator <=(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        If this.npos() Then
            Return False
        End If
        Return this.raw_value() <= that
    End Operator

    Public Shared Operator >=(ByVal this As positive_size_t_64, ByVal that As UInt64) As Boolean
        If this.npos() Then
            Return True
        End If
        Return this.raw_value() >= that
    End Operator

    Public Shared Operator <=(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that >= this
    End Operator

    Public Shared Operator >=(ByVal this As UInt64, ByVal that As positive_size_t_64) As Boolean
        Return that <= this
    End Operator

    Public Shared Widening Operator CType(ByVal this As positive_size_t_64) As Int64
        If this.npos() Then
            Return constants.npos
        End If
        assert(this.raw_value() <= max_int32)
        Return CInt(this.raw_value())
    End Operator

    Public Shared Widening Operator CType(ByVal this As positive_size_t_64) As UInt64
        assert(Not this.npos())
        Return this.raw_value()
    End Operator

#If True Then
    Public Shared Operator +(Byval this As positive_size_t_64) As UInt64
        Return this.raw_value()
    End Operator    
#End If

    Public Overrides Function ToString() As String
        If npos() Then
            Return Convert.ToString(constants.npos)
        End If
        Return Convert.ToString(raw_value())
    End Function

    Public Function CompareTo(ByVal that As positive_size_t_64) As Int32 Implements IComparable(Of positive_size_t_64).CompareTo
        If npos() = that.npos() Then
            Return 0
        End If
        If npos() Then
            Return 1
        End If
        If that.npos() Then
            Return -1
        End If
        Return raw_value().CompareTo(that.raw_value())
    End Function

    Public Function CompareTo(ByVal that As Object) As Int32 Implements IComparable.CompareTo
        Dim other As positive_size_t_64 = Nothing
        If cast(Of positive_size_t_64)(that, other) Then
            Return CompareTo(other)
        End If
        Return CompareTo(zero)
    End Function

    Public Function CloneT() As positive_size_t_64 Implements ICloneable(Of positive_size_t_64).Clone
        Return New positive_size_t_64(Me)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function
End Structure
'finish npos_uint.vbp --------
'finish npos_uint64.vbp --------
'finish positive_npos_uint64.vbp --------
'finish positive_size_t_64.vbp --------
