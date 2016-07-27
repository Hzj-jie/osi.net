
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class datagram_flow_adapter
    Inherits T_adapter(Of datagram)
    Implements flow

    Private ReadOnly pump As block_pump_flow_pump_adapter
    Private ReadOnly dt As datagram_transmitter

    Public Sub New(ByVal d As datagram)
        MyBase.New(d)
        Me.pump = New block_pump_flow_pump_adapter(underlying_device)
        Me.dt = attribute().get(Of datagram_transmitter)()
    End Sub

    Public Shared Function [New](ByVal d As datagram) As flow
        Return New datagram_flow_adapter(d)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As pointer(Of UInt32)) As event_comb Implements flow_injector.send
        Return underlying_device.send(buff, offset, min(count, dt.packet_size()), sent)
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As pointer(Of UInt32)) As event_comb Implements flow_pump.receive
        Return pump.receive(buff, offset, count, result)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
