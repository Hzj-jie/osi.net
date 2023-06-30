
Imports osi.root.utt
Imports osi.service.device

Public Class manual_device_exporter_adapter_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim c As Int32 = 0
        Dim started As Boolean = False
        Dim stopped As Boolean = False
        Dim exported As Int32 = 0
        Dim i As imanual_device_exporter(Of Int32) = Nothing
        i = New manual_device_exporter(Of Int32)()
        Dim j As imanual_device_exporter(Of Double) = Nothing
        j = manual_device_exporter_adapter.[New](i, Function(k As Int32) As Double
                                                        Return CDbl(k)
                                                    End Function)
        AddHandler j.new_device_exported, Sub(k As idevice(Of Double), ByRef export_result As Boolean)
                                              assertion.equal(k.get(), CDbl(c))
                                              export_result = True
                                              exported += 1
                                          End Sub
        AddHandler j.after_start, Sub()
                                      assertion.is_true(i.started())
                                      started = True
                                  End Sub
        AddHandler j.after_stop, Sub()
                                     assertion.is_true(i.stopped())
                                     stopped = True
                                 End Sub
        assertion.is_true(j.start())
        assertion.is_true(j.started())
        assertion.is_false(j.stopped())
        assertion.is_true(started)

        For c = 0 To 1000
            assertion.is_true(i.inject(c.make_device()))
            assertion.equal(exported, c + 1)
        Next

        assertion.is_true(j.stop())
        assertion.is_true(j.stopped())
        assertion.is_false(j.started())
        assertion.is_true(stopped)

        Return True
    End Function
End Class
