
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Private NotInheritable Class reduce_fraction_primes
        Public Const selected_prime_count As Int32 = 3

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
        reduce_fraction(n)
        For i As Int32 = 0 To reduce_fraction_primes.selected_prime_count - 1
            reduce_fraction(reduce_fraction_primes.selected_prime(i))
        Next
    End Sub
End Class
