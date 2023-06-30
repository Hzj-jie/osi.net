
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.device

Public Class delay_generate_device_pool_test
    Inherits [case]

    Public Overrides Function prepare() As Boolean
        mock_dev(Of delay_generate_device_pool_test).reset()
        Return MyBase.prepare()
    End Function

    Public Overrides Function run() As Boolean
        Const max_count As UInt32 = 128
        Dim p As delay_generate_device_pool(Of mock_dev(Of delay_generate_device_pool_test)) = Nothing
        p = delay_generate_device_pool.[New](New mock_device_creator(Of delay_generate_device_pool_test)(), max_count)
        assertion.equal(p.limited_max_count(), True)
        assertion.equal(p.total_count(), uint32_0)
        assertion.equal(p.free_count(), uint32_0)
        Dim ds() As idevice(Of mock_dev(Of delay_generate_device_pool_test)) = Nothing
        ReDim ds(max_count - 1)
        Dim last_round_closed_count As Int32 = 0
        last_round_closed_count = max_count
        For round As Int32 = 0 To 1
            For i As Int32 = 0 To max_count - 1
                assertion.is_true(p.get(ds(i)))
                If assertion.is_not_null(ds(i)) AndAlso assertion.is_not_null(ds(i).get()) Then
                    If round = 0 Then
                        assertion.equal(ds(i).get().id,
                                     i + mock_dev(Of delay_generate_device_pool_test).closed_instance_count())
                    ElseIf i > 0 AndAlso assertion.is_not_null(ds(i - 1)) AndAlso assertion.is_not_null(ds(i - 1).get()) Then
                        assertion.is_true(ds(i).get().id > ds(i - 1).get().id)
                    End If
                End If
                assertion.equal(CInt(p.total_count()),
                             max(max_count - last_round_closed_count, i + 1))
                assertion.equal(CInt(p.free_count()), max(max_count - last_round_closed_count - i - 1, 0))
            Next
            assertion.is_false(p.get(Nothing))
            assertion.equal(p.total_count(), CUInt(max_count))
            assertion.equal(p.free_count(), uint32_0)

            last_round_closed_count = 0
            For i As Int32 = 0 To max_count - 1
                If assertion.is_not_null(ds(i)) AndAlso
                   rnd_bool() Then
                    ds(i).close()
                    assertion.is_false(p.release(ds(i)))
                    last_round_closed_count += 1
                Else
                    assertion.is_true(p.release(ds(i)))
                End If
                assertion.equal(CInt(p.total_count()), max_count - last_round_closed_count)
                assertion.equal(CInt(p.free_count()), i - last_round_closed_count + 1)
            Next
        Next

        Dim expected_closed As UInt32 = 0
        expected_closed = mock_dev(Of delay_generate_device_pool_test).closed_instance_count() + p.free_count()
        p.close()
        assertion.equal(mock_dev(Of delay_generate_device_pool_test).closed_instance_count(),
                     expected_closed)
        assertion.equal(p.free_count(), uint32_0)
        Return True
    End Function
End Class
