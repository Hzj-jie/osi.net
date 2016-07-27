
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.device

Public Class auto_pre_generated_device_pool_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const max_count As UInt32 = 128
        Dim p As auto_pre_generated_device_pool(Of mock_dev(Of auto_pre_generated_device_pool_test)) = Nothing
        p = auto_pre_generated_device_pool.[New](mock_device_creator(Of auto_pre_generated_device_pool_test).exporter(),
                                                 max_count)
        assert_true(timeslice_sleep_wait_when(Function() p.free_count() < max_count, seconds_to_milliseconds(10)))
        Dim ds() As idevice(Of mock_dev(Of auto_pre_generated_device_pool_test)) = Nothing
        ReDim ds(max_count - 1)
        For i As UInt32 = 0 To max_count - uint32_1
            assert_true(p.get(ds(i)))
            assert_equal(p.free_count(), max_count - i - uint32_1)
            If assert_not_nothing(ds(i)) AndAlso assert_not_nothing(ds(i).get()) Then
                For j As Int32 = 0 To i - 1
                    If assert_not_nothing(ds(j)) AndAlso assert_not_nothing(ds(j).get()) Then
                        assert_not_equal(ds(i).get().id, ds(j).get().id)
                    End If
                Next
            End If
        Next
        assert_false(p.get(Nothing))

        For i As UInt32 = 0 To max_count - uint32_1
            If rnd_bool() AndAlso assert_not_nothing(ds(i)) Then
                ds(i).close()
                assert_false(p.release(ds(i)))
            Else
                assert_true(p.release(ds(i)))
            End If
        Next
        assert_true(timeslice_sleep_wait_when(Function() p.free_count() < max_count, seconds_to_milliseconds(10)))

        Dim before_closed As Int32 = 0
        before_closed = mock_dev(Of auto_pre_generated_device_pool_test).closed_instance_count()
        p.close()
        assert_true(timeslice_sleep_wait_until(Function() p.stopped(), seconds_to_milliseconds(1)))
        assert_equal(mock_dev(Of auto_pre_generated_device_pool_test).closed_instance_count(),
                     max_count + before_closed)
        assert_equal(p.free_count(), uint32_0)
        Return True
    End Function
End Class
