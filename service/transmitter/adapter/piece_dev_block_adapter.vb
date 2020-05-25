
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class piece_dev_block_adapter
    Inherits T_adapter(Of piece_dev)
    Implements block

    Public Sub New(ByVal p As piece_dev)
        MyBase.New(p)
    End Sub

    Public Shared Function [New](ByVal p As piece_dev) As block
        Return New piece_dev_block_adapter(p)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb Implements block_injector.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim p As piece = Nothing
                                  If piece.create(buff, offset, count, p) Then
                                      ec = underlying_device.send(p)
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

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of piece) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of piece)()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return eva(result, ++p) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
