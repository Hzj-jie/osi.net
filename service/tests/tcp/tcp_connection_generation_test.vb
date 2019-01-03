
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.transmitter
Imports osi.service.tcp
Imports osi.root.formation
Imports constants = osi.service.tcp.constants

Public Class tcp_connection_generation_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(New test(), 3)
    End Sub

    Private Class test
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
            Dim id As String = strcat("token: ", token,
                                      ", port: ", port,
                                      ", tokener: ", tokener,
                                      ", delay_connect: ", delay_connect)
            connection_state.bind()
            Dim ap As idevice_pool(Of flow) = Nothing
            Dim cp As idevice_pool(Of flow) = Nothing
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
                 create().flow_device_pool()
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
                 create().flow_device_pool()
            'wait for the accepter to start and connection to be generated
            If delay_connect OrElse
               (assertion.is_true(timeslice_sleep_wait_when(Function() As Boolean
                                                                Return ap.empty()
                                                            End Function,
                                                      seconds_to_milliseconds(20)), id) AndAlso
                assertion.is_false(ap.empty(), id) AndAlso
                assertion.is_false(cp.empty(), id)) Then
                Dim c As idevice(Of flow) = Nothing
                For i As Int32 = 0 To 10000
                    assertion.is_true(cp.get(c), id)
                    If assertion.is_not_null(c, id) Then
                        assertion.is_true(c.is_valid(), id)
                        If rnd_bool_trues(8) Then
                            c.close()
                            assertion.is_false(cp.release(c), id)
                            If Not delay_connect Then
                                assertion.is_true(timeslice_sleep_wait_when(Function() As Boolean
                                                                                Return cp.empty()
                                                                            End Function,
                                                                      seconds_to_milliseconds(30)), id)
                            End If
                        Else
                            assertion.is_true(cp.release(c), id)
                        End If
                    End If
                Next
                assertion.equal(cp.total_count(), uint32_1, id)
                assertion.is_true(timeslice_sleep_wait_when(Function() As Boolean
                                                                Return ap.total_count() > uint32_1
                                                            End Function,
                                                      seconds_to_milliseconds(60)), id)
            End If
            cp.close()
            assertion.equal(cp.total_count(), uint32_0, id)
            ap.close()
            assertion.equal(ap.total_count(), uint32_0, id)

            connection_state.release()
            powerpoint.waitfor_stop()
            Return True
        End Function

        Private Overloads Shared Function run(ByVal token As String,
                                              ByVal port As UInt16,
                                              ByVal tokener As String) As Boolean
            Return run(token, port, tokener, True) AndAlso
                   run(token, port, tokener, False)
        End Function

        Private Function bypass_tokener_case() As Boolean
            Return run(Nothing, port1, constants.bypass_tokener)
        End Function

        Private Function token1_case(ByVal token As String) As Boolean
            Return run(token, port2, constants.token1)
        End Function

        Private Function token1_case() As Boolean
            Return token1_case("token") AndAlso
                   token1_case(Nothing) AndAlso
                   token1_case(empty_string) AndAlso
                   token1_case(guid_str())
        End Function

        Private Function token2_case(ByVal token As String) As Boolean
            Return run(token, port3, constants.token1 + constants.bypass_tokener)
        End Function

        Private Function token2_case() As Boolean
            Return token2_case("token") AndAlso
                   token2_case(guid_str())
        End Function

        Public Overrides Function run() As Boolean
            Select Case multithreading_case_wrapper.thread_id()
                Case 0
                    Return bypass_tokener_case()
                Case 1
                    Return token1_case()
                Case 2
                    Return token2_case()
                Case Else
                    assert(False)
                    Return True
            End Select
        End Function
    End Class
End Class
