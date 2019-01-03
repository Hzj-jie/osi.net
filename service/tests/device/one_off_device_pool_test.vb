
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device

Public Class one_off_device_pool_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const max_count As UInt32 = 128
        Const max_round As Int32 = 2
        Dim p As one_off_device_pool(Of mock_dev(Of one_off_device_pool_test)) = Nothing
        p = one_off_device_pool.[New](New mock_device_creator(Of one_off_device_pool_test)(), max_count)
        assertion.equal(p.limited_max_count(), True)
        assertion.equal(p.total_count(), uint32_0)
        assertion.equal(p.free_count(), uint32_0)
        Dim ds() As idevice(Of mock_dev(Of one_off_device_pool_test)) = Nothing
        ReDim ds(max_count - 1)
        For round As Int32 = 0 To max_round - 1
            For i As Int32 = 0 To max_count - 1
                assertion.is_true(p.get(ds(i)))
                If assertion.is_not_null(ds(i)) AndAlso assertion.is_not_null(ds(i).get()) Then
                    assertion.equal(ds(i).get().id, i + round * max_count)
                End If
                assertion.equal(CInt(p.total_count()), i + 1)
                assertion.equal(p.free_count(), uint32_0)
            Next
            assertion.is_false(p.get(Nothing))
            assertion.equal(p.total_count(), CUInt(max_count))
            assertion.equal(p.free_count(), uint32_0)

            For i As Int32 = 0 To max_count - 1
                If assertion.is_not_null(ds(i)) AndAlso
                   rnd_bool() Then
                    ds(i).close()
                    assertion.is_false(p.release(ds(i)))
                Else
                    assertion.is_true(p.release(ds(i)))
                End If
                assertion.equal(CInt(p.total_count()), max_count - i - 1)
                assertion.equal(p.free_count(), uint32_0)
            Next
        Next

        p.close()
        assertion.equal(mock_dev(Of one_off_device_pool_test).closed_instance_count(), max_count * max_round)
        assertion.equal(p.free_count(), uint32_0)
        Return True
    End Function
End Class
