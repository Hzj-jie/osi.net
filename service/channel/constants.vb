
Imports osi.root.constants

Namespace constants
    Namespace remote
        Public Enum action As SByte
            connect = default_sbyte + 1
            close
            send
        End Enum

        Public Enum parameter As SByte
            local_id = default_sbyte + 1
            remote_id
            buff
        End Enum
    End Namespace

    Public Module _constants
        Public Const default_max_channel_count As UInt32 = 65536
    End Module
End Namespace
