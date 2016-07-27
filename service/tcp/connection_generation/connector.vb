
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.device
Imports osi.service.selector

'create connections for a powerpoint
Partial Public NotInheritable Class connector
    Implements iasync_device_creator(Of ref_client)

    Private ReadOnly p As powerpoint

    Public Sub New(ByVal p As powerpoint)
        assert(Not p Is Nothing)
        Me.p = p
        assert(p.is_outgoing)
    End Sub

    Public Function create(ByRef o As idevice(Of async_getter(Of ref_client))) As Boolean _
                          Implements iasync_device_creator(Of ref_client).create
        o = connection.[New](p, New async_preparer(Of ref_client)(AddressOf connect))
        Return True
    End Function
End Class
