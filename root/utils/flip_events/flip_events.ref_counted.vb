
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.lock

Partial Public NotInheritable Class flip_events
    Private NotInheritable Class ref_counted_impl
        Inherits flip_event

        Private ReadOnly r As atomic_int

        Public Sub New(ByVal to_high As Action, ByVal to_low As Action, ByVal r As atomic_int)
            MyBase.New(to_high, to_low)
            assert(Not r Is Nothing)
            Me.r = r
            If (+r) > 0 Then
                raise_to_high()
            End If
        End Sub

        Public Sub New(ByVal to_high As Action, ByVal to_low As Action, ByVal init_value As UInt32)
            Me.New(to_high, to_low, New atomic_int(CInt(assert_return(init_value <= max_int32, init_value))))
        End Sub

        Public Sub New(ByVal to_high As Action, ByVal to_low As Action)
            Me.New(to_high, to_low, New atomic_int())
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
