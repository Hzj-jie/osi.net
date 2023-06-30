
Option Explicit On
Option Infer Off
Option Strict On

#Const GCD_USE_SUCCESSIVE_DIVISION = False
#Const GCD_USE_SUCCESSIVE_SUB = False
#Const GCD_USE_BIG_INTEGER_GCD = False

Imports System.Numerics
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Partial Public NotInheritable Class big_uint
    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_successive_division(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        If a.less(b) Then
            root.connector.swap(a, b)
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
    Private Shared Function gcd_hybrid_loop(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        While True
#If DEBUG Then
            assert(Not a.is_zero())
            assert(Not b.is_zero())
#End If

            Dim c As big_uint = Nothing
            If small_number_gcd(a, b, c) Then
                Return c
            End If

            Dim cmp As Int32 = 0
            cmp = a.compare(b)

            If cmp < 0 Then
                root.connector.swap(a, b)
            End If

            If cmp = 0 Then
                Return a
            End If

            If a.remove_trailing_binary_zeros() > 0 Then
                Continue While
            End If

            If b.remove_trailing_binary_zeros() > 0 Then
                Continue While
            End If

            If a.uint32_size() - b.uint32_size() >= 2 OrElse b.uint32_size() <= 2 Then
                c = a.assert_modulus(b)
                If c.is_zero() Then
                    Return b
                End If
                a = b
                b = c
                Continue While
            End If

            a.assert_sub(b)
        End While
        assert(False)
        Return Nothing
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_hybrid(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        Dim shift As UInt32 = 0
        shift = shifting(a, b)
        a.remove_trailing_binary_zeros()
        b.remove_trailing_binary_zeros()
        Return gcd_hybrid_loop(a, b).left_shift(shift)
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function small_number_gcd(ByVal a As big_uint, ByVal b As big_uint, ByRef o As big_uint) As Boolean
        If Not a.fit_uint64() OrElse Not b.fit_uint64 Then
            Return False
        End If
        If a.fit_uint32() AndAlso b.fit_uint32() Then
            o = New big_uint(relatively_prime.gcd(a.as_uint32(), b.as_uint32()))
        Else
            o = New big_uint(relatively_prime.gcd(a.as_uint64(), b.as_uint64()))
        End If
        Return True
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Private Shared Function gcd_big_integer(ByVal a As big_uint, ByVal b As big_uint) As big_uint
        Return big_int.from_BigInteger(BigInteger.GreatestCommonDivisor(
                                           New big_int(a).as_BigInteger(),
                                           New big_int(b).as_BigInteger())).unsigned_ref()
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

        Dim r As big_uint = Nothing
        If small_number_gcd(a, b, r) Then
            Return r
        End If

        If a.equal(b) Then
            Return a.CloneT()
        End If

#If GCD_USE_BIG_INTEGER_GCD Then
        Return gcd_big_integer(a, b)
#End If

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
