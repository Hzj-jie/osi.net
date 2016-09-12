
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.lock
Imports osi.service.streamer
Imports osi.service.device
Imports osi.tests.service.device

Public MustInherit Class flower_test
    Inherits [case]

    Protected MustOverride Function create_flower(ByVal first As T_receiver(Of Int32),
                                                  ByVal last As T_sender(Of Int32)) As flower(Of Int32)

    Private Sub execute(ByVal f As flower(Of Int32), ByVal ended As pointer(Of Boolean))
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        ec = (+f)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Return eva(ended, True) AndAlso
                                               assert_true(ec.end_result()) AndAlso
                                               goto_end()
                                    End Function))
    End Sub

    Public Overrides Function run() As Boolean
        Dim first As mock_dev_int = Nothing
        Dim last As mock_dev_int = Nothing
        first = New mock_dev_int()
        last = New mock_dev_int()
        Dim f As flower(Of Int32) = Nothing
        f = create_flower(first, last)
        flower(Of Int32).is_eos.set_local(Function(i As Int32) As Boolean
                                              Return i = max_int32
                                          End Function)
        Dim ended As pointer(Of Boolean) = Nothing
        ended = New pointer(Of Boolean)()
        execute(f, ended)
        Dim v() As Int32 = Nothing
        v = rnd_ints(rnd_int(16384, 32768))
        For i As Int32 = 0 To array_size(v) - 1
            assert(v(i) <> max_int32)
            first.receive_pump.emplace(v(i))
        Next
        first.receive_pump.emplace(max_int32)
        assert_true(timeslice_sleep_wait_until(Function() f.stopped(), minute_to_milliseconds(1)))
        assert_true(last.send_pump_equal(v))
        assert_true(timeslice_sleep_wait_until(Function() +ended, minute_to_milliseconds(1)))
        Return True
    End Function
End Class
