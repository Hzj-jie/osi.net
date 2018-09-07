
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event

Partial Public NotInheritable Class flip_events
    Private NotInheritable Class manual_impl
        Inherits flip_event

        Public Sub New(ByVal to_high As Action, ByVal to_low As Action)
            MyBase.New(to_high, to_low)
        End Sub

        Public Shadows Sub raise_to_high()
            MyBase.raise_to_high()
        End Sub

        Public Shadows Sub raise_to_low()
            MyBase.raise_to_low()
        End Sub
    End Class
End Class
