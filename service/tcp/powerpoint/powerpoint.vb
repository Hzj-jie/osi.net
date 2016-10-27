
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.commander
Imports osi.service.transmitter

Partial Public Class powerpoint
    Public ReadOnly token As String
    Public ReadOnly host_or_ip As String
    Public ReadOnly port As UInt16
    Public ReadOnly connecting_timeout_ms As Int64
    Public ReadOnly send_rate_sec As UInt32
    Public ReadOnly response_timeout_ms As Int64
    Public ReadOnly receive_rate_sec As UInt32
    Public ReadOnly max_connecting As UInt32
    Public ReadOnly max_connected As UInt32
    Public ReadOnly max_lifetime_ms As Int64
    Public ReadOnly no_delay As Boolean
    Public ReadOnly is_outgoing As Boolean
    Public ReadOnly enable_keepalive As Boolean
    Public ReadOnly first_keepalive_ms As UInt32
    Public ReadOnly keepalive_interval_ms As UInt32
    Public ReadOnly ipv4 As Boolean
    Public ReadOnly address_family As AddressFamily
    Public ReadOnly identity As String
    Public ReadOnly tokener As String
    Public ReadOnly delay_connect As Boolean
    Private ReadOnly overhead As UInt32
    Private ReadOnly _ref_client_creator As iasync_device_creator(Of ref_client)
    Private ReadOnly _ref_client_auto_device_exporter As iauto_device_exporter(Of ref_client)
    Private ReadOnly _ref_client_manual_device_exporter As imanual_device_exporter(Of ref_client)
    Private ReadOnly _flow_creator As idevice_creator(Of flow)
    Private ReadOnly _flow_auto_device_exporter As iauto_device_exporter(Of flow)
    Private ReadOnly _flow_manual_device_exporter As imanual_device_exporter(Of flow)

    Private Sub New(ByVal token As String,
                    ByVal host_or_ip As String,
                    ByVal port As UInt32,
                    ByVal ipv4 As Boolean,
                    ByVal connecting_timeout_ms As Int64,
                    ByVal send_rate_sec As UInt32,
                    ByVal response_timeout_ms As Int64,
                    ByVal receive_rate_sec As UInt32,
                    ByVal max_connecting As UInt32,
                    ByVal max_connected As UInt32,
                    ByVal no_delay As Boolean,
                    ByVal max_lifetime_ms As Int64,
                    ByVal is_outgoing As Boolean,
                    ByVal enable_keepalive As Boolean,
                    ByVal first_keepalive_ms As UInt32,
                    ByVal keepalive_interval_ms As UInt32,
                    ByVal tokener As String,
                    ByVal delay_connect As Boolean)
        assert(max_connecting > 0)
        assert(max_connecting <= max_connected OrElse max_connected = 0)
        assert(is_outgoing Xor String.IsNullOrEmpty(host_or_ip))
        assert(port <> socket_invalid_port)

        Me.token = token
        Me.host_or_ip = host_or_ip
        Me.port = port
        Me.ipv4 = ipv4
        Me.address_family = If(ipv4, AddressFamily.InterNetwork, AddressFamily.InterNetworkV6)
        Me.connecting_timeout_ms = connecting_timeout_ms
        Me.send_rate_sec = send_rate_sec
        Me.response_timeout_ms = response_timeout_ms
        Me.receive_rate_sec = receive_rate_sec
        Me.max_connecting = max_connecting
        Me.max_connected = max_connected
        Me.no_delay = no_delay
        Me.max_lifetime_ms = max_lifetime_ms
        Me.is_outgoing = is_outgoing
        Me.enable_keepalive = enable_keepalive
        Me.first_keepalive_ms = first_keepalive_ms
        Me.keepalive_interval_ms = keepalive_interval_ms
        Me.tokener = tokener
        Me.delay_connect = delay_connect

        Me.overhead = If(ipv4, 40, 60)
        Me.identity = strcat("tcp@",
                             If(is_outgoing, strcat("outgoing@", host_or_ip, ":", port),
                                strcat("incoming@", port)))

        If is_outgoing Then
            _ref_client_creator = New connector(Me)
            _ref_client_auto_device_exporter = auto_device_exporter_new(_ref_client_creator)
            _flow_creator = device_creator_adapter.[New](_ref_client_creator, AddressOf ref_client_to_flow)
            _flow_auto_device_exporter = auto_device_exporter_new(_flow_creator)
        Else
            _ref_client_manual_device_exporter = New manual_device_exporter(Of ref_client)(identity)
            _flow_manual_device_exporter = manual_device_exporter_adapter.[New](_ref_client_manual_device_exporter,
                                                                                AddressOf ref_client_to_flow)
            accepter.listen(Me)
        End If
    End Sub

    Public Shared Function auto_device_exporter_new(Of T)(ByVal i As iasync_device_creator(Of T)) _
                                                   As iauto_device_exporter(Of T)
        assert(Not i Is Nothing)
        Return auto_device_exporter.[New](i,
                                          constants.interval_ms.connector_check,
                                          constants.interval_ms.connector_fail,
                                          constants.default_value.connector_concurrently_connecting)
    End Function

    Public Shared Function auto_device_exporter_new(Of T)(ByVal i As idevice_creator(Of T)) _
                                                   As iauto_device_exporter(Of T)
        assert(Not i Is Nothing)
        Return auto_device_exporter.[New](i,
                                          constants.interval_ms.connector_check,
                                          constants.interval_ms.connector_fail,
                                          constants.default_value.connector_concurrently_connecting)
    End Function

    Public Function ref_client_to_flow(ByVal i As ref_client) As flow
        Return New client_flow_adapter(i, Me)
    End Function

    Public Function send_timeout_ms(ByVal count As Int64) As Int64
        Return rate_to_ms(send_rate_sec, count + overhead)
    End Function

    Public Function receive_timeout_ms(ByVal count As Int64) As Int64
        Return rate_to_ms(receive_rate_sec, count + overhead)
    End Function

    Public Function transceive_timeout() As transceive_timeout
        Return New transceive_timeout(send_rate_sec, receive_rate_sec, overhead)
    End Function

    Public Function ref_client_creator(ByRef o As iasync_device_creator(Of ref_client)) As Boolean
        If _ref_client_creator Is Nothing Then
            Return False
        Else
            o = _ref_client_creator
            Return True
        End If
    End Function

    Public Function ref_client_creator() As iasync_device_creator(Of ref_client)
        assert(Not _ref_client_creator Is Nothing)
        Return _ref_client_creator
    End Function

    Public Function ref_client_auto_device_exporter(ByRef o As iauto_device_exporter(Of ref_client)) As Boolean
        If _ref_client_auto_device_exporter Is Nothing Then
            Return False
        Else
            o = _ref_client_auto_device_exporter
            Return True
        End If
    End Function

    Public Function ref_client_auto_device_exporter() As iauto_device_exporter(Of ref_client)
        assert(Not _ref_client_auto_device_exporter Is Nothing)
        Return _ref_client_auto_device_exporter
    End Function

    Public Function ref_client_manual_device_exporter(ByRef o As imanual_device_exporter(Of ref_client)) As Boolean
        If _ref_client_manual_device_exporter Is Nothing Then
            Return False
        Else
            o = _ref_client_manual_device_exporter
            Return True
        End If
    End Function

    Public Function ref_client_manual_device_exporter() As imanual_device_exporter(Of ref_client)
        assert(Not _ref_client_manual_device_exporter Is Nothing)
        Return _ref_client_manual_device_exporter
    End Function

    Public Function flow_creator(ByRef o As idevice_creator(Of flow)) As Boolean
        If _flow_creator Is Nothing Then
            Return False
        Else
            o = _flow_creator
            Return True
        End If
    End Function

    Public Function flow_creator() As idevice_creator(Of flow)
        assert(Not _flow_creator Is Nothing)
        Return _flow_creator
    End Function

    Public Function flow_manual_device_exporter(ByRef o As imanual_device_exporter(Of flow)) As Boolean
        If _flow_manual_device_exporter Is Nothing Then
            Return False
        Else
            o = _flow_manual_device_exporter
            Return True
        End If
    End Function

    Public Function flow_manual_device_exporter() As imanual_device_exporter(Of flow)
        assert(Not _flow_manual_device_exporter Is Nothing)
        Return _flow_manual_device_exporter
    End Function

    Public Function flow_auto_device_exporter(ByRef o As iauto_device_exporter(Of flow)) As Boolean
        If _flow_auto_device_exporter Is Nothing Then
            Return False
        Else
            o = _flow_auto_device_exporter
            Return True
        End If
    End Function

    Public Function flow_auto_device_exporter() As iauto_device_exporter(Of flow)
        assert(Not _flow_auto_device_exporter Is Nothing)
        Return _flow_auto_device_exporter
    End Function

    Public Function herald_creator(ByRef o As idevice_creator(Of herald)) As Boolean
        Dim c As idevice_creator(Of flow) = Nothing
        If flow_creator(c) Then
            o = device_creator_adapter.[New](c, Function(i As flow) As herald
                                                    Return flow_herald_adapter.[New](i)
                                                End Function)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function _herald_creator() As idevice_creator(Of herald)
        Dim o As idevice_creator(Of herald) = Nothing
        herald_creator(o)
        Return o
    End Function

    Public Function herald_creator() As idevice_creator(Of herald)
        Dim o As idevice_creator(Of herald) = Nothing
        assert(herald_creator(o))
        Return o
    End Function

    Public Function herald_manual_device_exporter(ByRef o As imanual_device_exporter(Of herald)) As Boolean
        Dim e As imanual_device_exporter(Of flow) = Nothing
        If flow_manual_device_exporter(e) Then
            o = manual_device_exporter_adapter.[New](e, Function(i As flow) As herald
                                                            Return flow_herald_adapter.[New](i)
                                                        End Function)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function _herald_manual_device_exporter() As imanual_device_exporter(Of herald)
        Dim o As imanual_device_exporter(Of herald) = Nothing
        herald_manual_device_exporter(o)
        Return o
    End Function

    Public Function herald_manual_device_exporter() As imanual_device_exporter(Of herald)
        Dim o As imanual_device_exporter(Of herald) = Nothing
        assert(herald_manual_device_exporter(o))
        Return o
    End Function

    Public Function herald_auto_device_exporter(ByRef o As iauto_device_exporter(Of herald)) As Boolean
        Dim c As idevice_creator(Of herald) = Nothing
        If herald_creator(c) Then
            o = auto_device_exporter_new(c)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function _herald_auto_device_exporter() As iauto_device_exporter(Of herald)
        Dim o As iauto_device_exporter(Of herald) = Nothing
        herald_auto_device_exporter(o)
        Return o
    End Function

    Public Function herald_auto_device_exporter() As iauto_device_exporter(Of herald)
        Dim o As iauto_device_exporter(Of herald) = Nothing
        assert(herald_auto_device_exporter(o))
        Return o
    End Function

    Private Function device_pool_new(Of T)(ByVal c As idevice_creator(Of T),
                                           ByVal a As iauto_device_exporter(Of T),
                                           ByVal m As imanual_device_exporter(Of T)) As idevice_pool(Of T)
        Dim r As device_pool(Of T) = Nothing
        If is_outgoing Then
            If delay_connect AndAlso Not c Is Nothing Then
                r = delay_generate_device_pool.[New](c, max_connected, identity)
            Else
                r = auto_pre_generated_device_pool.[New](a, max_connected, identity)
            End If
        Else
            r = manual_pre_generated_device_pool.[New](m, max_connected, identity)
        End If
        r.attach_checker(constants.interval_ms.connection_check_interval)
        Return r
    End Function

    Public Function ref_client_device_pool() As idevice_pool(Of ref_client)
        Return device_pool_new(Nothing, _ref_client_auto_device_exporter, _ref_client_manual_device_exporter)
    End Function

    Public Function flow_device_pool() As idevice_pool(Of flow)
        Return device_pool_new(_flow_creator, _flow_auto_device_exporter, _flow_manual_device_exporter)
    End Function

    Public Function herald_device_pool() As idevice_pool(Of herald)
        Return device_pool_new(_herald_creator(), _herald_auto_device_exporter(), _herald_manual_device_exporter())
    End Function

    'test purpose only, make sure all the related event_combs and stopwatch events are stopped
    Public Shared Sub waitfor_stop(ByVal ParamArray this() As powerpoint)
        sleep(stop_milliseconds(this))
    End Sub

    'the longest time in milliseconds to let all the related event_combs stop
    Public Shared Function stop_milliseconds(ByVal ParamArray this() As powerpoint) As Int64
        Dim m As Int64 = 0
        m = max(constants.interval_ms.connection_check_interval,
                constants.interval_ms.connector_check)
        m = max(m, constants.interval_ms.connector_fail)
        For i As Int32 = 0 To array_size(this) - 1
            If Not this(i) Is Nothing AndAlso
               this(i).is_outgoing Then
                m = max(m, this(i).connecting_timeout_ms)
            End If
        Next
        'accepter should be finished almost immediately
        Return m
    End Function
End Class
