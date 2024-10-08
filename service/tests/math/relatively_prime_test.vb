
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Public NotInheritable Class relatively_prime_test
    Inherits [case]

    Private Shared Function stupid_relatively_prime(ByVal x As Int32, ByVal y As Int32) As Boolean
        If x = 1 OrElse y = 1 Then
            Return True
        End If
        If x Mod y = 0 OrElse y Mod x = 0 Then
            Return False
        End If
        For i As UInt32 = 0 To primes.precalculated_count - uint32_1
            If primes.precalculated(i) > x OrElse
               primes.precalculated(i) > y Then
                Exit For
            End If
            If x Mod primes.precalculated(i) = 0 AndAlso
               y Mod primes.precalculated(i) = 0 Then
                Return False
            ElseIf x Mod primes.precalculated(i) = 0 Then
                While x Mod primes.precalculated(i) = 0
                    x \= CInt(primes.precalculated(i))
                End While
            ElseIf y Mod primes.precalculated(i) = 0 Then
                While y Mod primes.precalculated(i) = 0
                    y \= CInt(primes.precalculated(i))
                End While
            End If
        Next

        Return x = 1 OrElse y = 1 OrElse
               (x Mod y <> 0 AndAlso y Mod x <> 0)
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 256 - 1
            Dim x As Int32 = 0
            Dim y As Int32 = 0
            x = rnd_int(0, max_int32)
            y = rnd_int(0, max_int32)
            assertion.equal(relatively_prime.between(x, y), stupid_relatively_prime(x, y), x, character.tab, y)
        Next
        Return True
    End Function
End Class
