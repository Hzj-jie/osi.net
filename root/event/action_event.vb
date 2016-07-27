
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.template

Public Class action_event(Of _ONCE As _boolean)
    Private ReadOnly q As event_queue(Of parameters, _ONCE)

    Public Sub New()
        q = New event_queue(Of parameters, _ONCE)()
    End Sub

    Public Function attached_count() As UInt32
        Return q.attached_count()
    End Function

    Public Function attached() As Boolean
        Return q.attached()
    End Function

    Public Function attach(ByVal v As iparameter_action(Of parameters)) As Boolean
        Return q.attach(v)
    End Function

    Public Function attach(ByVal v As iaction) As Boolean
        Return q.attach(New action_wrapper(v))
    End Function

    Public Function attach(ByVal v As Action) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Return assert(attach(New action_adapter(v)))
        End If
    End Function

    Public Sub raise()
        q.raise(parameters.null)
    End Sub

    Public Function raise_one() As Boolean
        Return q.raise_one(parameters.null)
    End Function

    Public Sub clear()
        q.clear()
    End Sub
End Class

Public Class action_event(Of _ONCE As _boolean, T)
    Private ReadOnly q As event_queue(Of parameters(Of T), _ONCE)

    Public Sub New()
        q = New event_queue(Of parameters(Of T), _ONCE)()
    End Sub

    Public Function attached_count() As UInt32
        Return q.attached_count()
    End Function

    Public Function attached() As Boolean
        Return q.attached()
    End Function

    Public Function attach(ByVal v As iparameter_action(Of parameters(Of T))) As Boolean
        Return q.attach(v)
    End Function

    Public Function attach(ByVal v As iaction(Of T)) As Boolean
        Return q.attach(New action_wrapper(Of T)(v))
    End Function

    Public Sub raise(ByVal p As T)
        q.raise(New parameters(Of T)(p))
    End Sub

    Public Sub raise(ByVal p As parameters(Of T))
        q.raise(p)
    End Sub

    Public Function raise_one(ByVal p As parameters(Of T)) As Boolean
        Return q.raise_one(p)
    End Function

    Public Sub clear()
        q.clear()
    End Sub
End Class

Public Class action_event(Of _ONCE As _boolean, T1, T2)
    Private ReadOnly q As event_queue(Of parameters(Of T1, T2), _ONCE)

    Public Sub New()
        q = New event_queue(Of parameters(Of T1, T2), _ONCE)()
    End Sub

    Public Function attached_count() As UInt32
        Return q.attached_count()
    End Function

    Public Function attached() As Boolean
        Return q.attached()
    End Function

    Public Function attach(ByVal v As iparameter_action(Of parameters(Of T1, T2))) As Boolean
        Return q.attach(v)
    End Function

    Public Function attach(ByVal v As iaction(Of T1, T2)) As Boolean
        Return q.attach(New action_wrapper(Of T1, T2)(v))
    End Function

    Public Sub raise(ByVal v1 As T1, ByVal v2 As T2)
        q.raise(New parameters(Of T1, T2)(v1, v2))
    End Sub

    Public Sub raise(ByVal v As parameters(Of T1, T2))
        q.raise(v)
    End Sub

    Public Function raise_one(ByVal p As parameters(Of T1, T2)) As Boolean
        Return q.raise_one(p)
    End Function

    Public Sub clear()
        q.clear()
    End Sub
End Class

Public Class action_event(Of _ONCE As _boolean, T1, T2, T3)
    Private ReadOnly q As event_queue(Of parameters(Of T1, T2, T3), _ONCE)

    Public Sub New()
        q = New event_queue(Of parameters(Of T1, T2, T3), _ONCE)()
    End Sub

    Public Function attached_count() As UInt32
        Return q.attached_count()
    End Function

    Public Function attached() As Boolean
        Return q.attached()
    End Function

    Public Function attach(ByVal v As iparameter_action(Of parameters(Of T1, T2, T3))) As Boolean
        Return q.attach(v)
    End Function

    Public Function attach(ByVal v As iaction(Of T1, T2, T3)) As Boolean
        Return q.attach(v)
    End Function

    Public Sub raise(ByVal v1 As T1, ByVal v2 As T2, ByVal v3 As T3)
        q.raise(New parameters(Of T1, T2, T3)(v1, v2, v3))
    End Sub

    Public Sub raise(ByVal v As parameters(Of T1, T2, T3))
        q.raise(v)
    End Sub

    Public Function raise_one(ByVal p As parameters(Of T1, T2, T3)) As Boolean
        Return q.raise_one(p)
    End Function

    Public Sub clear()
        q.clear()
    End Sub
End Class
