
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_GCD = False
#Const REDUCE_FRACTION_OF_EACH_OTHER = False
#Const REDUCE_SELECTED_PRIMES = True

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Private NotInheritable Class reduce_fraction_primes
        Public Const selected_prime_count As Int32 = 2  ' 3 5

        Shared Sub New()
            assert(prime_count >= selected_prime_count + 1)
        End Sub

        Public Shared Function selected_prime(ByVal i As Int32) As UInt32
            assert(i >= 0 AndAlso i < selected_prime_count)
            Return prime(i + 1)
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Shared Sub reduce_fraction(ByVal n As big_uint,
                                       ByVal d As big_uint,
                                       ByVal reduce_of_each_other As Boolean,
                                       ByVal reduce_with_selected_primes As Boolean,
                                       ByVal reduce_with_gcd As Boolean)
        assert(Not n Is Nothing)
        assert(Not d Is Nothing)

        If n.is_zero() Then
            d.set_one()
            Return
        End If
        If n.is_one() OrElse d.is_one() Then
            Return
        End If
        If n.equal(d) Then
            n.set_one()
            d.set_one()
            Return
        End If

        Dim m As UInt32 = 0
        m = min(n.binary_trailing_zero_count(), d.binary_trailing_zero_count())
        n.right_shift(m)
        d.right_shift(m)

        If reduce_of_each_other Then
            assert(Not n.is_zero())
            assert(Not d.is_zero())

            Dim cmp As Int32 = 0
            cmp = n.compare(d)
            assert(cmp <> 0)
            Dim l As big_uint = Nothing
            Dim r As big_uint = Nothing
            If cmp < 0 Then
                l = d.CloneT()
                r = n.CloneT()
            ElseIf cmp > 0 Then
                l = n.CloneT()
                r = d.CloneT()
            End If
            Dim c As big_uint = Nothing
            l.assert_divide(r, c)
            If c.is_zero() Then
                If cmp < 0 Then
                    d.replace_by(l)
                    n.set_one()
                Else
                    d.set_one()
                    n.replace_by(l)
                End If
            End If
        End If

        If reduce_with_selected_primes Then
            For j As Int32 = 0 To reduce_fraction_primes.selected_prime_count - 1
                Dim i As UInt32 = 0
                i = reduce_fraction_primes.selected_prime(j)
                While True
                    Dim nn As big_uint = Nothing
                    Dim nd As big_uint = Nothing
                    nn = n.CloneT()
                    nd = d.CloneT()
                    Dim r As big_uint = Nothing
                    nn.assert_divide(i, r)
                    If Not r.is_zero() Then
                        Exit While
                    End If
                    nd.assert_divide(i, r)
                    If Not r.is_zero() Then
                        Exit While
                    End If
                    n.replace_by(nn)
                    d.replace_by(nd)
                End While
            Next
        End If

        If reduce_with_gcd Then
            Dim b As big_uint = Nothing
            b = big_uint.gcd(n, d)
            Dim c As big_uint = Nothing
            n.assert_divide(b, c)
            assert(c.is_zero())
            d.assert_divide(b, c)
            assert(c.is_zero())
        End If
    End Sub

    Private Shared Sub reduce_fraction(ByVal n As big_uint, ByVal d As big_uint)
        Dim reduce_of_each_other As Boolean = False
#If REDUCE_FRACTION_OF_EACH_OTHER Then
        reduce_of_each_other = True
#Else
        reduce_of_each_other = False
#End If

        Dim reduce_with_selected_primes As Boolean = False
#If REDUCE_SELECTED_PRIMES Then
        reduce_with_selected_primes = True
#Else
        reduce_with_selected_primes = False
#End If

        Dim reduce_with_gcd As Boolean = False
#If USE_GCD Then
        reduce_with_gcd = True
#Else
        reduce_with_gcd = False
#End If
        reduce_fraction(n,
                        d,
                        reduce_of_each_other:=reduce_of_each_other,
                        reduce_with_selected_primes:=reduce_with_selected_primes,
                        reduce_with_gcd:=reduce_with_gcd)
    End Sub
End Class
