
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector
Imports osi.service.transmitter

' Consumers should use async_device_creator_device_creator_adatper, instead of using this class directly.
<type_attribute()>
Public Class async_getter_stream_text
    Inherits async_getter_adapter(Of stream_text)
    Implements stream_text

    Private Sub New(ByVal p As pair(Of async_getter(Of stream_text), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function [New](Of T, ST As stream_text) _
                                        (ByVal i As async_getter(Of T),
                                         ByVal c As Func(Of T, ST)) As async_getter_stream_text
        Return New async_getter_stream_text(async_getter_adapter(Of stream_text).convert(i, c))
    End Function

    Public Shared Shadows Function [New](Of T)(ByVal i As async_getter(Of T),
                                               ByVal c As Func(Of T, stream_text)) As async_getter_stream_text
        Return New async_getter_stream_text(async_getter_adapter(Of stream_text).convert(i, c))
    End Function

    Public Shared Shadows Function [New](ByVal i As async_getter(Of stream_text)) As async_getter_stream_text
        Return New async_getter_stream_text(async_getter_adapter(Of stream_text).convert(i))
    End Function

    Public Function send(ByVal s As Stream, ByVal len As UInt32) As event_comb Implements stream_injector.send
        Return _do(Function(x) x.send(s, len))
    End Function

    Public Function receive(ByVal result As pointer(Of String)) As event_comb Implements text_pump.receive
        Return _do(Function(x) x.receive(result))
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return _do(Function(x) x.sense(pending, timeout_ms))
    End Function
End Class
