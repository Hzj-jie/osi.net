
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event
Imports osi.root.lock

Partial Public NotInheritable Class flip_events
    Public Shared Function manual() As flip_event.New(Of manual_flip_event)
        Return Function(ByVal e As flip_event.events) As manual_flip_event
                   Return New manual_flip_event(e)
               End Function
    End Function

    Public Shared Function ref_counted() As flip_event.New(Of ref_counted_flip_event)
        Return Function(ByVal e As flip_event.events) As ref_counted_flip_event
                   Return New ref_counted_flip_event(e)
               End Function
    End Function

    Public Shared Function ref_counted(ByVal init_value As UInt32) As flip_event.New(Of ref_counted_flip_event)
        Return Function(ByVal e As flip_event.events) As ref_counted_flip_event
                   Return New ref_counted_flip_event(e, init_value)
               End Function
    End Function

    Public Shared Function ref_counted(ByVal r As atomic_int) As flip_event.New(Of ref_counted_flip_event)
        Return Function(ByVal e As flip_event.events) As ref_counted_flip_event
                   Return New ref_counted_flip_event(e, r)
               End Function
    End Function

    Public Shared Function timeout(ByVal timeout_ms As UInt32) As flip_event.New(Of flip_event)
        Return Function(ByVal e As flip_event.events) As flip_event
                   Return New timeout_flip_event(e, timeout_ms)
               End Function
    End Function

    Public Shared Function combiner() As flip_event.[New](Of combiner_flip_event)
        Return Function(ByVal e As flip_event.events) As combiner_flip_event
                   Return New combiner_flip_event(e)
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
