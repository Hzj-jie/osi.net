
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.event

Public Class weak_event_test
    Inherits [case]

    Private Class event_receiver
        Private Shared ReadOnly r As atomic_int

        Shared Sub New()
            r = New atomic_int()
        End Sub

        Public Shared Function received() As Int32
            Return +r
        End Function

        Public Shared Sub clear()
            r.set(0)
        End Sub

        Public Shared Sub receive(ByVal e As event_receiver)
            assertion.is_not_null(e)
            r.increment()
        End Sub
    End Class

    Private Shared Function attach_one_case() As Boolean
        Const count_per_round As Int32 = 100

        event_receiver.clear()
        Dim e As weak_event = Nothing
        e = New weak_event()
        Dim r As event_receiver = Nothing
        r = New event_receiver()
        e.attach(r, AddressOf event_receiver.receive)
        garbage_collector.repeat_collect()
        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), count_per_round)
        assertion.is_true(e.attached())
        assertion.equal(e.attached_count(), uint32_1)

        event_receiver.clear()
        GC.KeepAlive(r)
        r = Nothing
        garbage_collector.repeat_collect()
        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), 0)
        assertion.is_false(e.attached())
        assertion.equal(e.attached_count(), uint32_0)

        Return True
    End Function

    Private Shared Function attach_several_case(ByVal c As UInt32) As Boolean
        assert(c > 0)
        Const count_per_round As Int32 = 100

        event_receiver.clear()
        Dim e As weak_event = Nothing
        e = New weak_event()
        Dim rs() As event_receiver = Nothing
        ReDim rs(c - 1)
        For i As Int32 = 0 To c - 1
            rs(i) = New event_receiver()
            e.attach(rs(i), AddressOf event_receiver.receive)
        Next

        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), count_per_round * c)
        assertion.is_true(e.attached())
        assertion.equal(e.attached_count(), c)
        rs.gc_keepalive()

        event_receiver.clear()
        Dim detached As Int32 = 0
        For i As Int32 = 0 To c - 1
            If rnd_bool() Then
                rs(i) = Nothing
                detached += 1
            End If
        Next
        garbage_collector.repeat_collect()
        assert(detached <= c)

        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), count_per_round * (c - detached))
        assertion.equal(e.attached(), detached < c)
        assertion.equal(e.attached_count(), c - detached)
        rs.gc_keepalive()

        event_receiver.clear()
        For i As Int32 = 0 To c - 1
            rs(i) = Nothing
        Next
        garbage_collector.repeat_collect()

        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), 0)
        assertion.is_false(e.attached())
        assertion.equal(e.attached_count(), uint32_0)

        Return True
    End Function

    Private Shared Function attach_two_case() As Boolean
        Const count_per_round As Int32 = 100
        Const c As UInt32 = 2

        event_receiver.clear()
        Dim e As weak_event = Nothing
        e = New weak_event()
        Dim r1 As event_receiver = Nothing
        r1 = New event_receiver()
        Dim r2 As event_receiver = Nothing
        r2 = New event_receiver()
        e.attach(r1, AddressOf event_receiver.receive)
        e.attach(r2, AddressOf event_receiver.receive)
        garbage_collector.repeat_collect()
        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), count_per_round * c)
        assertion.is_true(e.attached())
        assertion.equal(e.attached_count(), c)
        GC.KeepAlive(r1)
        GC.KeepAlive(r2)

        event_receiver.clear()
        Dim detached As UInt32 = 0
        If rnd_bool() Then
            r1 = Nothing
            detached += 1
        End If
        If rnd_bool() Then
            r2 = Nothing
            detached += 1
        End If
        garbage_collector.repeat_collect()
        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), (c - detached) * count_per_round)
        assertion.equal(e.attached(), detached < c)
        assertion.equal(e.attached_count(), c - detached)
        GC.KeepAlive(r1)
        GC.KeepAlive(r2)

        event_receiver.clear()
        r1 = Nothing
        r2 = Nothing
        garbage_collector.repeat_collect()
        For i As Int32 = 0 To count_per_round - 1
            e.raise()
        Next
        assertion.equal(event_receiver.received(), 0)
        assertion.is_false(e.attached())
        assertion.equal(e.attached_count(), uint32_0)
        Return True
    End Function

    Private Shared Function attach_several_case() As Boolean
        For i As Int32 = 0 To 1000
            If Not attach_several_case(rnd_int(3, 1000)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return attach_one_case() AndAlso
               attach_two_case() AndAlso
               attach_several_case()
    End Function
End Class
