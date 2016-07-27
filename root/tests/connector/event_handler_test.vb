
Imports osi.root.connector
Imports osi.root.utt

Public Class event_handler_test
    Inherits [case]

    Private Class with_event
        Public Const event_name As String = "some_event"
        Public Event some_event(ByVal obj As with_event, ByRef c As Int32)

        Public Sub trigger_event(ByRef c As Int32)
            RaiseEvent some_event(Me, c)
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        Const trigger_size As Int32 = 100
        Dim v As with_event = Nothing
        v = New with_event()
        Dim h As with_event.some_eventEventHandler = Nothing
        h = Sub(arg1 As with_event, ByRef arg2 As Int32)
                assert_reference_equal(arg1, v)
                arg2 += 1
            End Sub
        AddHandler v.some_event, h
        assert_equal(attached_event_count(Of with_event)(v, with_event.event_name), 1)
        Dim c As Int32 = 0
        For i As Int32 = 0 To trigger_size - 1
            v.trigger_event(c)
        Next
        assert_equal(c, trigger_size)
        RemoveHandler v.some_event, h
        assert_equal(attached_event_count(Of with_event)(v, with_event.event_name), 0)
        c = 0
        For i As Int32 = 0 To trigger_size - 1
            v.trigger_event(c)
        Next
        assert_equal(c, 0)
        Return True
    End Function
End Class
