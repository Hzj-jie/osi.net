
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Namespace constants
    Namespace remote
        <global_init(global_init_level.server_services)>
        Friend Module binder_register
            Sub New()
                bytes_serializer(Of action).forward_registration.from(Of SByte)()
                bytes_serializer(Of parameters).forward_registration.from(Of SByte)()
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