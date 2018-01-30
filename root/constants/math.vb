
Option Explicit On
Option Infer Off
Option Strict On

Public Module _math
    Private ReadOnly _doubled_prime_sequence_int32_log3() As UInt32

    Sub New()
        _doubled_prime_sequence_int32_log3 = (Function() As UInt32()
                                                  Dim i() As UInt32 = Nothing
                                                  i = doubled_prime_sequence_int32()
                                                  Dim r() As UInt32 = Nothing
                                                  ReDim r(i.Length() - 1)
                                                  For j As Int32 = 0 To i.Length() - 1
                                                      r(j) = CUInt(Math.Log(i(j), 3))
                                                  Next
                                                  Return r
                                              End Function)()
    End Sub

    ' Return a set of prime-numbers in the range of int32, each one is the smallest prime larger than the twice of its
    ' predecessor. The first one is three.
    Public Function doubled_prime_sequence_int32() As UInt32()
        Return New UInt32() {3, 7, 17, 37, 79, 163, 331, 673, 1361, 2729, 5471, 10949, 21911, 43853, 87719, 175447,
                             350899, 701819, 1403641, 2807303, 5614657, 11229331, 22458671, 44917381, 89834777,
                             179669557, 359339171, 718678369, 1437356741}
    End Function

    Public Function doubled_prime_sequence_int32_log3() As UInt32()
        Return DirectCast(_doubled_prime_sequence_int32_log3.Clone(), UInt32())
    End Function
End Module
