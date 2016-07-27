﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class cluster
    Private Const INVALID_ID As Int64 = npos
    Private Const INVALID_OFFSET As Int64 = npos
    Private Const FREE_USED As Int64 = npos
    Private Const DISK_CLUSTER_SIZE As UInt16 = 512
    Private Shared ReadOnly CLUSTERID_OFFSET As Int32 = 0
    Private Shared ReadOnly USED_OFFSET As Int32 = CLUSTERID_OFFSET + sizeof_int64
    Private Shared ReadOnly LENGTH_OFFSET As Int32 = USED_OFFSET + sizeof_int64
    Private Shared ReadOnly NEXTCLUSTERID_OFFSET As Int32 = LENGTH_OFFSET + sizeof_int64
    Private Shared ReadOnly PREVCLUSTERID_OFFSET As Int32 = NEXTCLUSTERID_OFFSET + sizeof_int64
    Private Shared ReadOnly CHECKSUM() As Byte = Text.Encoding.Unicode().GetBytes("HHHH")
    Private Shared ReadOnly CHECKSUM_SIZE As Int32 = array_size(CHECKSUM)
    Private Shared ReadOnly CHECKSUM_OFFSET As Int32 = PREVCLUSTERID_OFFSET + sizeof_int64
    Private Shared ReadOnly DATA_OFFSET As Int32 = CHECKSUM_OFFSET + CHECKSUM_SIZE
    Private Shared ReadOnly STRUCTURE_SIZE As Int32 = DATA_OFFSET
    Private Shared ReadOnly MIN_CLUSTER_LENGTH As Int32 =
        assert_return(DISK_CLUSTER_SIZE > DATA_OFFSET, DISK_CLUSTER_SIZE - DATA_OFFSET)

    Private Shared Sub verify_constants()
        assert(Not valid_id(INVALID_ID))
        assert(Not valid_offset(INVALID_OFFSET))
        assert(FREE_USED < 0)
        assert(CHECKSUM_SIZE = sizeof_int64) 'not neccessary
    End Sub
End Class
