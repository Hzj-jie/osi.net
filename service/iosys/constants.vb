
Imports osi.root.constants
Imports osi.root.envs
Imports osi.service.convertor

Namespace constants
    Namespace remote
        <global_init(global_init_level.server_services)>
        Friend Module binder_register
            Sub New()
                bytes_sbyte_convertor_register(Of action).assert_bind()
                bytes_sbyte_convertor_register(Of parameters).assert_bind()
            End Sub

            Private Sub init()
            End Sub
        End Module

        Public Enum action As SByte
            push = default_sbyte + 1
        End Enum

        Public Enum parameters As SByte
            bytes = default_sbyte + 1
        End Enum
    End Namespace
End Namespace