
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Function add(ByVal that As big_uint) As big_uint
        If that Is Nothing OrElse that.is_zero() Then
            Return Me
        End If
        If is_zero() Then
            assert(replace_by(that))
            Return Me
        End If
        assert(v.size() > 0 AndAlso that.v.size() > 0)
        If that.v.size() > v.size() Then
            v.resize(that.v.size())
        End If
        Dim c As UInt32 = 0
        Dim i As UInt32 = 0
        For i = 0 To that.v.size() - uint32_1
            add(that.v.get(i), c, i)
            assert(c = 0 OrElse c = 1)
        Next
        For i = i To v.size() - uint32_1
            add(0, c, i)
            assert(c = 0 OrElse c = 1)
            If c = 0 Then
                Exit For
            End If
        Next
        If c > 0 Then
            v.push_back(c)
        End If
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint) As big_uint
        Dim c As UInt32 = 0
        [sub](that, c)
        assert(c = 0 OrElse c = 1)
        If c = 1 Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_sub(ByVal that As big_uint) As big_uint
        Dim c As UInt32 = 0
        [sub](that, c)
        assert(c = 0 OrElse c = 1)
        assert(c = 0)
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint, ByRef overflow As Boolean) As big_uint
        Dim c As UInt32 = 0
        [sub](that, c)
        assert(c = 0 OrElse c = 1)
        overflow = (c = 1)
        Return Me
    End Function

    Public Function multiply(ByVal that As big_uint) As big_uint
        multiply(move(Me), that)
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_uint = Nothing) As big_uint
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint, ByRef divide_by_zero As Boolean) As big_uint
        Dim remainder As big_uint = Nothing
        ' TODO: Do not use divide, implement a dedicate modulus operator.
        divide(that, divide_by_zero, remainder)
        assert(replace_by(remainder))
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function assert_modulus(ByVal that As big_uint) As big_uint
        Dim r As Boolean = False
        modulus(that, r)
        assert(Not r)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt32,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt32,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As UInt32,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function divide(ByVal that As UInt16,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As UInt16,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As UInt16,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Return assert_divide(CUInt(that), remainder)
    End Function

    Public Function divide(ByVal that As Byte,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As Byte,
                           Optional ByRef remainder As UInt32 = 0) As big_uint
        Return divide(CUInt(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As Byte,
                                  Optional ByRef remainder As UInt32 = 0) As big_uint
        Return assert_divide(CUInt(that), remainder)
    End Function

    Public Function power_2() As big_uint
        Dim s As big_uint = Nothing
        s = move(Me)
        multiply(s, s)
        Return Me
    End Function

    Public Function power(ByVal that As big_uint) As big_uint
        If that Is Nothing Then
            Return Me
        End If
        If that.is_zero() Then
            '0 ^ 0 = 1
            set_one()
            Return Me
        End If
        If is_zero_or_one() OrElse that.is_one() Then
            Return Me
        End If
        Dim c As big_uint = Nothing
        If that.getrbit(0) Then
            c = New big_uint(Me)
        Else
            c = move(Me)
            set_one()
        End If
        assert(that.bit_count() > 0)
        Dim last As UInt64 = 0
        For i As UInt64 = 1 To that.bit_count() - uint64_1
            If that.getrbit(i) Then
                While last < i
                    c.power_2()
                    last += uint64_1
                End While
                multiply(c)
            End If
        Next
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            Optional ByRef remainder As big_uint = Nothing) As big_uint
        extract(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        extract(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_uint, Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        extract(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function factorial() As big_uint
        If is_zero_or_one() Then
            set_one()
        Else
            Dim i As big_uint = Nothing
            i = move(Me)
            set_one()
            While Not i.is_one()
                Me.multiply(i)
                i.assert_sub(uint32_1)
            End While
        End If
        Return Me
    End Function

    Public Shared Function gcd(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        assert(Not a Is Nothing)
        assert(Not b Is Nothing)
        If a.equal(b) Then
            Return a.CloneT()
        End If
        Dim c As big_uint = Nothing
        If a.less(b) Then
            c = a
            a = b
            b = c
        End If
        c = a.CloneT().assert_modulus(b)
        While Not c.is_zero()
            a = b.CloneT()
            b = c
            c = a.assert_modulus(b)
        End While
        Return b
    End Function
End Class
