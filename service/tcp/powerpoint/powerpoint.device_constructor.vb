
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.transmitter

<global_init(global_init_level.server_services)>
Partial Public Class powerpoint
    Private Const type_str As String = "tcp"

    Private Shared Sub init()
        assert(constructor.register(type_str,
                                    Function(v As var, ByRef o As idevice_creator(Of flow)) As Boolean
                                        Dim p As powerpoint = Nothing
                                        Return powerpoint.create(v, p) AndAlso
                                               assert(Not p Is Nothing) AndAlso
                                               p.flow_creator(o)
                                    End Function))
        assert(constructor.register(type_str,
                                    Function(v As var, ByRef o As imanual_device_exporter(Of flow)) As Boolean
                                        Dim p As powerpoint = Nothing
                                        Return powerpoint.create(v, p) AndAlso
                                               assert(Not p Is Nothing) AndAlso
                                               p.flow_manual_device_exporter(o)
                                    End Function))
        assert(constructor.register(type_str,
                                    Function(v As var, ByRef o As iauto_device_exporter(Of flow)) As Boolean
                                        Dim p As powerpoint = Nothing
                                        Return powerpoint.create(v, p) AndAlso
                                               assert(Not p Is Nothing) AndAlso
                                               p.flow_auto_device_exporter(o)
                                    End Function))
    End Sub
End Class
