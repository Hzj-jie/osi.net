
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class cluster
    Private Shared Function valid_id(ByVal id As Int64) As Boolean
        Return id >= 0
    End Function

    Private Shared Sub assert_valid_id(ByVal id As Int64)
        assert(valid_id(id), "valid_id, id = ", id)
    End Sub

    Private Function valid_id() As Boolean
        Return valid_id(_id)
    End Function

    Private Sub assert_valid_id()
        assert_valid_id(_id)
    End Sub

    Private Shared Function valid_offset(ByVal offset As UInt64) As Boolean
        Return offset >= 0
    End Function

    Private Shared Sub assert_valid_offset(ByVal offset As UInt64)
        assert(valid_offset(offset), "valid_offset, offset = ", offset)
    End Sub

    Private Function valid_offset() As Boolean
        Return valid_offset(_offset)
    End Function

    Private Sub assert_valid_offset()
        assert_valid_offset(_offset)
    End Sub

    Private Shared Function valid_length(ByVal length As UInt64) As Boolean
        Return length > 0
    End Function

    Private Shared Sub assert_valid_length(ByVal length As UInt64)
        assert(valid_length(length), "valid_length, length = ", length)
    End Sub

    Private Function valid_length() As Boolean
        Return valid_length(_length)
    End Function

    Private Sub assert_valid_length()
        assert_valid_length(_length)
    End Sub

    Private Shared Function valid_used(ByVal used As Int64) As Boolean
        Return used >= 0 OrElse used = FREE_USED
    End Function

    Private Shared Sub assert_valid_used(ByVal used As Int64)
        assert(valid_used(used), "valid_used, used = ", used)
    End Sub

    Private Function valid_used() As Boolean
        Return valid_used(_used)
    End Function

    Private Sub assert_valid_used()
        assert_valid_used(_used)
    End Sub

    Private Shared Function valid_chain_id(ByVal id As Int64) As Boolean
        Return valid_id(id) OrElse id = INVALID_ID
    End Function

    Private Shared Sub assert_valid_chain_id(ByVal id As Int64)
        assert(valid_chain_id(id), "valid_chain_id, id = ", id)
    End Sub

    Private Shared Function valid_prev_id(ByVal id As Int64, ByVal prev_id As Int64) As Boolean
        Return valid_id(id) AndAlso valid_chain_id(prev_id) AndAlso id <> prev_id
    End Function

    Private Shared Sub assert_valid_prev_id(ByVal id As Int64, ByVal prev_id As Int64)
        assert(valid_prev_id(id, prev_id), "valid_prev_id, id = ", id, ", prev_id = ", prev_id)
    End Sub

    Private Function valid_prev_id() As Boolean
        Return valid_prev_id(_id, _prev_id)
    End Function

    Private Sub assert_valid_prev_id()
        assert_valid_prev_id(_id, _prev_id)
    End Sub

    Private Shared Function valid_next_id(ByVal id As Int64, ByVal next_id As Int64) As Boolean
        Return valid_id(id) AndAlso valid_chain_id(next_id) AndAlso id <> next_id
    End Function

    Private Shared Sub assert_valid_next_id(ByVal id As Int64, ByVal next_id As Int64)
        assert(valid_prev_id(id, next_id), "valid_next_id, id = ", id, ", next_id = ", next_id)
    End Sub

    Private Function valid_next_id() As Boolean
        Return valid_next_id(_id, _next_id)
    End Function

    Private Sub assert_valid_next_id()
        assert_valid_next_id(_id, _next_id)
    End Sub

    Private Shared Function valid_used_length(ByVal used As Int64, ByVal length As UInt64) As Boolean
        Return valid_used(used) AndAlso
               valid_length(length) AndAlso
               used <= length
    End Function

    Private Shared Sub assert_valid_used_length(ByVal used As Int64, ByVal length As UInt64)
        assert(valid_used_length(used, length), "valid_used_length, used = ", used, ", length = ", length)
    End Sub

    Private Function valid_used_length() As Boolean
        Return valid_used_length(_used, _length)
    End Function

    Private Sub assert_valid_used_length()
        assert_valid_used_length(_used, _length)
    End Sub

    Private Shared Function valid_used_chain_id(ByVal used As Int64,
                                                ByVal id As Int64,
                                                ByVal prev_id As Int64,
                                                ByVal next_id As Int64) As Boolean
        Return valid_used(used) AndAlso
               valid_prev_id(id, prev_id) AndAlso
               valid_next_id(id, next_id) AndAlso
               (used <> FREE_USED OrElse (prev_id = INVALID_ID AndAlso next_id = INVALID_ID))
    End Function

    Private Shared Sub assert_valid_used_chain_id(ByVal used As Int64,
                                                  ByVal id As Int64,
                                                  ByVal prev_id As Int64,
                                                  ByVal next_id As Int64)
        assert(valid_used_chain_id(used, id, prev_id, next_id),
               "valid_used_chain_id, used = ",
               used,
               ", id = ",
               id,
               ", prev_id = ",
               prev_id,
               ", next_id = ",
               next_id)
    End Sub

    Private Function valid_used_chain_id() As Boolean
        Return valid_used_chain_id(_used, _id, _prev_id, _next_id)
    End Function

    Private Sub assert_valid_used_chain_id()
        assert_valid_used_chain_id(_used, _id, _prev_id, _next_id)
    End Sub

    Private Shared Function valid_virtdisk(ByVal vd As virtdisk) As Boolean
        Return Not vd Is Nothing
    End Function

    Private Shared Sub assert_valid_virtdisk(ByVal vd As virtdisk)
        assert(valid_virtdisk(vd), "valid_virtdisk, vd = ", If(vd Is Nothing, "Nothing", CStr(vd.valid())))
    End Sub

    Private Function valid_virtdisk() As Boolean
        Return valid_virtdisk(_vd)
    End Function

    Private Sub assert_valid_virtdisk()
        assert_valid_virtdisk(_vd)
    End Sub

    Private Shared Function virtdisk_is_fitting(ByVal vd As virtdisk,
                                                ByVal offset As UInt64,
                                                ByVal length As UInt64) As Boolean
        Return valid_virtdisk(vd) AndAlso
               vd.size() >= end_disk_offset(offset, length)
    End Function

    Private Function virtdisk_is_fitting() As Boolean
        Return virtdisk_is_fitting(virtdisk(), disk_offset(), data_length())
    End Function

    Private Sub assert_valid_status()
        assert_valid_used()
        assert_valid_prev_id()
        assert_valid_next_id()
        assert_valid_used_length()
        assert_valid_used_chain_id()
    End Sub

    Private Sub assert_valid_status_ctor()
        assert_valid_status()
        assert_valid_id()
        assert_valid_offset()
        assert_valid_length()
        assert_valid_virtdisk()
    End Sub
End Class
