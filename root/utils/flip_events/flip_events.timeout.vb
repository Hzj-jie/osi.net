
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event

Partial Public NotInheritable Class flip_events
    Private NotInheritable Class timeout_impl
        Inherits flip_event

        Public Sub New(ByVal to_high As Action, ByVal to_low As Action, ByVal timeout_ms As UInt32)
            MyBase.New(to_high, to_low)
            raise_to_high()
            stopwatch.push(timeout_ms, AddressOf raise_to_low)
        End Sub
    End Class
End Class
