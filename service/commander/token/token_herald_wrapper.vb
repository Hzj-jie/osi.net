﻿
'TODO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Class token_herald_wrapper
    Implements herald

    Private ReadOnly h As herald

    Public Sub New(ByVal h As herald)
        assert(Not h Is Nothing)
        Me.h = h
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements device.sensor.sense

    End Function

    Public Function send(ByVal i As command) As event_comb Implements device.T_injector(Of command).send

    End Function

    Public Function receive(ByVal o As pointer(Of command)) As event_comb Implements device.T_pump(Of command).receive

    End Function
End Class
