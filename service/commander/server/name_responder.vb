
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.service.device

Public Class name_responder(Of CONTINUOUS As _boolean)
    Inherits iexecutable_responder(Of CONTINUOUS)

    Private ReadOnly device_change_check_interval_ms As Int64
    Private ReadOnly n As String

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean),
                   Optional ByVal device_change_check_interval_ms As Int64 = constants.device_change_check_interval_ms)
        MyBase.New(pending_request_timeout_ms, e, stopping)
        assert(Not String.IsNullOrEmpty(name))
        Me.n = name
        If device_change_check_interval_ms < 0 Then
            Me.device_change_check_interval_ms = constants.device_change_check_interval_ms
        Else
            Me.device_change_check_interval_ms = device_change_check_interval_ms
        End If
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   Optional ByVal device_change_check_interval_ms As Int64 = constants.device_change_check_interval_ms)
        Me.New(name, pending_request_timeout_ms, e, Nothing, device_change_check_interval_ms)
    End Sub

    Public NotOverridable Overrides Function respond() As event_comb
        assert(False)
        Return Nothing
    End Function

    Public Overrides Function respond_all() As Boolean
        Dim ec As event_comb = Nothing
        Dim p As idevice_pool(Of herald) = Nothing
        begin_lifetime_event_comb(expiration_controller.from_func_bool(AddressOf expired),
                                  Function() As Boolean
                                      Dim t As idevice_pool(Of herald) = Nothing
                                      If device_pool_manager.get(n, t) AndAlso
                                         object_compare(t, p) <> 0 Then
                                          p = t
                                          Return create(p).respond_all()
                                      Else
                                          Return waitfor(device_change_check_interval_ms)
                                      End If
                                  End Function)
        Return True
    End Function

    Public Overrides Function respond_one_of() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim p As idevice_pool(Of herald) = Nothing
                                  If device_pool_manager.get(n, p) Then
                                      ec = create(p).respond_one_of()
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
End Class
