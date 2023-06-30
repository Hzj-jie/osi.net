
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.selector

Public Class auto_device_exporter_async_adapter_test
    Inherits [case]

    Private Shared Function basic_case() As Boolean
        Dim e As auto_device_exporter(Of mock_dev(Of auto_device_exporter_async_adapter_test)) = Nothing
        e = auto_device_exporter.[New](New mock_device_async_creator(Of auto_device_exporter_async_adapter_test)())
        Dim count As Int32 = 0
        AddHandler e.new_device_exported, Sub(d As idevice(Of mock_dev(Of auto_device_exporter_async_adapter_test)),
                                              ByRef export_result As Boolean)
                                              assertion.is_false(d.GetType().is(GetType(device_adapter(Of ,))))
                                              assertion.is_true(d.GetType().is(GetType(delegate_device(Of ))))
                                              export_result = True
                                              count += 1
                                          End Sub
        assertion.is_true(e.start())
        e.require()
        assertion.is_true(timeslice_sleep_wait_when(Function() count = 0, seconds_to_milliseconds(1)))
        assertion.is_true(e.stop())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return basic_case()
    End Function
End Class
