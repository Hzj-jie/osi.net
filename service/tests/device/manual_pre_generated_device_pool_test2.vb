﻿
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.device

Public Class manual_pre_generated_device_pool_test2
    Inherits multithreading_case_wrapper

    Private Const thread_count As UInt16 = 64
    Private Const size As UInt64 = 16384

    Public Sub New()
        MyBase.New(repeat(New manual_pre_generated_device_pool_case2(), size), thread_count)
    End Sub

    Private Class manual_pre_generated_device_pool_case2
        Inherits [case]

        Private ReadOnly e As mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_case2)
        Private ReadOnly p As manual_pre_generated_device_pool(Of mock_dev(Of manual_pre_generated_device_pool_case2))

        Public Sub New()
            e = New mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_case2)()
            p = manual_pre_generated_device_pool.[New](e)
        End Sub

        Public Overrides Function prepare() As Boolean
            mock_dev(Of manual_pre_generated_device_pool_case2).reset()
            Return MyBase.prepare()
        End Function

        Public Overrides Function run() As Boolean
            Dim r As idevice(Of mock_dev(Of manual_pre_generated_device_pool_case2)) = Nothing
            assert_true(e.go())
            assert_more_or_equal(p.total_count(), uint32_1)
            assert_true(p.get(r))
            assert_not_nothing(r)
            assert_true(p.release(r))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            p.close()
            assert_equal(mock_dev(Of manual_pre_generated_device_pool_case2).closed_instance_count(),
                         thread_count * size)
            Return MyBase.finish()
        End Function
    End Class
End Class
