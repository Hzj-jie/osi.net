
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.math

Public Class prime_divisors_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 256 - 1
            Dim x As Int32 = 0
            x = rnd_int(prime(0), max_int32)
            Dim r As vector(Of pair(Of Int32, Int32)) = Nothing
            r = prime_factorization(x)
            If assert_not_nothing(r) Then
                Dim v As Int32 = 1
                For j As Int32 = 0 To r.size() - 1
                    assert_true(is_prime(r(j).first))
                    assert_more(r(j).second, 0)
                    For k As Int32 = 0 To r(j).second - 1
                        Try
                            v *= r(j).first
                        Catch ex As OverflowException
                            assert_true(False)
                        End Try
                    Next
                Next
                assert_equal(v, x)
            End If
        Next
        For i As Int32 = 0 To prime(0) - 1
            If assert_not_nothing(prime_factorization(i)) Then
                assert_true(prime_factorization(i).empty())
            End If
        Next
        For i As Int32 = 0 To 1024 - 1
            assert_nothing(prime_factorization(0 - rnd_int(1, max_int32)))
        Next
        Return True
    End Function
End Class
