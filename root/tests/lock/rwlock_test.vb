
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class rwlock_test
    Private NotInheritable Class test_class
        Private ReadOnly l As rwlock
        Private ReadOnly rac As atomic_int
        Private ReadOnly wac As atomic_int
        Private max_rac As Int32
        Private max_wac As Int32
        Private k As String
        Private v As Int32

        Public Sub New()
            l = New rwlock()
            rac = New atomic_int()
            wac = New atomic_int()
            write()
        End Sub

        Public Sub read()
            Dim c As Int32 = 0
            c = rac.increment()
            assert(c > 0)
            If c > max_rac Then
                max_rac = c
            End If
            Using l.scoped_read_lock()
                assert_equal(k.GetHashCode(), v)
            End Using
            assert(rac.decrement() >= 0)
        End Sub

        Public Sub write()
            Dim c As Int32 = 0
            c = wac.increment()
            assert(c > 0)
            If c > max_wac Then
                max_wac = c
            End If
            Using l.scoped_write_lock()
                k = guid_str()
                v = k.GetHashCode()
            End Using
            assert(wac.decrement() >= 0)
        End Sub

        Public Function max_concurrent_read() As UInt32
            Return CUInt(max_rac)
        End Function

        Public Function max_concurrent_write() As UInt32
            Return CUInt(max_wac)
        End Function
    End Class

    <test>
    Private Shared Sub multiple_reads_can_happen_at_same_time()
        Dim t As test_class = Nothing
        t = New test_class()
        Dim zre As zero_reset_event = Nothing
        zre = New zero_reset_event(101)
        For i As Int32 = 0 To 100
            queue_in_managed_threadpool(Sub()
                                            For j As Int32 = 0 To 100
                                                t.read()
                                            Next
                                            zre.decrease()
                                        End Sub)
        Next
        zre.wait_and_dispose()
        assert_more(t.max_concurrent_read(), uint32_1)
    End Sub

    <test>
    Private Shared Sub multiple_writes_can_not_happen_in_parallel()
        Dim t As test_class = Nothing
        t = New test_class()
        Dim zre As zero_reset_event = Nothing
        zre = New zero_reset_event(101)
        For i As Int32 = 0 To 100
            queue_in_managed_threadpool(Sub()
                                            For j As Int32 = 0 To 100
                                                t.write()
                                                t.read()
                                            Next
                                            zre.decrease()
                                        End Sub)
        Next
        zre.wait_and_dispose()
        assert_more(t.max_concurrent_read(), uint32_1)
        assert_more(t.max_concurrent_write(), uint32_1)
    End Sub

    Private Sub New()
    End Sub
End Class
