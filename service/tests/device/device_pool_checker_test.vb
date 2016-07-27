﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.device

Public Class device_pool_checker_test
    Inherits [case]

    Public Overrides Function preserved_processors() As Int16
        Return 2
    End Function

    Public Overrides Function run() As Boolean
        Const ms As Int64 = 128
        Const size As UInt32 = 1024
        Dim e As imanual_device_exporter(Of Int32) = Nothing
        e = New manual_device_exporter(Of Int32)()
        Dim p As manual_pre_generated_device_pool(Of Int32) = Nothing
        p = manual_pre_generated_device_pool.[New](e)
        Dim called As atomic_uint = Nothing
        called = New atomic_uint()
        Dim valid As Boolean = False
        valid = True
        For i As Int32 = 0 To CInt(size) - 1
            Dim j As Int32 = 0
            j = i
            e.inject(i.make_device(validator:=Function(x As Int32) As Boolean
                                                  Return valid
                                              End Function,
                                   checker:=Sub(x As Int32)
                                                assert_equal(x, j)
                                                called.increment()
                                            End Sub))
        Next
        assert_equal(p.free_count(), size)
        assert_true(p.attach_checker(ms))
        sleep(1.5 * ms)
        assert_equal(+called, size << 1)
        assert_true(p.clear_checker())
        sleep(ms)
        assert_equal(+called, size << 1)

        valid = False
        sleep(ms)
        assert_equal(+called, size << 1)
        assert_equal(p.free_count(), size)
        assert_true(p.attach_checker(ms))
        sleep(1.5 * ms)
        assert_equal(+called, size << 1)
        assert_equal(p.free_count(), uint32_0)
        assert_true(p.clear_checker())

        For i As Int32 = 0 To CInt(size) - 1
            e.inject(i.make_device(checker:=Sub(x As Int32)
                                                called.increment()
                                            End Sub))
        Next
        called.exchange(uint32_0)
        assert_true(p.attach_checker(ms))
        sleep(1.5 * ms)
        assert_equal(+called, size << 1)
        p.close()
        sleep(1.5 * ms)
        assert_equal(+called, size << 1)
        Return True
    End Function
End Class
