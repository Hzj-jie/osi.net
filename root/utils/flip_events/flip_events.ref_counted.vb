
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.lock

Partial Public NotInheritable Class flip_events
    Public NotInheritable Class ref_counted_flip_event
        Inherits flip_event

        Private ReadOnly r As atomic_int

        Public Sub New(ByVal e As events, ByVal r As atomic_int)
            MyBase.New(e)
            assert(Not r Is Nothing)
            Me.r = r
            If (+r) > 0 Then
                raise_to_high()
            End If
        End Sub

        Public Sub New(ByVal e As events, ByVal init_value As UInt32)
            Me.New(e, New atomic_int(CInt(assert_return(init_value <= max_int32, init_value))))
        End Sub

        Public Sub New(ByVal e As events)
            Me.New(e, New atomic_int())
        End Sub

        Public Sub ref()
            Dim v As Int32 = 0
            v = r.increment()
            assert(v > 0)
            If v = 1 Then
                raise_to_high()
            End If
        End Sub

        Public Sub unref()
            Dim v As Int32 = 0
            v = r.decrement()
            assert(v >= 0)
            If v = 0 Then
                raise_to_low()
            End If
        End Sub
    End Class
End Class
