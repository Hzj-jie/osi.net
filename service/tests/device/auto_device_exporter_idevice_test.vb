
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.device

Public Class auto_device_exporter_idevice_test
    Inherits [case]

    Private Shared Function no_concurrent_generation_case() As Boolean
        mock_dev(Of auto_device_exporter_idevice_test).reset()
        Const count As UInt32 = 100
        Dim d As auto_device_exporter(Of mock_dev(Of auto_device_exporter_idevice_test)) = Nothing
        d = auto_device_exporter.[New](New mock_device_creator(Of auto_device_exporter_idevice_test)(),
                                       max_concurrent_generations:=1)
        Dim c As UInt32 = 0
        AddHandler d.new_device_exported, Sub(i As idevice(Of mock_dev(Of auto_device_exporter_idevice_test)),
                                              ByRef export_result As Boolean)
                                              If assert_not_nothing(i) AndAlso assert_not_nothing(i.get()) Then
                                                  assert_equal(i.get().id, c)
                                              End If
                                              c += uint32_1
                                              export_result = True
                                          End Sub
        Dim started As Boolean = False
        Dim stopped As Boolean = False
        AddHandler d.after_start, Sub()
                                      started = True
                                  End Sub
        AddHandler d.after_stop, Sub()
                                     stopped = True
                                 End Sub
        assert_false(d.started())
        d.require(count)
        ' d has not been started yet
        assert_equal(c, uint32_0)
        assert_equal(d.exported(), uint32_0)
        d.start()
        'd.wait_for_start() ' Start procedure is synchronized.
        assert_true(started)
        assert_true(d.started())
        assert_false(d.stopping())
        assert_false(d.stopped())
        assert_true(timeslice_sleep_wait_until(Function() c = count, seconds_to_milliseconds(10)))
        assert_equal(c, count)
        assert_equal(d.exported(), count)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
        d.require(count)
        assert_true(timeslice_sleep_wait_until(Function() c = (count << 1), seconds_to_milliseconds(10)))
        assert_equal(c, count << 1)
        assert_equal(d.exported(), count << 1)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
        d.stop()
        d.wait_for_stop()
        assert_true(d.stopped())
        assert_true(timeslice_sleep_wait_until(Function() stopped, seconds_to_milliseconds(10)))
        d.require(count)
        assert_false(timeslice_sleep_wait_when(Function() c = (count << 1), seconds_to_milliseconds(10)))
        assert_equal(c, count << 1)
        assert_equal(d.exported(), count << 1)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
        Return True
    End Function

    Private Shared Function concurrent_generation_case() As Boolean
        mock_dev(Of auto_device_exporter_idevice_test).reset()
        Const count As UInt32 = 100
        Dim d As auto_device_exporter(Of mock_dev(Of auto_device_exporter_idevice_test)) = Nothing
        d = auto_device_exporter.[New](New mock_device_creator(Of auto_device_exporter_idevice_test))
        Dim c As atomic_uint = Nothing
        c = New atomic_uint()
        AddHandler d.new_device_exported, Sub(i As idevice(Of mock_dev(Of auto_device_exporter_idevice_test)),
                                              ByRef export_result As Boolean)
                                              c.increment()
                                              export_result = True
                                          End Sub
        assert_false(d.started())
        d.require(count)
        ' d has not been started yet
        assert_equal(+c, uint32_0)
        assert_equal(d.exported(), uint32_0)
        d.start()
        assert_true(d.started())
        assert_false(d.stopping())
        assert_false(d.stopped())
        assert_true(timeslice_sleep_wait_until(Function() +c = count, seconds_to_milliseconds(10)))
        assert_equal(+c, count)
        assert_equal(d.exported(), count)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        d.require(count)
        assert_true(timeslice_sleep_wait_until(Function() +c = (count << 1), seconds_to_milliseconds(10)))
        assert_equal(+c, count << 1)
        assert_equal(d.exported(), count << 1)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        d.stop()
        d.wait_for_stop()
        assert_true(d.stopped())
        d.require(count)
        assert_false(timeslice_sleep_wait_when(Function() +c = (count << 1), seconds_to_milliseconds(10)))
        assert_equal(+c, count << 1)
        assert_equal(d.exported(), count << 1)
        assert_equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return no_concurrent_generation_case() AndAlso
               concurrent_generation_case()
    End Function
End Class
