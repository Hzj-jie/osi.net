
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports Encoding = System.Text.Encoding

<type_attribute()>
Public Class stream_text_dev_T_adapter(Of T)
    Inherits T_adapter(Of stream_text)
    Implements dev_T(Of T)

    Private ReadOnly enc As Encoding

    Public Sub New(ByVal st As stream_text, Optional ByVal enc As Encoding = Nothing)
        MyBase.New(st)
        Me.enc = If(enc Is Nothing, default_encoding, enc)
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Dim ec As event_comb = Nothing
        Dim ms As MemoryStream = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim s As String = Nothing
                                  s = string_serializer.to_str(i)
                                  If memory_stream.[New](s, enc, ms) Then
                                      assert(Not ms Is Nothing)
                                      ec = underlying_device.send(ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not ms Is Nothing)
                                  ms.Close()
                                  ms.Dispose()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements dev_T(Of T).receive
        Return underlying_device.receive(o)
    End Function
End Class
