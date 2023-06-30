
Imports osi.root.constants

Partial Public Class device_pool
    Protected Overridable Function get_free_count() As UInt32
        Return uint32_0
    End Function

    Protected Overridable Sub close_devices()
    End Sub
End Class