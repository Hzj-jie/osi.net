
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class big_uint
    Public Shared Operator +(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.add(that)
    End Operator

    Public Shared Operator -(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.sub(that)
    End Operator

    Public Shared Operator *(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint()
        'avoid an extra copy
        r.multiply(this, that)
        Return r
    End Operator

    Public Shared Operator \(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.divide(that)
    End Operator

    Public Shared Operator /(ByVal this As big_uint, ByVal that As big_uint) As pair(Of big_uint, big_uint)
        Dim q As big_uint = Nothing
        Dim r As big_uint = Nothing
        q = New big_uint(this)
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    Public Shared Operator \(ByVal this As big_uint, ByVal that As UInt32) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.divide(that)
    End Operator

    Public Shared Operator /(ByVal this As big_uint, ByVal that As UInt32) As pair(Of big_uint, UInt32)
        Dim q As big_uint = Nothing
        Dim r As UInt32 = 0
        q = New big_uint(this)
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    Public Shared Operator \(ByVal this As big_uint, ByVal that As UInt16) As big_uint
        Return this \ CUInt(that)
    End Operator

    Public Shared Operator /(ByVal this As big_uint, ByVal that As UInt16) As pair(Of big_uint, UInt32)
        Return this / CUInt(that)
    End Operator

    Public Shared Operator \(ByVal this As big_uint, ByVal that As Byte) As big_uint
        Return this \ CUInt(that)
    End Operator

    Public Shared Operator /(ByVal this As big_uint, ByVal that As Byte) As pair(Of big_uint, UInt32)
        Return this / CUInt(that)
    End Operator

    Public Shared Operator Mod(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim q As big_uint = Nothing
        Dim r As big_uint = Nothing
        q = New big_uint(this)
        q.divide(that, r)
        Return r
    End Operator

    Public Shared Operator >(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <=(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator <>(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator >>(ByVal this As big_uint, ByVal that As Int32) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.right_shift(that)
    End Operator

    Public Shared Operator <<(ByVal this As big_uint, ByVal that As Int32) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.left_shift(that)
    End Operator

    Public Shared Operator And(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.and(that)
    End Operator

    Public Shared Operator Or(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.or(that)
    End Operator

    Public Shared Operator Xor(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.xor(that)
    End Operator

    Public Shared Operator Not(ByVal this As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.not()
    End Operator

    Public Shared Operator ^(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint(this)
        Return r.power(that)
    End Operator
End Class
