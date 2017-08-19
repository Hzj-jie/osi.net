
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation

Public Class auto_updating_resolver_test
    Inherits [case]

    Private Class test_class
    End Class

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        Using New assert_equal(Of Int32)(Function() resolver.registered_event_count())
            Using New assert_equal(Of Int32)(Function() resolver.erased_event_count())
                Dim wef As disposer(Of weak_pointer(Of test_class)) = Nothing
                Using New assert_less(Of Int32)(Function() resolver.registered_event_count())
                    Using New assert_less(Of Int32)(Function() resolver.erased_event_count())
                        wef = resolver.auto_updating_resolve(Of test_class)()
                    End Using
                End Using
                assert_not_nothing(wef)
                assert_not_nothing(+wef)
                assert_nothing(++wef)
                'register before auto_updating_resolver.ctor
                For i As Int32 = 0 To 1024 - 1
                    wef = Nothing
                    repeat_gc_collect()
                    Dim instance As test_class = Nothing
                    instance = New test_class()
                    resolver.register(Of test_class)(instance)
                    wef = resolver.auto_updating_resolve(Of test_class)()
                    assert_not_nothing(++wef)
                    assert_not_nothing(+wef)
                    assert_equal(object_compare(++wef, instance), 0)
                Next

                'register after auto_updating_resolver.ctor
                For i As Int32 = 0 To 1024 - 1
                    Dim instance As test_class = Nothing
                    instance = New test_class()
                    resolver.register(Of test_class)(instance)
                    assert_not_nothing(++wef)
                    assert_not_nothing(+wef)
                    assert_equal(object_compare(++wef, instance), 0)
                Next
                Using New assert_more(Of Int32)(Function() resolver.registered_event_count())
                    Using New assert_more(Of Int32)(Function() resolver.erased_event_count())
                        wef = Nothing
                        repeat_gc_collect()
                    End Using
                End Using
            End Using
        End Using
        Return True
    End Function
End Class
