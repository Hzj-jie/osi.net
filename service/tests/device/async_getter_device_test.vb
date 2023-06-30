
Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.event
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.selector

Public Class async_getter_device_test
    Inherits [case]

    Private Shared Function eventually_closed() As Boolean
        Dim w As signal_event = Nothing
        w = New signal_event()
        Dim d As idevice(Of async_getter(Of mock_dev(Of async_getter_device_test))) = Nothing
        d = New async_getter_mock_dev_device(Of async_getter_device_test)(w)
        Dim r As mock_dev(Of async_getter_device_test) = Nothing
        r = New mock_dev(Of async_getter_device_test)()
        assert(d.get().not_initialized())
        d.close()
        assertion.is_true(d.closed())
        assertion.is_false(d.is_valid())
        assertion.is_false(r.closed())
        w.mark()
        assertion.is_true(d.get().wait_until_initialized(seconds_to_milliseconds(10)))
        assertion.is_true(timeslice_sleep_wait_until(Function() r.closed(), seconds_to_milliseconds(1)))
        assertion.is_true(d.closed())
        assertion.is_false(d.is_valid())
        Return True
    End Function

    Private Shared Function valid_before_initialized() As Boolean
        Dim w As signal_event = Nothing
        w = New signal_event()
        Dim d As idevice(Of async_getter(Of mock_dev(Of async_getter_device_test))) = Nothing
        d = New async_getter_mock_dev_device(Of async_getter_device_test)(w)
        Dim r As mock_dev(Of async_getter_device_test) = Nothing
        assert(d.get().not_initialized())
        assertion.is_true(d.is_valid())
        assert(Not d.get().get(r))
        assert(r Is Nothing)
        w.mark()
        If assertion.is_true(d.get().wait_until_initialized(seconds_to_milliseconds(1))) Then  ' may not always success
            assertion.is_true(d.is_valid())
            assert(d.get().get(r))
            assert(Not r Is Nothing)
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return valid_before_initialized()
    End Function
End Class
