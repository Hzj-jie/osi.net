
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

<type_attribute()>
Public Class block_dev_T_adapter(Of T)
    Inherits T_adapter(Of block)
    Implements dev_T(Of T)

    Public Sub New(ByVal b As block)
        MyBase.New(b)
    End Sub

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim b() As Byte = Nothing
                                  b = bytes_serializer.to_bytes(i)
                                  ec = underlying_device.send(b)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal o As ref(Of T)) As event_comb Implements dev_T(Of T).receive
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of Byte())()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As T = Nothing
                                  Return bytes_serializer.from_bytes(+p, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
