
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
Imports osi.service.sharedtransmitter

Partial Public Class sharedtransmitter_test
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

        Public Overrides Function reserved_processors() As Int16
            Return 1
        End Function

        Private Function run_case() As Boolean
            Const ip_size As Byte = 10
            Const port_size As Byte = 10
            Dim s As vector(Of sharedtransmitter(Of Byte, Byte, component, Int32, parameter)) = Nothing
            s = _new(s)
            AddHandler c.new_sharedtransmitter_exported,
                       Sub(ByVal new_component As sharedtransmitter(Of Byte, Byte, component, Int32, parameter))
                           s.emplace_back(new_component)
                       End Sub
            Dim p As pointer(Of Int32) = Nothing
            p = New pointer(Of Int32)()
            If assertion.is_true(component.referred()) Then
                For i As Byte = 0 To ip_size - uint8_1
                    For j As Byte = 1 To port_size
                        assert(sharedtransmitter_test.component.is_valid_port(j))
                        Dim exp_size As UInt32
                        exp_size = i * CUInt(10) + j
                        assertion.less(s.size(), exp_size)
                        component.get().receive(200, i, j)
                        If assertion.is_true(timeslice_sleep_wait_until(Function() As Boolean
                                                                            assertion.less_or_equal(s.size(), exp_size)
                                                                            Return s.size() = exp_size
                                                                        End Function,
                                                                  seconds_to_milliseconds(10))) Then
                            assertion.is_true(s.back().is_valid())
                            assertion.is_true(async_sync(s.back().receiver.receive(p), seconds_to_milliseconds(10)),
                                        "@", i, "-", j)
                            assertion.equal(+p, 200, "@", i, "-", j)

                            For k As Int32 = 0 To 1000
                                component.get().receive(k, i, j)
                                assertion.is_true(async_sync(s.back().receiver.receive(p), seconds_to_milliseconds(10)))
                                assertion.equal(+p, k)
                            Next

                            If i > 0 Then
                                For k As Byte = 0 To i - uint8_1
                                    For l As Byte = 1 To port_size
                                        component.get().receive(k * l, k, l)
                                        assertion.is_true(async_sync(s(k * port_size + l - uint16_1).receiver.receive(p),
                                                               seconds_to_milliseconds(10)))
                                        assertion.equal(+p, k * l)
                                    Next
                                Next
                            End If
                            If j > 1 Then
                                For k As Byte = 1 To j - uint8_1
                                    component.get().receive(k * i, i, k)
                                    assertion.is_true(async_sync(s(i * port_size + k - uint16_1).receiver.receive(p),
                                                           seconds_to_milliseconds(10)))
                                    assertion.equal(+p, k * i)
                                Next
                            End If
                        End If
                    Next
                Next
            End If
            If assertion.more(s.size(), uint32_0) Then
                For i As UInt32 = 0 To s.size() - uint32_1
                    s(i).dispose()
                Next
            End If
            assertion.equal(dispenser.binding_count(), uint32_1)
            assertion.equal(component.ref_count(), uint32_1)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            assertion.is_true(_sharedtransmitter_collection.[New](Of Byte, Byte, component, Int32, parameter) _
                                                          (c, p, component, dispenser))

            Dim r As Boolean = False
            r = run_case()

            assertion.is_true(dispenser.release())
            assertion.equal(dispenser.binding_count(), uint32_0)
            assertion.is_true(dispenser.expired())
            assertion.is_true(dispenser.wait_for_stop(constants.default_sense_timeout_ms))
            assertion.is_true(dispenser.stopped())
            assertion.equal(component.ref_count(), uint32_0)
            assertion.is_false(component.referred())
            ' TODO: Why cannot sharedtransmitter_test.dispenser.receiver.sense be finished here.
            ' [event_comb] instance count 3,
            ' [event_sync_T_pump_T_receiver_adapter.vb(64):osi.service.transmitter.event_sync_T_pump_T_receiver_adapter`1.sense] - 1,
            ' [sharedtransmitter_test.dispenser.receiver.vb(44):osi.tests.service.selector.sharedtransmitter_test+receiver.sense] - 1,
            ' [sensor.vb(95):osi.service.transmitter.dll.osi.service.transmitter._sensor.sense] - 1
            sleep(constants.default_sense_timeout_ms)

            Return r
        End Function
    End Class
End Class
