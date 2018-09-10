
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event

Partial Public NotInheritable Class flip_events
    Private NotInheritable Class timeout_flip_event
        Inherits flip_event

        Public Sub New(ByVal e As events, ByVal timeout_ms As UInt32)
            MyBase.New(e)
            raise_to_high()
            AddHandler cancelled, AddressOf stopwatch.push(timeout_ms, AddressOf raise_to_low).cancel
        End Sub
    End Class
End Class
