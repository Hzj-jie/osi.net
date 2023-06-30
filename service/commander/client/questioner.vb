
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class questioner
    Inherits questioner(Of _true)

    Public Sub New(ByVal h As herald, Optional ByVal timeout_ms As Int64 = npos)
        MyBase.New(h, timeout_ms)
    End Sub

    Public Sub New(ByVal h As idevice(Of herald), Optional ByVal timeout_ms As Int64 = npos)
        MyBase.New(h, timeout_ms)
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald), Optional ByVal timeout_ms As Int64 = npos)
        MyBase.New(p, timeout_ms)
    End Sub

    Public Sub New(ByVal name As String, Optional ByVal timeout_ms As Int64 = npos)
        MyBase.New(name, timeout_ms)
    End Sub

    Public Shared Function create(ByVal v As var, ByRef o As questioner) As Boolean
        Dim timeout_ms As Int64 = 0
        timeout_ms = v("timeout-ms").to(Of Int64)(npos)
        Dim name As String = Nothing
        If v.value("name", name) Then
            o = New questioner(name, timeout_ms)
            Return True
        Else
            Return secondary_resolve(v,
                                     constants.herald_secondary_type_name,
                                     Function(i As idevice_pool(Of herald), ByRef r As questioner) As Boolean
                                         r = New questioner(i, timeout_ms)
                                         Return True
                                     End Function,
                                     o)
        End If
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As questioner(Of _true)) As Boolean
        Dim x As questioner = Nothing
        If create(v, x) Then
            o = x
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of questioner)(AddressOf create))
        assert(constructor.register(Of questioner(Of _true))(AddressOf create))
    End Sub
End Class

Public Class questioner(Of ENABLE_AUTO_PING As _boolean)
    Inherits iquestioner(Of ENABLE_AUTO_PING)

    Private ReadOnly q As iquestioner(Of _false)

    Private Sub New(ByVal q As iquestioner(Of _false))
        assert(Not q Is Nothing)
        Me.q = q
    End Sub

    Public Sub New(ByVal h As herald, Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New herald_questioner(Of _false)(h, timeout_ms))
    End Sub

    Public Sub New(ByVal h As idevice(Of herald), Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New herald_questioner(Of _false)(h, timeout_ms))
    End Sub

    Public Sub New(ByVal h As Func(Of herald), Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New creator_questioner(Of _false)(h, timeout_ms))
    End Sub

    Public Sub New(ByVal h As Func(Of idevice(Of herald)), Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New creator_questioner(Of _false)(h, timeout_ms))
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald), Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New pool_questioner(Of _false)(p, timeout_ms))
    End Sub

    Public Sub New(ByVal name As String, Optional ByVal timeout_ms As Int64 = npos)
        Me.New(New name_questioner(Of _false)(name, timeout_ms))
    End Sub

    Protected NotOverridable Overrides Function communicate(ByVal request As command,
                                                            ByVal response As ref(Of command)) As event_comb
        Return q(request, response)
    End Function
End Class
