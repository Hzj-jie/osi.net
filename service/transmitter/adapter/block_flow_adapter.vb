
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class block_flow_adapter
    Inherits T_adapter(Of block)
    Implements flow

    Private ReadOnly pump As block_pump_flow_pump_adapter

    Public Sub New(ByVal b As block)
        MyBase.New(b)
        assert(Not b Is Nothing)
        Me.pump = New block_pump_flow_pump_adapter(underlying_device)
    End Sub

    Public Shared Function [New](ByVal b As block) As flow
        Return New block_flow_adapter(b)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As ref(Of UInt32)) As event_comb Implements flow_injector.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = underlying_device.send(buff, offset, count)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(sent, count) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As ref(Of UInt32)) As event_comb Implements flow_pump.receive
        Return pump.receive(buff, offset, count, result)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
