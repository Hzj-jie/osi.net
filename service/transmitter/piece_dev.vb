
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.procedure

Public Interface piece_dev
    Inherits dev_T(Of piece)
End Interface

Public Module _piece_dev
    <Extension()> Public Function transmit_mode(ByVal this As piece_dev) As trait.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {T_pump(Of piece), sensor}) _
                                                  (ByVal this As T,
                                                   ByVal timeout_ms As Int64,
                                                   ByVal r As pointer(Of piece)) As event_comb
        Return _dev_T.wait_and_receive(this, timeout_ms, r)
    End Function

    <Extension()> Public Function wait_and_receive(Of T As {T_pump(Of piece), sensor}) _
                                                  (ByVal this As T,
                                                   ByVal r As pointer(Of piece)) As event_comb
        Return _dev_T.wait_and_receive(this, r)
    End Function
End Module
