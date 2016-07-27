
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
                                              assert_equal(k.get(), CDbl(c))
                                              export_result = True
                                              exported += 1
                                          End Sub
        AddHandler j.after_start, Sub()
                                      assert_true(i.started())
                                      started = True
                                  End Sub
        AddHandler j.after_stop, Sub()
                                     assert_true(i.stopped())
                                     stopped = True
                                 End Sub
        assert_true(j.start())
        assert_true(j.started())
        assert_false(j.stopped())
        assert_true(started)

        For c = 0 To 1000
            assert_true(i.inject(c.make_device()))
            assert_equal(exported, c + 1)
        Next

        assert_true(j.stop())
        assert_true(j.stopped())
        assert_false(j.started())
        assert_true(stopped)

        Return True
    End Function
End Class
