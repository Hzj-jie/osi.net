
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

' Test the behavior of GC, whether adding an event handler to an object pin the object.
' An event handler won't pin the object owns the event.
Public Class event_pin_object_behavior_test
    Inherits [case]

    Private Class c
        Inherits cd_object(Of event_pin_object_behavior_test)
        Public Event e()
    End Class

    Private Shared Sub handler()
    End Sub

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1000
            Dim a As c = Nothing
            a = New c()
            AddHandler a.e, AddressOf handler
            garbage_collector.repeat_collect()
        Next
        assertion.more(c.destructed(), uint32_0)
        For i As Int32 = 0 To 1000
            Dim a As c = Nothing
            a = New c()
            AddHandler a.e, Sub()
                            End Sub
            garbage_collector.repeat_collect()
        Next
        assertion.more(c.destructed(), uint32_0)
        Return True
    End Function
End Class
