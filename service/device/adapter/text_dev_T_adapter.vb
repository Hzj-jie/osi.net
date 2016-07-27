
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.selector

<type_attribute()>
Public Class text_dev_T_adapter(Of T)
    Inherits T_adapter(Of text)
    Implements dev_T(Of T)

    Private ReadOnly T_string As binder(Of _do_val_ref(Of T, String, Boolean), string_conversion_binder_protector)
    Private ReadOnly string_T As binder(Of _do_val_ref(Of String, T, Boolean), string_conversion_binder_protector)

    Public Sub New(ByVal t As text,
                   Optional ByVal T_string As binder(Of _do_val_ref(Of T, String, Boolean), 
                                                        string_conversion_binder_protector) = Nothing,
                   Optional ByVal string_T As binder(Of _do_val_ref(Of String, T, Boolean), 
                                                        string_conversion_binder_protector) = Nothing)
        MyBase.New(t)
        assert(T_string.has_value())
        assert(string_T.has_value())
        Me.T_string = T_string
        Me.string_T = string_T
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return underlying_device.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim s As String = Nothing
                                  If (+T_string)(i, s) Then
                                      ec = underlying_device.send(s)
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
        Return underlying_device.receive(o, string_T)
    End Function
End Class
