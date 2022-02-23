
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _prime_divisors
    <Extension()> Public Function prime_factorization(ByVal i As Int32) As vector(Of pair(Of UInt32, Int32))
        If i < 0 Then
            Return Nothing
        End If
        Return prime_factorization(CUInt(i))
    End Function

    <Extension()> Public Function prime_factorization(ByVal i As UInt32) As vector(Of pair(Of UInt32, Int32))
        If i < primes.precalculated(0) Then
            Return New vector(Of pair(Of UInt32, Int32))()
        End If
        Dim r As vector(Of pair(Of UInt32, Int32)) = Nothing
        r = New vector(Of pair(Of UInt32, Int32))()
        For j As UInt32 = 0 To primes.precalculated_count - uint32_1
#If DEBUG Then
            assert(primes.precalculated(j) <= i)
#End If
            If primes.precalculated(j) = i Then
                r.emplace_back(pair.of(i, 1))
                i = 1
            ElseIf i Mod primes.precalculated(j) = 0 Then
                Dim p As pair(Of UInt32, Int32) = Nothing
                p = pair.of(primes.precalculated(j), 0)
                While i Mod primes.precalculated(j) = 0
                    p.second += 1
                    i \= primes.precalculated(j)
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
            r.emplace_back(pair.of(i, 1))
        End If
        Return r
    End Function

    <Extension()> Public Function prime_divisors(ByVal i As Int32) As vector(Of UInt32)
        If i < 0 Then
            Return Nothing
        End If
        Return prime_divisors(CUInt(i))
    End Function

    <Extension()> Public Function prime_divisors(ByVal i As UInt32) As vector(Of UInt32)
        Dim t As vector(Of pair(Of UInt32, Int32)) = Nothing
        t = prime_factorization(i)
        If t Is Nothing Then
            Return Nothing
        End If
        Return t.stream().
                 map(Function(ByVal x As pair(Of UInt32, Int32)) As UInt32
                         assert(x IsNot Nothing)
                         Return x.first
                     End Function).
                 collect_to(Of vector(Of UInt32))()
    End Function
End Module
