
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' A positive decimal in range of [0, +inf) by the representation of n / d.
Partial Public NotInheritable Class big_udec
    Private ReadOnly n As big_uint
    Private ReadOnly d As big_uint

    Public Shared Function zero() As big_udec
        Return New big_udec()
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
        assert(Not n Is Nothing)
        assert(Not d Is Nothing)
        Me.n = n
        Me.d = d
    End Sub

    Public Sub New(ByVal b() As Byte)
        Me.New()
        assert(replace_by(b))
    End Sub

    Public Sub replace_by(ByVal v As Double, ByVal max_shift As UInt32)
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

    Public Function replace_by(ByVal n As big_uint, ByVal d As big_uint) As Boolean
        If n Is Nothing OrElse d Is Nothing Then
            Return False
        End If
        assert(Me.n.replace_by(n))
        assert(Me.d.replace_by(d))
        Return True
    End Function

    Public Function replace_by(ByVal n As big_udec) As Boolean
        If n Is Nothing Then
            Return False
        End If
        Return assert(replace_by(n.n, n.d))
    End Function

    Public Function replace_by(ByVal b() As Byte) As Boolean
        If isemptyarray(b) Then
            set_zero()
            Return True
        End If
        Dim l As UInt32 = 0
        Dim i As UInt32 = 0
        If Not bytes_uint32(b, l, i) Then
            Return False
        End If
        Dim p As piece = Nothing
        p = New piece(b)
        If Not p.consume(i, p) Then
            Return False
        End If
        Dim n As piece = Nothing
        Dim d As piece = Nothing
        If Not p.keep(l, n) Then
            Return False
        End If
        If Not p.consume(l, d) Then
            Return False
        End If
        Me.n.replace_by(n.export())
        Me.d.replace_by(d.export())
        Return True
    End Function

    Public Sub set_zero()
        n.set_zero()
        d.set_one()
    End Sub

    Public Sub set_one()
        n.set_one()
        d.set_one()
    End Sub

    Public Shared Function move(ByVal i As big_udec) As big_udec
        If i Is Nothing Then
            Return Nothing
        End If
        Return New big_udec(big_uint.move(i.n), big_uint.move(i.d))
    End Function

    Public Shared Function swap(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Return assert(big_uint.swap(this.n, that.n)) AndAlso
               assert(big_uint.swap(this.d, that.d))
    End Function

    Public Function is_zero() As Boolean
        Return n.is_zero()
    End Function

    Public Function is_one() As Boolean
        Return n.equal(d)
    End Function

    Public Function dec_part() As big_udec
        If is_pure_dec() Then
            Return CloneT()
        End If
        Dim r As big_uint = Nothing
        n.CloneT().assert_divide(d, r)
        Return New big_udec(r, d)
    End Function

    Public Function int_part() As big_uint
        If is_pure_dec() Then
            Return big_uint.zero()
        End If
        Return New big_uint(n \ d)
    End Function

    Public Function is_pure_dec() As Boolean
        Return n.less(d)
    End Function
End Class
