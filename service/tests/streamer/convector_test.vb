
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
Imports osi.tests.service.transmitter

Public MustInherit Class convector_test
    Inherits [case]

    Protected MustOverride Function create_convector(ByVal dev1 As mock_dev_int,
                                                     ByVal dev2 As mock_dev_int) As convector(Of Int32)

    Private Sub execute(ByVal c As convector(Of Int32), ByVal ended As ref(Of Boolean))
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        ec = (+c)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Return eva(ended, True) AndAlso
                                               assertion.is_true(ec.end_result()) AndAlso
                                               goto_end()
                                    End Function))
    End Sub

    Public Overrides Function run() As Boolean
        Dim dev1 As mock_dev_int = Nothing
        Dim dev2 As mock_dev_int = Nothing
        dev1 = New mock_dev_int()
        dev2 = New mock_dev_int()
        Dim c As convector(Of Int32) = Nothing
        c = create_convector(dev1, dev2)
        Dim ended As ref(Of Boolean) = Nothing
        ended = New ref(Of Boolean)()
        execute(c, ended)
        Dim v1() As Int32 = Nothing
        v1 = rnd_ints(rnd_int(16384, 32768))
        Dim v2() As Int32 = Nothing
        v2 = rnd_ints(array_size_i(v1))
        For i As Int32 = 0 To array_size_i(v1) - 1
            assert(v1(i) <> max_int32)
            dev1.receive_pump.emplace(v1(i))
            assert(v2(i) <> max_int32)
            dev2.receive_pump.emplace(v2(i))
        Next
        dev1.receive_pump.emplace(max_int32)
        dev2.receive_pump.emplace(max_int32)
        assertion.is_true(timeslice_sleep_wait_until(Function() c.stopped(), minutes_to_milliseconds(10)))
        assertion.is_true(dev1.send_pump_equal(v2))
        assertion.is_true(dev2.send_pump_equal(v1))
        assertion.is_true(timeslice_sleep_wait_until(Function() +ended, minutes_to_milliseconds(10)))
        Return True
    End Function
End Class
