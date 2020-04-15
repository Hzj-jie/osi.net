
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_GCD = True
#Const REDUCE_FRACTION_OF_EACH_OTHER = True
#Const REDUCE_SELECTED_PRIMES = True

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Private NotInheritable Class reduce_fraction_primes
        Public Const selected_prime_count As Int32 = 3  ' 3 5 7

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

    Private Shared Sub reduce_fraction(ByVal n As big_uint, ByVal d As big_uint)
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

#If REDUCE_FRACTION_OF_EACH_OTHER Then
        Using code_block
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
                Return
            End If
        End Using
#End If

#If REDUCE_SELECTED_PRIMES Then
        Using code_block
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
        End Using
#End If

#If USE_GCD Then
        Using code_block
            Dim b As big_uint = Nothing
            b = big_uint.gcd(n, d)
            Dim c As big_uint = Nothing
            n.assert_divide(b, c)
            assert(c.is_zero())
            d.assert_divide(b, c)
            assert(c.is_zero())
        End Using
#End If
    End Sub

    Public Function reduce_fraction() As big_udec
        reduce_fraction(Me.n, Me.d)
        Return Me
    End Function
End Class
