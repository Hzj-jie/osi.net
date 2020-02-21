﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

'TODO: consider to add a temporary value for uint32 / uint64 to big_uint convert to save object create time
Partial Public NotInheritable Class big_uint
    Private ReadOnly v As adaptive_array_uint32

    Public Sub New()
        Me.v = New adaptive_array_uint32()
    End Sub

    Public Sub New(ByVal i As UInt32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As big_uint)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i() As Byte)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As Single)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Sub New(ByVal i As Double)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Sub New(ByVal i As Decimal)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Shared Function move(ByVal i As big_uint) As big_uint
        If i Is Nothing Then
            Return Nothing
        End If
        Return New big_uint(adaptive_array_uint32.move(i.v))
    End Function

    Public Shared Function swap(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Return assert(adaptive_array_uint32.swap(this.v, that.v))
    End Function

    Public Function replace_by(ByVal i As big_uint) As Boolean
        If i Is Nothing Then
            Return False
        End If
        If i.is_zero() Then
            set_zero()
        ElseIf i.is_one() Then
            set_one()
        Else
            set_zero()
            v.resize(i.v.size())
            memcpy(v.data(), i.v.data(), i.v.size())
        End If
        Return True
    End Function

    Public Sub replace_by(ByVal i As UInt32)
        set_zero()
        If i > 0 Then
            v.push_back(i)
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt64)
        set_zero()
        If i = 0 Then
            Return
        End If
        v.push_back(CUInt(i And max_uint32))
        i >>= bit_count_in_uint32
        assert(i <= max_uint32)
        If i > 0 Then
            v.push_back(CUInt(i))
        End If
    End Sub

    Public Sub replace_by(ByVal i As Boolean)
        set_zero()
        If i Then
            replace_by(max_uint32)
        Else
            replace_by(uint32_0)
        End If
    End Sub

    Public Function replace_by(ByVal a() As Byte) As Boolean
        If isemptyarray(a) Then
            Return False
        End If
        set_zero()
        v.reserve((array_size(a) + uint32_3) \ byte_count_in_uint32)
        Dim start As UInt32 = 0
        Dim [end] As UInt32 = 0
        Dim [step] As Int32 = 0
        If BitConverter.IsLittleEndian Then
            start = 0
            [end] = array_size(a) - uint32_1
            [step] = 1
        Else
            start = array_size(a) - uint32_1
            [end] = 0
            [step] = -1
        End If
        Dim x As UInt32 = 0
        Dim j As UInt32 = 0
        For i As Int32 = CInt(start) To CInt([end]) Step [step]
            x += (CUInt(a(i)) << assert_which.of(j << bit_shift_in_byte).can_cast_to_int32())
            j += uint32_1
            If j = byte_count_in_uint32 Then
                v.push_back(x)
                j = 0
                x = 0
            End If
        Next
        v.push_back(x)
        remove_extra_blank()
        Return True
    End Function

    Public Function replace_by_big_endian(ByVal d() As UInt32) As Boolean
        If isemptyarray(d) Then
            Return False
        End If
        set_zero()
        For i As Int32 = 0 To array_size_i(d) - 1
            v.push_back(d(i))
        Next
        Return True
    End Function

    Public Function replace_by_little_endian(ByVal d() As UInt32) As Boolean
        If isemptyarray(d) Then
            Return False
        End If
        set_zero()
        For i As Int32 = array_size_i(d) - 1 To 0 Step -1
            v.push_back(d(i))
        Next
        Return True
    End Function

    Private Function replace_by_negative_or_zero(ByVal d As Double) As ternary
        If d < 0 Then
            Return ternary.false
        End If
        If d = 0 Then
            set_zero()
            Return ternary.true
        End If
        Return ternary.unknown
    End Function

    Public Function replace_by(ByVal d As Single) As Boolean
        Return replace_by(CDbl(d))
    End Function

    Public Function replace_by(ByVal d As Double) As Boolean
        Dim t As ternary = Nothing
        t = replace_by_negative_or_zero(d)
        If t.notunknown() Then
            Return t.true_()
        End If
        Dim v As vector(Of UInt32) = Nothing
        v = New vector(Of UInt32)()
        While d >= 1
            v.emplace_back(assert_which.of(d Mod max_uint32_plus_1).can_truncate_to_uint32())
            d /= max_uint32_plus_1
            d = System.Math.Truncate(d)
        End While
        assert(replace_by_big_endian(+v))
        Return True
    End Function

    Public Function replace_by(ByVal d As Decimal) As Boolean
        Dim t As ternary = Nothing
        t = replace_by_negative_or_zero(d)
        If t.notunknown() Then
            Return t.true_()
        End If
        Dim v As vector(Of UInt32) = Nothing
        v = New vector(Of UInt32)()
        While d >= 1
            v.emplace_back(assert_which.of(d Mod max_uint32_plus_1).can_truncate_to_uint32())
            d /= max_uint32_plus_1
            d = System.Math.Truncate(d)
        End While
        assert(replace_by_big_endian(+v))
        Return True
    End Function

    Public Sub set_zero()
        v.clear()
    End Sub

    Public Function is_zero() As Boolean
        Return v.empty()
    End Function

    Public Sub set_one()
        v.resize(1)
        v(0) = 1
    End Sub

    Public Function is_one() As Boolean
        Return v.size() = 1 AndAlso v.back() = 1
    End Function

    Public Function is_zero_or_one() As Boolean
        Return is_zero() OrElse is_one()
    End Function

    Public Function [true]() As Boolean
        Return Not [false]()
    End Function

    Public Function [false]() As Boolean
        Return is_zero()
    End Function

    Public Function uint32_size() As UInt32
        Return v.size()
    End Function
End Class
