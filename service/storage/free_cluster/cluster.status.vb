
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class cluster
    Public Function id() As Int64
        Return _id
    End Function

    Public Function disk_offset() As UInt64
        Return _offset
    End Function

    Public Function data_length() As UInt64
        Return _length
    End Function

    Public Function virtdisk() As virtdisk
        Return _vd
    End Function

    Private Shared Function total_length(ByVal length As UInt64) As UInt64
        Return length + STRUCTURE_SIZE
    End Function

    Public Function total_length() As UInt64
        Return total_length(data_length())
    End Function

    ' A free cluster returns -1 for this function.
    Public Function used() As Int64
        assert_valid_status()
        Return _used
    End Function

    'this cluster is not in use
    Public Function free() As Boolean
        Return used() = FREE_USED
    End Function

    Public Function empty() As Boolean
        Return used() = 0
    End Function

    Public Function used_bytes() As UInt64
        Return If(free(), uint64_0, CULng(used()))
    End Function

    Public Function remain_bytes() As UInt64
        Return data_length() - used_bytes()
    End Function

    Public Function full() As Boolean
        Return remain_bytes() = 0
    End Function

    Public Function data_disk_offset() As UInt64
        Return disk_offset() + DATA_OFFSET
    End Function

    Public Function remain_data_disk_offset() As UInt64
        Return data_disk_offset() + used_bytes()
    End Function

    Private Shared Function end_disk_offset(ByVal offset As UInt64, ByVal length As UInt64) As UInt64
        Return offset + total_length(length)
    End Function

    Public Function end_disk_offset() As UInt64
        Return end_disk_offset(disk_offset(), data_length())
    End Function

    Public Function prev_id() As Int64
        assert_valid_status()
        Return _prev_id
    End Function

    Public Function has_prev_id() As Boolean
        Return prev_id() <> INVALID_ID
    End Function

    Public Function next_id() As Int64
        assert_valid_status()
        Return _next_id
    End Function

    Public Function has_next_id() As Boolean
        Return next_id() <> INVALID_ID
    End Function
End Class
