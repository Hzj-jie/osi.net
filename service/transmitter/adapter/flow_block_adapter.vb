
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

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

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb _
                           Implements block.receive
        Dim ec As event_comb = Nothing
        Dim exp_len As UInt32 = 0
        Dim r() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  r = chunk.head()
                                  ec = underlying_device.receive(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     chunk.parse_head(r, r) Then
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
                                  Dim p As piece = Nothing
                                  If Not piece.create(buff, offset, count, p) Then
                                      Return False
                                  End If
                                  ec = underlying_device.send(chunk.from_bytes(p))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements block.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
