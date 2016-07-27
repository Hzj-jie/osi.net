
Imports osi.root.utt

Public Class addhandler_behavior_test
    Inherits [case]

    Private Class CE
        Public Event an_event()

        Public Sub raise()
            RaiseEvent an_event()
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        Dim ce As CE = Nothing
        ce = New CE()
        Dim x As Int32 = 0
        Dim action As CE.an_eventEventHandler = Sub()
                                                    x += 1
                                                End Sub
        AddHandler ce.an_event, Nothing
        AddHandler ce.an_event, Sub()
                                    x += 1
                                End Sub
        AddHandler ce.an_event, action
        ce.raise()
        assert_equal(x, 2)
        RemoveHandler ce.an_event, Nothing
        ce.raise()
        assert_equal(x, 4)
        RemoveHandler ce.an_event, action
        ce.raise()
        assert_equal(x, 5)
        Return True
    End Function
End Class
