
Option Explicit On
Option Infer Off
Option Strict On

#Const REDUCE_FRACTION_OF_EACH_OTHER = False
#Const REDUCE_FRACTION_OF_PREDEFINED_PRIMES = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Private fraction_dirty_rate As Int32 = 0

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

    Private Shared Function fast_reduce_fraction(ByVal n As big_uint, ByVal d As big_uint) As Boolean
        assert(Not n Is Nothing)
        assert(Not d Is Nothing)

        If n.is_zero() Then
            d.set_one()
            Return True
        End If
        If n.is_one() OrElse d.is_one() Then
            Return True
        End If
        If n.equal(d) Then
            n.set_one()
            d.set_one()
            Return True
        End If

        Dim m As UInt32 = 0
        m = min(n.trailing_binary_zero_count(), d.trailing_binary_zero_count())
        n.right_shift(m)
        d.right_shift(m)

        Return False
    End Function

    Private Shared Sub reduce_fraction(ByVal n As big_uint, ByVal d As big_uint)
        If fast_reduce_fraction(n, d) Then
            Return
        End If

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

#If REDUCE_FRACTION_OF_PREDEFINED_PRIMES Then
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
                    n = nn
                    d = nd
                End While
            Next
        End Using
#End If

        Using code_block
            Dim b As big_uint = Nothing
            b = big_uint.gcd(n, d)
            Dim c As big_uint = Nothing
            n.assert_divide(b, c)
            assert(c.is_zero())
            d.assert_divide(b, c)
            assert(c.is_zero())
        End Using
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function fast_reduce_fraction() As big_udec
        fast_reduce_fraction(Me.n, Me.d)
        Return Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function increase_fraction_dirty_rate() As Boolean
        fraction_dirty_rate += 1
        If fraction_dirty_rate = max_int32 Then
            reduce_fraction()
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub reduce_fraction()
        reduce_fraction(Me.n, Me.d)
        fraction_dirty_rate = 0
    End Sub
End Class
