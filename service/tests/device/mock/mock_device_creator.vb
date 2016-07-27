
Imports osi.service.device

Public Class mock_device_creator(Of PROTECTOR)
    Inherits mock_creator(Of mock_dev(Of PROTECTOR))
    Implements idevice_creator(Of mock_dev(Of PROTECTOR))

    Public Sub New(Optional ByVal random_fail As Boolean = False,
                   Optional ByVal fake_cpu_ms As Int32 = 0)
        MyBase.New(random_fail, fake_cpu_ms)
    End Sub

    Protected Overrides Function create() As idevice(Of mock_dev(Of PROTECTOR))
        Return mock_dev(Of PROTECTOR).create()
    End Function

    Public Overloads Function create(ByRef o As idevice(Of mock_dev(Of PROTECTOR))) As Boolean _
                                    Implements idevice_creator(Of mock_dev(Of PROTECTOR)).create
        Return MyBase.create(o)
    End Function

    Public Shared Function exporter(Optional ByVal random_fail As Boolean = False,
                                    Optional ByVal fake_cpu_ms As Int32 = 0) _
                                   As iauto_device_exporter(Of mock_dev(Of PROTECTOR))
        Return auto_device_exporter.[New](New mock_device_creator(Of PROTECTOR)(random_fail, fake_cpu_ms))
    End Function
End Class
