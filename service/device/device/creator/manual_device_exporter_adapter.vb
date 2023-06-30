
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.selector

Public NotInheritable Class manual_device_exporter_adapter
    Public Shared Function [New](Of IT, OT)(ByVal i As imanual_device_exporter(Of IT),
                                            ByVal c As Func(Of IT, OT)) As manual_device_exporter_adapter(Of IT, OT)
        Return New manual_device_exporter_adapter(Of IT, OT)(i, c)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class manual_device_exporter_adapter(Of IT, OT)
    Inherits device_exporter_adapter(Of IT, OT)
    Implements imanual_device_exporter(Of OT)

    Public Sub New(ByVal i As imanual_device_exporter(Of IT), ByVal c As Func(Of IT, OT))
        MyBase.New(i, c)
    End Sub

    Public Function inject(ByVal d As idevice(Of async_getter(Of OT))) As event_comb _
                          Implements imanual_device_exporter(Of OT).inject
        assert(False)
        Return Nothing
    End Function

    Public Function inject(ByVal d As idevice(Of OT)) As Boolean Implements imanual_device_exporter(Of OT).inject
        assert(False)
        Return Nothing
    End Function
End Class
