
Imports osi.root.constants
Imports osi.service.convertor

Namespace constants
    Namespace remote
        <global_init(global_init_level.server_services)>
        Friend Module binder_register
            Sub New()
                bytes_sbyte_convertor_register(Of action).assert_bind()
                bytes_sbyte_convertor_register(Of parameter).assert_bind()
            End Sub

            Private Sub init()
            End Sub
        End Module

        Public Enum action As SByte
            istrkeyvt_read = default_sbyte + 1
            istrkeyvt_append
            istrkeyvt_delete
            istrkeyvt_seek
            istrkeyvt_list
            istrkeyvt_modify
            istrkeyvt_sizeof
            istrkeyvt_full
            istrkeyvt_empty
            istrkeyvt_retire
            istrkeyvt_capacity
            istrkeyvt_valuesize
            istrkeyvt_keycount
            istrkeyvt_heartbeat
            istrkeyvt_stop
            istrkeyvt_unique_write

            ifs_create
            ifs_open
            ifs_exist
            ifs_list

            inode_path
            inode_properties
            inode_subnodes
            inode_create
            inode_open

            iproperty_path
            iproperty_name
            iproperty_get
            iproperty_set
            iproperty_append
            iproperty_lock
            iproperty_release
        End Enum

        Public Enum parameter As SByte
            key = default_sbyte + 1
            buff
            timestamp
            result
            keys
            size

            path
            name
            wait_ms
        End Enum
    End Namespace
End Namespace