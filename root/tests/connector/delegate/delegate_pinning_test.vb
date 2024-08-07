
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class delegate_pinning_test
    Inherits [case]

    Private Class test_class
        Private i As Int32

        Public Sub New()
            i = 0
        End Sub

        Public Sub run()
            i += 1
        End Sub

        Public Function count() As Int32
            Return i
        End Function
    End Class

    Public Overrides Function run() As Boolean
        Dim c As test_class = Nothing
        c = New test_class()
        Dim p As weak_ref(Of test_class) = Nothing
        p = weak_ref.of(c)

        Dim d As Action = Nothing
        d = AddressOf c.run

        garbage_collector.repeat_collect()

        assertion.is_true(p.alive())
        d()
        assertion.equal(c.count(), 1)
        GC.KeepAlive(c)
        c = Nothing
        garbage_collector.repeat_collect()

        assertion.is_true(p.alive())
        d()
        Dim c2 As test_class = Nothing
        assertion.is_true(p.get(c2))
        assertion.equal(c2.count(), 2)
        c2 = Nothing
        GC.KeepAlive(d)
        garbage_collector.repeat_collect()

        d = Nothing
        garbage_collector.repeat_collect()

        assertion.is_false(p.alive())
        Return True
    End Function
End Class
