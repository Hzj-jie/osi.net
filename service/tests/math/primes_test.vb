
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Public NotInheritable Class primes_test
    Inherits [case]

    Private Shared ReadOnly known_primes1() As Int32 = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59,
                                                        61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127,
                                                        131, 137, 139, 149, 151, 157, 163, 167}
    Private Shared ReadOnly known_primes2() As Int32 = {100003, 100019, 100043, 100049, 100057, 100069, 100103, 100109,
                                                        100129, 100151, 100153, 100169, 100183, 100189, 100193, 100207,
                                                        100213, 100237, 100267, 100271, 100279, 100291, 100297, 100313,
                                                        100333, 100343, 100357, 100361, 100363, 100379, 100391, 100393,
                                                        100403, 100411, 100417, 100447, 100459, 100469, 100483, 100493,
                                                        100501, 100511, 100517, 100519, 100523, 100537, 100547, 100549,
                                                        100559, 100591, 100609, 100613, 100621, 100649, 100669, 100673,
                                                        100693, 100699, 100703, 100733, 100741, 100747, 100769, 100787,
                                                        100799, 100801, 100811, 100823, 100829, 100847, 100853, 100907,
                                                        100913, 100927, 100931, 100937, 100943, 100957, 100981, 100987,
                                                        100999, 101009, 101021, 101027, 101051, 101063, 101081, 101089,
                                                        101107, 101111, 101113, 101117, 101119, 101141, 101149, 101159,
                                                        101161, 101173, 101183, 101197, 101203, 101207, 101209, 101221,
                                                        101267, 101273, 101279, 101281, 101287, 101293, 101323, 101333,
                                                        101341, 101347, 101359, 101363, 101377, 101383, 101399, 101411,
                                                        101419, 101429, 101449, 101467, 101477, 101483, 101489, 101501,
                                                        101503, 101513, 101527, 101531, 101533, 101537, 101561, 101573,
                                                        101581, 101599, 101603, 101611, 101627, 101641, 101653, 101663,
                                                        101681, 101693, 101701, 101719, 101723, 101737, 101741, 101747,
                                                        101749, 101771, 101789, 101797, 101807, 101833, 101837, 101839,
                                                        101863, 101869, 101873, 101879, 101891, 101917, 101921, 101929,
                                                        101939, 101957, 101963, 101977, 101987, 101999, 102001, 102013,
                                                        102019, 102023, 102031, 102043, 102059, 102061, 102071, 102077,
                                                        102079, 102101, 102103, 102107, 102121, 102139, 102149, 102161,
                                                        102181, 102191, 102197, 102199, 102203, 102217, 102229, 102233,
                                                        102241, 102251, 102253, 102259, 102293, 102299, 102301, 102317,
                                                        102329, 102337, 102359, 102367, 102397, 102407, 102409, 102433}

    Private Shared Function stupid_is_prime(ByVal x As Int32) As Boolean
        If x < min_uint32 Then
            Return False
        End If
        Return stupid_is_prime(CUInt(x))
    End Function

    Private Shared Function stupid_is_prime(ByVal x As UInt32) As Boolean
        If x <= 1 Then
            Return False
        End If
        For i As Int32 = 2 To CInt(System.Math.Sqrt(x))
            If x Mod i = 0 Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function test_known_primes(ByVal known_primes() As Int32) As Boolean
        For i As Int32 = 0 To array_size_i(known_primes) - 1
            assertion.is_true(is_prime(known_primes(i)))
        Next
        For i As Int32 = 0 To array_size_i(known_primes) - 2
            For j As Int32 = known_primes(i) + 1 To known_primes(i + 1) - 1
                assertion.is_false(is_prime(j))
            Next
        Next
        Return True
    End Function

    Private Shared Function test_rnd_prime() As Boolean
        For i As Int32 = 0 To 1024 * 1024 - 1
            Dim x As Int32 = rnd_int(2, max_int32)
            assertion.equal(is_prime(x), stupid_is_prime(x))
            Dim y As UInt32 = rnd_uint(2, max_uint32)
            assertion.equal(is_prime(y), stupid_is_prime(y))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        assertion.equal(primes.precalculated_count, CUInt(9592))
        Return test_known_primes(known_primes1) AndAlso
               test_known_primes(known_primes2) AndAlso
               test_rnd_prime()
    End Function
End Class
