
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

Public MustInherit Class iexecutable_responder(Of CONTINUOUS As _boolean)
    Inherits iresponder(Of CONTINUOUS)

    Protected ReadOnly pending_request_timeout_ms As Int64
    Protected ReadOnly e As executor
    Private ReadOnly stopping As Func(Of Boolean)

    Protected Sub New(ByVal pending_request_timeout_ms As Int64,
                      ByVal e As executor,
                      ByVal stopping As Func(Of Boolean))
        assert(stopping IsNot Nothing OrElse Not continuous)
        assert(e IsNot Nothing)
        Me.pending_request_timeout_ms = pending_request_timeout_ms
        Me.e = e
        Me.stopping = stopping
    End Sub

    Protected Sub New(ByVal pending_request_timeout_ms As Int64, ByVal e As executor)
        Me.New(pending_request_timeout_ms, e, Nothing)
    End Sub

    Protected Function create(ByVal d As herald) As herald_responder(Of CONTINUOUS)
        Return New herald_responder(Of CONTINUOUS)(d, pending_request_timeout_ms, e, stopping)
    End Function

    Protected Function create(ByVal d As idevice(Of herald)) As herald_responder(Of CONTINUOUS)
        Return New herald_responder(Of CONTINUOUS)(d, pending_request_timeout_ms, e, stopping)
    End Function

    Protected Function create(ByVal p As idevice_pool(Of herald)) As pool_responder(Of CONTINUOUS)
        Return New pool_responder(Of CONTINUOUS)(p, pending_request_timeout_ms, e, stopping)
    End Function

    Protected Function expired() As Boolean
        assert(continuous)
        assert(stopping IsNot Nothing)
        Return stopping()
    End Function
End Class

Public MustInherit Class iresponder(Of _CONTINUOUS As _boolean)
    Protected Shared ReadOnly continuous As Boolean

    Shared Sub New()
        continuous = +(alloc(Of _CONTINUOUS)())
    End Sub

    Public MustOverride Function respond() As event_comb
    Public MustOverride Function respond_one_of() As event_comb
    Public MustOverride Function respond_all() As Boolean

    Public Shared Operator +(ByVal this As iresponder(Of _CONTINUOUS)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.respond()
        End If
    End Operator

    Public Shared Operator -(ByVal this As iresponder(Of _CONTINUOUS)) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.respond_all()
        End If
    End Operator
End Class
