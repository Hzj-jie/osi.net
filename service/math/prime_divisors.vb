
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _prime_divisors
    <Extension()> Public Function prime_factorization(ByVal i As Int32) As vector(Of pair(Of Int32, Int32))
        If i < 0 Then
            Return Nothing
        ElseIf i < prime(0) Then
            Return New vector(Of pair(Of Int32, Int32))()
        Else
            Dim r As vector(Of pair(Of Int32, Int32)) = Nothing
            r = New vector(Of pair(Of Int32, Int32))()
            For j As Int32 = 0 To prime_count - 1
#If DEBUG Then
                assert(prime(j) <= i)
#End If
                If prime(j) = i Then
                    r.emplace_back(make_pair(i, 1))
                    i = 1
                ElseIf i Mod prime(j) = 0 Then
                    Dim p As pair(Of Int32, Int32) = Nothing
                    p = make_pair(prime(j), 0)
                    While i Mod prime(j) = 0
                        p.second += 1
                        i \= prime(j)
                    End While
                    r.emplace_back(p)
                End If
                If i = 1 Then
                    Exit For
                End If
            Next
            If i > 1 Then
#If DEBUG Then
                assert(is_prime(i))
#End If
                r.emplace_back(make_pair(i, 1))
            End If
            Return r
        End If
    End Function

    Public Function prime_divisors(ByVal i As Int32) As vector(Of Int32)
        Dim r As vector(Of Int32) = Nothing
        Dim t As vector(Of pair(Of Int32, Int32)) = Nothing
        t = prime_factorization(i)
        If Not t Is Nothing Then
            r = New vector(Of Int32)(t.size())
            For j As Int32 = 0 To t.size() - 1
                r.emplace_back(t(j).first)
            Next
        End If
        Return r
    End Function
End Module
