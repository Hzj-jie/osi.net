
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class block_piece_dev_adapter
    Inherits T_adapter(Of block)
    Implements piece_dev

    Public Sub New(ByVal b As block)
        MyBase.New(b)
    End Sub

    Public Shared Function [New](ByVal b As block) As piece_dev
        Return New block_piece_dev_adapter(b)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements piece_dev.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As piece) As event_comb Implements piece_dev.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      ec = underlying_device.send(i.buff, i.offset, i.count)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal o As pointer(Of piece)) As event_comb Implements piece_dev.receive
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of Byte())()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not (+p) Is Nothing AndAlso
                                         eva(o, New piece(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
