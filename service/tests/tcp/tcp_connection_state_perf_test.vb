
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.tcp
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.lock
Imports osi.service.device
Imports envs = osi.root.envs

Public Class tcp_connection_state_perf_test
    Inherits [case]

    Private Class get_tcp_connection_states
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim m As map(Of String, TcpState) = Nothing
            m = connection_state.states()
            Return True
        End Function
    End Class

    Private Class get_active_connections
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim m() As TcpConnectionInformation = Nothing
            m = connection_state.active_connections()
            Return True
        End Function
    End Class

    Private Class query_state
        Inherits [case]

        Private ReadOnly port As UInt16

        Public Sub New(ByVal port As UInt16)
            Me.port = port
        End Sub

        Public Overrides Function run() As Boolean
            sleep(tcp_socket_state_update_interval_ms)
            timeslice_sleep_wait_until(Function() connection_state.valid())
            Dim a() As TcpConnectionInformation = Nothing
            a = connection_state.active_connections()
            If assert_true(Not isemptyarray(a)) Then
                Dim c As UInt32 = 0
                For i As Int32 = 0 To array_size(a) - 1
                    If assert_not_nothing(a(i)) AndAlso
                       assert_not_nothing(a(i).LocalEndPoint()) AndAlso
                       assert_not_nothing(a(i).RemoteEndPoint()) AndAlso
                       (a(i).LocalEndPoint().Port() = port OrElse
                        a(i).RemoteEndPoint().Port() = port) Then
                        c += uint32_1
                        assert_equal(connection_state.query(a(i).LocalEndPoint(), a(i).RemoteEndPoint()),
                                     a(i).State())
                    Else
                        connection_state.query(a(i).LocalEndPoint(), a(i).RemoteEndPoint())
                    End If
                Next
                assert_equal(c, max_connected << 1)
            End If

            a = connection_state.active_connections()
            If assert_true(Not isemptyarray(a)) Then
                Dim m As map(Of String, TcpState) = Nothing
                m = connection_state.states(a)
                If assert_not_nothing(m) Then
                    For i As Int32 = 0 To array_size(a) - 1
                        If assert_not_nothing(a(i)) AndAlso
                           assert_not_nothing(a(i).LocalEndPoint()) AndAlso
                           assert_not_nothing(a(i).RemoteEndPoint()) Then
                            assert_equal(connection_state.query(m,
                                                                a(i).LocalEndPoint(),
                                                                a(i).RemoteEndPoint()),
                                         a(i).State())
                        End If
                    Next
                End If
            End If
            Return True
        End Function
    End Class

    Private Const max_connected As UInt32 = 4096
    Private Const repeat_count As Int32 = 128
    Private Const thread_count As Int32 = 4
    Private ReadOnly port As UInt16

    Public Sub New()
        port = rnd_port()
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        If envs.disable_tcp_socket_state Then
            Return True
        Else
            connection_state.bind()
            Dim ap As idevice_pool(Of ref_client) = Nothing
            Dim cp As idevice_pool(Of ref_client) = Nothing
            ap = powerpoint.creator.[New]().
                 with_endpoint(New IPEndPoint(IPAddress.Any, port)).
                 with_connecting_timeout_ms(npos).
                 with_send_rate_sec(uint32_1).
                 with_response_timeout_ms(npos).
                 with_receive_rate_sec(uint32_1).
                 with_max_connecting(uint32_2).
                 with_max_connected(max_connected).
                 with_no_delay(False).
                 with_max_lifetime_ms(npos).
                 with_incoming().
                 with_enable_keepalive(True).
                 with_first_keepalive_ms(CUInt(8000)).
                 with_keepalive_interval_ms(CUInt(8000)).
                 create().ref_client_device_pool()
            cp = powerpoint.creator.[New]().
                 with_endpoint(New IPEndPoint(IPAddress.Loopback, port)).
                 with_connecting_timeout_ms(npos).
                 with_send_rate_sec(uint32_1).
                 with_response_timeout_ms(npos).
                 with_receive_rate_sec(uint32_1).
                 with_max_connecting(uint32_2).
                 with_max_connected(max_connected).
                 with_no_delay(False).
                 with_max_lifetime_ms(npos).
                 with_outgoing().
                 with_enable_keepalive(True).
                 with_first_keepalive_ms(CUInt(8000)).
                 with_keepalive_interval_ms(CUInt(8000)).
                 create().ref_client_device_pool()
            'wait for the accepter to start and connection to be generated
            assert_true(timeslice_sleep_wait_until(Function() As Boolean
                                                       Return ap.total_count() = max_connected AndAlso
                                                              cp.total_count() = max_connected
                                                   End Function,
                                                   seconds_to_milliseconds(1000)))
            Dim c As [case] = Nothing
            Dim r As Boolean = False
            r = True

            If commandline_specified() Then
                c = performance(multithreading(repeat(New get_tcp_connection_states(),
                                                      repeat_count),
                                               thread_count))
                r = c.run() AndAlso r
                c = performance(multithreading(repeat(New get_active_connections(),
                                                      repeat_count),
                                               thread_count))
                r = c.run() AndAlso r
                c = performance(multithreading(repeat(New query_state(port),
                                                      16),
                                               thread_count))
                r = c.run() AndAlso r
            Else
                c = multithreading(repeat(New query_state(port),
                                          2),
                                   thread_count)
                r = c.run() AndAlso r
            End If

            ap.close()
            cp.close()
            connection_state.release()
            powerpoint.waitfor_stop()
            Return r
        End If
    End Function
End Class
