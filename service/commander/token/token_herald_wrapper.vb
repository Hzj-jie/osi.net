
'TODO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

Public Class token_herald_wrapper
    Implements herald

    Private ReadOnly h As herald

    Public Sub New(ByVal h As herald)
        assert(Not h Is Nothing)
        Me.h = h
    End Sub

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return Nothing
    End Function

    Public Function send(ByVal i As command) As event_comb Implements T_injector(Of command).send
        Return Nothing
    End Function

    Public Function receive(ByVal o As ref(Of command)) As event_comb Implements T_pump(Of command).receive
        Return Nothing
    End Function
End Class
