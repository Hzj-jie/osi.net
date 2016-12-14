﻿
Imports osi.root.formation

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    ' Implementation of this interface should be singleton to represent all devices in the system.
    Public Interface collection
        ' Create a new or retrieve existing device with the PARAMETER_T.local_port if it's valid.
        ' Or find next free local_port, and create a new device if PARAMTER_T.local_port is invalid.
        ' This function always set @local_port to the "real" id.
        Function [New](ByVal p As PARAMETER_T,
                       ByRef local_port As PORT_T,
                       ByRef o As ref_instance(Of COMPONENT_T)) As Boolean
        ' Create a new or retrieve existing dispenser to receive data from @i.
        Function [New](ByVal p As PARAMETER_T,
                       ByVal local_port As PORT_T,
                       ByVal i As ref_instance(Of COMPONENT_T),
                       ByRef dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) As Boolean
    End Interface
End Class
