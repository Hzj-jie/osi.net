
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class error_context
    Private Shared Function current() As String

    End Function

    <global_init(global_init_level.log_and_counter_services)>
    Private NotInheritable Class init
        Private Shared Sub init()
            connector.error_context.register(AddressOf error_context.current)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
