
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class incoming_test
        Inherits [case]

        Private ReadOnly c As collection
        Private ReadOnly p As parameter
        Private component As ref_instance(Of component)
        Private dispenser As dispenser(Of Int32, const_pair(Of Byte, Byte))

        Public Sub New()
            c = New collection()
            p = New parameter(10, True)
        End Sub

        Public Overrides Function preserved_processors() As Int16
            Return 1
        End Function

        Private Function run_case() As Boolean
            Const ip_size As Byte = 10
            Const port_size As Byte = 10
            Dim s As vector(Of shared_component(Of Byte, Byte, component, Int32, parameter)) = Nothing
            s = _new(s)
            AddHandler c.new_shared_component_exported,
                       Sub(ByVal new_component As shared_component(Of Byte, Byte, component, Int32, parameter))
                           s.emplace_back(new_component)
                       End Sub
            Dim p As pointer(Of Int32) = Nothing
            p = New pointer(Of Int32)()
            If assert_true(component.referred()) Then
                For i As Byte = 0 To ip_size - uint8_1
                    For j As Byte = 1 To port_size
                        assert(shared_component_test.component.is_valid_port(j))
                        Dim exp_size As UInt32
                        exp_size = i * CUInt(10) + j
                        assert_less(s.size(), exp_size)
                        component.get().receive(200, i, j)
                        If assert_true(timeslice_sleep_wait_until(Function() As Boolean
                                                                      assert_less_or_equal(s.size(), exp_size)
                                                                      Return s.size() = exp_size
                                                                  End Function,
                                                                  seconds_to_milliseconds(10))) Then
                            assert_true(s.back().is_valid())
                            assert_true(async_sync(s.back().receiver.receive(p), seconds_to_milliseconds(10)),
                                        "@", i, "-", j)
                            assert_equal(+p, 200, "@", i, "-", j)

                            For k As Int32 = 0 To 1000
                                component.get().receive(k, i, j)
                                assert_true(async_sync(s.back().receiver.receive(p), seconds_to_milliseconds(10)))
                                assert_equal(+p, k)
                            Next

                            If i > 0 Then
                                For k As Byte = 0 To i - uint8_1
                                    For l As Byte = 1 To port_size
                                        component.get().receive(k * l, k, l)
                                        assert_true(async_sync(s(k * port_size + l - uint16_1).receiver.receive(p),
                                                               seconds_to_milliseconds(10)))
                                        assert_equal(+p, k * l)
                                    Next
                                Next
                            End If
                            If j > 1 Then
                                For k As Byte = 1 To j - uint8_1
                                    component.get().receive(k * i, i, k)
                                    assert_true(async_sync(s(i * port_size + k - uint16_1).receiver.receive(p),
                                                           seconds_to_milliseconds(10)))
                                    assert_equal(+p, k * i)
                                Next
                            End If
                        End If
                    Next
                Next
            End If
            If assert_more(s.size(), uint32_0) Then
                For i As UInt32 = 0 To s.size() - uint32_1
                    s(i).dispose()
                Next
            End If
            assert_equal(dispenser.binding_count(), uint32_1)
            assert_equal(component.ref_count(), uint32_1)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            assert_true(_shared_component_collection.[New](Of UInt16, UInt16, component, Int32, parameter) _
                                                          (c, p, component, dispenser))

            Dim r As Boolean = False
            r = run_case()

            assert_true(dispenser.release())
            assert_equal(dispenser.binding_count(), uint32_0)
            assert_true(dispenser.expired())
            assert_true(dispenser.wait_for_stop(constants.default_sense_timeout_ms))
            assert_true(dispenser.stopped())
            assert_equal(component.ref_count(), uint32_0)
            assert_false(component.referred())
            ' TODO: Why cannot shared_component_test.dispenser.receiver.sense be finished here.
            ' [event_comb] instance count 3,
            ' [event_sync_T_pump_T_receiver_adapter.vb(64):osi.service.transmitter.event_sync_T_pump_T_receiver_adapter`1.sense] - 1,
            ' [shared_component_test.dispenser.receiver.vb(44):osi.tests.service.selector.shared_component_test+receiver.sense] - 1,
            ' [sensor.vb(95):osi.service.transmitter.dll.osi.service.transmitter._sensor.sense] - 1
            sleep(constants.default_sense_timeout_ms)

            Return r
        End Function
    End Class
End Class
