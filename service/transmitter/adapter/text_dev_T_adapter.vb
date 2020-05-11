
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class text_dev_T_adapter(Of T)
    Inherits T_adapter(Of text)
    Implements dev_T(Of T)

    Public Sub New(ByVal t As text)
        MyBase.New(t)
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim s As String = Nothing
                                  s = string_serializer.to_str(i)
                                  ec = underlying_device.send(s)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements dev_T(Of T).receive
        Return underlying_device.receive(o)
    End Function
End Class
