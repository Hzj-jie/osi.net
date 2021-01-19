
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Interface flow_receiver
    Inherits flow_pump, sensor
End Interface

Public Interface flow_sender
    Inherits flow_injector
End Interface

Public Interface flow
    Inherits flow_sender, flow_receiver
End Interface

Public Module _flow
    <Extension()> Public Function transmit_mode(ByVal this As flow) As trait.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {flow_pump, sensor}) _
                                                  (ByVal this As T,
                                                   ByVal timeout_ms As Int64,
                                                   ByVal buff() As Byte,
                                                   ByVal offset As UInt32,
                                                   ByVal count As UInt32,
                                                   ByVal result As ref(Of UInt32)) As event_comb
        assert(Not this Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = sense(this, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = this.receive(buff, offset, count, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {flow_pump, sensor}) _
                                                  (ByVal this As T,
                                                   ByVal buff() As Byte,
                                                   ByVal offset As UInt32,
                                                   ByVal count As UInt32,
                                                   ByVal result As ref(Of UInt32)) As event_comb
        Return wait_and_receive(this, npos, buff, offset, count, result)
    End Function
End Module
