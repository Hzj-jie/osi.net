﻿
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Partial Public Class big_uint
    Public Function add(ByVal that As big_uint) As big_uint
        If Not that Is Nothing AndAlso Not that.is_zero() Then
            If is_zero() Then
                assert(replace_by(that))
            Else
                assert(v.size() > 0 AndAlso that.v.size() > 0)
                If that.v.size() > v.size() Then
                    v.resize(that.v.size())
                End If
                Dim c As UInt32 = 0
                Dim i As UInt32 = 0
                For i = 0 To that.v.size() - uint32_1
                    add(that.v(i), c, i)
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
            End If
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

    Public Function divide(ByVal that As big_uint,
                           Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint,
                                  Optional ByRef remainder As big_uint = Nothing) As big_uint
        Dim r As Boolean = False
        divide(that, remainder, r)
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
        If Not that Is Nothing Then
            If that.is_zero() Then
                '0 ^ 0 = 1
                set_one()
            ElseIf Not is_zero_or_one() AndAlso
                   Not that.is_one() Then
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
            End If
        End If
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
End Class
