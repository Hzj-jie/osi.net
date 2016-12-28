
Imports osi.root.connector

Partial Public Class device_pool(Of T)
    Protected Overridable Function get_device(ByRef r As idevice(Of T)) As Boolean
        assert(False)
        Return False
    End Function

    Protected Overridable Function release_device(ByVal c As idevice(Of T)) As Boolean
        assert(False)
        Return False
    End Function

    Protected Overridable Function device_creator() As idevice_creator(Of T)
        assert(False)
        Return Nothing
    End Function

    Protected Overridable Function auto_device_exporter() As iauto_device_exporter(Of T)
        assert(False)
        Return Nothing
    End Function

    Protected Overridable Function manual_device_exporter() As imanual_device_exporter(Of T)
        assert(False)
        Return Nothing
    End Function
End Class
