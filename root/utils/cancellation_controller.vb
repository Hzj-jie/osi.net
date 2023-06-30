
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.lock

' A combination of mutual excluded flip_event implementations.
Public NotInheritable Class cancellation_controller
    Private ReadOnly manual_ref As New atomic_ref(Of flip_events.manual_flip_event)()
    Private ReadOnly ref_counted_ref As New atomic_ref(Of flip_events.ref_counted_flip_event)()
    Private ReadOnly timeout_ref As New atomic_ref(Of flip_event)()

    ' Prefers a weak_ref_delegate to avoid binding object.
    Public Function manual(Of T)(ByVal obj As T, ByVal when_cancel As Action(Of T)) As flip_events.manual_flip_event
        Return manual(weak_ref_delegate.bind(obj, when_cancel))
    End Function

    Public Function manual(ByVal when_cancel As Action) As flip_events.manual_flip_event
        replace(manual_ref, flip_events.manual(), Nothing, when_cancel)
        Return manual()
    End Function

    Public Function manual() As flip_events.manual_flip_event
        Return manual_ref.get()
    End Function

    Public Sub cancel_manual()
        cancel(manual_ref)
    End Sub

    Public Function ref_counted(Of T)(ByVal init_value As UInt32,
                                      ByVal obj As T,
                                      ByVal when_start As Action(Of T),
                                      ByVal when_cancel As Action(Of T)) As flip_events.ref_counted_flip_event
        Return ref_counted(init_value,
                           If(when_start Is Nothing, Nothing, weak_ref_delegate.bind(obj, when_start)),
                           weak_ref_delegate.bind(obj, when_cancel))
    End Function

    Public Function ref_counted(Of T)(ByVal init_value As UInt32,
                                      ByVal obj As T,
                                      ByVal when_cancel As Action(Of T)) As flip_events.ref_counted_flip_event
        Return ref_counted(init_value, obj, Nothing, when_cancel)
    End Function

    Public Function ref_counted(ByVal init_value As UInt32,
                                ByVal when_start As Action,
                                ByVal when_cancel As Action) As flip_events.ref_counted_flip_event
        replace(ref_counted_ref, flip_events.ref_counted(init_value), when_start, when_cancel)
        Return ref_counted()
    End Function

    Public Function ref_counted(ByVal init_value As UInt32,
                                ByVal when_cancel As Action) As flip_events.ref_counted_flip_event
        Return ref_counted(init_value, Nothing, when_cancel)
    End Function

    Public Function ref_counted(Of T)(ByVal obj As T,
                                      ByVal when_start As Action(Of T),
                                      ByVal when_cancel As Action(Of T)) As flip_events.ref_counted_flip_event
        Return ref_counted(uint32_1, obj, when_start, when_cancel)
    End Function

    Public Function ref_counted(Of T)(ByVal obj As T,
                                      ByVal when_cancel As Action(Of T)) As flip_events.ref_counted_flip_event
        Return ref_counted(obj, Nothing, when_cancel)
    End Function

    Public Function ref_counted(ByVal when_start As Action,
                                ByVal when_cancel As Action) As flip_events.ref_counted_flip_event
        Return ref_counted(uint32_1, when_start, when_cancel)
    End Function

    Public Function ref_counted(ByVal when_cancel As Action) As flip_events.ref_counted_flip_event
        Return ref_counted([default](Of Action).null, when_cancel)
    End Function

    Public Function ref_counted() As flip_events.ref_counted_flip_event
        Return ref_counted_ref.get()
    End Function

    Public Sub cancel_ref_counted()
        cancel(ref_counted_ref)
    End Sub

    Public Function timeout(Of T)(ByVal ms As UInt32, ByVal obj As T, ByVal when_cancel As Action(Of T)) As flip_event
        Return timeout(ms, weak_ref_delegate.bind(obj, when_cancel))
    End Function

    Public Function timeout(ByVal ms As UInt32, ByVal when_cancel As Action) As flip_event
        replace(timeout_ref, flip_events.timeout(ms), Nothing, when_cancel)
        Return timeout()
    End Function

    Public Function timeout() As flip_event
        Return timeout_ref.get()
    End Function

    Public Sub cancel_timeout()
        cancel(timeout_ref)
    End Sub

    Private Sub replace(Of T As flip_event)(ByVal ref As atomic_ref(Of T),
                                            ByVal f As flip_event.[New](Of T),
                                            ByVal when_start As Action,
                                            ByVal when_cancel As Action)
        assert(Not ref Is Nothing)
        assert(Not f Is Nothing)
        assert(Not when_cancel Is Nothing)
        Dim old As T = ref.exchange(f(flip_event.events.of(when_start,
                                                           Sub()
                                                               when_cancel()
                                                               cancel_if_not(manual_ref, ref)
                                                               cancel_if_not(ref_counted_ref, ref)
                                                               cancel_if_not(timeout_ref, ref)
                                                           End Sub)))
        If Not old Is Nothing Then
            old.cancel()
        End If
    End Sub

    Public Sub cancel()
        cancel(manual_ref)
        cancel(ref_counted_ref)
        cancel(timeout_ref)
    End Sub

    Private Shared Sub cancel_if_not(Of T As flip_event, T2 As flip_event) _
                                    (ByVal ref As atomic_ref(Of T), ByVal cmp As atomic_ref(Of T2))
        Dim f As flip_event = Nothing
        f = ref.exchange(Nothing)
        If Not f Is Nothing AndAlso object_compare(ref, cmp) <> 0 Then
            f.cancel()
        End If
    End Sub

    Private Shared Sub cancel(Of T As flip_event)(ByVal ref As atomic_ref(Of T))
        cancel_if_not(ref, [default](Of atomic_ref(Of flip_event)).null)
    End Sub
End Class
