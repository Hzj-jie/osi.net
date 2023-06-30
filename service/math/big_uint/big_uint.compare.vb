
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function less(ByVal that As big_uint) As Boolean
        Return compare(that) < 0
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function equal(ByVal that As big_uint) As Boolean
        Return compare(that) = 0
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function less_or_equal(ByVal that As big_uint) As Boolean
        Return compare(that) <= 0
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Function compare(ByVal that As big_uint) As Int32
        Return compare(Me, that)
    End Function

    Private Shared Function compare(ByVal this As big_uint, ByVal that As big_uint, ByVal offset As UInt32) As Int32
        If offset = 0 Then
            Dim c As Int32 = 0
            c = object_compare(this, that)
            If c <> object_compare_undetermined Then
                Return c
            End If
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        If this.is_zero() Then
            If that.is_zero() Then
                Return 0
            End If
            Return -1
        End If
        If that.is_zero() Then
            Return 1
        End If
        If this.is_one() Then
            If that.is_one() AndAlso offset = 0 Then
                Return 0
            End If
            Return -1
        End If
        If this.v.size() <> that.v.size() + offset Then
            Return If(this.v.size() > that.v.size() + offset, 1, -1)
        End If
        assert(this.v.size() > 0)
        Dim i As UInt32 = 0
        i = that.v.size() - uint32_1
        While True
            If this.v.get(i + offset) <> that.v.get(i) Then
                Return If(this.v.get(i) > that.v.get(i), 1, -1)
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        If offset > 0 Then
            i = offset - uint32_1
            While True
                If this.v.get(i) > 0 Then
                    Return 1
                End If
                If i = 0 Then
                    Exit While
                End If
                i -= uint32_1
            End While
        End If
        Return 0
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function compare(ByVal this As big_uint, ByVal that As big_uint) As Int32
        Return compare(this, that, 0)
    End Function
End Class
