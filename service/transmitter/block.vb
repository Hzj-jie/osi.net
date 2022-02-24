
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Interface block_receiver
    Inherits block_pump, sensor
End Interface

Public Interface block_sender
    Inherits block_injector
End Interface

Public Interface block
    Inherits block_sender, block_receiver
End Interface

Public Module _block
    <Extension()> Public Function transmit_mode(ByVal this As block) As trait.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {block_pump, sensor}) _
                                                  (ByVal this As T,
                                                   ByVal timeout_ms As Int64,
                                                   ByVal r As ref(Of Byte())) As event_comb
        assert(Not this Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = sense(this, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = this.receive(r)
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

    <Extension()> Public Function wait_and_receive(Of T As {block_pump, sensor}) _
                                                  (ByVal this As T,
                                                   ByVal r As ref(Of Byte())) As event_comb
        Return wait_and_receive(this, npos, r)
    End Function
End Module
