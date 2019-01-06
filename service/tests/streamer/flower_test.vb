
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.streamer
Imports osi.service.transmitter
Imports osi.tests.service.transmitter

Public MustInherit Class flower_test
    Inherits [case]

    Protected MustOverride Function create_flower(ByVal first As T_receiver(Of Int32),
                                                  ByVal last As T_sender(Of Int32)) As flower(Of Int32)

    Private Shared Sub execute(ByVal f As flower(Of Int32), ByVal ended As pointer(Of Boolean))
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        ec = (+f)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Return eva(ended, True) AndAlso
                                               assertion.is_true(ec.end_result()) AndAlso
                                               goto_end()
                                    End Function))
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 2
    End Function

    Public Overrides Function run() As Boolean
        Dim first As mock_dev_int = Nothing
        Dim last As mock_dev_int = Nothing
        first = New mock_dev_int()
        last = New mock_dev_int()
        Dim f As flower(Of Int32) = Nothing
        f = create_flower(first, last)
        Dim ended As pointer(Of Boolean) = Nothing
        ended = New pointer(Of Boolean)()
        execute(f, ended)
        Dim v() As Int32 = Nothing
        v = rnd_ints(rnd_int(16384, 32768))
        For i As Int32 = 0 To array_size_i(v) - 1
            assert(v(i) <> max_int32)
            first.receive_pump.emplace(v(i))
        Next
        first.receive_pump.emplace(max_int32)
        assertion.is_true(timeslice_sleep_wait_until(Function() f.stopped(), minutes_to_milliseconds(1)))
        assertion.is_true(last.send_pump_equal(v))
        assertion.is_true(timeslice_sleep_wait_until(Function() +ended, minutes_to_milliseconds(1)))
        Return True
    End Function
End Class
