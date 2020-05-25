
Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.procedure

Public Interface flow_injector
    Function send(ByVal buff() As Byte,
                  ByVal offset As UInt32,
                  ByVal count As UInt32,
                  ByVal sent As ref(Of UInt32)) As event_comb
End Interface

Public Interface block_injector
    Function send(ByVal buff() As Byte,
                  ByVal offset As UInt32,
                  ByVal count As UInt32) As event_comb
End Interface

Public Interface text_injector
    Function send(ByVal s As String,
                  ByVal offset As UInt32,
                  ByVal len As UInt32) As event_comb
End Interface

Public Interface stream_injector
    Function send(ByVal s As Stream,
                  ByVal len As UInt32) As event_comb
End Interface

Public Interface T_injector(Of T)
    Function send(ByVal i As T) As event_comb
End Interface

Public Module _injector
    <Extension()> Public Function send(ByVal f As flow_injector,
                                       ByVal buff() As Byte,
                                       ByVal count As UInt32,
                                       ByVal sent As ref(Of UInt32)) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, uint32_0, count, sent)
    End Function

    <Extension()> Public Function send(ByVal f As flow_injector,
                                       ByVal buff() As Byte,
                                       ByVal sent As ref(Of UInt32)) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, array_size(buff), sent)
    End Function

    <Extension()> Public Function send(ByVal f As flow_injector,
                                       ByVal buff() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32) As event_comb
        assert(Not f Is Nothing)
        Return complete_io_4(buff, offset, count, AddressOf f.send)
    End Function

    <Extension()> Public Function send(ByVal f As flow_injector,
                                       ByVal buff() As Byte,
                                       ByVal count As UInt32) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, uint32_0, count)
    End Function

    <Extension()> Public Function send(ByVal f As flow_injector,
                                       ByVal buff() As Byte) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, array_size(buff))
    End Function

    <Extension()> Public Function send(ByVal f As block_injector,
                                       ByVal buff() As Byte,
                                       ByVal count As UInt32) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, uint32_0, count)
    End Function

    <Extension()> Public Function send(ByVal f As block_injector,
                                       ByVal buff() As Byte) As event_comb
        assert(Not f Is Nothing)
        Return f.send(buff, array_size(buff))
    End Function

    <Extension()> Public Function send(ByVal f As text_injector,
                                       ByVal s As String,
                                       ByVal len As UInt32) As event_comb
        assert(Not f Is Nothing)
        Return f.send(s, uint32_0, len)
    End Function

    <Extension()> Public Function send(ByVal f As text_injector,
                                       ByVal s As String) As event_comb
        assert(Not f Is Nothing)
        Return f.send(s, strlen(s))
    End Function

    <Extension()> Public Function send(ByVal f As stream_injector,
                                       ByVal s As Stream) As event_comb
        assert(Not f Is Nothing)
        Return f.send(s, If(s Is Nothing, uint32_0, If(s.CanSeek(), s.Length(), max_uint32)))
    End Function
End Module
