
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.selector
Imports Encoding = System.Text.Encoding

<type_attribute()>
Public Class stream_text_dev_T_adapter(Of T)
    Inherits T_adapter(Of stream_text)
    Implements dev_T(Of T)

    Private ReadOnly enc As Encoding
    Private ReadOnly T_string As binder(Of _do_val_ref(Of T, String, Boolean), string_conversion_binder_protector)
    Private ReadOnly string_T As binder(Of _do_val_ref(Of String, T, Boolean), string_conversion_binder_protector)

    Public Sub New(ByVal st As stream_text,
                   Optional ByVal enc As Encoding = Nothing,
                   Optional ByVal T_string As binder(Of _do_val_ref(Of T, String, Boolean), 
                                                        string_conversion_binder_protector) = Nothing,
                   Optional ByVal string_T As binder(Of _do_val_ref(Of String, T, Boolean), 
                                                        string_conversion_binder_protector) = Nothing)
        MyBase.New(st)
        assert(T_string.has_value())
        assert(string_T.has_value())
        Me.enc = If(enc Is Nothing, default_encoding, enc)
        Me.T_string = T_string
        Me.string_T = string_T
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
                                  If (+T_string)(i, s) AndAlso
                                     memory_stream.create(s, enc, ms) Then
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
        Return underlying_device.receive(o, string_T)
    End Function
End Class
