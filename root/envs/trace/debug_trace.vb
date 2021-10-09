
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.debugging)>
Public Module _debug_trace
    Public ReadOnly debug_trace As Boolean =
        env_bool(env_keys("debug", "trace")) OrElse
        env_bool(env_keys("debug", "mode"))

    Private Sub init()
        If debug_trace Then
            set_debug_mode()
        End If
    End Sub
End Module
