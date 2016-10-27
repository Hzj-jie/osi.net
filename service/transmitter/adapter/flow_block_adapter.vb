
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

<type_attribute()>
Public Class flow_block_adapter
    Inherits T_adapter(Of flow)
    Implements block

    Public Sub New(ByVal f As flow)
        MyBase.New(f)
    End Sub

    Public Shared Function [New](ByVal f As flow) As block
        Return New flow_block_adapter(f)
    End Function

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb _
                           Implements block.receive
        Dim ec As event_comb = Nothing
        Dim exp_len As UInt32 = 0
        Dim r() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim r(preamble_length - 1)
                                  ec = underlying_device.receive(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     parse_as_preamble(r, exp_len) Then
                                      ReDim r(exp_len - 1)
                                      ec = underlying_device.receive(r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb _
                        Implements block.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If offset + count > array_size(buff) Then
                                      Return False
                                  ElseIf count = 0 Then
                                      Return goto_end()
                                  Else
                                      ec = underlying_device.send(to_chunk(buff, offset, count))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements block.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
