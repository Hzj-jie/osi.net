﻿
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.tcp
Imports osi.root.utils
Imports osi.root.formation
Imports constants = osi.service.tcp.constants

Public Class tcp_connection_generation_test
    Inherits [case]

    Private ReadOnly port1 As UInt16
    Private ReadOnly port2 As UInt16
    Private ReadOnly port3 As UInt16

    Public Sub New()
        port1 = rnd_port()
        port2 = rnd_port()
        port3 = rnd_port()
    End Sub

    Private Overloads Shared Function run(ByVal token As String,
                                          ByVal port As UInt16,
                                          ByVal tokener As String,
                                          ByVal delay_connect As Boolean) As Boolean
        connection_state.bind()
        Dim ap As idevice_pool(Of ref_client) = Nothing
        Dim cp As idevice_pool(Of ref_client) = Nothing
        ap = powerpoint.creator.[New]().
                 with_token(token).
                 with_endpoint(New IPEndPoint(IPAddress.Any, port)).
                 with_connecting_timeout_ms(1000).
                 with_send_rate_sec(uint32_1).
                 with_response_timeout_ms(npos).
                 with_receive_rate_sec(uint32_1).
                 with_max_connecting(uint32_0).
                 with_max_connected(uint32_0).
                 with_no_delay(False).
                 with_max_lifetime_ms(npos).
                 with_incoming().
                 with_enable_keepalive(False).
                 with_first_keepalive_ms(CUInt(8000)).
                 with_keepalive_interval_ms(CUInt(8000)).
                 with_tokener(tokener).
                 create().ref_client_device_pool()
        cp = powerpoint.creator.[New]().
                 with_token(token).
                 with_endpoint(New IPEndPoint(IPAddress.Loopback, port)).
                 with_connecting_timeout_ms(1000).
                 with_send_rate_sec(uint32_1).
                 with_response_timeout_ms(npos).
                 with_receive_rate_sec(uint32_1).
                 with_max_connecting(uint32_1).
                 with_max_connected(uint32_1).
                 with_no_delay(False).
                 with_max_lifetime_ms(npos).
                 with_outgoing().
                 with_enable_keepalive(True).
                 with_first_keepalive_ms(CUInt(8000)).
                 with_keepalive_interval_ms(CUInt(8000)).
                 with_tokener(tokener).
                 with_delay_connect(delay_connect).
                 create().ref_client_device_pool()
        'wait for the accepter to start and connection to be generated
        If assert_true(timeslice_sleep_wait_when(Function() As Boolean
                                                     Return ap.empty()
                                                 End Function,
                                                 seconds_to_milliseconds(20))) AndAlso
           assert_false(ap.empty()) AndAlso
           assert_false(cp.empty()) Then
            Dim c As idevice(Of ref_client) = Nothing
            For i As Int32 = 0 To 10000
                assert_true(cp.get(c))
                If assert_not_nothing(c) Then
                    assert_true(c.is_valid())
                    If rnd_bool_trues(8) Then
                        c.close()
                        assert_false(cp.release(c))
                        assert_true(timeslice_sleep_wait_when(Function() As Boolean
                                                                  Return cp.empty()
                                                              End Function,
                                                              seconds_to_milliseconds(30)))
                    Else
                        assert_true(cp.release(c))
                    End If
                End If
            Next
            assert_equal(cp.total_count(), uint32_1)
            assert_true(timeslice_sleep_wait_when(Function() As Boolean
                                                      Return ap.total_count() > uint32_1
                                                  End Function,
                                                  seconds_to_milliseconds(30)))
        End If
        cp.close()
        assert_equal(cp.total_count(), uint32_0)
        ap.close()
        assert_equal(ap.total_count(), uint32_0)

        connection_state.release()
        powerpoint.waitfor_stop()
        Return True
    End Function

    Private Overloads Shared Function run(ByVal token As String,
                                          ByVal port As UInt16,
                                          ByVal tokener As String) As Action()
        Return {Sub() assert_true(run(token, port, tokener, True)),
                Sub() assert_true(run(token, port, tokener, False))}
    End Function

    Private Function bypass_tokener_case() As Action()
        Return run(Nothing, port1, constants.bypass_tokener)
    End Function

    Private Function token1_case(ByVal token As String) As Action()
        Return run(token, port2, constants.token1)
    End Function

    Private Function token1_case() As Action()
        Return array_concat(token1_case("token"),
                            token1_case(Nothing),
                            token1_case(empty_string),
                            token1_case(guid_str()))
    End Function

    Private Function token2_case(ByVal token As String) As Action()
        Return run(token, port3, constants.token1 + constants.bypass_tokener)
    End Function

    Private Function token2_case() As Action()
        Return array_concat(token2_case("token"), token2_case(guid_str()))
    End Function

    Public Overrides Function run() As Boolean
        Dim c As concurrency_runner(Of _max_int64) = Nothing
        c = New concurrency_runner(Of _max_int64)()
        c.execute(array_concat(bypass_tokener_case(), token1_case(), token2_case()))
        Return True
    End Function
End Class
