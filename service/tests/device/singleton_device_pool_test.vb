
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.device

Public Class singleton_device_pool_test
    Inherits [case]

    Private Shared Function run_case(ByVal attach As Boolean) As Boolean
        mock_dev(Of singleton_device_pool_test).reset()
        Dim d As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
        d = mock_dev(Of singleton_device_pool_test).create(attach)
        Dim p As singleton_device_pool(Of mock_dev(Of singleton_device_pool_test)) = Nothing
        p = New singleton_device_pool(Of mock_dev(Of singleton_device_pool_test))(d)
        assert_equal(p.free_count(), uint32_1)
        assert_equal(p.total_count(), uint32_1)
        assert_equal(p.limited_max_count(), False)
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assert_true(p.get(v))
            assert_reference_equal(v, d)
            assert_reference_equal(v.get(), d.get())
            If assert_not_nothing(v.get()) AndAlso assert_not_nothing(d.get()) Then
                assert_equal(v.get().id, d.get().id)
            End If
        Next
        assert_equal(p.free_count(), uint32_1)
        assert_equal(p.total_count(), uint32_1)
        assert_equal(p.limited_max_count(), False)

        d.close()
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assert_false(p.get(v))
        Next
        assert_equal(p.free_count(), uint32_0)
        assert_equal(p.total_count(), uint32_0)
        assert_equal(p.limited_max_count(), False)

        If attach Then
            assert_equal(d.get().close_times(), uint32_1)
        Else
            assert_equal(d.get().close_times(), uint32_0)
        End If

        p.close()
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assert_false(p.get(v))
        Next
        If attach Then
            assert_equal(mock_dev(Of singleton_device_pool_test).closed_instance_count(), uint32_1)
        Else
            assert_equal(mock_dev(Of singleton_device_pool_test).closed_instance_count(), uint32_0)
        End If
        assert_equal(p.free_count(), uint32_0)
        assert_equal(p.total_count(), uint32_0)
        assert_equal(p.limited_max_count(), False)
        If attach Then
            assert_equal(d.get().close_times(), uint32_1)
        Else
            assert_equal(d.get().close_times(), uint32_0)
        End If

        GC.KeepAlive(p)
        p = Nothing
        repeat_gc_collect()
        If attach Then
            assert_equal(d.get().close_times(), uint32_1)
        Else
            assert_equal(d.get().close_times(), uint32_0)
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(True) AndAlso
               run_case(False)
    End Function
End Class
