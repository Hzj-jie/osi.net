
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.device

Public Class manual_pre_generated_device_pool_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const max_count As UInt32 = 1024
        Dim e As mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_test) = Nothing
        e = New mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_test)()
        Dim p As manual_pre_generated_device_pool(Of mock_dev(Of manual_pre_generated_device_pool_test)) = Nothing
        p = manual_pre_generated_device_pool.[New](e, max_count)
        assert_equal(p.free_count(), uint32_0)
        assert_false(p.get(Nothing))
        assert_true(p.limited_max_count())
        For i As UInt32 = uint32_0 To max_count - uint32_1
            assert_true(e.go())
            assert_equal(p.total_count(), i + 1)
        Next
        assert_false(e.go())
        assert_equal(p.total_count(), max_count)
        assert_equal(mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count(), uint32_1)
        Dim ds() As idevice(Of mock_dev(Of manual_pre_generated_device_pool_test)) = Nothing
        ReDim ds(max_count - 1)
        For i As UInt32 = 0 To max_count - uint32_1
            assert_true(p.get(ds(i)))
            assert_equal(p.free_count(), max_count - i - uint32_1)
            If assert_not_nothing(ds(i)) AndAlso assert_not_nothing(ds(i).get()) Then
                assert_equal(ds(i).get().id, i)
            End If
        Next
        assert_false(p.get(Nothing))

        For i As Int32 = 0 To max_count - 1
            If rnd_bool() AndAlso assert_not_nothing(ds(i)) Then
                ds(i).close()
                assert_false(p.release(ds(i)))
            Else
                assert_true(p.release(ds(i)))
            End If
        Next
        assert_equal(CInt(p.free_count()),
                     max_count - mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count() + 1)
        p.close()
        assert_equal(p.free_count(), uint32_0)
        assert_equal(mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count(), max_count + 1)

        Return True
    End Function
End Class
