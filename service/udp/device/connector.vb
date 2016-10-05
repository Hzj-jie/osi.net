
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.device
Imports osi.service.selector

Partial Public NotInheritable Class connector
    Implements iasync_device_creator(Of delegator), 
               idevice_creator(Of UdpClient), 
               iasync_device_creator(Of udp_dev)

    Private ReadOnly p As powerpoint

    Public Sub New(ByVal p As powerpoint)
        assert(Not p Is Nothing)
        Me.p = p
    End Sub

    Public Function create(ByRef o As idevice(Of async_getter(Of delegator))) As Boolean _
                          Implements idevice_creator(Of async_getter(Of delegator)).create
        o = connection.[New](New async_preparer(Of delegator)(AddressOf connect))
        Return True
    End Function

    Public Function create(ByRef o As idevice(Of UdpClient)) As Boolean Implements idevice_creator(Of UdpClient).create
        Dim c As UdpClient = Nothing
        If connect(c) Then
            o = connection.[New](c)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function create(ByRef o As async_getter(Of listener.multiple_accepter)) As Boolean
        o = New async_preparer(Of listener.multiple_accepter)(AddressOf multiple_accepter)
        Return True
    End Function

    Public Function create(ByRef o As async_getter(Of udp_dev)) As Boolean
        o = New async_preparer(Of udp_dev)(AddressOf udp_dev)
        Return True
    End Function

    Public Function create(ByRef o As idevice(Of async_getter(Of udp_dev))) As Boolean _
                          Implements iasync_device_creator(Of udp_dev).create
        Dim x As async_getter(Of udp_dev) = Nothing
        If create(x) Then
            o = x.make_device(validator:=AddressOf osi.service.udp.udp_dev.validator,
                              closer:=AddressOf osi.service.udp.udp_dev.closer,
                              identifier:=AddressOf osi.service.udp.udp_dev.identifier)
            Return True
        Else
            Return False
        End If
    End Function
End Class
