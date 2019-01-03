
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.device

Public Class manual_pre_generated_device_pool_test3
    Inherits multithreading_case_wrapper

    Private Const thread_count As UInt16 = 64
    Private Const size As UInt64 = 16384

    Public Sub New()
        MyBase.New(repeat(New manual_pre_generated_device_pool_case3(), size), thread_count)
    End Sub

    Private Class manual_pre_generated_device_pool_case3
        Inherits [case]

        Private ReadOnly e As mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_case3)
        Private ReadOnly p As manual_pre_generated_device_pool(Of mock_dev(Of manual_pre_generated_device_pool_case3))

        Public Sub New()
            e = New mock_dev_manual_device_exporter(Of manual_pre_generated_device_pool_case3)()
            p = New manual_pre_generated_device_pool(Of mock_dev(Of manual_pre_generated_device_pool_case3)) _
                                                    (e, CUInt(1024))
        End Sub

        Public Overrides Function prepare() As Boolean
            mock_dev(Of manual_pre_generated_device_pool_case3).reset()
            Return MyBase.prepare()
        End Function

        Public Overrides Function run() As Boolean
            Dim r As idevice(Of mock_dev(Of manual_pre_generated_device_pool_case3)) = Nothing
            If rnd_bool() Then
                e.go()
            End If
            If Not p.get(r) Then
                Return True
            End If
            assertion.is_not_null(r)
            If rnd_bool() Then
                r.close()
                assertion.is_false(p.release(r))
            Else
                assertion.is_true(p.release(r))
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.more(mock_dev(Of manual_pre_generated_device_pool_case3).closed_instance_count(),
                        thread_count * size / 2 / 1.2)
            p.close()
            assertion.more(mock_dev(Of manual_pre_generated_device_pool_case3).closed_instance_count(),
                        thread_count * size / 2 / 1.1)
            Return MyBase.finish()
        End Function
    End Class
End Class
