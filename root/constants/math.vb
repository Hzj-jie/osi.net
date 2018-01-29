
Option Explicit On
Option Infer Off
Option Strict On

Public Module _math
    ' Return a set of prime-numbers in the range of int32, each one is the smallest prime larger than the twice of its
    ' predecessor. The first one is three.
    Public Function doubled_prime_sequence_int32() As UInt32()
        Return New UInt32() {3, 7, 17, 37, 79, 163, 331, 673, 1361, 2729, 5471, 10949, 21911, 43853, 87719, 175447,
                             350899, 701819, 1403641, 2807303, 5614657, 11229331, 22458671, 44917381, 89834777,
                             179669557, 359339171, 718678369, 1437356741}
    End Function
End Module
