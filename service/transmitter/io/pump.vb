
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Interface sync_T_pump(Of T)
    Function receive(ByRef o As T) As Boolean
    Function pending() As Boolean
End Interface

Public Interface event_sync_T_pump(Of T)
    Inherits sync_T_pump(Of T)
    Event data_arrived()
End Interface

' All the following pumps should block the procedure until data received.
Public Interface block_pump
    Function receive(ByVal result As pointer(Of Byte())) As event_comb
End Interface

Public Interface flow_pump
    Function receive(ByVal buff() As Byte,
                     ByVal offset As UInt32,
                     ByVal count As UInt32,
                     ByVal result As pointer(Of UInt32)) As event_comb
End Interface

Public Interface text_pump
    Function receive(ByVal r As pointer(Of String)) As event_comb
End Interface

Public Interface T_pump(Of T)
    Function receive(ByVal o As pointer(Of T)) As event_comb
End Interface

Public Module _pump
    <Extension()> Public Function receive(ByVal f As flow_pump,
                                          ByVal buff() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32) As event_comb
        assert(Not f Is Nothing)
        Return complete_io_4(buff, offset, count, AddressOf f.receive)
    End Function

    <Extension()> Public Function receive(ByVal f As flow_pump,
                                          ByVal buff() As Byte,
                                          ByVal count As UInt32) As event_comb
        Return receive(f, buff, uint32_0, count)
    End Function

    <Extension()> Public Function receive(ByVal f As flow_pump, ByVal buff() As Byte) As event_comb
        Return receive(f, buff, array_size(buff))
    End Function

    <Extension()> Public Function receive(ByVal f As flow_pump,
                                          ByVal buff() As Byte,
                                          ByVal count As UInt32,
                                          ByVal result As pointer(Of UInt32)) As event_comb
        assert(Not f Is Nothing)
        Return f.receive(buff, uint32_0, count, result)
    End Function

    <Extension()> Public Function receive(ByVal f As flow_pump,
                                          ByVal buff() As Byte,
                                          ByVal result As pointer(Of UInt32)) As event_comb
        assert(Not f Is Nothing)
        Return f.receive(buff, array_size(buff), result)
    End Function

    <Extension()> Public Function receive(Of T)(ByVal i As text_pump, ByVal o As pointer(Of T)) As event_comb
        assert(Not i Is Nothing)
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of String) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of String)()
                                  ec = i.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As T = Nothing
                                  Return string_serializer.from_str(+p, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
