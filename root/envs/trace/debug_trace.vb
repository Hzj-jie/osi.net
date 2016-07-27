
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.debugging)>
Public Module _debug_trace
    Public ReadOnly debug_trace As Boolean = False

    Sub New()
        debug_trace = env_bool(env_keys("debug", "trace")) OrElse
                      env_bool(env_keys("debug", "mode"))
        If debug_trace Then
            set_debug_mode()
        End If
    End Sub

    Private Sub init()
    End Sub
End Module
