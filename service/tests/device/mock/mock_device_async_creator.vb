
Imports osi.service.device
Imports osi.service.selector

Public Class mock_device_async_creator(Of PROTECTOR)
    Inherits mock_creator(Of async_getter(Of mock_dev(Of PROTECTOR)))
    Implements iasync_device_creator(Of mock_dev(Of PROTECTOR))

    Public Sub New(Optional ByVal random_fail As Boolean = False,
                   Optional ByVal fake_cpu_ms As Int32 = 0)
        MyBase.New(random_fail, fake_cpu_ms)
    End Sub

    Protected Overrides Function create() As idevice(Of async_getter(Of mock_dev(Of PROTECTOR)))
        Return New async_getter_mock_dev_device(Of PROTECTOR)()
    End Function

    Public Overloads Function create(ByRef o As idevice(Of async_getter(Of mock_dev(Of PROTECTOR)))) As Boolean _
                                    Implements idevice_creator(Of async_getter(Of mock_dev(Of PROTECTOR))).create
        Return MyBase.create(o)
    End Function
End Class
