
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_int.vbp ----------
'so change big_int.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_unsigned_to_signed.vbp ----------
'so change big_unsigned_to_signed.vbp instead of this file


Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_assert.vbp ----------
'so change big_assert.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Shared Function support_base(ByVal b As Byte) As Boolean
        Return big_uint.support_base(b)
    End Function
End Class

'finish big_assert.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_bit.vbp ----------
'so change big_bit.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Function even() As Boolean
        Return d.even()
    End Function

    Public Function odd() As Boolean
        Return d.odd()
    End Function
End Class

'finish big_bit.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_calculation.vbp ----------
'so change big_calculation.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Function add(ByVal that As big_uint) As big_int
        Return add(share(that))
    End Function

    Public Function add(ByVal that As big_int) As big_int
        If Not that Is Nothing AndAlso Not that.is_zero() Then
            If is_zero() Then
                replace_by(that)
            ElseIf positive() = that.positive() Then
                d.add(that.d)
            Else
                If d.less(that.d) Then
                    Dim t As big_uint = Nothing
                    t = New big_uint(that.d)
                    t.assert_sub(d)
                    assert(big_uint.swap(d, t))
                    set_signal(that.signal())
                Else
                    d.assert_sub(that.d)
                    confirm_signal()
                End If
            End If
        End If
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint) As big_int
        Return [sub](share(that))
    End Function

    Public Function [sub](ByVal that As big_int) As big_int
        Dim t As big_int = Nothing
        t = share(that)
        If t Is Nothing Then
            Return Me
        Else
            t.reverse_signal()
            Return add(t)
        End If
    End Function

    Public Function multiply(ByVal that As big_uint) As big_int
        Return multiply(share(that))
    End Function

    Public Function multiply(ByVal that As big_int) As big_int
        If that Is Nothing OrElse that.is_zero() Then
            set_zero()
            Return Me
        End If
        d.multiply(that.d)
        set_signal(positive() = that.positive())
        Return Me
    End Function

    Public Function power_2() As big_int
        d.power_2()
        set_positive()
        Return Me
    End Function

    Public Function power(ByVal that As big_uint) As big_int
        Return power(share(that))
    End Function

    Public Function power(ByVal that As big_int) As big_int
        If that Is Nothing OrElse that.is_zero() Then
            set_positive()
            set_one()
            Return Me
        End If
        If is_zero() OrElse is_one() OrElse that.is_one() Then
            Return Me
        End If
        If that.not_negative() Then
            d.power(that.d)
        ElseIf Not is_one_or_negative_one() Then
            set_zero()
        End If
        If that.d.even() Then
            set_positive()
        End If
        Return Me
    End Function
End Class

'finish big_calculation.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_cast.vbp ----------
'so change big_cast.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Shared Widening Operator CType(ByVal this As Int32) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt32) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Int64) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt64) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As big_uint) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As big_int) As Boolean
        Return Not this Is Nothing AndAlso this.true()
    End Operator

    Public Shared Operator Not(ByVal this As big_int) As Boolean
        Return this Is Nothing OrElse this.false()
    End Operator

    Public Function as_bytes() As Byte()
        Using r As MemoryStream = New MemoryStream(CInt(d.byte_size() + uint32_1))
            r.WriteByte(If(negative(), byte_1, byte_0))
            assert(r.write(d.as_bytes()))
            Return r.export()
        End Using
    End Function
End Class

'finish big_cast.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_compare.vbp ----------
'so change big_compare.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Function less(ByVal that As big_uint) As Boolean
        Return compare(that) < 0
    End Function

    Public Function less(ByVal that As big_int) As Boolean
        Return compare(that) < 0
    End Function

    Public Function equal(ByVal that As big_uint) As Boolean
        Return compare(that) = 0
    End Function

    Public Function equal(ByVal that As big_int) As Boolean
        Return compare(that) = 0
    End Function

    Public Function less_or_equal(ByVal that As big_uint) As Boolean
        Return compare(that) <= 0
    End Function

    Public Function less_or_equal(ByVal that As big_int) As Boolean
        Return compare(that) <= 0
    End Function

    Public Function compare(ByVal that As big_uint) As Int32
        Return compare(Me, that)
    End Function

    Public Function compare(ByVal that As big_int) As Int32
        Return compare(Me, that)
    End Function

    Public Shared Function compare(ByVal this As big_int, ByVal that As big_int) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            If this.positive() <> that.positive() Then
                Return If(this.positive(), 1, -1)
            Else
                Dim r As Int32 = 0
                r = this.d.compare(that.d)
                assert(r <> min_int32)
                Return If(this.positive(), r, -r)
            End If
        Else
            Return c
        End If
    End Function

    Public Shared Function compare(ByVal this As big_uint, ByVal that As big_int) As Int32
        Return compare(share(this), that)
    End Function

    Public Shared Function compare(ByVal this As big_int, ByVal that As big_uint) As Int32
        Return compare(this, share(that))
    End Function
End Class

'finish big_compare.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_constants.vbp ----------
'so change big_constants.vbp instead of this file


Partial Public NotInheritable Class big_int
    Private Const default_str_base As Byte = 10
    Private Const positive_signal_mask As Char = character.plus_sign
    Private Const negative_signal_mask As Char = character.minus_sign

    Shared Sub New()
        assert(strlen(positive_signal_mask) = 1)
        assert(strlen(negative_signal_mask) = 1)
    End Sub
End Class

'finish big_constants.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_implement.vbp ----------
'so change big_implement.vbp instead of this file


Partial Public NotInheritable Class big_int
    Implements ICloneable,
               ICloneable(Of big_int),
               IComparable,
               IComparable(Of big_uint),
               IComparable(Of big_int),
               IEquatable(Of big_int)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As big_int Implements ICloneable(Of big_int).Clone
        Return New big_int(Me)
    End Function

    Public Function CompareTo(ByVal other As big_uint) As Int32 Implements IComparable(Of big_uint).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As big_int) As Int32 Implements IComparable(Of big_int).CompareTo
        Return compare(other)
    End Function

    Public Function CompareTo(ByVal other As Object) As Int32 Implements IComparable.CompareTo
        Return compare(cast(Of big_int)().from(other, False))
    End Function

    Public Overloads Function Equals(ByVal other As big_int) As Boolean Implements IEquatable(Of big_int).Equals
        Return equal(other)
    End Function
End Class

'finish big_implement.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_operator.vbp ----------
'so change big_operator.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Shared Operator +(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.add(that)
    End Operator

    Public Shared Operator +(ByVal this As big_int) As big_int
        Return this
    End Operator

    Public Shared Operator -(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.sub(that)
    End Operator

    Public Shared Operator -(ByVal this As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        r.reverse_signal()
        Return r
    End Operator

    Public Shared Operator *(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.multiply(that)
    End Operator

    Public Shared Operator \(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.divide(that)
    End Operator

    Public Shared Operator >(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <>(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator <=(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator ^(ByVal this As big_int, ByVal that As big_int) As big_int
        Return this.CloneT().power(that)
    End Operator
End Class

'finish big_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_override.vbp ----------
'so change big_override.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_int)().from(obj, False))
    End Function

    Public Overrides Function GetHashCode() As Int32
        If positive() Then
            Return d.GetHashCode()
        End If
        Return Not d.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class

'finish big_override.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_predefined.vbp ----------
'so change big_predefined.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Shared Function zero() As big_int
        Return New big_int()
    End Function

    Public Shared Function _0() As big_int
        Return zero()
    End Function

    Public Shared Function one() As big_int
        Return New big_int(uint32_1)
    End Function

    Public Shared Function _1() As big_int
        Return one()
    End Function

    Public Shared Function negative_one() As big_int
        Return New big_int(-1)
    End Function

    Public Shared Function n1() As big_int
        Return negative_one()
    End Function
End Class

'finish big_predefined.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_private.vbp ----------
'so change big_private.vbp instead of this file


Partial Public NotInheritable Class big_int
    'should be only to use as temporary value, since the big_uint has not been copied
    'do not call this ctor directly, use shared ctor function
    Private Sub New(ByVal d As big_uint, ByVal s As Boolean)
        Me.New()
        Me.d = d
        Me.s = s
    End Sub

    Private Shared Function share(ByVal i As big_uint) As big_int
        If i Is Nothing Then
            Return Nothing
        Else
            Return New big_int(i, True)
        End If
    End Function

    Private Shared Function share(ByVal i As big_int) As big_int
        If i Is Nothing Then
            Return Nothing
        Else
            Return New big_int(i.d, i.s)
        End If
    End Function

    'the behavior of the following two functions
    'is to make sure when is_zero(), always set the signal as positive
    Private Sub set_signal(ByVal v As Boolean)
        If d.is_zero() Then
            s = True
        Else
            s = v
        End If
        'make sure the logic is correct to set the signal as true when is_zero
        is_zero()
    End Sub

    Private Sub confirm_signal()
        set_signal(signal())
    End Sub
End Class

'finish big_private.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_rnd.vbp ----------
'so change big_rnd.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Shared Function rnd_support_str(ByVal length As UInt32,
                                           Optional ByVal base As Byte = default_str_base) As String
        If length <= 1 OrElse rnd_bool() Then
            Return big_uint.rnd_support_str(length, base)
        End If
        Return negative_signal_mask + big_uint.rnd_support_str(length - uint32_1, base)
    End Function

    Public Shared Function rnd_support_base() As Byte
        Return big_uint.rnd_support_base()
    End Function

    Public Shared Function rnd_unsupport_str_char(Optional ByVal base As Byte = default_str_base) As Char
        Return big_uint.rnd_unsupport_str_char(base)
    End Function

    Public Shared Function random(Optional ByVal length As UInt32 = 0) As big_int
        Dim r As big_int = Nothing
        r = share(big_uint.random(length))
        If rnd_bool() Then
            r.set_negative()
        End If
        Return r
    End Function
End Class

'finish big_rnd.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_str.vbp ----------
'so change big_str.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Function str(Optional ByVal base As Byte = default_str_base,
                        Optional ByVal require_positive_signal_mask As Boolean = False) As String
        Dim r As String = Nothing
        r = d.str(base)
        If negative() Then
            Return negative_signal_mask + r
        End If
        If require_positive_signal_mask Then
            Return positive_signal_mask + r
        End If
        Return r
    End Function

    Public Shared Function parse(ByVal s As String,
                                 ByRef r As big_int,
                                 Optional ByVal base As Byte = default_str_base) As Boolean
        Dim signal As Boolean = False
        signal = True
        If Not String.IsNullOrEmpty(s) Then
            If s(0) = negative_signal_mask Then
                signal = False
                s = s.Substring(1)
            ElseIf s(0) = positive_signal_mask Then
                signal = True
                s = s.Substring(1)
            Else
                assert(signal)
            End If
        End If
        Dim d As big_uint = Nothing
        If big_uint.parse(s, d, base) Then
            r = share(d)
            r.set_signal(signal)
            Return True
        End If
        Return False
    End Function

    Public Shared Function parse(ByVal s As String,
                                 Optional ByVal base As Byte = default_str_base) As big_int
        Dim r As big_int = Nothing
        assert(parse(s, r, base))
        Return r
    End Function
End Class

'finish big_str.vbp --------

Partial Public NotInheritable Class big_int
    Private ReadOnly d As big_uint
    Private s As Boolean

    Public Sub New()
        d = New big_uint()
        set_positive()
    End Sub

    Public Sub New(ByVal i As Int32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As Int64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As big_int)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Sub New(ByVal i As big_uint)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Sub New(ByVal i() As Byte)
        Me.New()
        assert(replace_by(i))
    End Sub

    Public Shared Function move(ByVal i As big_int) As big_int
        If i Is Nothing Then
            Return Nothing
        End If
        Dim s As Boolean = False
        s = i.signal()
        Dim r As big_int = Nothing
        r = New big_int(big_uint.move(i.d), s)
        i.confirm_signal()
        Return r
    End Function

    Public Shared Function swap(ByVal this As big_int,
                                ByVal that As big_int) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        root.connector.swap(this.s, that.s)
        Return assert(big_uint.swap(this.d, that.d))
    End Function

    ' replace_by does not need to return Me, because,
    ' 1. Some replace_by returns boolean, the signature should be kept consistent.
    ' 2. ???.replace_by(...) equals to new ???(...). replace_by is unlikely to be an intermediate
    '    operation.
    Public Sub replace_by(ByVal i As Int32)
        If i >= 0 Then
            replace_by(CUInt(i))
        Else
            replace_by(CUInt(-CLng(i)))
            set_negative()
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt32)
        d.replace_by(i)
        set_positive()
    End Sub

    Public Sub replace_by(ByVal i As Int64)
        If i >= 0 Then
            replace_by(CULng(i))
        Else
            replace_by(CULng(-CDec(i)))
            set_negative()
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt64)
        d.replace_by(i)
        set_positive()
    End Sub

    Public Function replace_by(ByVal i As big_int) As Boolean
        If i Is Nothing Then
            Return False
        End If
        assert(d.replace_by(i.d))
        set_signal(i.signal())
        Return True
    End Function

    Public Function replace_by(ByVal i As big_uint) As Boolean
        If d.replace_by(i) Then
            set_positive()
            Return True
        End If
        Return False
    End Function

    Public Function replace_by(ByVal a() As Byte) As Boolean
        If isemptyarray(a) Then
            Return False
        End If
        Dim p As piece = Nothing
        p = New piece(a)
        If Not p.consume(uint32_1, p) Then
            Return False
        End If
#If False Then
        If Not d.replace_by(p.export()) Then
            Return False
        End If
#Else
        d.replace_by(p.export())
#End If
        If a(0) = byte_0 Then
            set_positive()
        Else
            set_negative()
        End If
        Return True
    End Function

    Public Function signal() As Boolean
        Return s
    End Function

    Public Function not_negative() As Boolean
        Return signal()
    End Function

    Public Function not_positive() As Boolean
        Return Not signal() OrElse is_zero()
    End Function

    Public Function positive() As Boolean
        Return signal() AndAlso Not is_zero()
    End Function

    Public Function negative() As Boolean
        Return Not signal() AndAlso Not is_zero()
    End Function

    Public Function set_positive() As big_int
        set_signal(True)
        Return Me
    End Function

    Public Function abs() As big_int
        Return set_positive()
    End Function

    Public Function set_negative() As big_int
        set_signal(False)
        Return Me
    End Function

    Public Function reverse_signal() As big_int
        set_signal(Not positive())
        Return Me
    End Function

    Public Function set_zero() As big_int
        d.set_zero()
        set_positive()
        Return Me
    End Function

    Public Function is_zero() As Boolean
        'just make sure there is no logic error in the class,
        'but no matter s is true or false, d.is_zero() can determine whether is_zero()
        Return d.is_zero() AndAlso assert(signal())
    End Function

    Public Function set_one() As big_int
        d.set_one()
        set_positive()
        Return Me
    End Function

    Public Function is_one() As Boolean
        Return positive() AndAlso d.is_one()
    End Function

    Public Function set_negative_one() As big_int
        d.set_one()
        set_negative()
        Return Me
    End Function

    Public Function is_negative_one() As Boolean
        Return negative() AndAlso d.is_one()
    End Function

    Public Function is_zero_or_one() As Boolean
        Return is_zero() OrElse is_one()
    End Function

    Public Function is_zero_or_negative_one() As Boolean
        Return is_zero() OrElse is_negative_one()
    End Function

    Public Function is_one_or_negative_one() As Boolean
        Return is_one() OrElse is_negative_one()
    End Function

    Public Function is_zero_or_one_or_negative_one() As Boolean
        Return is_zero() OrElse is_one() OrElse is_negative_one()
    End Function

    Public Function [true]() As Boolean
        Return Not [false]()
    End Function

    Public Function [false]() As Boolean
        Return is_zero()
    End Function
End Class

'finish big_unsigned_to_signed.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with big_shift.vbp ----------
'so change big_shift.vbp instead of this file


Partial Public NotInheritable Class big_int
    Public Function left_shift(ByVal size As Int32) As big_int
        d.left_shift(size)
        Return Me
    End Function

    Public Function left_shift(ByVal size As UInt64) As big_int
        d.left_shift(size)
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_int, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(left_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(left_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_left_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function assert_left_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function right_shift(ByVal size As Int32) As big_int
        d.right_shift(size)
        Return Me
    End Function

    Public Function right_shift(ByVal size As UInt64) As big_int
        d.right_shift(size)
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_int, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(right_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(right_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_right_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function assert_right_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Shared Operator >>(ByVal this As big_int, ByVal that As Int32) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.right_shift(that)
    End Operator

    Public Shared Operator <<(ByVal this As big_int, ByVal that As Int32) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.left_shift(that)
    End Operator
End Class
'finish big_shift.vbp --------

Partial Public NotInheritable Class big_int
    Public Sub replace_by(ByVal i As Boolean)
        d.replace_by(i)
        set_signal(Not i)
    End Sub

    Public Shared Widening Operator CType(ByVal this As Boolean) As big_int
        Dim r As big_int = Nothing
        r = New big_int()
        r.replace_by(this)
        Return r
    End Operator

    Public Function power_of_2() As Boolean
        Return positive() AndAlso d.power_of_2()
    End Function

    Public Function as_uint64(ByRef overflow As Boolean) As UInt64
        If negative() Then
            overflow = True
            Return 0
        End If
        Return d.as_uint64(overflow)
    End Function

    Public Function as_uint32(ByRef overflow As Boolean) As UInt32
        If negative() Then
            overflow = True
            Return 0
        End If
        Return d.as_uint32(overflow)
    End Function

    Public Function as_int32(ByRef overflow As Boolean) As Int32
        Dim v As Int32 = 0
        v = d.as_int32(overflow)
        Return If(negative(), -v, v)
    End Function

    Private Sub divide(ByVal that As big_int, ByRef remainder As big_int, ByRef divide_by_zero As Boolean)
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return
        End If
        Dim n As Boolean = False
        n = negative()
        Dim r As big_uint = Nothing
        d.divide(that.d, divide_by_zero, r)
        assert(Not divide_by_zero)
        remainder = share(r)
        confirm_signal()
        If n Then
            remainder.set_negative()
        End If
        If n = that.negative() Then
            set_positive()
        Else
            set_negative()
        End If
    End Sub

    Private Sub extract(ByVal that As big_int,
                        ByRef remainder As big_int,
                        ByRef divide_by_zero As Boolean,
                        ByRef imaginary_number As Boolean)
        divide_by_zero = False
        imaginary_number = False
        If that Is Nothing OrElse that.is_zero() Then
            If is_one() Then
                divide_by_zero = False
                remainder = zero()
            Else
                divide_by_zero = True
            End If
            Return
        End If
        imaginary_number = (negative() AndAlso that.even())
        If imaginary_number Then
            Return
        End If
        remainder = New big_int()
        If is_zero_or_one_or_negative_one() Then
            Return
        End If
        If that.negative() Then
            remainder.replace_by(Me)
            set_zero()
        Else
            Dim r As big_uint = Nothing
            d.extract(that.d, divide_by_zero, r)
            remainder.replace_by(r)
            remainder.set_signal(signal())
        End If
        confirm_signal()
    End Sub

    Public Function divide(ByVal that As big_uint,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_divide(share(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As big_int,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean) As big_int
        Return modulus(share(that), divide_by_zero)
    End Function

    Public Function modulus(ByVal that As big_int,
                            ByRef divide_by_zero As Boolean) As big_int
        If that Is Nothing OrElse that.is_zero() Then
            divide_by_zero = True
            Return Me
        End If
        d.modulus(that.d, divide_by_zero)
        assert(Not divide_by_zero)
        confirm_signal()
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint) As big_int
        Return modulus(share(that))
    End Function

    Public Function modulus(ByVal that As big_int) As big_int
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_modulus(ByVal that As big_uint) As big_int
        Return assert_modulus(share(that))
    End Function

    Public Function assert_modulus(ByVal that As big_int) As big_int
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), divide_by_zero, imaginary_number, remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        extract(that, remainder, divide_by_zero, imaginary_number)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        If d Then
            Throw divide_by_zero()
        End If
        If i Then
            Throw imaginary_number()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_uint,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_extract(share(that), remainder)
    End Function

    Public Function assert_extract(ByVal that As big_int,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        assert(Not d)
        assert(Not i)
        Return Me
    End Function

    Public Shared Operator /(ByVal this As big_int, ByVal that As big_int) As pair(Of big_int, big_int)
        Dim q As big_int = Nothing
        Dim r As big_int = Nothing
        q = New big_int(this)
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    Public Shared Operator Mod(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim q As big_int = Nothing
        q = New big_int(this)
        q.modulus(that)
        Return q
    End Operator
End Class
'finish big_int.vbp --------
