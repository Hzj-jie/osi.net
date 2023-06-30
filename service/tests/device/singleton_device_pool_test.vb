
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
        assertion.equal(p.free_count(), uint32_1)
        assertion.equal(p.total_count(), uint32_1)
        assertion.equal(p.limited_max_count(), False)
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assertion.is_true(p.get(v))
            assertion.reference_equal(v, d)
            assertion.reference_equal(v.get(), d.get())
            If assertion.is_not_null(v.get()) AndAlso assertion.is_not_null(d.get()) Then
                assertion.equal(v.get().id, d.get().id)
            End If
        Next
        assertion.equal(p.free_count(), uint32_1)
        assertion.equal(p.total_count(), uint32_1)
        assertion.equal(p.limited_max_count(), False)

        d.close()
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assertion.is_false(p.get(v))
        Next
        assertion.equal(p.free_count(), uint32_0)
        assertion.equal(p.total_count(), uint32_0)
        assertion.equal(p.limited_max_count(), False)

        If attach Then
            assertion.equal(d.get().close_times(), uint32_1)
        Else
            assertion.equal(d.get().close_times(), uint32_0)
        End If

        p.close()
        For i As Int32 = 0 To 4096 - 1
            Dim v As idevice(Of mock_dev(Of singleton_device_pool_test)) = Nothing
            assertion.is_false(p.get(v))
        Next
        If attach Then
            assertion.equal(mock_dev(Of singleton_device_pool_test).closed_instance_count(), uint32_1)
        Else
            assertion.equal(mock_dev(Of singleton_device_pool_test).closed_instance_count(), uint32_0)
        End If
        assertion.equal(p.free_count(), uint32_0)
        assertion.equal(p.total_count(), uint32_0)
        assertion.equal(p.limited_max_count(), False)
        If attach Then
            assertion.equal(d.get().close_times(), uint32_1)
        Else
            assertion.equal(d.get().close_times(), uint32_0)
        End If

        GC.KeepAlive(p)
        p = Nothing
        garbage_collector.repeat_collect()
        If attach Then
            assertion.equal(d.get().close_times(), uint32_1)
        Else
            assertion.equal(d.get().close_times(), uint32_0)
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(True) AndAlso
               run_case(False)
    End Function
End Class
