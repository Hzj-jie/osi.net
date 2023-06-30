
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.formation

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator +(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().add(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator -(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().sub(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator *(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint()
        'avoid an extra copy
        r.multiply(this, that)
        Return r
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator *(ByVal this As big_uint, ByVal that As UInt32) As big_uint
        Return this.CloneT().multiply(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator *(ByVal this As UInt32, ByVal that As big_uint) As big_uint
        Return that.CloneT().multiply(this)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator \(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().divide(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator /(ByVal this As big_uint, ByVal that As big_uint) As pair(Of big_uint, big_uint)
        Dim q As big_uint = Nothing
        Dim r As big_uint = Nothing
        q = this.CloneT()
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator \(ByVal this As big_uint, ByVal that As UInt32) As big_uint
        Return this.CloneT().divide(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator /(ByVal this As big_uint, ByVal that As UInt32) As pair(Of big_uint, UInt32)
        Dim q As big_uint = Nothing
        Dim r As UInt32 = 0
        q = this.CloneT()
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator Mod(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().modulus(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator Mod(ByVal this As big_uint, ByVal that As UInt32) As UInt32
        Return this.CloneT().modulus(that).as_uint32()
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator >(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) > 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator <(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) < 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator =(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) = 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator <=(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) <= 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator >=(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) >= 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        Return compare(this, that) <> 0
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator >>(ByVal this As big_uint, ByVal that As Int32) As big_uint
        Return this.CloneT().right_shift(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator <<(ByVal this As big_uint, ByVal that As Int32) As big_uint
        Return this.CloneT().left_shift(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator And(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().and(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator Or(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().or(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator Xor(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().xor(that)
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator Not(ByVal this As big_uint) As big_uint
        Return this.CloneT().not()
    End Operator

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Operator ^(ByVal this As big_uint, ByVal that As big_uint) As big_uint
        Return this.CloneT().power(that)
    End Operator
End Class
