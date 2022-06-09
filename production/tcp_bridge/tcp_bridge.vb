
Imports System.DateTime
Imports System.IO
Imports System.Net.Sockets
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.convertor
Imports osi.service.configuration
Imports osi.service.tcp
Imports osi.root.threadpool
Imports osi.service.streamer
Imports osi.service.device
Imports td = osi.service.tcp.constants.default_value
Imports counter = osi.root.utils.counter

Public Module tcp_bridge
    Private ReadOnly config As config
    Private ReadOnly TOTAL_BYTES As Int64

    Sub New()
        enable_domain_unhandled_exception_handler()
        register_slimqless2_threadpool()
        connection_state.bind()
        Dim config_file As String = "tcp_bridge.ini"
        If Not My.Application().CommandLineArgs() Is Nothing AndAlso
           My.Application().CommandLineArgs().Count() > 0 AndAlso
           Not My.Application(.null_or_empty().CommandLineArgs()(0)) Then
            config_file = My.Application().CommandLineArgs()(0)
        End If
        raise_error("using configuration file ", config_file)
        assert_load(config_file)
        config = configuration.default(config_file)
        TOTAL_BYTES = counter.register_average_and_last_average("TOTAL_BYTES")
    End Sub

    Private Function create_dev(ByVal c As TcpClient, ByVal p As powerpoint) As piece_dev
        assert(Not c Is Nothing)
        assert(Not p Is Nothing)
        Return New flow_piece_dev_adapter(p.as_flow(c), max(c.send_buff_size(), c.receive_buff_size()))
    End Function

    Public Sub main()
        Const connection_section_name As String = "connection"
        Const connection_section_index As Int32 = 1
        Dim sections As vector(Of section) = Nothing
        sections = config.sections(connection_section_name, connection_section_index)
        assert(Not sections.empty())
        For i As UInt32 = 0 To sections.size() - uint32_1
            Dim name As String = Nothing
            Dim is_outgoing As Boolean = False
            Dim token As String = Nothing
            Dim host As String = Nothing
            Dim port As UInt16 = 0
            Dim connecting_timeout_ms As Int64 = 0
            Dim send_rate_sec As Int32 = 0
            Dim response_timeout_ms As Int64 = 0
            Dim receive_rate_sec As Int32 = 0
            Dim max_connecting As Int32 = 0
            Dim max_connected As Int32 = 0
            Dim no_delay As Boolean = False
            Dim max_lifetime_ms As Int64 = 0
            Dim ipv4 As Boolean = False
            Dim target As String = Nothing
            Dim reset_connection As Boolean = False
            Dim chunk_count As Int32 = 0
            Dim enable_keepalive As Boolean = False
            Dim first_keepalive_ms As UInt32 = 0
            Dim keepalive_interval_ms As UInt32 = 0
            Dim treat_no_flow_as_failure As Boolean = False
            Dim wait_data_before_bridging As Boolean = False

            name = sections(i)("name", default_value:=strcat(connection_section_name, i + 1))
            is_outgoing = strsame(sections(i)("type"), "outgoing", False)
            token = sections(i)("token")
            host = sections(i)("host")
            port = sections(i)("port", default_value:=0).to_uint16()
            connecting_timeout_ms = sections(i)("connecting_timeout_ms",
                                                default_value:=td.outgoing.connecting_timeout_ms).to_int64()
            send_rate_sec = sections(i)("send_rate_sec", default_value:=td.send_rate_sec).to_int32()
            response_timeout_ms = sections(i)("response_timeout_ms", default_value:=td.response_timeout_ms).to_int64()
            receive_rate_sec = sections(i)("receive_rate_sec", default_value:=td.receive_rate_sec).to_int32()
            max_connecting = sections(i)("max_connecting", default_value:=If(is_outgoing,
                                                                             td.outgoing.max_connecting,
                                                                             td.incoming.max_connecting)).to_int32()
            max_connected = sections(i)("max_connected",
                                        default_value:=If(is_outgoing,
                                                          td.outgoing.max_connected,
                                                          td.incoming.max_connected)).to_int32()
            no_delay = sections(i)("no_delay", default_value:=False).to_bool()
            max_lifetime_ms = sections(i)("max_lifetime_ms",
                                          default_value:=If(response_timeout_ms < 0,
                                                            npos,
                                                            If(is_outgoing,
                                                               td.outgoing.max_lifetime_ms,
                                                               td.incoming.max_lifetime_ms))).to_int64()
            ipv4 = sections(i)("ipv4", default_value:=True).to_bool()
            target = sections(i)("target")
            reset_connection = sections(i)("reset_connection", default_value:=(response_timeout_ms < 0)).to_bool()
            chunk_count = sections(i)("chunk_count", default_value:=0).to_int32()
            enable_keepalive = sections(i)("enable_keepalive", default_value:=td.enable_keepalive).to_bool()
            first_keepalive_ms = sections(i)("first_keepalive_ms", default_value:=td.first_keepalive_ms).to_uint32()
            keepalive_interval_ms = sections(i)("keepalive_interval_ms",
                                                default_value:=td.keepalive_interval_ms).to_uint32()
            treat_no_flow_as_failure = sections(i)("treat_no_flow_as_failure", default_value:=True)
            wait_data_before_bridging = sections(i)("wait_data_before_bridging", default_value:=True)

            Dim p As powerpoint = Nothing
            Dim prefer_no_delay As Boolean = False
            prefer_no_delay = Not use_socket_send()
            If is_outgoing Then
                assert(powerpoint.create(token,
                                         host,
                                         port,
                                         p,
                                         connecting_timeout_ms,
                                         send_rate_sec,
                                         response_timeout_ms,
                                         receive_rate_sec,
                                         max_connecting,
                                         max_connected,
                                         If(prefer_no_delay, no_delay, True),
                                         max_lifetime_ms,
                                         ipv4,
                                         enable_keepalive,
                                         first_keepalive_ms,
                                         keepalive_interval_ms),
                       "failed to create connection ", connection_section_name, i + connection_section_index)
            Else
                assert(powerpoint.create(token,
                                         port,
                                         p,
                                         send_rate_sec,
                                         response_timeout_ms,
                                         receive_rate_sec,
                                         max_connecting,
                                         max_connected,
                                         If(prefer_no_delay, no_delay, True),
                                         max_lifetime_ms,
                                         enable_keepalive,
                                         first_keepalive_ms,
                                         keepalive_interval_ms),
                       "failed to create connection ", connection_section_name, i + connection_section_index)
            End If
            assert(Not p Is Nothing)
            assert(connection_manager.register(name, p))
            If Not target.null_or_empty() Then
                Dim cid As Int64 = 0
                cid = counter.register(strtoupper(strcat(name,
                                                         "_to_",
                                                         target,
                                                         "_used_time_ms")),
                                       last_average_length:=32,
                                       write_count:=True,
                                       write_average:=True,
                                       write_last_average:=True)
                Dim sid As Int64 = 0
                sid = counter.register(strtoupper(strcat(name,
                                                         "_to_",
                                                         target,
                                                         "_bytes_transfered")),
                                       last_average_length:=32,
                                       last_rate_length:=32,
                                       write_count:=True,
                                       write_average:=True,
                                       write_last_average:=True,
                                       write_rate:=True,
                                       write_last_rate:=True)
                assert(p.get_all(Function(c1 As pre_initialized_device(Of ref_client)) As event_comb
                                     Dim c2 As pre_initialized_device(Of ref_client) = Nothing
                                     Dim p2 As pointer(Of powerpoint) = Nothing
                                     Dim ec As event_comb = Nothing
                                     Dim start_ms As Int64 = 0
                                     Dim total_bytes As atomic_int64 = Nothing
                                     Return New event_comb(
                                         Function() As Boolean
                                             If wait_data_before_bridging Then
                                                 ec = p.as_flow(c1).sense(response_timeout_ms)
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             Else
                                                 ec = Nothing
                                                 Return goto_next()
                                             End If
                                         End Function,
                                         Function() As Boolean
                                             If ec.end_result_or_null() Then
                                                 If network_trace Then
                                                     raise_error("start to answer on connection ",
                                                                 name,
                                                                 ", to target ",
                                                                 target)
                                                 End If
                                                 start_ms = Now().milliseconds()
                                                 c2 = New pointer(Of TcpClient)()
                                                 p2 = New pointer(Of powerpoint)()
                                                 ec = connection_manager.get(target,
                                                                             c2,
                                                                             connecting_timeout_ms,
                                                                             p2)
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             Else
                                                 Return False
                                             End If
                                         End Function,
                                         Function() As Boolean
                                             If ec.end_result() Then
                                                 assert(Not (+c2) Is Nothing)
                                                 assert(Not (+p2) Is Nothing)
                                                 total_bytes = New atomic_int64()
                                                 If chunk_count = 0 Then
                                                     ec = +(New convector(Of piece)(
                                                                create_dev(c1, p),
                                                                create_dev(+c2, +p2),
                                                                npos,
                                                                False,
                                                                p.response_timeout_ms,
                                                                total_bytes,
                                                                treat_no_flow_as_failure:=treat_no_flow_as_failure))
                                                 Else
                                                     ec = +(New convector(Of piece)(
                                                                create_dev(c1, p),
                                                                create_dev(+c2, +p2),
                                                                npos,
                                                                New pipe(Of piece)(If(chunk_count < 0,
                                                                                      uint32_0,
                                                                                      chunk_count),
                                                                                   npos,
                                                                                   True),
                                                                p.response_timeout_ms,
                                                                total_bytes,
                                                                treat_no_flow_as_failure:=treat_no_flow_as_failure))
                                                 End If
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             Else
                                                 raise_error(error_type.warning,
                                                             "failed to get connection from target ",
                                                             target,
                                                             ", consider to increase the ",
                                                             "max_connected value of the connection")
                                                 Return False
                                             End If
                                         End Function,
                                         Function() As Boolean
                                             assert(Not (+c2) Is Nothing)
                                             If Not ec.end_result() OrElse reset_connection Then
                                                 c2.get().shutdown()
                                             End If
                                             'the release will return false
                                             'if the connection has been reset,
                                             'so the s will also be reset
                                             counter.increase(tcp_bridge.TOTAL_BYTES, +total_bytes)
                                             Dim used_ms As Int64 = 0
                                             used_ms = Now().milliseconds() - start_ms
                                             counter.increase(cid, used_ms)
                                             counter.increase(sid, +total_bytes)
                                             If network_trace Then
                                                 raise_error("finish answer on connection ",
                                                             name,
                                                             ", release connection on target ",
                                                             target,
                                                             ", convect result ",
                                                             ec.end_result(),
                                                             ", used time in ms ",
                                                             used_ms,
                                                             ", total bytes transfered ",
                                                             +total_bytes)
                                             End If
                                             'release false == not ec.end_result() orelse reset_connection
                                             Return connection_manager.release(target, +c2) AndAlso
                                                    ec.end_result() AndAlso
                                                    goto_begin()
                                         End Function)
                                 End Function))
            End If
        Next

        'just for test, make sure no memory leak from event_comb
        gc_trigger()
    End Sub
End Module
