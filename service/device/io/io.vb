
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Public Module _io
    <Extension()> Public Function copy_to(ByVal this As flow_pump,
                                          ByVal that As flow_injector,
                                          ByVal count As UInt64,
                                          Optional ByVal buff_size As UInt32 = uint32_0) As event_comb
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return complete_io(AddressOf this.receive, AddressOf that.send, count, buff_size)
    End Function

    <Extension()> Public Function copy_to(ByVal this As flow_pump,
                                          ByVal that As flow_injector,
                                          Optional ByVal buff_size As UInt32 = uint32_0,
                                          Optional ByVal result As pointer(Of UInt64) = Nothing) As event_comb
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return until_pending(AddressOf this.receive, AddressOf that.send, buff_size, result)
    End Function

    <Extension()> Public Function copy_to(ByVal this As block_pump,
                                          ByVal that As block_injector) As event_comb
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return complete_io(AddressOf this.receive, AddressOf that.send)
    End Function
End Module

