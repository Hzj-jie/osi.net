
#If TODO Then
Imports osi.root.connector
Imports osi.service.selector

' Share one local sharable device with several devices.
' Refer to osi.service.selector.shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
Partial Public Class shared_device(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Private ReadOnly sc As shared_component(Of PORT_T, ADDRESS_T, idevice(Of COMPONENT_T), DATA_T, PARAMETER_T)

    ' A shared_device is always created by shared_device.creator, shared_device.auto_exporter or
    ' shared_device.manual_exporter.
    Private Sub New(ByVal sc As shared_component(Of PORT_T, ADDRESS_T, idevice(Of COMPONENT_T), DATA_T, PARAMETER_T))
        assert(Not sc Is Nothing)
        Me.sc = sc
    End Sub

    Public Shared Operator +(ByVal this As shared_device(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)) As _
                            idevice(Of shared_device(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T))

    End Operator
End Class
#End If
