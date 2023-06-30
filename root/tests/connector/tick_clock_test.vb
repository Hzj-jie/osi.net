
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt.attributes

' Smoke tests of tick_clock implementations.
<test>
Public NotInheritable Class tick_clock_test
    <test>
    <repeat(1000)>
    <multi_threading(16)>
    Private Shared Sub run()
        tick_clock.low_resolution.milliseconds()
        tick_clock.low_resolution.ticks()
        tick_clock.low_resolution.seconds()
        tick_clock.low_resolution.ToString()

        tick_clock.high_resolution.milliseconds()
        tick_clock.high_resolution.ticks()
        tick_clock.high_resolution.seconds()
        tick_clock.high_resolution.ToString()

        tick_clock.normal_resolution.milliseconds()
        tick_clock.normal_resolution.ticks()
        tick_clock.normal_resolution.seconds()
        tick_clock.normal_resolution.ToString()

        tick_clock.default.milliseconds()
        tick_clock.default.ticks()
        tick_clock.default.seconds()
        tick_clock.default.ToString()
    End Sub

    Private Sub New()
    End Sub
End Class
