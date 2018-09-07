
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event
Imports osi.root.lock

Partial Public NotInheritable Class flip_events
    Public Shared Function manual() As flip_event.[New]
        Return Function(ByVal to_high As Action, ByVal to_low As Action) As flip_event
                   Return New manual_impl(to_high, to_low)
               End Function
    End Function

    Public Shared Function ref_counted() As flip_event.[New]
        Return Function(ByVal to_high As Action, ByVal to_low As Action) As flip_event
                   Return New ref_counted_impl(to_high, to_low)
               End Function
    End Function

    Public Shared Function ref_counted(ByVal init_value As UInt32) As flip_event.[New]
        Return Function(ByVal to_high As Action, ByVal to_low As Action) As flip_event
                   Return New ref_counted_impl(to_high, to_low, init_value)
               End Function
    End Function

    Public Shared Function ref_counted(ByVal r As atomic_int) As flip_event.[New]
        Return Function(ByVal to_high As Action, ByVal to_low As Action) As flip_event
                   Return New ref_counted_impl(to_high, to_low, r)
               End Function
    End Function

    Public Shared Function timeout(ByVal timeout_ms As UInt32) As flip_event.[New]
        Return Function(ByVal to_high As Action, ByVal to_low As Action) As flip_event
                   Return New timeout_impl(to_high, to_low, timeout_ms)
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
