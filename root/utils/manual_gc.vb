
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.foundamental)>
Public Module _manual_gc
    Private Sub init()
        If envs.env_bool(envs.env_keys("enable", "manual", "gc")) Then
            assert(stopwatch.repeat(seconds_to_milliseconds(5), AddressOf waitfor_gc_collect))
        End If
    End Sub
End Module
