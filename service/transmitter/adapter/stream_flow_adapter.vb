
Imports System.IO
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.constants

Public Module _stream_flow_adapter
    <Extension()> Public Function copy_to(ByVal this As Stream,
                                          ByVal that As Stream,
                                          ByVal count As UInt64,
                                          Optional ByVal buff_size As UInt32 = uint32_0,
                                          Optional ByVal receive_rate_sec As UInt32 = uint32_0,
                                          Optional ByVal send_rate_sec As UInt32 = uint32_0,
                                          Optional ByVal close_input_stream As Boolean = True,
                                          Optional ByVal close_output_stream As Boolean = False) As event_comb
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Dim i As stream_flow_adapter = Nothing
        Dim o As stream_flow_adapter = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  i = New stream_flow_adapter(this,
                                                              send_rate_sec,
                                                              receive_rate_sec,
                                                              close_input_stream)
                                  o = New stream_flow_adapter(that,
                                                              send_rate_sec,
                                                              receive_rate_sec,
                                                              close_output_stream)
                                  ec = i.copy_to(o, count, buff_size)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If close_input_stream Then
                                      i.Dispose()
                                  End If
                                  If close_output_stream Then
                                      o.Dispose()
                                  End If
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function copy_to(ByVal this As Stream,
                                          ByVal that As Stream,
                                          Optional ByVal buff_size As UInt32 = 0,
                                          Optional ByVal receive_rate_sec As UInt32 = 0,
                                          Optional ByVal send_rate_sec As UInt32 = 0,
                                          Optional ByVal close_input_stream As Boolean = True,
                                          Optional ByVal close_output_stream As Boolean = False,
                                          Optional ByVal result As ref(Of UInt64) = Nothing) As event_comb
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Dim i As stream_flow_adapter = Nothing
        Dim o As stream_flow_adapter = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  i = New stream_flow_adapter(this,
                                                              send_rate_sec,
                                                              receive_rate_sec,
                                                              close_input_stream)
                                  o = New stream_flow_adapter(that,
                                                              send_rate_sec,
                                                              receive_rate_sec,
                                                              close_output_stream)
                                  ec = i.copy_to(o, buff_size, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If close_input_stream Then
                                      i.Dispose()
                                  End If
                                  If close_output_stream Then
                                      o.Dispose()
                                  End If
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function send(ByVal this As Stream,
                                       ByVal buff() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32,
                                       Optional ByVal rate_sec As UInt32 = 0,
                                       Optional ByVal close_stream As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Dim o As stream_flow_adapter = Nothing
        Return New event_comb(Function() As Boolean
                                  o = New stream_flow_adapter(this,
                                                              rate_sec,
                                                              rate_sec,
                                                              close_stream)
                                  ec = o.send(buff, offset, count)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  o.Dispose()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function receive(ByVal this As Stream,
                                          ByVal buff() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32,
                                          Optional ByVal rate_sec As UInt32 = 0,
                                          Optional ByVal close_stream As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Dim o As stream_flow_adapter = Nothing
        Return New event_comb(Function() As Boolean
                                  o = New stream_flow_adapter(this,
                                                              rate_sec,
                                                              rate_sec,
                                                              close_stream)
                                  ec = o.receive(buff, offset, count)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  o.Dispose()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function receive(ByVal this As Stream,
                                          ByVal buff() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32,
                                          ByVal result As ref(Of UInt32),
                                          Optional ByVal rate_sec As UInt32 = 0,
                                          Optional ByVal close_stream As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Dim o As stream_flow_adapter = Nothing
        Return New event_comb(Function() As Boolean
                                  o = New stream_flow_adapter(this,
                                                              rate_sec,
                                                              rate_sec,
                                                              close_stream)
                                  ec = o.receive(buff, offset, count, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  o.Dispose()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module

<type_attribute()>
Public Class stream_flow_adapter
    Implements flow, IDisposable

    Shared Sub New()
        type_attribute.of(Of stream_flow_adapter)().set(trait.[New]().
            with_transmit_mode(trait.mode_t.duplex))
    End Sub

    'usually should not be called, upstream device should have its own indicator / sensor implementations
    Public Class stream_indicator
        Implements sync_indicator

        Private ReadOnly s As Stream
        Private ReadOnly ns As NetworkStream

        Public Sub New(ByVal s As Stream)
            Me.s = s
            ' Do not care whether s can be casted to NetworkStream, but only whether s IS NetworkStream.
            ' So using cast is pretty wrong, it may create a new object.
            Me.ns = TryCast(s, NetworkStream)
        End Sub

        Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
            If s Is Nothing Then
                Return False
            ElseIf ns Is Nothing Then
                Return eva(pending, Not s.CanSeek() OrElse s.Length() > s.Position())
            Else
                Dim t As ternary = Nothing
                t = ns.data_available()
                Return Not t.unknown_() AndAlso
                       eva(pending, t.true_())
            End If
        End Function
    End Class

    Public Event dispose_exception(ByVal ex As Exception)
    Private ReadOnly s As disposer(Of Stream)
    Private ReadOnly sensor As sensor
    Private ReadOnly send_rate_sec As UInt32
    Private ReadOnly receive_rate_sec As UInt32

    Public Sub New(ByVal s As Stream,
                   Optional ByVal send_rate_sec As UInt32 = 0,
                   Optional ByVal receive_rate_sec As UInt32 = 0,
                   Optional ByVal close_stream_when_finish As Boolean = True)
        Me.s = New disposer(Of Stream)(s,
                                       disposer:=If(close_stream_when_finish,
                                                    Sub(x As Stream)
                                                        If Not x Is Nothing Then
                                                            x.Flush()
                                                            x.Close()
                                                            x.Dispose()
                                                        End If
                                                    End Sub,
                                                    Sub(x As Stream)
                                                    End Sub))
        Me.sensor = New indicator_sensor_adapter(New stream_indicator(s))
        Me.send_rate_sec = send_rate_sec
        Me.receive_rate_sec = receive_rate_sec
        AddHandler Me.s.dispose_exception,
                   Sub(ex As Exception)
                       RaiseEvent dispose_exception(ex)
                   End Sub
    End Sub

    Public Function stream() As Stream
        Return (+s)
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         ByVal sent As ref(Of UInt32)) As event_comb Implements flow.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = stream().send(buff, offset, count, send_rate_sec, False, True)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(sent, count) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As ref(Of UInt32)) As event_comb Implements flow.receive
        Return stream().receive(buff, offset, count, result, receive_rate_sec, False, True)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements flow.sense
        'usually this function should not be called
        Return sensor.sense(pending, timeout_ms)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        s.dispose()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
