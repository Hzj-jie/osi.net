
Imports System.Runtime.CompilerServices
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

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
        Return complete_io(buff, offset, count, AddressOf f.receive)
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

    <Extension()> Public Function receive(Of T)( _
                                      ByVal i As text_pump,
                                      ByVal o As pointer(Of T),
                                      ByVal string_T As binder(Of _do_val_ref(Of String, T, Boolean), 
                                                                  string_conversion_binder_protector)) As event_comb
        assert(Not i Is Nothing)
        assert(string_T.has_value())
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
                                  Return (+string_T)(+p, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
