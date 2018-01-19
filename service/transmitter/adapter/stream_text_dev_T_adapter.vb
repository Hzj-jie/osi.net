
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
    Private ReadOnly T_string As string_serializer(Of T)

    Public Sub New(ByVal st As stream_text,
                   Optional ByVal enc As Encoding = Nothing,
                   Optional ByVal T_string As string_serializer(Of T) = Nothing)
        MyBase.New(st)
        Me.enc = If(enc Is Nothing, default_encoding, enc)
        Me.T_string = +T_string
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
                                  s = T_string.to_str(i)
                                  If memory_stream.create(s, enc, ms) Then
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
        Return underlying_device.receive(o, T_string)
    End Function
End Class
