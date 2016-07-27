
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

<global_init(globaL_init_level.log_and_counter_services)>
Friend Class strong_assert_setter
    Shared Sub New()
        If assert_trace Then
            setstrongassert()
        End If
    End Sub

    Private Shared Sub init()
    End Sub
End Class
