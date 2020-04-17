
Option Explicit On
Option Infer Off
Option Strict On

#Const GCD_USE_SUCCESSIVE_DIVISION = True
#Const GCD_USE_SUCCESSIVE_SUB = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_successive_division(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        If a.less(b) Then
            swap(a, b)
        End If

        Dim c As big_uint = Nothing
        Do
            c = a.assert_modulus(b)
            a = b
            b = c
        Loop Until c.is_zero()
        Return a
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_successive_sub(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        Dim az As UInt32 = 0
        Dim bz As UInt32 = 0
        az = a.trailing_binary_zero_count()
        bz = b.trailing_binary_zero_count()
        a.right_shift(az)
        b.right_shift(bz)
        Dim shift As UInt32 = 0
        shift = min(az, bz)

        While True
            assert(Not a.is_zero())
            assert(Not b.is_zero())

            If a.equal(b) Then
                Return a.left_shift(shift)
            End If

            If a.remove_trailing_binary_zeros() = 0 AndAlso b.remove_trailing_binary_zeros() = 0 Then
                If a.less(b) Then
                    b.assert_sub(a)
                Else
                    a.assert_sub(b)
                End If
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_hybrid(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        If a.less(b) Then
            swap(a, b)
        End If

        Dim c As big_uint = Nothing
        Do
            c = a.assert_modulus(b)
            a = b
            b = c
        Loop Until c.is_zero() OrElse (a.uint32_size() - b.uint32_size() < 2)
        If c.is_zero() Then
            Return a
        End If
        Return gcd_successive_sub(a, b)
    End Function

    Public Shared Function gcd(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        assert(Not a Is Nothing)
        assert(Not b Is Nothing)
        If a.is_zero() OrElse b.is_zero() Then
            Return zero()
        End If
        If a.is_one() OrElse b.is_one() Then
            Return one()
        End If
        If a.equal(b) Then
            Return a.CloneT()
        End If

        a = a.CloneT()
        b = b.CloneT()

#If GCD_USE_SUCCESSIVE_DIVISION Then
        Return gcd_successive_division(a, b)
#ElseIf GCD_USE_SUCCESSIVE_SUB Then
        Return gcd_successive_sub(a, b)
#Else
        Return gcd_hybrid(a, b)
#End If
    End Function
End Class
