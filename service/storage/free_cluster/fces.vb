
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports store_t = osi.root.formation.hashmap(Of osi.root.formation.array_pointer(Of Byte),
                                                osi.root.formation.pair(Of System.Int64, System.Int64))

Partial Public Class fces
    Private ReadOnly index As free_cluster
    Private ReadOnly content As free_cluster
    Private ReadOnly max_key_count As Int64
    Private ReadOnly m As store_t

    'cid + key = index_buff
    Private Shared Function parse_index(ByVal b() As Byte, ByRef key() As Byte, ByRef cid As Int64) As Boolean
        'the key should at least have one character
        If array_size(b) <= sizeof_int64 Then
            Return False
        Else
            cid = bytes_int64(b)
            ReDim key(CInt(array_size(b) - sizeof_int64 - uint32_1))
            arrays.copy(key, 0, b, sizeof_int64, array_size(key))
            Return True
        End If
    End Function

    Private Shared Function generate_index(ByVal key() As Byte, ByVal cid As Int64, ByRef b() As Byte) As Boolean
        If array_size(key) <= 0 Then
            Return False
        Else
            ReDim b(CInt(array_size(key) + sizeof_int64 - uint32_1))
            assert(int64_bytes(cid, b))
            arrays.copy(b, sizeof_int64, key)
            Return True
        End If
    End Function

    Private Function inject_index(ByVal iid As Int64) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Byte())()
                                  ec = index.read(iid, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Dim key() As Byte = Nothing
                                      Dim cid As Int64 = 0
                                      If parse_index(+r, key, cid) Then
                                          inject_index(key, iid, cid)
                                          If Not content.seek(cid) Then
                                              raise_error(error_type.warning,
                                                          "fail to find content cluster id ",
                                                          cid,
                                                          " from content free_cluster ",
                                                          content.file_name(),
                                                          ", the following read operation of this key ",
                                                          "will be treated as not existing")
                                          End If
                                      Else
                                          raise_error(error_type.warning,
                                                      "fail to parse index from index free_cluster ",
                                                      index.file_name(),
                                                      ", ignore")
                                      End If
                                      Return goto_end()
                                  Else
                                      'should not happen, but just return false if it really happened
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Function open() As event_comb
        Dim hcs As vector(Of Int64) = Nothing
        Dim i As UInt32 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  m.clear()
                                  hcs = index.head_clusters()
                                  If hcs Is Nothing Then
                                      Return False
                                  Else
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If i > 0 Then
                                      assert(Not ec Is Nothing)
                                      If Not ec.end_result() Then
                                          Return False
                                      End If
                                  End If
                                  If i = hcs.size() Then
                                      Return goto_end()
                                  End If
                                  ec = inject_index(hcs(i))
                                  i += uint32_1
                                  Return waitfor(ec)
                              End Function)
    End Function

    Private Sub inject_index(ByVal key() As Byte, ByVal iid As Int64, ByVal cid As Int64)
        If m.find(array_pointer.of(key)) <> m.end() Then
            raise_error(error_type.warning,
                        "duplicate key ",
                        bytes_str(key),
                        " found in index free_cluster ",
                        index.file_name(),
                        ", over-written")
        End If
        m(array_pointer.of(key)) = pair.of(iid, cid)
    End Sub

    Private Function find_cluster_id(ByVal key() As Byte,
                                     ByRef it As store_t.iterator,
                                     ByRef iid As Int64,
                                     ByRef cid As Int64) As Boolean
        it = m.find(array_pointer.of(key))
        Return find_cluster_id(it, iid, cid)
    End Function

    Private Function find_cluster_id(ByVal it As store_t.iterator, ByRef iid As Int64, ByRef cid As Int64) As Boolean
        If it Is Nothing OrElse it = m.end() Then
            Return False
        Else
            iid = (+it).second.first
            cid = (+it).second.second
            Return True
        End If
    End Function
End Class
