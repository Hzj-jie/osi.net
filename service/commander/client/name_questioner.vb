
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.connector
Imports osi.service.device

Public NotInheritable Class name_questioner
    Inherits name_questioner(Of _true)

    Public Sub New(ByVal name As String, ByVal timeout_ms As Int64)
        MyBase.New(name, timeout_ms)
    End Sub
End Class

Public Class name_questioner(Of ENABLE_AUTO_PING As _boolean)
    Inherits iexecutable_questioner(Of ENABLE_AUTO_PING)

    Private ReadOnly n As String

    Public Sub New(ByVal name As String, ByVal timeout_ms As Int64)
        MyBase.New(timeout_ms)
        assert(Not name.null_or_empty())
        Me.n = name
    End Sub

    Protected NotOverridable Overrides Function communicate(ByVal request As command,
                                                            ByVal response As ref(Of command)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As idevice_pool(Of herald) = Nothing
        Return New event_comb(Function() As Boolean
                                  If device_pool_manager.get(n, p) Then
                                      ec = create(p)(request, response)
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
