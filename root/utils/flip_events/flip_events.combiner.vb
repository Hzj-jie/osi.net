
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event

Partial Public NotInheritable Class flip_events
    Public NotInheritable Class combiner_flip_event
        Inherits flip_event

        Private ReadOnly f As ref_counted_flip_event

        Public Sub New(ByVal e As events)
            MyBase.New(e)
            Me.f = New ref_counted_flip_event(events.of(AddressOf raise_to_high, AddressOf raise_to_low))
        End Sub

        Public Function attach(Of T As flip_event)(ByVal f As flip_event.[New](Of T)) As T
            Return f(events.of(AddressOf Me.f.ref, AddressOf Me.f.unref))
        End Function
    End Class
End Class
