
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

<type_attribute()>
Public Class block_dev_T_adapter(Of T)
    Inherits T_adapter(Of block)
    Implements dev_T(Of T)

    Private ReadOnly T_bytes As binder(Of _do_val_ref(Of T, Byte(), Boolean), bytes_conversion_binder_protector)
    Private ReadOnly bytes_T As binder(Of _do_val_ref(Of Byte(), T, Boolean), bytes_conversion_binder_protector)

    Public Sub New(ByVal b As block,
                   Optional ByVal T_bytes As binder(Of _do_val_ref(Of T, Byte(), Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing,
                   Optional ByVal bytes_T As binder(Of _do_val_ref(Of Byte(), T, Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing)
        MyBase.New(b)
        assert(T_bytes.has_value())
        assert(bytes_T.has_value())
        Me.T_bytes = T_bytes
        Me.bytes_T = bytes_T
    End Sub

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim b() As Byte = Nothing
                                  If (+T_bytes)(i, b) Then
                                      ec = underlying_device.send(b)
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

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements dev_T(Of T).receive
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of Byte())()
                                  ec = underlying_device.receive(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As T = Nothing
                                  Return (+bytes_T)(+p, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function
End Class
