
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.foundamental)>
Public NotInheritable Class disable_windows_error_popup
    Shared Sub New()
        windows_error_mode.[set](windows_error_mode.SEM_NOALIGNMENTFAULTEXCEPT Or
                                 windows_error_mode.SEM_NOGPFAULTERRORBOX Or
                                 windows_error_mode.SEM_NOOPENFILEERRORBOX)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
