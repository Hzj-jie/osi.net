﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' A pure decimal in the range of [0, 1).
Partial Public NotInheritable Class upure_dec
    Private ReadOnly n As big_uint
    Private ReadOnly d As big_uint

    Public Shared Function zero() As upure_dec
        Return New upure_dec()
    End Function

    Public Sub New()
        n = big_uint.zero()
        d = big_uint.one()
    End Sub

    Public Sub New(ByVal d As Double, ByVal max_shift As UInt32)
        Me.New()
        replace_by(d, max_shift)
    End Sub

    Public Sub New(ByVal d As Double)
        Me.New()
        replace_by(d)
    End Sub

    Public Sub New(ByVal d As Single, ByVal max_shift As UInt32)
        Me.New()
        replace_by(d, max_shift)
    End Sub

    Public Sub New(ByVal d As Single)
        Me.New()
        replace_by(d)
    End Sub

    Public Sub New(ByVal d As Decimal, ByVal max_shift As UInt32)
        Me.New()
        replace_by(d, max_shift)
    End Sub

    Public Sub New(ByVal d As Decimal)
        Me.New()
        replace_by(d)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal n As big_uint, ByVal d As big_uint)
        assert_n_d(n, d)
        Me.n = n
        Me.d = d
    End Sub

    Public Sub replace_by(ByVal v As Double, ByVal max_shift As UInt32)
        assert(v < 1)
        assert(v >= 0)
        assert(max_shift > 0)
        d.set_one()
        While v.is_not_integral() AndAlso max_shift > 0
            d.left_shift(uint32_1)
            v *= 2
            max_shift -= uint32_1
        End While
        assert(n.replace_by(v))
    End Sub

    Public Sub replace_by(ByVal v As Double)
        replace_by(v, constants.replace_by_dec_max_shift)
    End Sub

    Public Sub replace_by(ByVal d As Single, ByVal max_shift As UInt32)
        replace_by(CDbl(d), max_shift)
    End Sub

    Public Sub replace_by(ByVal d As Single)
        replace_by(CDbl(d))
    End Sub

    Public Sub replace_by(ByVal v As Decimal, ByVal max_shift As UInt32)
        assert(v < 1)
        assert(v >= 0)
        d.set_one()
        While v.is_not_integral() AndAlso max_shift > 0
            d.left_shift(uint32_1)
            v *= 2
            max_shift -= uint32_1
        End While
        assert(n.replace_by(v))
    End Sub

    Public Sub replace_by(ByVal v As Decimal)
        replace_by(v, constants.replace_by_dec_max_shift)
    End Sub

    Private Shared Sub assert_n_d(ByVal n As big_uint, ByVal d As big_uint)
        assert(Not n Is Nothing)
        assert(Not d Is Nothing)
        assert(n.less(d))
    End Sub

    Public Sub replace_by(ByVal n As big_uint, ByVal d As big_uint)
        assert_n_d(n, d)
        assert(Me.n.replace_by(n))
        assert(Me.d.replace_by(d))
    End Sub

    Public Sub replace_by(ByVal n As upure_dec)
        assert(Not n Is Nothing)
        replace_by(n.n, n.d)
    End Sub

    Public Sub set_zero()
        n.set_zero()
        d.set_one()
    End Sub

    Public Shared Function move(ByVal i As upure_dec) As upure_dec
        If i Is Nothing Then
            Return Nothing
        End If
        Return New upure_dec(big_uint.move(i.n), big_uint.move(i.d))
    End Function

    Public Shared Function swap(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Return assert(big_uint.swap(this.n, that.n)) AndAlso
               assert(big_uint.swap(this.d, that.d))
    End Function

    Public Function is_zero() As Boolean
        Return n.is_zero()
    End Function
End Class
