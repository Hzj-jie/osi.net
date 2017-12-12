
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
        low_res_tick_clock.instance.milliseconds()
        low_res_tick_clock.instance.ticks()
        low_res_tick_clock.instance.seconds()
        low_res_tick_clock.instance.ToString()

        high_res_tick_clock.instance.milliseconds()
        high_res_tick_clock.instance.ticks()
        high_res_tick_clock.instance.seconds()
        high_res_tick_clock.instance.ToString()

        normal_res_tick_clock.instance.milliseconds()
        normal_res_tick_clock.instance.ticks()
        normal_res_tick_clock.instance.seconds()
        normal_res_tick_clock.instance.ToString()

        default_res_tick_clock.instance.milliseconds()
        default_res_tick_clock.instance.ticks()
        default_res_tick_clock.instance.seconds()
        default_res_tick_clock.instance.ToString()
    End Sub

    Private Sub New()
    End Sub
End Class
