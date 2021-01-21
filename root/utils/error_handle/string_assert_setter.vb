
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

<global_init(global_init_level.log_and_counter_services)>
Friend NotInheritable Class strong_assert_setter
    Private Shared Sub init()
        If assert_trace Then
            setstrongassert()
        End If
    End Sub
End Class
