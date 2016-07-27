
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class cluster
    Private Function create_structure() As Byte()
        Dim r() As Byte = Nothing
        ReDim r(STRUCTURE_SIZE - 1)
        assert(int64_bytes(id(), r, CLUSTERID_OFFSET))
        assert(int64_bytes(used(), r, USED_OFFSET))
        assert(int64_bytes(data_length(), r, LENGTH_OFFSET))
        assert(int64_bytes(next_id(), r, NEXTCLUSTERID_OFFSET))
        assert(int64_bytes(prev_id(), r, PREVCLUSTERID_OFFSET))
        memcpy(r, CHECKSUM_OFFSET, CHECKSUM, 0, CHECKSUM_SIZE)
        Return r
    End Function

    Private Shared Function parse_structure(ByVal buff() As Byte,
                                            ByRef id As Int64,
                                            ByRef used As Int64,
                                            ByRef length As Int64,
                                            ByRef next_id As Int64,
                                            ByRef prev_id As Int64) As Boolean
        Return assert(array_size(buff) = STRUCTURE_SIZE) AndAlso
               assert(bytes_int64(buff, id, CLUSTERID_OFFSET)) AndAlso
               assert(bytes_int64(buff, used, USED_OFFSET)) AndAlso
               assert(bytes_int64(buff, length, LENGTH_OFFSET)) AndAlso
               assert(bytes_int64(buff, next_id, NEXTCLUSTERID_OFFSET)) AndAlso
               assert(bytes_int64(buff, prev_id, PREVCLUSTERID_OFFSET)) AndAlso
               memcmp(buff, CHECKSUM_OFFSET, CHECKSUM, 0, CHECKSUM_SIZE) = 0 AndAlso
               valid_id(id) AndAlso
               valid_used(used) AndAlso
               valid_length(length) AndAlso
               valid_next_id(id, next_id) AndAlso
               valid_prev_id(id, prev_id) AndAlso
               valid_used_length(used, length) AndAlso
               valid_used_chain_id(used, id, prev_id, next_id)
    End Function

    Private Shared Function parse_structure(ByVal buff() As Byte,
                                            ByVal id As pointer(Of Int64),
                                            ByVal used As pointer(Of Int64),
                                            ByVal length As pointer(Of Int64),
                                            ByVal next_id As pointer(Of Int64),
                                            ByVal prev_id As pointer(Of Int64)) As Boolean
        Dim i As Int64 = 0
        Dim u As Int64 = 0
        Dim l As Int64 = 0
        Dim n As Int64 = 0
        Dim p As Int64 = 0
        Return parse_structure(buff, i, u, l, n, p) AndAlso
               eva(id, i) AndAlso
               eva(used, u) AndAlso
               eva(length, l) AndAlso
               eva(next_id, n) AndAlso
               eva(prev_id, p)
    End Function

    Private Function write_structure() As event_comb
        Dim ec As event_comb = Nothing
        Dim struct() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  struct = create_structure()
                                  assert(array_size(struct) = STRUCTURE_SIZE)
                                  ec = virtdisk().write(disk_offset(), STRUCTURE_SIZE, struct)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function read_structure(ByVal vd As virtdisk,
                                           ByVal offset As Int64,
                                           ByVal id As pointer(Of Int64),
                                           ByVal used As pointer(Of Int64),
                                           ByVal length As pointer(Of Int64),
                                           ByVal next_id As pointer(Of Int64),
                                           ByVal prev_id As pointer(Of Int64)) As event_comb
        Dim r() As Byte = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If valid_virtdisk(vd) AndAlso
                                     valid_offset(offset) Then
                                      ReDim r(STRUCTURE_SIZE - 1)
                                      ec = vd.read(offset, STRUCTURE_SIZE, r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If length Is Nothing Then
                                      'the following end_disk_offset logic requires length to be readable
                                      length = New pointer(Of Int64)()
                                  End If
                                  Return ec.end_result() AndAlso
                                         parse_structure(r, id, used, length, next_id, prev_id) AndAlso
                                         virtdisk_is_fitting(vd, offset, +length) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
