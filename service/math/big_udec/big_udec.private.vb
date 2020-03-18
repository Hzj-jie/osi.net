
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Private NotInheritable Class reduce_fraction_primes
        Public Const selected_prime_count As Int32 = 2

        Shared Sub New()
            assert(prime_count >= selected_prime_count)
        End Sub

        Public Shared Function selected_prime(ByVal i As Int32) As UInt32
            assert(i >= 0 AndAlso i < selected_prime_count)
            Return prime(i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub reduce_fraction()
        If is_zero() Then
            set_zero()
            Return
        End If
        If is_one() Then
            set_one()
            Return
        End If
        If n.is_zero_or_one() Then
            Return
        End If
#If USE_GCD Then
        Dim b As big_uint = Nothing
        b = big_uint.gcd(n, d)
        Dim c As big_uint = Nothing
        n.assert_divide(b, c)
        assert(c.is_zero())
        d.assert_divide(b, c)
        assert(c.is_zero())
#Else
#If REDUCE_FRACTION_OF_NUMERATOR Then
        reduce_fraction(n)
#End If
        For i As Int32 = 0 To reduce_fraction_primes.selected_prime_count - 1
            reduce_fraction(reduce_fraction_primes.selected_prime(i))
        Next
#End If
    End Sub
End Class
