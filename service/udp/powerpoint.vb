
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.transmitter
Imports osi.service.selector
Imports osi.service.commander

Partial Public Class powerpoint
    Public ReadOnly host_or_ip As String
    Public ReadOnly remote_port As UInt16
    Public ReadOnly local_port As UInt16
    Public ReadOnly response_timeout_ms As Int64
    Public ReadOnly ipv4 As Boolean
    Public ReadOnly max_receive_buffer_size As UInt32
    Public ReadOnly address_family As AddressFamily
    Public ReadOnly identity As String
    Public ReadOnly packet_size As UInt32
    Public ReadOnly accept_new_connection As Boolean
    Public ReadOnly max_connected As UInt32
    Private ReadOnly send_rate_sec As UInt32
    Private ReadOnly receive_rate_sec As UInt32
    Private ReadOnly overhead As UInt32
    ' Fixed local & target
    Private ReadOnly _udp_dev_device As idevice(Of async_getter(Of udp_dev))
    Private ReadOnly _async_getter_datagram_device As idevice(Of async_getter(Of datagram))
    Private ReadOnly _datagram_device As idevice(Of datagram)
    Private ReadOnly _herald_device As idevice(Of herald)
    ' Target is defined
    Private ReadOnly _udp_dev_creator As iasync_device_creator(Of udp_dev)
    Private ReadOnly _datagram_creator As idevice_creator(Of datagram)
    Private ReadOnly _herald_creator As idevice_creator(Of herald)
    ' Local port is defined
    Private ReadOnly _udp_dev_manual_device_exporter As imanual_device_exporter(Of udp_dev)
    Private ReadOnly _datagram_manual_device_exporter As imanual_device_exporter(Of datagram)
    Private ReadOnly _herald_manual_device_exporter As imanual_device_exporter(Of herald)
    Private ReadOnly closer As IDisposable

    Private Sub New(ByVal host_or_ip As String,
                    ByVal remote_port As UInt16,
                    ByVal local_port As UInt16,
                    ByVal response_timeout_ms As Int64,
                    ByVal send_rate_sec As UInt32,
                    ByVal receive_rate_sec As UInt32,
                    ByVal ipv4 As Boolean,
                    ByVal max_receive_buffer_size As UInt32,
                    ByVal accept_new_connection As Boolean,
                    ByVal max_connected As UInt32)
        Me.host_or_ip = host_or_ip
        Me.remote_port = remote_port
        Me.local_port = local_port
        Me.response_timeout_ms = response_timeout_ms
        Me.send_rate_sec = send_rate_sec
        Me.receive_rate_sec = receive_rate_sec
        Me.ipv4 = ipv4
        Me.max_receive_buffer_size = max_receive_buffer_size
        Me.accept_new_connection = accept_new_connection AndAlso local_defined() AndAlso Not remote_defined()
        Me.max_connected = max_connected

        Me.address_family = If(ipv4, AddressFamily.InterNetwork, AddressFamily.InterNetworkV6)
        Me.overhead = If(ipv4, constants.ipv4_overhead, constants.ipv6_overhead)
        Me.identity = strcat("udp@port:", local_port, "@remote:", host_or_ip, ":", remote_port)
        Me.packet_size = If(ipv4, constants.ipv4_packet_size, constants.ipv6_packet_size)
        If Me.max_receive_buffer_size < (packet_size << 1) Then
            Me.max_receive_buffer_size = (packet_size << 1)
        End If

        If p2p() Then
            assert((New connector(Me)).create(_udp_dev_device))
            _async_getter_datagram_device = device_adapter.[New](_udp_dev_device, AddressOf udp_dev_to_datagram)
            assert(async_device_device_converter.adapt(_async_getter_datagram_device, _datagram_device))
            _herald_device = device_adapter.[New](_datagram_device, AddressOf datagram_to_herald)
            closer = New disposer(Sub()
                                      async_getter_datagram_device().close()
                                      herald_device().close()
                                  End Sub)
        ElseIf incoming() Then
            _udp_dev_manual_device_exporter = New manual_device_exporter(Of udp_dev)(identity)
            _datagram_manual_device_exporter =
                manual_device_exporter_adapter.[New](_udp_dev_manual_device_exporter, AddressOf udp_dev_to_datagram)
            _herald_manual_device_exporter = manual_device_exporter_adapter.[New](_udp_dev_manual_device_exporter,
                                                                                  AddressOf udp_dev_to_herald)
            ' listeners.listen(Me)
            closer = New disposer(Sub()
                                      udp_dev_manual_device_exporter().dispose()
                                      datagram_manual_device_exporter().dispose()
                                      herald_manual_device_exporter().dispose()
                                  End Sub)
        ElseIf outgoing() Then
            _udp_dev_creator = New connector(Me)
            _datagram_creator = device_creator_adapter.[New](_udp_dev_creator, AddressOf udp_dev_to_datagram)
            _herald_creator = device_creator_adapter.[New](_udp_dev_creator, AddressOf udp_dev_to_herald)
            ' Do nothing
            closer = New empty_idisposable()
        Else
            assert(False)
        End If

        assert(Not closer Is Nothing)
    End Sub

    Public Function local_defined() As Boolean
        Return local_port <> socket_invalid_port
    End Function

    Public Function remote_defined() As Boolean
        Return Not String.IsNullOrEmpty(host_or_ip) AndAlso remote_port <> socket_invalid_port
    End Function

    Public Function p2p() As Boolean
        Return local_defined() AndAlso remote_defined()
    End Function

    Public Function incoming() As Boolean
        Return local_defined() AndAlso Not remote_defined()
    End Function

    Public Function outgoing() As Boolean
        Return remote_defined() AndAlso Not local_defined()
    End Function

    Public Shared Function udp_dev_to_datagram(ByVal i As udp_dev) As datagram
        Return i
    End Function

    Public Shared Function udp_dev_to_herald(ByVal i As udp_dev) As herald
        Return datagram_to_herald(udp_dev_to_datagram(i))
    End Function

    Public Shared Function datagram_to_herald(ByVal i As datagram) As herald
        Return flow_herald_adapter.[New](datagram_flow_adapter.[New](i))
    End Function

    Public Function udp_dev_device(ByRef o As idevice(Of async_getter(Of udp_dev))) As Boolean
        If _udp_dev_device Is Nothing Then
            Return False
        Else
            o = _udp_dev_device
            Return True
        End If
    End Function

    Public Function udp_dev_device() As idevice(Of async_getter(Of udp_dev))
        Dim o As idevice(Of async_getter(Of udp_dev)) = Nothing
        assert(udp_dev_device(o))
        Return o
    End Function

    Public Function async_getter_datagram_device(ByRef o As idevice(Of async_getter(Of datagram))) As Boolean
        If _async_getter_datagram_device Is Nothing Then
            Return False
        Else
            o = _async_getter_datagram_device
            Return True
        End If
    End Function

    Public Function async_getter_datagram_device() As idevice(Of async_getter(Of datagram))
        Dim o As idevice(Of async_getter(Of datagram)) = Nothing
        assert(async_getter_datagram_device(o))
        Return o
    End Function

    Public Function datagram_device(ByRef o As idevice(Of datagram)) As Boolean
        If _datagram_device Is Nothing Then
            Return False
        Else
            o = _datagram_device
            Return True
        End If
    End Function

    Public Function datagram_device() As idevice(Of datagram)
        Dim o As idevice(Of datagram) = Nothing
        assert(datagram_device(o))
        Return o
    End Function

    Public Function herald_device(ByRef o As idevice(Of herald)) As Boolean
        If _herald_device Is Nothing Then
            Return False
        Else
            o = _herald_device
            Return True
        End If
    End Function

    Public Function herald_device() As idevice(Of herald)
        Dim o As idevice(Of herald) = Nothing
        assert(herald_device(o))
        Return o
    End Function

    Public Function udp_dev_creator(ByRef o As iasync_device_creator(Of udp_dev)) As Boolean
        If _udp_dev_creator Is Nothing Then
            Return False
        Else
            o = _udp_dev_creator
            Return True
        End If
    End Function

    Public Function udp_dev_creator() As iasync_device_creator(Of udp_dev)
        Dim o As iasync_device_creator(Of udp_dev) = Nothing
        assert(udp_dev_creator(o))
        Return o
    End Function

    Public Function datagram_creator(ByRef o As idevice_creator(Of datagram)) As Boolean
        If _datagram_creator Is Nothing Then
            Return False
        Else
            o = _datagram_creator
            Return True
        End If
    End Function

    Public Function datagram_creator() As idevice_creator(Of datagram)
        Dim o As idevice_creator(Of datagram) = Nothing
        assert(datagram_creator(o))
        Return o
    End Function

    Public Function herald_creator(ByRef o As idevice_creator(Of herald)) As Boolean
        If _herald_creator Is Nothing Then
            Return False
        Else
            o = _herald_creator
            Return True
        End If
    End Function

    Public Function herald_creator() As idevice_creator(Of herald)
        Dim o As idevice_creator(Of herald) = Nothing
        assert(herald_creator(o))
        Return o
    End Function

    Public Function udp_dev_manual_device_exporter(ByRef o As imanual_device_exporter(Of udp_dev)) As Boolean
        If _udp_dev_manual_device_exporter Is Nothing Then
            Return False
        Else
            o = _udp_dev_manual_device_exporter
            Return True
        End If
    End Function

    Public Function udp_dev_manual_device_exporter() As imanual_device_exporter(Of udp_dev)
        Dim o As imanual_device_exporter(Of udp_dev) = Nothing
        assert(udp_dev_manual_device_exporter(o))
        Return o
    End Function

    Public Function datagram_manual_device_exporter(ByRef o As imanual_device_exporter(Of datagram)) As Boolean
        If _datagram_manual_device_exporter Is Nothing Then
            Return False
        Else
            o = _datagram_manual_device_exporter
            Return True
        End If
    End Function

    Public Function datagram_manual_device_exporter() As imanual_device_exporter(Of datagram)
        Dim o As imanual_device_exporter(Of datagram) = Nothing
        assert(datagram_manual_device_exporter(o))
        Return o
    End Function

    Public Function herald_manual_device_exporter(ByRef o As imanual_device_exporter(Of herald)) As Boolean
        If _herald_manual_device_exporter Is Nothing Then
            Return False
        Else
            o = _herald_manual_device_exporter
            Return True
        End If
    End Function

    Public Function herald_manual_device_exporter() As imanual_device_exporter(Of herald)
        Dim o As imanual_device_exporter(Of herald) = Nothing
        assert(herald_manual_device_exporter(o))
        Return o
    End Function

    Private Function device_pool_new(Of T)(ByVal dev As idevice(Of T),
                                           ByVal creator As idevice_creator(Of T),
                                           ByVal exporter As imanual_device_exporter(Of T)) As idevice_pool(Of T)
        If local_defined() AndAlso remote_defined() Then
            assert(Not dev Is Nothing)
            Return singleton_device_pool.[New](dev, identity)
        ElseIf local_defined() Then
            assert(Not exporter Is Nothing)
            Return manual_pre_generated_device_pool.[New](exporter, max_connected, identity)
        ElseIf remote_defined() Then
            Return delay_generate_device_pool.[New](creator, max_connected, identity)
        Else
            assert(False)
            Return Nothing
        End If
    End Function

    Public Function datagram_device_pool() As idevice_pool(Of datagram)
        Return device_pool_new(_datagram_device, _datagram_creator, _datagram_manual_device_exporter)
    End Function

    Public Function herald_device_pool() As idevice_pool(Of herald)
        Return device_pool_new(_herald_device, _herald_creator, _herald_manual_device_exporter)
    End Function

    Public Function transceive_timeout() As transceive_timeout
        Return New transceive_timeout(send_rate_sec, receive_rate_sec, overhead)
    End Function

    Public Function remote_endpoint(ByRef o As const_pair(Of String, UInt16)) As Boolean
        If remote_defined() Then
            o = emplace_make_const_pair(host_or_ip, remote_port)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function remote_endpoint() As const_pair(Of String, UInt16)
        Dim r As const_pair(Of String, UInt16) = Nothing
        assert(remote_endpoint(r))
        Return r
    End Function

    Public Sub close()
        closer.dispose()
    End Sub
End Class
