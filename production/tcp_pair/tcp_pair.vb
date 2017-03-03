
Imports System.DateTime
Imports System.Net
Imports System.Threading
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.threadpool
Imports osi.service
Imports osi.service.tcp
Imports osi.service.convertor
Imports osi.service.device
Imports osi.service.commander
Imports td = osi.service.tcp.constants.default_value
Imports responder = osi.service.commander.responder
Imports execution_wrapper = osi.service.commander.executor_wrapper

Public Module tcp_pair
    Private Class arguments
        Private Const _i As String = "i"
        Private Const _host As String = "host"
        Private Const _port As String = "port"
        Private Const _token As String = "token"
        Private Const _no_delay As String = "no-delay"
        Private Const _send_rate_sec As String = "send-rate-sec"
        Private Const _response_timeout_ms As String = "response-timeout-ms"
        Private Const _receive_rate_sec As String = "receive-rate-sec"
        Private Const _connecting_timeout_ms As String = "connecting-timeout-ms"
        Private Const _connection_count As String = "connection-count"
        Private Const _half_connection_count As String = "half-connection-count"
        Private Const _max_lifetime_ms As String = "max-lifetime-ms"
        Private Const _ipv4 As String = "ipv4"
        Private Const _question As String = "question"
        Private Const _reset_connection As String = "reset-connection"
        Private Const _enable_keepalive As String = "enable-keepalive"
        Private Const _first_keepalive_ms As String = "first-keepalive-ms"
        Private Const _keepalive_interval_ms As String = "keepalive-interval-ms"

        Public Shared ReadOnly i As Boolean
        Public Shared ReadOnly host As String
        Public Shared ReadOnly port As UInt16
        Public Shared ReadOnly token As String
        Public Shared ReadOnly send_rate_sec As UInt32
        Public Shared ReadOnly response_timeout_ms As Int64
        Public Shared ReadOnly receive_rate_sec As UInt32
        Public Shared ReadOnly connecting_timeout_ms As Int64
        Public Shared ReadOnly no_delay As Boolean
        Public Shared ReadOnly connection_count As UInt32
        Public Shared ReadOnly half_connection_count As UInt32
        Public Shared ReadOnly max_lifetime_ms As Int64
        Public Shared ReadOnly ipv4 As Boolean
        Public Shared ReadOnly question As Boolean
        Public Shared ReadOnly reset_connection As Boolean
        Public Shared ReadOnly enable_keepalive As Boolean
        Public Shared ReadOnly first_keepalive_ms As UInt32
        Public Shared ReadOnly keepalive_interval_ms As UInt32

        Shared Sub New()
            argument.bind(_i,
                          _host,
                          _port,
                          _token,
                          _send_rate_sec,
                          _response_timeout_ms,
                          _receive_rate_sec,
                          _connecting_timeout_ms,
                          _no_delay,
                          _connection_count,
                          _half_connection_count,
                          _max_lifetime_ms,
                          _ipv4,
                          _question,
                          _reset_connection,
                          _enable_keepalive,
                          _first_keepalive_ms,
                          _keepalive_interval_ms)

            i = argument.switch(_i)
            host = argument.value(_host).to_string("localhost")
            port = argument.value(_port).to_uint16(rnd_int(10000, 60001))
            token = argument.value(_token).to_string()
            send_rate_sec = argument.value(_send_rate_sec).to_int32(td.send_rate_sec)
            response_timeout_ms = argument.value(_response_timeout_ms).to_int64(td.response_timeout_ms)
            receive_rate_sec = argument.value(_receive_rate_sec).to_int32(td.receive_rate_sec)
            connecting_timeout_ms = argument.value(_connecting_timeout_ms).to_int64(td.outgoing.connecting_timeout_ms)
            no_delay = argument.switch(_no_delay)
            connection_count = argument.value(_connection_count).to_int32(td.outgoing.max_connected)
            half_connection_count = argument.value(_half_connection_count).to_int32(If(i,
                                                                                       td.incoming.max_connecting,
                                                                                       td.outgoing.max_connecting))
            max_lifetime_ms = argument.value(_max_lifetime_ms).to_int64(If(i,
                                                                           td.incoming.max_lifetime_ms,
                                                                           td.outgoing.max_lifetime_ms))
            ipv4 = argument.switch(_ipv4)
            question = argument.switch(_question)
            reset_connection = argument.switch(_reset_connection)
            enable_keepalive = argument.switch(_enable_keepalive)
            first_keepalive_ms = argument.value(_first_keepalive_ms).to_uint32(socket_first_keepalive_ms)
            keepalive_interval_ms = argument.value(_keepalive_interval_ms).to_uint32(socket_keepalive_interval_ms)
            Console.Write(strcat(_i, " = ", i, character.newline,
                                 _host, " = ", host, character.newline,
                                 _port, " = ", port, character.newline,
                                 _token, " = ", token, character.newline,
                                 _send_rate_sec, " = ", send_rate_sec, character.newline,
                                 _receive_rate_sec, " = ", receive_rate_sec, character.newline,
                                 _connecting_timeout_ms, " = ", connecting_timeout_ms, character.newline,
                                 _no_delay, " = ", no_delay, character.newline,
                                 _connection_count, " = ", connection_count, character.newline,
                                 _half_connection_count, " = ", half_connection_count, character.newline,
                                 _max_lifetime_ms, " = ", max_lifetime_ms, character.newline,
                                 _ipv4, " = ", ipv4, character.newline,
                                 _question, " = ", question, character.newline,
                                 _reset_connection, " = ", reset_connection, character.newline,
                                 _enable_keepalive, " = ", enable_keepalive, character.newline,
                                 _first_keepalive_ms, " = ", first_keepalive_ms, character.newline,
                                 _keepalive_interval_ms, " = ", keepalive_interval_ms, character.newline))
        End Sub
    End Class

    Sub New()
        enable_domain_unhandled_exception_handler()
        register_slimqless2_threadpool()
    End Sub

    Public Sub main(ByVal args() As String)
        debugpause()
        argument.parse(args)
        ServicePointManager.DefaultConnectionLimit() = max_int32
        ServicePointManager.MaxServicePoints() = max_int32
        Dim b As powerpoint.creator = Nothing
        b = New powerpoint.creator()
        b.with_token(arguments.token).
          with_port(arguments.port).
          with_send_rate_sec(arguments.send_rate_sec).
          with_response_timeout_ms(arguments.response_timeout_ms).
          with_receive_rate_sec(arguments.receive_rate_sec).
          with_max_connecting(arguments.half_connection_count).
          with_max_connected(arguments.connection_count).
          with_no_delay(arguments.no_delay).
          with_max_lifetime_ms(arguments.max_lifetime_ms).
          with_enable_keepalive(arguments.enable_keepalive).
          with_first_keepalive_ms(arguments.first_keepalive_ms).
          with_keepalive_interval_ms(arguments.keepalive_interval_ms)
        If arguments.i Then
            b.with_outgoing().
              with_host_or_ip(arguments.host).
              with_connecting_timeout_ms(arguments.connecting_timeout_ms)
            If arguments.ipv4 Then
                b.with_ipv4()
            End If
        Else
            b.with_incoming()
        End If
        Dim p As idevice_pool(Of herald) = Nothing
        p = b.create().herald_device_pool()
        Dim c As Int64 = 0
        If arguments.question Then
            For i As Int32 = 0 To p.max_count() - 1
                Dim r As pointer(Of Byte()) = Nothing
                r = New pointer(Of Byte())
                Dim ec As event_comb = Nothing
                Dim s As String = Nothing
                begin_application_lifetime_event_comb(Function() As Boolean
                                                          s = guid_str()
                                                          ec = (New questioner(p))(str_bytes(s), r)
                                                          Return waitfor(ec) AndAlso
                                                                 goto_next()
                                                      End Function,
                                                      Function() As Boolean
                                                          If ec.end_result() Then
                                                              raise_error("question ", s, ", answer ", bytes_str(+r))
                                                              Interlocked.Increment(c)
                                                          Else
                                                              raise_error(error_type.warning,
                                                                          "failed to question ",
                                                                          arguments.host,
                                                                          ":",
                                                                          arguments.port)
                                                              assert_waitfor(sixteen_timeslice_length_ms)
                                                          End If
                                                          Return goto_begin()
                                                      End Function)
            Next
        Else
            assert(-(New responder(p,
                                   New execution_wrapper(
                                       Function(i() As Byte, o As pointer(Of Byte())) As event_comb
                                           Return New event_comb(Function() As Boolean
                                                                     Dim s As String = Nothing
                                                                     s = guid_str()
                                                                     raise_error("input ",
                                                                                 bytes_str(i),
                                                                                 ", output ",
                                                                                 s)
                                                                     Interlocked.Increment(c)
                                                                     Return eva(o, str_bytes(s)) AndAlso
                                                                            goto_end()
                                                                 End Function)
                                       End Function))))
        End If
        Dim start_ms As Int64 = 0
        start_ms = Now().milliseconds()
        begin_application_lifetime_event_comb(Function() As Boolean
                                                  Return waitfor(1000) AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  rewrite_console("total requests ",
                                                                  c,
                                                                  ", qps ",
                                                                  seconds_to_milliseconds(c) \ _
                                                                  (Now().milliseconds() - start_ms))
                                                  Return goto_begin()
                                              End Function)
        gc_trigger()
    End Sub
End Module
