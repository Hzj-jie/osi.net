
Imports osi.root.formation

Partial Public Class shared_component(Of RESOURCE_ID_T, REMOTE_ID_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Class endpoint
        Public ReadOnly resource_id As RESOURCE_ID_T
        Public ReadOnly remote_id As REMOTE_ID_T

        Public Sub New(ByVal resource_id As RESOURCE_ID_T, ByVal remote_id As REMOTE_ID_T)
            Me.resource_id = resource_id
            Me.remote_id = remote_id
        End Sub
    End Class

    ' Implementation of this interface should be singleton to represent all devices in the system.
    Public Interface collection
        ' Create a new or retrieve existing device with the PARAMETER_T.local_resource_id if it's valid.
        ' Or find next free local_resource_id, and create a new device if PARAMTER_T.local_resource_id is invalid.
        ' This function always set @local_resource_id to the "real" id.
        Function [New](ByVal p As PARAMETER_T,
                       ByRef local_resource_id As RESOURCE_ID_T,
                       ByRef o As ref_instance(Of COMPONENT_T)) As Boolean
        ' Create a new or retrieve existing dispenser to receive data from @i.
        Function [New](ByVal p As PARAMETER_T,
                       ByVal local_resource_id As RESOURCE_ID_T,
                       ByVal i As ref_instance(Of COMPONENT_T),
                       ByRef dispenser As dispenser(Of DATA_T, endpoint)) As Boolean
    End Interface
End Class
