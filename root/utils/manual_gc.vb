
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.other)>
Public Module _manual_gc
    Private Sub init()
        If envs.env_bool(envs.env_keys("enable", "manual", "gc")) Then
            assert(stopwatch.repeat(seconds_to_milliseconds(5), AddressOf garbage_collector.waitfor_collect))
        End If
    End Sub
End Module
