
'this file is generated by /osi/root/codegen/unchecked_int/unchecked_int.exe
'so change /osi/root/codegen/unchecked_int/unchecked_int.cs instead of this file

'why no checked / unchecked in vb.net

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Imports int8 = System.SByte
Imports int16 = System.Int16
Imports int32 = System.Int32
Imports int64 = System.Int64

Imports uint8 = System.Byte
Imports uint16 = System.UInt16
Imports uint32 = System.UInt32
Imports uint64 = System.UInt64

Imports int = System.Int32
Imports uint = System.UInt32

Public Module _unchecked
    Private Const int8_1 As int8 = 1
    <Extension()> Private Function inc(ByVal v As int8, ByVal i As int8) As int8
        assert(i > 0)
        Try
            If v > max_int8 - i Then
                'avoid overflow, same as following
                v = v - max_int8 + i + min_int8 - int8_1
            Else
                v += i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec(ByVal v As int8, ByVal i As int8) As int8
        assert(i > 0)
        Try
            If v < i + min_int8 Then
                v = v + max_int8 - i - min_int8 + int8_1
            Else
                v -= i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function inc_min_int8(ByVal v As int8) As int8
        Try
            If v > 0 Then
                v = v + min_int8
            ElseIf v < 0 Then
                v = v - min_int8
            Else
                v = min_int8
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec_min_int8(ByVal v As int8) As int8
        Try
            If v > 0 Then
                v = v - max_int8 - int8_1
            ElseIf v < 0 Then
                v = v + max_int8 + int8_1
            Else
                v = min_int8
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As int8, Optional ByVal that As int8 = int8_1) As int8
        If that = 0 Then
            Return this
        ElseIf that = min_int8 Then
            Return this.inc_min_int8()
        ElseIf that > 0 Then
            Return this.inc(that)
        Else
            assert(that < 0 AndAlso that <> min_int8)
            Return this.dec(-that)
        End If
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As int8, Optional ByVal that As int8 = int8_1) As int8
        If that = 0 Then
            Return this
        ElseIf that = min_int8 Then
            Return this.dec_min_int8()
        ElseIf that > 0 Then
            Return this.dec(that)
        Else
            assert(that < 0 AndAlso that <> min_int8)
            Return this.inc(-that)
        End If
    End Function

    Private Const int16_1 As int16 = 1
    <Extension()> Private Function inc(ByVal v As int16, ByVal i As int16) As int16
        assert(i > 0)
        Try
            If v > max_int16 - i Then
                'avoid overflow, same as following
                v = v - max_int16 + i + min_int16 - int16_1
            Else
                v += i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec(ByVal v As int16, ByVal i As int16) As int16
        assert(i > 0)
        Try
            If v < i + min_int16 Then
                v = v + max_int16 - i - min_int16 + int16_1
            Else
                v -= i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function inc_min_int16(ByVal v As int16) As int16
        Try
            If v > 0 Then
                v = v + min_int16
            ElseIf v < 0 Then
                v = v - min_int16
            Else
                v = min_int16
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec_min_int16(ByVal v As int16) As int16
        Try
            If v > 0 Then
                v = v - max_int16 - int16_1
            ElseIf v < 0 Then
                v = v + max_int16 + int16_1
            Else
                v = min_int16
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As int16, Optional ByVal that As int16 = int16_1) As int16
        If that = 0 Then
            Return this
        ElseIf that = min_int16 Then
            Return this.inc_min_int16()
        ElseIf that > 0 Then
            Return this.inc(that)
        Else
            assert(that < 0 AndAlso that <> min_int16)
            Return this.dec(-that)
        End If
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As int16, Optional ByVal that As int16 = int16_1) As int16
        If that = 0 Then
            Return this
        ElseIf that = min_int16 Then
            Return this.dec_min_int16()
        ElseIf that > 0 Then
            Return this.dec(that)
        Else
            assert(that < 0 AndAlso that <> min_int16)
            Return this.inc(-that)
        End If
    End Function

    Private Const int32_1 As int32 = 1
    <Extension()> Private Function inc(ByVal v As int32, ByVal i As int32) As int32
        assert(i > 0)
        Try
            If v > max_int32 - i Then
                'avoid overflow, same as following
                v = v - max_int32 + i + min_int32 - int32_1
            Else
                v += i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec(ByVal v As int32, ByVal i As int32) As int32
        assert(i > 0)
        Try
            If v < i + min_int32 Then
                v = v + max_int32 - i - min_int32 + int32_1
            Else
                v -= i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function inc_min_int32(ByVal v As int32) As int32
        Try
            If v > 0 Then
                v = v + min_int32
            ElseIf v < 0 Then
                v = v - min_int32
            Else
                v = min_int32
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec_min_int32(ByVal v As int32) As int32
        Try
            If v > 0 Then
                v = v - max_int32 - int32_1
            ElseIf v < 0 Then
                v = v + max_int32 + int32_1
            Else
                v = min_int32
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As int32, Optional ByVal that As int32 = int32_1) As int32
        If that = 0 Then
            Return this
        ElseIf that = min_int32 Then
            Return this.inc_min_int32()
        ElseIf that > 0 Then
            Return this.inc(that)
        Else
            assert(that < 0 AndAlso that <> min_int32)
            Return this.dec(-that)
        End If
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As int32, Optional ByVal that As int32 = int32_1) As int32
        If that = 0 Then
            Return this
        ElseIf that = min_int32 Then
            Return this.dec_min_int32()
        ElseIf that > 0 Then
            Return this.dec(that)
        Else
            assert(that < 0 AndAlso that <> min_int32)
            Return this.inc(-that)
        End If
    End Function

    Private Const int64_1 As int64 = 1
    <Extension()> Private Function inc(ByVal v As int64, ByVal i As int64) As int64
        assert(i > 0)
        Try
            If v > max_int64 - i Then
                'avoid overflow, same as following
                v = v - max_int64 + i + min_int64 - int64_1
            Else
                v += i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec(ByVal v As int64, ByVal i As int64) As int64
        assert(i > 0)
        Try
            If v < i + min_int64 Then
                v = v + max_int64 - i - min_int64 + int64_1
            Else
                v -= i
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function inc_min_int64(ByVal v As int64) As int64
        Try
            If v > 0 Then
                v = v + min_int64
            ElseIf v < 0 Then
                v = v - min_int64
            Else
                v = min_int64
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Private Function dec_min_int64(ByVal v As int64) As int64
        Try
            If v > 0 Then
                v = v - max_int64 - int64_1
            ElseIf v < 0 Then
                v = v + max_int64 + int64_1
            Else
                v = min_int64
            End If
            Return v
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As int64, Optional ByVal that As int64 = int64_1) As int64
        If that = 0 Then
            Return this
        ElseIf that = min_int64 Then
            Return this.inc_min_int64()
        ElseIf that > 0 Then
            Return this.inc(that)
        Else
            assert(that < 0 AndAlso that <> min_int64)
            Return this.dec(-that)
        End If
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As int64, Optional ByVal that As int64 = int64_1) As int64
        If that = 0 Then
            Return this
        ElseIf that = min_int64 Then
            Return this.dec_min_int64()
        ElseIf that > 0 Then
            Return this.dec(that)
        Else
            assert(that < 0 AndAlso that <> min_int64)
            Return this.inc(-that)
        End If
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As uint8, Optional ByVal that As uint8 = uint8_1) As uint8
        Return int8_uint8(unchecked_inc(uint8_int8(this), uint8_int8(that)))
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As uint8, Optional ByVal that As uint8 = uint8_1) As uint8
        Return int8_uint8(unchecked_dec(uint8_int8(this), uint8_int8(that)))
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As uint16, Optional ByVal that As uint16 = uint16_1) As uint16
        Return int16_uint16(unchecked_inc(uint16_int16(this), uint16_int16(that)))
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As uint16, Optional ByVal that As uint16 = uint16_1) As uint16
        Return int16_uint16(unchecked_dec(uint16_int16(this), uint16_int16(that)))
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As uint32, Optional ByVal that As uint32 = uint32_1) As uint32
        Return int32_uint32(unchecked_inc(uint32_int32(this), uint32_int32(that)))
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As uint32, Optional ByVal that As uint32 = uint32_1) As uint32
        Return int32_uint32(unchecked_dec(uint32_int32(this), uint32_int32(that)))
    End Function

    <Extension()> Public Function unchecked_inc(ByVal this As uint64, Optional ByVal that As uint64 = uint64_1) As uint64
        Return int64_uint64(unchecked_inc(uint64_int64(this), uint64_int64(that)))
    End Function

    <Extension()> Public Function unchecked_dec(ByVal this As uint64, Optional ByVal that As uint64 = uint64_1) As uint64
        Return int64_uint64(unchecked_dec(uint64_int64(this), uint64_int64(that)))
    End Function

    <Extension()> Public Function try_inc(ByVal this As int8, ByVal that As int8, ByRef o As int8) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As int8, ByVal that As int8, ByRef o As int8) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As int8, Optional ByVal that As int8 = int8_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As int8, Optional ByVal that As int8 = int8_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As int8, Optional ByVal that As int8 = int8_1) As int8
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As int8, Optional ByVal that As int8 = int8_1) As int8
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As int16, ByVal that As int16, ByRef o As int16) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As int16, ByVal that As int16, ByRef o As int16) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As int16, Optional ByVal that As int16 = int16_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As int16, Optional ByVal that As int16 = int16_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As int16, Optional ByVal that As int16 = int16_1) As int16
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As int16, Optional ByVal that As int16 = int16_1) As int16
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As int32, ByVal that As int32, ByRef o As int32) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As int32, ByVal that As int32, ByRef o As int32) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As int32, Optional ByVal that As int32 = int32_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As int32, Optional ByVal that As int32 = int32_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As int32, Optional ByVal that As int32 = int32_1) As int32
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As int32, Optional ByVal that As int32 = int32_1) As int32
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As int64, ByVal that As int64, ByRef o As int64) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As int64, ByVal that As int64, ByRef o As int64) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As int64, Optional ByVal that As int64 = int64_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As int64, Optional ByVal that As int64 = int64_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As int64, Optional ByVal that As int64 = int64_1) As int64
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As int64, Optional ByVal that As int64 = int64_1) As int64
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As uint8, ByVal that As uint8, ByRef o As uint8) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As uint8, ByVal that As uint8, ByRef o As uint8) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As uint8, Optional ByVal that As uint8 = uint8_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As uint8, Optional ByVal that As uint8 = uint8_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As uint8, Optional ByVal that As uint8 = uint8_1) As uint8
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As uint8, Optional ByVal that As uint8 = uint8_1) As uint8
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As uint16, ByVal that As uint16, ByRef o As uint16) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As uint16, ByVal that As uint16, ByRef o As uint16) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As uint16, Optional ByVal that As uint16 = uint16_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As uint16, Optional ByVal that As uint16 = uint16_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As uint16, Optional ByVal that As uint16 = uint16_1) As uint16
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As uint16, Optional ByVal that As uint16 = uint16_1) As uint16
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As uint32, ByVal that As uint32, ByRef o As uint32) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As uint32, ByVal that As uint32, ByRef o As uint32) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As uint32, Optional ByVal that As uint32 = uint32_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As uint32, Optional ByVal that As uint32 = uint32_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As uint32, Optional ByVal that As uint32 = uint32_1) As uint32
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As uint32, Optional ByVal that As uint32 = uint32_1) As uint32
        this = this.unchecked_dec(that)
        Return this
    End Function

    <Extension()> Public Function try_inc(ByVal this As uint64, ByVal that As uint64, ByRef o As uint64) As Boolean
        Try
            o = this + that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_dec(ByVal this As uint64, ByVal that As uint64, ByRef o As uint64) As Boolean
        Try
            o = this - that
        Catch
            Return False
        End Try
        Return True
    End Function

    <Extension()> Public Function try_inc(ByRef this As uint64, Optional ByVal that As uint64 = uint64_1) As Boolean
        Return try_inc(this, that, this)
    End Function

    <Extension()> Public Function try_dec(ByRef this As uint64, Optional ByVal that As uint64 = uint64_1) As Boolean
        Return try_dec(this, that, this)
    End Function

    <Extension()> Public Function self_unchecked_inc(ByRef this As uint64, Optional ByVal that As uint64 = uint64_1) As uint64
        this = this.unchecked_inc(that)
        Return this
    End Function

    <Extension()> Public Function self_unchecked_dec(ByVal this As uint64, Optional ByVal that As uint64 = uint64_1) As uint64
        this = this.unchecked_dec(that)
        Return this
    End Function

End Module

Public Structure unchecked_int8
    Private ReadOnly v As int8
    Public Sub New(ByVal v As int8)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_int8) As int8
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As int8) As unchecked_int8
        Return New unchecked_int8(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_int8) As int8
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int8, ByVal that As unchecked_int8) As unchecked_int8
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int8, ByVal that As int8) As unchecked_int8
        Return New unchecked_int8(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As int8, ByVal that As unchecked_int8) As unchecked_int8
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int8, ByVal that As unchecked_int8) As unchecked_int8
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int8, ByVal that As int8) As unchecked_int8
        Return New unchecked_int8(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As int8, ByVal that As unchecked_int8) As unchecked_int8
        Return New unchecked_int8(this) - that
    End Operator

End Structure

Public Structure unchecked_int16
    Private ReadOnly v As int16
    Public Sub New(ByVal v As int16)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_int16) As int16
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As int16) As unchecked_int16
        Return New unchecked_int16(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_int16) As int16
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int16, ByVal that As unchecked_int16) As unchecked_int16
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int16, ByVal that As int16) As unchecked_int16
        Return New unchecked_int16(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As int16, ByVal that As unchecked_int16) As unchecked_int16
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int16, ByVal that As unchecked_int16) As unchecked_int16
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int16, ByVal that As int16) As unchecked_int16
        Return New unchecked_int16(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As int16, ByVal that As unchecked_int16) As unchecked_int16
        Return New unchecked_int16(this) - that
    End Operator

End Structure

Public Structure unchecked_int32
    Private ReadOnly v As int32
    Public Sub New(ByVal v As int32)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_int32) As int32
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As int32) As unchecked_int32
        Return New unchecked_int32(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_int32) As int32
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int32, ByVal that As unchecked_int32) As unchecked_int32
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int32, ByVal that As int32) As unchecked_int32
        Return New unchecked_int32(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As int32, ByVal that As unchecked_int32) As unchecked_int32
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int32, ByVal that As unchecked_int32) As unchecked_int32
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int32, ByVal that As int32) As unchecked_int32
        Return New unchecked_int32(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As int32, ByVal that As unchecked_int32) As unchecked_int32
        Return New unchecked_int32(this) - that
    End Operator

End Structure

Public Structure unchecked_int64
    Private ReadOnly v As int64
    Public Sub New(ByVal v As int64)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_int64) As int64
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As int64) As unchecked_int64
        Return New unchecked_int64(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_int64) As int64
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int64, ByVal that As unchecked_int64) As unchecked_int64
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int64, ByVal that As int64) As unchecked_int64
        Return New unchecked_int64(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As int64, ByVal that As unchecked_int64) As unchecked_int64
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int64, ByVal that As unchecked_int64) As unchecked_int64
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int64, ByVal that As int64) As unchecked_int64
        Return New unchecked_int64(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As int64, ByVal that As unchecked_int64) As unchecked_int64
        Return New unchecked_int64(this) - that
    End Operator

End Structure

Public Structure unchecked_uint8
    Private ReadOnly v As uint8
    Public Sub New(ByVal v As uint8)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_uint8) As uint8
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As uint8) As unchecked_uint8
        Return New unchecked_uint8(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_uint8) As uint8
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint8, ByVal that As unchecked_uint8) As unchecked_uint8
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint8, ByVal that As uint8) As unchecked_uint8
        Return New unchecked_uint8(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As uint8, ByVal that As unchecked_uint8) As unchecked_uint8
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint8, ByVal that As unchecked_uint8) As unchecked_uint8
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint8, ByVal that As uint8) As unchecked_uint8
        Return New unchecked_uint8(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As uint8, ByVal that As unchecked_uint8) As unchecked_uint8
        Return New unchecked_uint8(this) - that
    End Operator

End Structure

Public Structure unchecked_uint16
    Private ReadOnly v As uint16
    Public Sub New(ByVal v As uint16)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_uint16) As uint16
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As uint16) As unchecked_uint16
        Return New unchecked_uint16(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_uint16) As uint16
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint16, ByVal that As unchecked_uint16) As unchecked_uint16
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint16, ByVal that As uint16) As unchecked_uint16
        Return New unchecked_uint16(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As uint16, ByVal that As unchecked_uint16) As unchecked_uint16
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint16, ByVal that As unchecked_uint16) As unchecked_uint16
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint16, ByVal that As uint16) As unchecked_uint16
        Return New unchecked_uint16(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As uint16, ByVal that As unchecked_uint16) As unchecked_uint16
        Return New unchecked_uint16(this) - that
    End Operator

End Structure

Public Structure unchecked_uint32
    Private ReadOnly v As uint32
    Public Sub New(ByVal v As uint32)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_uint32) As uint32
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As uint32) As unchecked_uint32
        Return New unchecked_uint32(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_uint32) As uint32
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint32, ByVal that As unchecked_uint32) As unchecked_uint32
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint32, ByVal that As uint32) As unchecked_uint32
        Return New unchecked_uint32(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As uint32, ByVal that As unchecked_uint32) As unchecked_uint32
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint32, ByVal that As unchecked_uint32) As unchecked_uint32
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint32, ByVal that As uint32) As unchecked_uint32
        Return New unchecked_uint32(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As uint32, ByVal that As unchecked_uint32) As unchecked_uint32
        Return New unchecked_uint32(this) - that
    End Operator

End Structure

Public Structure unchecked_uint64
    Private ReadOnly v As uint64
    Public Sub New(ByVal v As uint64)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_uint64) As uint64
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As uint64) As unchecked_uint64
        Return New unchecked_uint64(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_uint64) As uint64
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint64, ByVal that As unchecked_uint64) As unchecked_uint64
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint64, ByVal that As uint64) As unchecked_uint64
        Return New unchecked_uint64(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As uint64, ByVal that As unchecked_uint64) As unchecked_uint64
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint64, ByVal that As unchecked_uint64) As unchecked_uint64
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint64, ByVal that As uint64) As unchecked_uint64
        Return New unchecked_uint64(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As uint64, ByVal that As unchecked_uint64) As unchecked_uint64
        Return New unchecked_uint64(this) - that
    End Operator

End Structure

Public Structure unchecked_int
    Private ReadOnly v As int
    Public Sub New(ByVal v As int)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_int) As int
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As int) As unchecked_int
        Return New unchecked_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_int) As int
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int, ByVal that As unchecked_int) As unchecked_int
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_int, ByVal that As int) As unchecked_int
        Return New unchecked_int(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As int, ByVal that As unchecked_int) As unchecked_int
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int, ByVal that As unchecked_int) As unchecked_int
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_int, ByVal that As int) As unchecked_int
        Return New unchecked_int(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As int, ByVal that As unchecked_int) As unchecked_int
        Return New unchecked_int(this) - that
    End Operator

End Structure

Public Structure unchecked_uint
    Private ReadOnly v As uint
    Public Sub New(ByVal v As uint)
        Me.v = v
    End Sub

    Public Shared Operator +(ByVal this As unchecked_uint) As uint
        Return this.v
    End Operator

    Public Shared Widening Operator CType(ByVal this As uint) As unchecked_uint
        Return New unchecked_uint(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As unchecked_uint) As uint
        Return this.v
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint, ByVal that As unchecked_uint) As unchecked_uint
        Return this + (+that)
    End Operator

    Public Shared Operator +(ByVal this As unchecked_uint, ByVal that As uint) As unchecked_uint
        Return New unchecked_uint(unchecked_inc(+this, that))
    End Operator

    Public Shared Operator +(ByVal this As uint, ByVal that As unchecked_uint) As unchecked_uint
        Return that + this
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint, ByVal that As unchecked_uint) As unchecked_uint
        Return this - (+that)
    End Operator

    Public Shared Operator -(ByVal this As unchecked_uint, ByVal that As uint) As unchecked_uint
        Return New unchecked_uint(unchecked_dec(+this, that))
    End Operator

    Public Shared Operator -(ByVal this As uint, ByVal that As unchecked_uint) As unchecked_uint
        Return New unchecked_uint(this) - that
    End Operator

End Structure

