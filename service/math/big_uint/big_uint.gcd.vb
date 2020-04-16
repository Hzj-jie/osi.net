
Option Explicit On
Option Infer Off
Option Strict On

#Const GCD_USE_SUCCESSIVE_DIVISION = True
#Const GCD_USE_SUCCESSIVE_SUB = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Sub ensure_a_is_larger(ByRef a As big_uint, ByRef b As big_uint)
        If a.less(b) Then
            swap(a, b)
        End If
    End Sub

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_successive_division(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        ensure_a_is_larger(a, b)
        Dim c As big_uint = Nothing
        c = a.assert_modulus(b)
        While Not c.is_zero()
            a = b
            b = c
            c = a.assert_modulus(b)
        End While
        Return b
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function shifting(ByVal a As big_uint, ByVal b As big_uint) As UInt32
        Dim az As UInt32 = 0
        Dim bz As UInt32 = 0
        az = a.trailing_binary_zero_count()
        bz = b.trailing_binary_zero_count()
        a.right_shift(az)
        b.right_shift(bz)
        Return min(az, bz)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_successive_sub(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        Dim shift As UInt32 = 0
        shift = shifting(a, b)

        While True
            assert(Not a.is_zero())
            assert(Not b.is_zero())
            a.remove_trailing_binary_zeros()
            b.remove_trailing_binary_zeros()

            Dim cmp As Int32 = 0
            cmp = a.compare(b)
            If cmp = 0 Then
                Return a.left_shift(shift)
            End If
            If cmp < 0 Then
                b.assert_sub(a)
            Else
                assert(cmp > 0)
                a.assert_sub(b)
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_combined(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        Dim shift As UInt32 = 0
        shift = shifting(a, b)

        ensure_a_is_larger(a, b)
        While True
            a.remove_trailing_binary_zeros()
            b.remove_trailing_binary_zeros()
            ensure_a_is_larger(a, b)

            a.assert_sub(b)
            a.remove_trailing_binary_zeros()
            ensure_a_is_larger(a, b)

            Dim c As big_uint = Nothing
            c = a.assert_modulus(b)
            If c.is_zero() Then
                Return b.left_shift(shift)
            End If
            a = b
            b = c
        End While
        assert(False)
        Return Nothing
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
        Return gcd_combined(a, b)
#End If
    End Function
End Class
