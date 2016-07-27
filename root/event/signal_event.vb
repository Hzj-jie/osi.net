
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.template

Public Class signal_event
    Inherits signal_event(Of action_event(Of _true))

    Public Sub New(ByVal init_value As Boolean)
        MyBase.New(init_value)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

Public Class weak_signal_event
    Inherits signal_event(Of weak_reference_event(Of _true))

    Public Sub New(ByVal init_value As Boolean)
        MyBase.New(init_value)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

Public Class signal_event(Of EVENT_TYPE As {action_event(Of _true), New})
    Private ReadOnly c As count_event(Of EVENT_TYPE)

    Public Sub New(ByVal init_value As Boolean)
        c = New count_event(Of EVENT_TYPE)(If(init_value, uint32_0, uint32_1))
        assert(c.marked() = init_value)
    End Sub

    Public Sub New()
        Me.New(False)
    End Sub

    Public Function attach(ByVal v As iaction) As Boolean
        Return c.attach(v)
    End Function

    Public Function attach(ByVal v As Action) As Boolean
        Return c.attach(v)
    End Function

    Public Function marked() As Boolean
        Return c.marked()
    End Function

    Public Function attached() As Boolean
        Return c.attached()
    End Function

    Public Function attached_count() As Int32
        Return c.attached_count()
    End Function

    Public Sub mark()
        c.try_decrement()
    End Sub

    Public Sub clear()
        c.clear()
    End Sub

    Public Sub unmark()
        If c.increment() > uint32_1 Then
            c.try_decrement()
        End If
    End Sub

    Public Function wait(Optional ByVal ms As Int64 = npos) As Boolean
        Return c.wait(ms)
    End Function
End Class
