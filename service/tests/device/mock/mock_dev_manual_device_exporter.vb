
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.device
Imports osi.service.selector

Public Class mock_dev_manual_device_exporter(Of PROTECTOR)
    Inherits device_exporter(Of mock_dev(Of PROTECTOR))
    Implements imanual_device_exporter(Of mock_dev(Of PROTECTOR))

    Private ReadOnly c As mock_device_creator(Of PROTECTOR)

    Public Sub New()
        c = New mock_device_creator(Of PROTECTOR)()
    End Sub

    Public Function go() As Boolean
        Return device_exported(c.create())
    End Function

    Public Function inject(ByVal d As idevice(Of async_getter(Of mock_dev(Of PROTECTOR)))) As event_comb _
                          Implements imanual_device_exporter(Of mock_dev(Of PROTECTOR)).inject
        assert(False)
        Return Nothing
    End Function

    Public Function inject(ByVal d As idevice(Of mock_dev(Of PROTECTOR))) As Boolean _
                          Implements imanual_device_exporter(Of mock_dev(Of PROTECTOR)).inject
        assert(False)
        Return False
    End Function
End Class
