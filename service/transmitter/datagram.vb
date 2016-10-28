
Imports System.Runtime.CompilerServices

Public Interface datagram_receiver
    Inherits block_pump, sensor
End Interface

Public Interface datagram_sender
    Inherits flow_injector
End Interface

Public Interface datagram
    Inherits datagram_sender, datagram_receiver
End Interface

Public Module _datagram
    <Extension()> Public Function transmit_mode(ByVal this As datagram) As trait.mode_t
        Return _transmitter.transmit_mode(this)
    End Function

    <Extension()> Public Function packet_size(ByVal this As datagram) As UInt32
        Return _transmitter.packet_size(this)
    End Function
End Module
