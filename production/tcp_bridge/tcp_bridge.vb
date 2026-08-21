
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
Imports osi.service.transmitter
Imports osi.service.argument
Imports td = osi.service.tcp.constants.default_value
Imports counter = osi.root.utils.counter

Public Module tcp_bridge
    Private ReadOnly config As osi.service.configuration.config
    Private ReadOnly TOTAL_BYTES As Int64

    Sub New()
        enable_domain_unhandled_exception_handler()
        connection_state.bind()
        Dim config_file As String = "tcp_bridge.ini"
        Dim args() As String = Environment.GetCommandLineArgs()
        If array_size(args) > 1 AndAlso Not args(1).null_or_empty() Then
            config_file = args(1)
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
            Dim name As String = sections(i)("name", default_value:=strcat(connection_section_name, i + 1))
            Dim p As powerpoint = powerpoint.create(New var(sections(i).values()))
            assert(Not p Is Nothing)
        Next

        'just for test, make sure no memory leak from event_comb
        garbage_collector.trigger()
    End Sub
End Module
