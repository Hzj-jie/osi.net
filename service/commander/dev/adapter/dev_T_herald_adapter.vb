
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

<type_attribute()>
Public Class dev_T_herald_adapter(Of T)
    Inherits T_adapter(Of dev_T(Of T))
    Implements herald

    Private ReadOnly convertor As command_T_conversion(Of T)

    Shared Sub New()
        assert(Not GetType(T).is(GetType(command)))
    End Sub

    Public Sub New(ByVal dev_T As dev_T(Of T),
                   Optional ByVal convertor As command_T_conversion(Of T) = Nothing)
        MyBase.New(dev_T)
        Me.convertor = convertor
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements herald.sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As command) As event_comb Implements herald.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim v As T = Nothing
                                  If convertor.command_to_T(i, v) Then
                                      ec = underlying_device.send(v)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal o As pointer(Of command)) As event_comb Implements herald.receive
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of T)()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim v As command = Nothing
                                  Return ec.end_result() AndAlso
                                         convertor.T_to_command(+p, v) AndAlso
                                         eva(o, v) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
