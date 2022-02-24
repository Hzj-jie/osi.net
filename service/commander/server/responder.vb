
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.selector
Imports constructor = osi.service.device.constructor

<global_init(global_init_level.server_services)>
Public Class responder
    Inherits responder(Of _true)

    Public Sub New(ByVal instance As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(instance, pending_request_timeout_ms, e, stopping)
    End Sub

    Public Sub New(ByVal instance As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        MyBase.New(instance, pending_request_timeout_ms, e)
    End Sub

    Public Sub New(ByVal instance As herald, ByVal e As executor)
        Me.New(instance, npos, e)
    End Sub

    Public Sub New(ByVal instance As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(instance, pending_request_timeout_ms, e, stopping)
    End Sub

    Public Sub New(ByVal instance As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        MyBase.New(instance, pending_request_timeout_ms, e)
    End Sub

    Public Sub New(ByVal instance As idevice(Of herald), ByVal e As executor)
        Me.New(instance, npos, e)
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(p, pending_request_timeout_ms, e, stopping)
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        MyBase.New(p, pending_request_timeout_ms, e)
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald), ByVal e As executor)
        Me.New(p, npos, e)
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        MyBase.New(name, pending_request_timeout_ms, e, stopping)
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        MyBase.New(name, pending_request_timeout_ms, e)
    End Sub

    Public Sub New(ByVal name As String, ByVal e As executor)
        Me.New(name, npos, e)
    End Sub

    Private Shared Function create(ByVal v As var,
                                   ByVal e As executor,
                                   ByVal s As Func(Of Boolean),
                                   ByRef o As responder) As Boolean
        Dim pending_request_timeout_ms As Int64 = 0
        pending_request_timeout_ms =
            v("pending-request-timeout-ms").to(Of Int64)(npos)
        Dim name As String = Nothing
        If v.value("name", name) Then
            Return eva(o,
                       New responder(name, pending_request_timeout_ms, e, s)) AndAlso
                   goto_end()
        Else
            Return secondary_resolve(v,
                                     constants.herald_secondary_type_name,
                                     Function(i As idevice_pool(Of herald),
                                              ByRef r As responder) As Boolean
                                         r = New responder(i,
                                                           pending_request_timeout_ms,
                                                           e,
                                                           s)
                                         Return True
                                     End Function,
                                     o)
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(
                   parameter_allocator(Of responder, var, executor, Func(Of Boolean))(AddressOf create)))
    End Sub
End Class

Public Class responder(Of CONTINUOUS As _boolean)
    Inherits iresponder(Of CONTINUOUS)

    Private ReadOnly r As iresponder(Of CONTINUOUS)

    Private Sub New(ByVal r As iresponder(Of CONTINUOUS))
        assert(Not r Is Nothing)
        Me.r = r
    End Sub

    Public Sub New(ByVal instance As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        Me.New(New herald_responder(Of CONTINUOUS)(instance, pending_request_timeout_ms, e, stopping))
    End Sub

    Public Sub New(ByVal instance As herald,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(New herald_responder(Of CONTINUOUS)(instance, pending_request_timeout_ms, e))
    End Sub

    Public Sub New(ByVal instance As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        Me.New(New herald_responder(Of CONTINUOUS)(instance, pending_request_timeout_ms, e, stopping))
    End Sub

    Public Sub New(ByVal instance As idevice(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(New herald_responder(Of CONTINUOUS)(instance, pending_request_timeout_ms, e))
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        Me.New(New pool_responder(Of CONTINUOUS)(p, pending_request_timeout_ms, e, stopping))
    End Sub

    Public Sub New(ByVal p As idevice_pool(Of herald),
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(New pool_responder(Of CONTINUOUS)(p, pending_request_timeout_ms, e))
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor,
                   ByVal stopping As Func(Of Boolean))
        Me.New(New name_responder(Of CONTINUOUS)(name, pending_request_timeout_ms, e, stopping))
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal pending_request_timeout_ms As Int64,
                   ByVal e As executor)
        Me.New(New name_responder(Of CONTINUOUS)(name, pending_request_timeout_ms, e))
    End Sub

    Public NotOverridable Overrides Function respond() As event_comb
        Return r.respond()
    End Function

    Public NotOverridable Overrides Function respond_all() As Boolean
        Return r.respond_all()
    End Function

    Public NotOverridable Overrides Function respond_one_of() As event_comb
        Return r.respond_one_of()
    End Function
End Class
