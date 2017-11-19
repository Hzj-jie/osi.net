
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.selector

Partial Public Class sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    ' Implementation of this interface should be singleton to represent all devices in the system.
    Public Interface collection
        ' Create a new or retrieve existing device with the @local_port if it's valid.
        ' Or find next free local_port, and create a new device if @local_port is invalid.
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

Public Module _sharedtransmitter_collection
    <Extension()> Public Function [New](Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                                       (ByVal this As sharedtransmitter(Of PORT_T,
                                                                          ADDRESS_T,
                                                                          COMPONENT_T,
                                                                          DATA_T,
                                                                          PARAMETER_T).collection,
                                        ByVal p As PARAMETER_T,
                                        ByRef component As ref_instance(Of COMPONENT_T),
                                        ByRef dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) _
                                       As Boolean
        assert(Not this Is Nothing)
        Dim local_port As PORT_T = Nothing
        Return this.[New](p, local_port, component) AndAlso
               this.[New](p, local_port, component, dispenser)
    End Function
End Module
