
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.envs
Imports osi.service.cache

Public Class mapheap_cache_test
    Inherits [case]

    Private Const max_size As UInt64 = 16
    Private Shared ReadOnly retire_ticks As UInt64
    Private Shared ReadOnly enc As Text.Encoding
    Private ReadOnly c As icache(Of String, Byte())

    Shared Sub New()
        retire_ticks = seconds_to_ticks(5)
        enc = Text.Encoding.Unicode()
    End Sub

    Public Sub New()
        mapheap_cache(c, max_size, retire_ticks)
    End Sub

    Private Shared Function key_value(ByVal k As String) As Byte()
        Return enc.GetBytes(strleft(k, 4))
    End Function

    Private Shared Sub assert_key_value(ByVal k As String, ByVal v() As Byte)
        assert_equal(memcmp(v, key_value(k)), 0, k)
    End Sub

    Private Shared Sub rnd_key_value(ByRef k As String, ByRef v() As Byte)
        k = guid_str()
        v = key_value(k)
    End Sub

    Private Shared Function create_data(ByVal count As Int32) As vector(Of pair(Of String, Byte()))
        assert(count > 0)
        Dim r As vector(Of pair(Of String, Byte())) = Nothing
        r = New vector(Of pair(Of String, Byte()))()
        For i As Int32 = 0 To count - 1
            Dim k As String = Nothing
            Dim v() As Byte = Nothing
            rnd_key_value(k, v)
            r.push_back(make_pair(k, v))
        Next
        Return r
    End Function

    Private Function retired_test() As Boolean
        Dim sleep_half_retire_ticks As Action = Sub()
                                                    sleep_ticks(retire_ticks * 4 / 7)
                                                End Sub
        assert(c.empty())
        Dim v As vector(Of pair(Of String, Byte())) = Nothing
        v = create_data(max_size)
        For i As Int32 = 0 To v.size() - 1
            c.set(v(i).first, v(i).second)
        Next
        sleep_half_retire_ticks()
        For i As Int32 = (v.size() >> 1) To v.size() - 1
            assert_true(c.have(v(i).first))
            Dim b() As Byte = Nothing
            assert_true(c.get(v(i).first, b))
            assert_key_value(v(i).first, b)
        Next
        sleep_half_retire_ticks()
        assert_equal(v.size(), c.size())
        For i As Int32 = 0 To (v.size() >> 1) - 1
            assert_true(c.have(v(i).first))
            assert_false(c.get(v(i).first, Nothing))
            assert_equal(v.size() - i - 1, c.size())
        Next
        sleep_half_retire_ticks()
        For i As Int32 = (v.size() >> 1) To v.size() - 1
            assert_true(c.have(v(i).first))
            assert_false(c.get(v(i).first, Nothing))
            assert_equal(v.size() - i - 1, c.size())
        Next
        assert_true(c.empty())
        c.clear()
        Return True
    End Function

    Private Function oversize_test() As Boolean
        assert(c.empty())
        Dim v As vector(Of pair(Of String, Byte())) = Nothing
        v = create_data(max_size << 1)
        For i As Int32 = 0 To 1
            For j As Int32 = 0 To max_size - 1
                Dim k As Int32 = 0
                k = i * max_size + j
                c.set(v(k).first, v(k).second)
                assert_true(c.size() <= max_size)
                assert_true(c.have(v(k).first))
                Dim b() As Byte = Nothing
                assert_true(c.get(v(k).first, b))
                assert_key_value(v(k).first, b)
            Next
            sleep(two_timeslice_length_ms)
        Next

        For i As Int32 = max_size To v.size() - 1
            assert_true(c.have(v(i).first), v(i).first)
            Dim b() As Byte = Nothing
            assert_true(c.get(v(i).first, b), v(i).first)
            assert_key_value(v(i).first, b)
        Next
        assert_equal(c.size(), Convert.ToInt64(max_size))
        c.clear()
        Return True
    End Function

    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        Return retired_test() AndAlso
               oversize_test()
    End Function
End Class
