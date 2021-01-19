
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class flow_piece_dev_adapter
    Inherits T_adapter(Of flow)
    Implements piece_dev

    Private ReadOnly buff_size As UInt32

    Public Sub New(ByVal f As flow, ByVal buff_size As UInt32)
        MyBase.New(f)
        If buff_size = 0 Then
            Me.buff_size = constants.default_io_buff_size
        Else
            Me.buff_size = buff_size
        End If
    End Sub

    Public Sub New(ByVal f As flow)
        Me.New(f, constants.default_io_buff_size)
    End Sub

    Public Shared Function [New](ByVal f As flow, ByVal buff_size As UInt32) As piece_dev
        Return New flow_piece_dev_adapter(f, buff_size)
    End Function

    Public Shared Function [New](ByVal f As flow) As piece_dev
        Return New flow_piece_dev_adapter(f)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements piece_dev.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As piece) As event_comb Implements piece_dev.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  ElseIf i.empty() Then
                                      Return goto_end()
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

    Public Function receive(ByVal o As ref(Of piece)) As event_comb Implements piece_dev.receive
        Dim ec As event_comb = Nothing
        Dim buff() As Byte = Nothing
        Dim result As ref(Of UInt32) = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim buff(CInt(buff_size) - 1)
                                  result = New ref(Of UInt32)()
                                  ec = underlying_device.receive(buff, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim p As piece = Nothing
                                  Return ec.end_result() AndAlso
                                         piece.create(buff, +result, p) AndAlso
                                         eva(o, p) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
