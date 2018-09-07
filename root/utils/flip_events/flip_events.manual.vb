
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.event

Partial Public NotInheritable Class flip_events
    Public NotInheritable Class manual_flip_event
        Inherits flip_event

        Public Sub New(ByVal e As events)
            MyBase.New(e)
        End Sub

        Public Shadows Sub raise_to_high()
            MyBase.raise_to_high()
        End Sub

        Public Shadows Sub raise_to_low()
            MyBase.raise_to_low()
        End Sub
    End Class
End Class
