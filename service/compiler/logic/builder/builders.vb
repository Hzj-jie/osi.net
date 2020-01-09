
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.envs

Namespace logic
    Partial Public NotInheritable Class builders
        Public Shared ReadOnly debug_dump As Boolean

        Shared Sub New()
            debug_dump = env_bool(env_keys("compiler", "debug", "dump"))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
