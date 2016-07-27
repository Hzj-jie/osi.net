
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation

Public Class weak_event_disposer_test
    Inherits [case]

    Private Class test_class
    End Class

    Private Shared Function create(ByVal disposing As atomic_int) As weak_event_disposer(Of cd_object(Of test_class))
        assert(Not disposing Is Nothing)
        Dim p As weak_event_disposer(Of cd_object(Of test_class)) = Nothing
        p = New weak_event_disposer(Of cd_object(Of test_class))(New cd_object(Of test_class)())
        'also assert finalizers are running in a single thread
        AddHandler p.disposing, Sub() disposing.increment()
        Return p
    End Function

    Private Shared Function event_case() As Boolean
        cd_object(Of test_class).reset()
        Const size As UInt32 = 1024 * 128
        Dim disposing As atomic_int = Nothing
        disposing = New atomic_int()
        Dim p As weak_event_disposer(Of cd_object(Of test_class)) = Nothing
        For i As UInt32 = 0 To size - uint32_1
            If Not p Is Nothing Then
                p.Dispose()
            End If
            p = create(disposing)
        Next
        waitfor_gc_collect()
        assert_equal(cd_object(Of test_class).constructed(), size)
        assert_more_or_equal_and_less_or_equal(cd_object(Of test_class).destructed(), size - uint32_1, size)
        assert_equal(CUInt(+disposing), size - uint32_1)
        GC.KeepAlive(p)
        p.Dispose()
        p = Nothing
        waitfor_gc_collect()
        assert_equal(cd_object(Of test_class).constructed(), size)
        assert_equal(cd_object(Of test_class).destructed(), size)
        assert_equal(CUInt(+disposing), size)
        Return True
    End Function

    Private Shared Function binding_case() As Boolean
        Const size As UInt32 = 1024
        cd_object(Of test_class).reset()
        Dim disposing As atomic_int = Nothing
        disposing = New atomic_int()
        Dim p() As weak_event_disposer(Of cd_object(Of test_class)) = Nothing
        ReDim p(size - uint32_1)
        For i As Int32 = 0 To size - uint32_1
            p(i) = create(disposing)
        Next
        waitfor_gc_collect()
        assert_equal(+disposing, 0)
        assert_equal(cd_object(Of test_class).constructed(), size)
        assert_equal(cd_object(Of test_class).destructed(), size)
        For i As Int32 = 0 To size - uint32_1
            assert_nothing(+p(i))
            p(i).Dispose()
            p(i) = Nothing
        Next
        waitfor_gc_collect()
        assert_equal(CUInt(+disposing), size)
        assert_equal(cd_object(Of test_class).constructed(), size)
        assert_equal(cd_object(Of test_class).destructed(), size)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return binding_case() AndAlso
               event_case()
    End Function
End Class
