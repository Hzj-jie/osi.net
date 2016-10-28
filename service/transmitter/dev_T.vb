
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Interface T_receiver(Of T)
    Inherits T_pump(Of T), sensor
End Interface

Public Interface T_sender(Of T)
    Inherits T_injector(Of T)
End Interface

Public Interface dev_T(Of T)
    Inherits T_sender(Of T), T_receiver(Of T)
End Interface

Public Module _dev_T
    <Extension()> Public Function transmit_mode(Of T)(ByVal this As dev_T(Of T)) As trait.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    Public Function wait_and_receive(Of T1, T2 As {T_pump(Of T1), sensor}) _
                                    (ByVal this As T2,
                                     ByVal timeout_ms As Int64,
                                     ByVal r As pointer(Of T1)) As event_comb
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

    Public Function wait_and_receive(Of T1, T2 As {T_pump(Of T1), sensor}) _
                                    (ByVal this As T2,
                                     ByVal r As pointer(Of T1)) As event_comb
        Return wait_and_receive(this, npos, r)
    End Function
End Module
