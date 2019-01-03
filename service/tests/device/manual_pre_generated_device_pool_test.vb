
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
        assertion.equal(p.free_count(), uint32_0)
        assertion.is_false(p.get(Nothing))
        assertion.is_true(p.limited_max_count())
        For i As UInt32 = uint32_0 To max_count - uint32_1
            assertion.is_true(e.go())
            assertion.equal(p.total_count(), i + 1)
        Next
        assertion.is_false(e.go())
        assertion.equal(p.total_count(), max_count)
        assertion.equal(mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count(), uint32_1)
        Dim ds() As idevice(Of mock_dev(Of manual_pre_generated_device_pool_test)) = Nothing
        ReDim ds(max_count - 1)
        For i As UInt32 = 0 To max_count - uint32_1
            assertion.is_true(p.get(ds(i)))
            assertion.equal(p.free_count(), max_count - i - uint32_1)
            If assertion.is_not_null(ds(i)) AndAlso assertion.is_not_null(ds(i).get()) Then
                assertion.equal(ds(i).get().id, i)
            End If
        Next
        assertion.is_false(p.get(Nothing))

        For i As Int32 = 0 To max_count - 1
            If rnd_bool() AndAlso assertion.is_not_null(ds(i)) Then
                ds(i).close()
                assertion.is_false(p.release(ds(i)))
            Else
                assertion.is_true(p.release(ds(i)))
            End If
        Next
        assertion.equal(CInt(p.free_count()),
                     max_count - mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count() + 1)
        p.close()
        assertion.equal(p.free_count(), uint32_0)
        assertion.equal(mock_dev(Of manual_pre_generated_device_pool_test).closed_instance_count(), max_count + 1)

        Return True
    End Function
End Class
