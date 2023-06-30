
Imports osi.root.constants

Namespace constants
    Namespace trigger_datawatcher
        Public Module _trigger_datawatcher
            Public Const default_interval_ms As Int64 = second_milli
        End Module
    End Namespace

    Namespace compare_datawatcher
        Public Module _compare_datawatcher
            Public Const default_field_count As Int32 = 8
            Public Const max_field_count As Int32 = 128
        End Module
    End Namespace

    Namespace size_time_datawatcher
        Public Module _size_time_datawatcher
            Public Const size_field As Int32 = 0
            Public Const time_field As Int32 = size_field + 1
            Public Const field_count As Int32 = time_field + 1
        End Module
    End Namespace

    Namespace stream_dataloader
        Public Module _stream_dataloader
            Public Const create_retry As Int32 = 8
            Public Const retry_interval_ms As Int32 = 100
        End Module
    End Namespace

    Namespace filestream_dataloader
        Public Module _filestream_dataloader
            Public Const buff_size As Int32 = 16 * 1024
        End Module
    End Namespace
End Namespace