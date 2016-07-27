
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.device

Public Class device_test
    Inherits [case]

    Private Shared Function attach_case(ByVal attach_by_function As Boolean) As Boolean
        mock_dev(Of device_test).reset()
        Dim d As idevice(Of mock_dev(Of device_test)) = Nothing
        d = mock_dev(Of device_test).create(True, attach_by_function)
        assert_false(d.closed())
        assert_true(d.is_valid())
        mock_dev(Of device_test).close(d.get())
        assert_false(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_2)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_2)
        Return True
    End Function

    Private Shared Function attach_case2(ByVal attach_by_function As Boolean) As Boolean
        mock_dev(Of device_test).reset()
        Dim d As idevice(Of mock_dev(Of device_test)) = Nothing
        d = mock_dev(Of device_test).create(True, attach_by_function)
        assert_false(d.closed())
        assert_true(d.is_valid())
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        mock_dev(Of device_test).close(d.get())
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_2)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_2)
        Return True
    End Function

    Private Shared Function unattach_case() As Boolean
        mock_dev(Of device_test).reset()
        Dim d As idevice(Of mock_dev(Of device_test)) = Nothing
        d = mock_dev(Of device_test).create(False)
        assert_false(d.closed())
        assert_true(d.is_valid())
        mock_dev(Of device_test).close(d.get())
        assert_false(d.closed())
        assert_true(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        Return True
    End Function

    Private Shared Function unattach_case2() As Boolean
        mock_dev(Of device_test).reset()
        Dim d As idevice(Of mock_dev(Of device_test)) = Nothing
        d = mock_dev(Of device_test).create(False)
        assert_false(d.closed())
        assert_true(d.is_valid())
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_false(d.get().closed())
        assert_equal(d.get().close_times(), uint32_0)
        mock_dev(Of device_test).close(d.get())
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        d.close()
        assert_true(d.closed())
        assert_false(d.is_valid())
        assert_true(d.get().closed())
        assert_equal(d.get().close_times(), uint32_1)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return attach_case(True) AndAlso
               attach_case(False) AndAlso
               attach_case2(True) AndAlso
               attach_case2(False) AndAlso
               unattach_case() AndAlso
               unattach_case2()
    End Function
End Class
