
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
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
                                              If assertion.is_not_null(i) AndAlso assertion.is_not_null(i.get()) Then
                                                  assertion.equal(i.get().id, c)
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
        assertion.is_false(d.started())
        d.require(count)
        ' d has not been started yet
        assertion.equal(c, uint32_0)
        assertion.equal(d.exported(), uint32_0)
        d.start()
        'd.wait_for_start() ' Start procedure is synchronized.
        assertion.is_true(started)
        assertion.is_true(d.started())
        assertion.is_false(d.stopping())
        assertion.is_false(d.stopped())
        assertion.is_true(timeslice_sleep_wait_until(Function() c = count, minutes_to_milliseconds(10)))
        assertion.equal(c, count)
        assertion.equal(d.exported(), count)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
        d.require(count)
        assertion.is_true(timeslice_sleep_wait_until(Function() c = (count << 1), minutes_to_milliseconds(10)))
        assertion.equal(c, count << 1)
        assertion.equal(d.exported(), count << 1)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
        d.stop()
        d.wait_for_stop()
        assertion.is_true(d.stopped())
        assertion.is_true(timeslice_sleep_wait_until(Function() stopped, minutes_to_milliseconds(10)))
        d.require(count)
        assertion.is_false(timeslice_sleep_wait_when(Function() c = (count << 1), minutes_to_milliseconds(10)))
        assertion.equal(c, count << 1)
        assertion.equal(d.exported(), count << 1)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), c)
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
        assertion.is_false(d.started())
        d.require(count)
        ' d has not been started yet
        assertion.equal(+c, uint32_0)
        assertion.equal(d.exported(), uint32_0)
        d.start()
        assertion.is_true(d.started())
        assertion.is_false(d.stopping())
        assertion.is_false(d.stopped())
        assertion.is_true(timeslice_sleep_wait_until(Function() +c = count, minutes_to_milliseconds(10)))
        assertion.equal(+c, count)
        assertion.equal(d.exported(), count)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        d.require(count)
        assertion.is_true(timeslice_sleep_wait_until(Function() +c = (count << 1), minutes_to_milliseconds(10)))
        assertion.equal(+c, count << 1)
        assertion.equal(d.exported(), count << 1)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        d.stop()
        d.wait_for_stop()
        assertion.is_true(d.stopped())
        d.require(count)
        assertion.is_false(timeslice_sleep_wait_when(Function() +c = (count << 1), minutes_to_milliseconds(10)))
        assertion.equal(+c, count << 1)
        assertion.equal(d.exported(), count << 1)
        assertion.equal(mock_dev(Of auto_device_exporter_idevice_test).constructed(), +c)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return no_concurrent_generation_case() AndAlso
               concurrent_generation_case()
    End Function
End Class
