
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_ufloat
    Private ReadOnly i As big_uint
    Private ReadOnly d As upure_dec

    Public Sub New()
        Me.i = New big_uint()
        Me.d = New upure_dec()
    End Sub

    Public Sub New(ByVal d As Double, ByVal max_shift As UInt32)
        Me.New()
        replace_by(d, max_shift)
    End Sub

    Public Sub New(ByVal d As Double)
        Me.New()
        replace_by(d)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal i As big_uint, ByVal d As upure_dec)
        assert(Not i Is Nothing)
        assert(Not d Is Nothing)
        Me.i = i
        Me.d = d
    End Sub

    Public Sub New(ByVal i As big_uint)
        Me.New(i, upure_dec.zero())
    End Sub

    Public Sub New(ByVal d As upure_dec)
        Me.New(big_uint.zero(), d)
    End Sub

    Public Sub replace_by(ByVal d As Double, ByVal max_shift As UInt32)
        Me.i.replace_by(int_part(d))
        Me.d.replace_by(dec_part(d), max_shift)
    End Sub

    Public Sub replace_by(ByVal d As Double)
        Me.i.replace_by(int_part(d))
        Me.d.replace_by(dec_part(d))
    End Sub

    Private Shared Function int_part(ByVal d As Double) As Double
        Return System.Math.Truncate(d)
    End Function

    Private Shared Function dec_part(ByVal d As Double) As Double
        Return d - int_part(d)
    End Function

    Public Function is_zero() As Boolean
        Return i.is_zero() AndAlso d.is_zero()
    End Function
End Class
