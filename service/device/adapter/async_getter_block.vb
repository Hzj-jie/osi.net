
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector
Imports osi.service.transmitter

' Consumers should use async_device_creator_device_creator_adatper, instead of using this class directly.
<type_attribute()>
Public Class async_getter_block
    Inherits async_getter_adapter(Of block)
    Implements block

    Private Sub New(ByVal p As pair(Of async_getter(Of block), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function [New](Of T, BT As block) _
                                        (ByVal i As async_getter(Of T),
                                         ByVal c As Func(Of T, BT)) As async_getter_block
        Return New async_getter_block(async_getter_adapter(Of block).convert(i, c))
    End Function

    Public Shared Shadows Function [New](Of BT As block)(ByVal i As async_getter(Of BT)) As async_getter_block
        Return New async_getter_block(async_getter_adapter(Of block).convert(i))
    End Function

    Public Shared Shadows Function [New](ByVal i As async_getter(Of block)) As async_getter_block
        Return New async_getter_block(async_getter_adapter(Of block).convert(i))
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb Implements block_injector.send
        Return _do(Function(x) x.send(buff, offset, count))
    End Function

    Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive
        Return _do(Function(x) x.receive(result))
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return _do(Function(x) x.sense(pending, timeout_ms))
    End Function
End Class
