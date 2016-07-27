
Imports osi.root.template
Imports osi.service.device

Public MustInherit Class iexecutable_questioner(Of _ENABLE_AUTO_PING As _boolean)
    Inherits iquestioner(Of _ENABLE_AUTO_PING)

    Protected ReadOnly timeout_ms As Int64

    Protected Sub New(ByVal timeout_ms As Int64)
        MyBase.New()
        Me.timeout_ms = timeout_ms
    End Sub

    Protected Function create(ByVal d As herald) As herald_questioner(Of _false)
        Return New herald_questioner(Of _false)(d, timeout_ms)
    End Function

    Protected Function create(ByVal d As idevice(Of herald)) As herald_questioner(Of _false)
        Return New herald_questioner(Of _false)(d, timeout_ms)
    End Function

    Protected Function create(ByVal d As idevice_pool(Of herald)) As pool_questioner(Of _false)
        Return New pool_questioner(Of _false)(d, timeout_ms)
    End Function
End Class
