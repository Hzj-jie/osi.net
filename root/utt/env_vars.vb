
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.envs

Friend NotInheritable Class env_vars
    Public Shared ReadOnly run_flaky_tests As Boolean

    Shared Sub New()
        run_flaky_tests = env_bool(env_keys("run", "flaky", "tests")) OrElse
                          env_bool(env_keys("run", "flaky", "cases"))
    End Sub

    Private Sub New()
    End Sub
End Class
