
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

Public Interface herald
    Inherits dev_T(Of command)
End Interface

Public Module _herald
    <Extension()> Public Function transmit_mode(ByVal this As herald) As osi.service.transmitter.transmitter.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {T_pump(Of command), sensor}) _
                                                  (ByVal this As T,
                                                   ByVal timeout_ms As Int64,
                                                   ByVal r As pointer(Of command)) As event_comb
        Return _dev_T.wait_and_receive(this, timeout_ms, r)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {T_pump(Of command), sensor}) _
                                                  (ByVal this As T,
                                                   ByVal r As pointer(Of command)) As event_comb
        Return _dev_T.wait_and_receive(this, r)
    End Function
End Module