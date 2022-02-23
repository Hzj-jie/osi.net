
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation

Public Class event_disposer_test
    Inherits [case]

    Private Class test_class
    End Class

    Private Shared Function create(ByVal disposing As atomic_int) As event_disposer(Of cd_object(Of test_class))
        assert(disposing IsNot Nothing)
        Dim p As event_disposer(Of cd_object(Of test_class)) = Nothing
        p = New event_disposer(Of cd_object(Of test_class))(New cd_object(Of test_class)())
        'also assert finalizers are running in a single thread
        AddHandler p.disposing, Sub() disposing.increment()
        Return p
    End Function

    Private Shared Function event_case() As Boolean
        cd_object(Of test_class).reset()
        Const size As UInt32 = 1024 * 128
        Dim disposing As atomic_int = Nothing
        disposing = New atomic_int()
        Dim p As event_disposer(Of cd_object(Of test_class)) = Nothing
        For i As UInt32 = 0 To size - uint32_1
            If p IsNot Nothing Then
                p.Dispose()
            End If
            p = create(disposing)
        Next
        garbage_collector.repeat_collect()
        assertion.equal(cd_object(Of test_class).constructed(), size)
        assertion.equal(cd_object(Of test_class).destructed(), size - uint32_1)
        assertion.equal(CUInt(+disposing), size - uint32_1)
        p.Dispose()
        p = Nothing
        garbage_collector.repeat_collect()
        assertion.equal(cd_object(Of test_class).constructed(), size)
        assertion.equal(cd_object(Of test_class).destructed(), size)
        assertion.equal(CUInt(+disposing), size)
        Return True
    End Function

    Private Shared Function binding_case() As Boolean
        Const size As UInt32 = 1024
        cd_object(Of test_class).reset()
        Dim disposing As atomic_int = Nothing
        disposing = New atomic_int()
        Dim p() As event_disposer(Of cd_object(Of test_class)) = Nothing
        ReDim p(size - uint32_1)
        For i As UInt32 = 0 To size - uint32_1
            p(i) = create(disposing)
        Next
        garbage_collector.repeat_collect()
        assertion.equal(+disposing, 0)
        assertion.equal(cd_object(Of test_class).constructed(), size)
        assertion.equal(cd_object(Of test_class).destructed(), uint32_0)
        For i As UInt32 = 0 To size - uint32_1
            assertion.is_not_null(+p(i))
            p(i).Dispose()
            p(i) = Nothing
        Next
        garbage_collector.repeat_collect()
        assertion.equal(CUInt(+disposing), size)
        assertion.equal(cd_object(Of test_class).constructed(), size)
        assertion.equal(cd_object(Of test_class).destructed(), size)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return binding_case() AndAlso
               event_case()
    End Function
End Class
