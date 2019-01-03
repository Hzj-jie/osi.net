
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
            If assertion.is_not_null(r) Then
                Dim v As Int32 = 1
                For j As Int32 = 0 To r.size() - 1
                    assertion.is_true(is_prime(r(j).first))
                    assertion.more(r(j).second, 0)
                    For k As Int32 = 0 To r(j).second - 1
                        Try
                            v *= r(j).first
                        Catch ex As OverflowException
                            assertion.is_true(False)
                        End Try
                    Next
                Next
                assertion.equal(v, x)
            End If
        Next
        For i As Int32 = 0 To prime(0) - 1
            If assertion.is_not_null(prime_factorization(i)) Then
                assertion.is_true(prime_factorization(i).empty())
            End If
        Next
        For i As Int32 = 0 To 1024 - 1
            assertion.is_null(prime_factorization(0 - rnd_int(1, max_int32)))
        Next
        Return True
    End Function
End Class
