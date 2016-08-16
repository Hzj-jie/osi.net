
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.device
Imports osi.service.selector

Partial Public NotInheritable Class connector
    Implements iasync_device_creator(Of delegator), idevice_creator(Of UdpClient)

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
End Class
