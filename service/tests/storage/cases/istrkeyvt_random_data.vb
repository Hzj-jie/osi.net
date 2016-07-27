
Imports System.DateTime
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt
Imports osi.service.storage

Public Structure istrkeyvt_random_data(Of _KEY_LENGTH_LOW As _int64,
                                          _KEY_LENGTH_UP As _int64,
                                          _BYTES_LENGTH_LOW As _int64,
                                          _BYTES_LENGTH_UP As _int64)
    Private Shared ReadOnly key_length_low As UInt32
    Private Shared ReadOnly key_length_up As UInt32
    Private Shared ReadOnly bytes_length_low As UInt32
    Private Shared ReadOnly bytes_length_up As UInt32
    Private Shared ReadOnly timestamp_low As Int64
    Private Shared ReadOnly timestamp_diff As UInt32

    Shared Sub New()
        key_length_low = +(alloc(Of _KEY_LENGTH_LOW)())
        key_length_up = +(alloc(Of _KEY_LENGTH_UP)()) + 1
        bytes_length_low = +(alloc(Of _BYTES_LENGTH_LOW)())
        bytes_length_up = +(alloc(Of _BYTES_LENGTH_UP)()) + 1
        timestamp_low = (Now().to_timestamp() + rnd_int64(-2048, -1024 + 1))
        timestamp_diff = (Now().to_timestamp() + rnd_int64(1024, 2048 + 1)) + 1 - timestamp_low
    End Sub

    'for quick access
    Public Function rand_key() As String
        Return rndenchars(rnd_int(key_length_low, key_length_up))
    End Function

    Public Function rand_bytes() As Byte()
        Return next_bytes(rnd_int(bytes_length_low, bytes_length_up))
    End Function

    Public Function fake_rand_key() As String
        Return fake_rnd_en_chars(rnd_uint(key_length_low, key_length_up))
    End Function

    Public Function is_fake_rand_key(ByVal k As String) As Boolean
        Return is_fake_rnd_en_chars(k)
    End Function

    Public Function fake_rand_bytes(ByVal key As String) As Byte()
        Return fake_next_bytes(key, bytes_length_low, bytes_length_up)
    End Function

    Public Function is_fake_rand_bytes(ByVal key As String, ByVal b() As Byte) As Boolean
        Return is_fake_next_bytes_chained(key, b, bytes_length_low, bytes_length_up)
    End Function

    Public Function fake_rand_timestamp(ByVal key As String) As Int64
        Return fake_rnd_uint(0, timestamp_diff, key) + timestamp_low
    End Function

    Public Function is_fake_rand_timestamp(ByVal key As String, ByVal ts As Int64) As Boolean
        Return is_fake_rnd_uint(CUInt(ts - timestamp_low), 0, timestamp_diff, key)
    End Function
End Structure
