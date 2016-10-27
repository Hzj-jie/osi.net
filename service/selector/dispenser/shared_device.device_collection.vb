
' TODO
Partial Public Class shared_device(Of RESOURCE_ID_T, REMOTE_ID_T, DEV_T, DATA_T, PARAMETER_T)
    Public Interface endpoint
        Function resource_id() As RESOURCE_ID_T
        Function remote_id() As REMOTE_ID_T
    End Interface

    ' Implementation of this interface should be singleton to represent all devices in the system.
    Public Interface collection
        ' Create a new or retrieve existing device with the PARAMETER_T.local_resource_id if it's valid.
        ' Or find next free local_resource_id, and create a new device.
        ' This function always set @local_resource_id to the "real" id.
        Function [New](ByVal p As PARAMETER_T, ByRef local_resource_id As RESOURCE_ID_T, ByRef o As DEV_T) As Boolean
        ' Create a new or retrieve existing dispenser to receive data from @i.
        Function [New](ByVal p As PARAMETER_T,
                       ByVal local_resource_id As RESOURCE_ID_T,
                       ByVal i As DEV_T,
                       ByRef dispenser As dispenser(Of DATA_T, endpoint)) As Boolean
    End Interface
End Class
