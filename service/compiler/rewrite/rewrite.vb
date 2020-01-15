
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.envs

Partial Public NotInheritable Class rewrite
    Public Shared ReadOnly debug_dump As Boolean

    Shared Sub New()
        debug_dump = env_bool(env_keys("rewrite", "debug", "dump"))
    End Sub
End Class
