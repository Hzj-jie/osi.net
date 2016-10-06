
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _fake_rnd
    Private ReadOnly str_prefix As String
    Private ReadOnly str_prefix_len As UInt32
    Private ReadOnly int_bit As UInt32
    Private ReadOnly bytes_prefix() As Byte
    Private ReadOnly bytes_prefix_len As Int32
    Private ReadOnly port As atomic_int

    Sub New()
        assert(is_cloneable(Of Byte())())
        str_prefix = rnd_en_chars(rnd_int(2, 4 + 1))
        str_prefix_len = strlen(str_prefix)
        int_bit = 0
        For i As Int32 = rnd_int(2, 4 + 1) - 1 To 0 Step -1
            Dim t As UInt32 = 0
            t = (uint32_1 << rnd_int(0, 31 + 1))
            If (int_bit And t) = 0 Then
                int_bit += t
            Else
                i -= 1
            End If
        Next
        assert(int_bit > 0)
        bytes_prefix = next_bytes(rnd_uint(2, 4 + 1))
        bytes_prefix_len = array_size(bytes_prefix)
        port = New atomic_int(10000)
    End Sub

    Public Function fake_rnd_uint() As UInt32
        Return ((rnd_uint(min_uint32, max_uint32)) Or int_bit)
    End Function

    Public Function is_fake_rnd_uint(ByVal i As UInt32) As Boolean
        Return ((i And int_bit) = int_bit)
    End Function

    Public Function fake_rnd_en_chars(ByVal l As UInt32) As String
        If l = 0 Then
            Return Nothing
        ElseIf l < str_prefix_len Then
            Return strleft(str_prefix, l)
        ElseIf l = str_prefix_len Then
            Return str_prefix
        Else
            Return strcat(str_prefix, rnd_en_chars(l - str_prefix_len))
        End If
    End Function

    Public Function is_fake_rnd_en_chars(ByVal s As String) As Boolean
        Dim l As UInt32 = 0
        l = strlen(s)
        Return l = 0 OrElse
               strsame(s, str_prefix, min(l, str_prefix_len))
    End Function

    Public Function fake_next_bytes(ByVal l As UInt32) As Byte()
        If l = 0 Then
            Return Nothing
        ElseIf l < bytes_prefix_len Then
            Dim r() As Byte = Nothing
            ReDim r(l - uint32_1)
            memcpy(r, bytes_prefix, l)
            Return r
        ElseIf l = bytes_prefix_len Then
            Return copy(bytes_prefix)
        Else
            Dim r() As Byte = Nothing
            ReDim r(l - uint32_1)
            assert(next_bytes(r))
            memcpy(r, bytes_prefix, bytes_prefix_len)
            If l < (bytes_prefix_len << 1) Then
                memcpy(r, bytes_prefix_len, bytes_prefix, 0, l - bytes_prefix_len)
            Else
                memcpy(r, l - bytes_prefix_len, bytes_prefix, 0, bytes_prefix_len)
            End If
            Return r
        End If
    End Function

    Public Function is_fake_next_bytes(ByVal b() As Byte,
                                       Optional ByVal start As UInt32 = 0,
                                       Optional ByVal len As UInt32 = max_uint32) As Boolean
        If start > array_size(b) Then
            Return False
        Else
            If len = max_uint32 Then
                len = array_size(b) - start
            ElseIf start + len > array_size(b) Then
                Return False
            End If
            If len = 0 Then
                Return True
            ElseIf memcmp(b, start, bytes_prefix, uint32_0, min(len, bytes_prefix_len)) <> 0 Then
                Return False
            ElseIf len <= bytes_prefix_len Then
                Return True
            ElseIf len < (bytes_prefix_len << 1) Then
                Return memcmp(b, bytes_prefix_len + start, bytes_prefix, uint32_0, len - bytes_prefix_len) = 0
            Else
                Return memcmp(b, start + len - bytes_prefix_len, bytes_prefix, uint32_0, bytes_prefix_len) = 0
            End If
        End If
    End Function

    'return a fake random uint in [min, max) with seed
    Public Function fake_rnd_uint(ByVal min As UInt32, ByVal max As UInt32, ByVal seed As UInt32) As UInt32
        If min > max Then
            Return max
        ElseIf min = max OrElse min = max - 1 Then
            Return min
        Else
            assert(max > 0)
            max -= 1
            Return min + (seed Mod (max - min))
        End If
    End Function

    Public Function is_fake_rnd_uint(ByVal i As UInt32,
                                     ByVal min As UInt32,
                                     ByVal max As UInt32,
                                     ByVal seed As UInt32) As Boolean
        Return i = fake_rnd_uint(min, max, seed)
    End Function

    Public Function fake_rnd_uint(Of T)(ByVal min As UInt32, ByVal max As UInt32, ByVal seed As T) As UInt32
        Return fake_rnd_uint(min, max, signing(seed))
    End Function

    Public Function is_fake_rnd_uint(Of T)(ByVal i As UInt32,
                                           ByVal min As UInt32,
                                           ByVal max As UInt32,
                                           ByVal seed As T) As Boolean
        Return i = fake_rnd_uint(min, max, seed)
    End Function

    Public Function fake_next_bytes(ByVal seed As UInt32, ByVal len_low As UInt32, ByVal len_up As UInt32) As Byte()
        Return fake_next_bytes(fake_rnd_uint(len_low, len_up, seed))
    End Function

    Public Function is_fake_next_bytes(ByVal seed As UInt32,
                                       ByVal b() As Byte,
                                       ByVal len_low As UInt32,
                                       ByVal len_up As UInt32) As Boolean
        Return is_fake_rnd_uint(array_size(b), len_low, len_up, seed) AndAlso
               is_fake_next_bytes(b)
    End Function

    Public Function fake_next_bytes(Of T)(ByVal seed As T, ByVal len_low As UInt32, ByVal len_up As UInt32) As Byte()
        Return fake_next_bytes(fake_rnd_uint(len_low, len_up, signing(seed)))
    End Function

    Public Function is_fake_next_bytes(Of T)(ByVal seed As T,
                                             ByVal b() As Byte,
                                             ByVal len_low As UInt32,
                                             ByVal len_up As UInt32) As Boolean
        Return is_fake_next_bytes(signing(seed), b, len_low, len_up)
    End Function

    Public Function is_fake_next_bytes_chained(Of T)(ByVal seed As T,
                                                     ByVal b() As Byte,
                                                     ByVal len_low As UInt32,
                                                     ByVal len_up As UInt32) As Boolean
        Dim l As Int32 = 0
        l = array_size(b)
        Dim sl As Int32 = 0
        sl = fake_rnd_uint(len_low, len_up, signing(seed))
        If l = 0 Then
            Return sl = 0
        ElseIf l Mod sl <> 0 Then
            Return False
        Else
            For i As Int32 = 0 To l - 1 Step sl
                If Not is_fake_next_bytes(b, i, sl) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function rnd_port() As UInt16
        Dim p As UInt16 = 0
        While True
            p = cast(Of UInt16)(port.increment() - 1)
            If local_port_available(p) Then
                Return p
            End If
        End While
        assert(False)
        Return 0
    End Function
End Module
