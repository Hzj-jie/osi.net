
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.lock
Imports osi.root.template

Public Class auto_reset_concurrency_event
    Inherits concurrency_event(Of _true)

    Public Sub New(Optional ByVal concurrency As UInt32 = 1)
        MyBase.New(concurrency)
    End Sub
End Class

Public Class manual_reset_concurrency_event
    Inherits concurrency_event(Of _false)

    Public Sub New(Optional ByVal concurrency As UInt32 = 1)
        MyBase.New(concurrency)
    End Sub
End Class

'raise 'concurrency' events at same time.
'do not allow weak_reference_event
Public Class concurrency_event(Of AutoReset As _boolean)
    Implements attachable_event

    Private Shared ReadOnly ar As Boolean = +(alloc(Of AutoReset)())
    Public ReadOnly concurrency As UInt32
    Private ReadOnly e As action_event(Of _true)
    Private ReadOnly i As atomic_int

    Public Sub New(Optional ByVal concurrency As UInt32 = 1)
        assert(concurrency > 0)
        Me.e = New action_event(Of _true)()
        Me.i = New atomic_int()
        Me.concurrency = concurrency
    End Sub

    Private Sub try_raise()
        If i.increment() > concurrency OrElse
           Not e.raise_one() Then
            i.decrement()
        End If
    End Sub

    Private Sub after_run()
        i.decrement()
        try_raise()
    End Sub

    Private Function to_action(ByVal i As iaction) As Action
        Return Sub()
                   assert(Not i Is Nothing)
                   If i.valid() Then
                       i.run()
                   End If
                   If ar Then
                       after_run()
                   End If
               End Sub
    End Function

    Public Sub release()
        assert(Not ar)
        after_run()
    End Sub

    Public Function attached() As Boolean
        Return e.attached()
    End Function

    Public Function attach_count() As UInt32
        Return e.attached_count()
    End Function

    Public Function attach(ByVal v As iaction) As Boolean Implements attachable_event.attach
        If v Is Nothing OrElse Not v.valid() Then
            Return False
        Else
            assert(e.attach(to_action(v)))
            try_raise()
            Return True
        End If
    End Function

    Public Function marked() As Boolean Implements attachable_event.marked
        ' Each attach call may impact the result. It's not safe to return true for marked.
        Return False
    End Function
End Class
