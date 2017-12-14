
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports store_t = osi.root.formation.hashmap(Of osi.root.formation.array_pointer(Of Byte),
                                                osi.root.formation.pair(Of System.Int64, System.Int64))

Partial Public Class fces
    Implements ikeyvalue2(Of store_t.iterator)

    Public Function append_existing(ByVal it As store_t.iterator,
                                    ByVal value() As Byte,
                                    ByVal result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvalue2(Of store_t.iterator).append_existing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim cid As Int64 = 0
                                  If find_cluster_id(it, Nothing, cid) Then
                                      ec = content.append(cid, value, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb _
                            Implements ikeyvalue2(Of store_t.iterator).capacity
        Return sync_async(Function() As Int64
                              Dim r As UInt64 = 0
                              r = content.capacity()
                              If r > max_int64 Then
                                  Return max_int64
                              Else
                                  Return CLng(r)
                              End If
                          End Function,
                          result,
                          +result)
    End Function

    Public Function delete_existing(ByVal it As store_t.iterator,
                                    ByVal result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvalue2(Of store_t.iterator).delete_existing
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Dim r1 As pointer(Of Boolean) = Nothing
        Dim r2 As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim iid As Int64 = 0
                                  Dim cid As Int64 = 0
                                  If find_cluster_id(it, iid, cid) Then
                                      r1 = New pointer(Of Boolean)()
                                      r2 = New pointer(Of Boolean)()
                                      ec1 = index.delete(iid, r1)
                                      ec2 = content.delete(cid, r2)
                                      Return waitfor(ec1, ec2) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec1.end_result() AndAlso
                                         ec2.end_result() AndAlso
                                         eva(result, (+r1) AndAlso
                                                     (+r2) AndAlso
                                                     m.erase(it)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb _
                         Implements ikeyvalue2(Of store_t.iterator).empty
        Return sync_async(Function() As Boolean
                              Return m.empty()
                          End Function,
                          result,
                          +result)
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb _
                        Implements ikeyvalue2(Of store_t.iterator).full
        Return sync_async(Function() As Boolean
                              Return m.size() >= max_key_count
                          End Function,
                          result,
                          +result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue2(Of store_t.iterator).heartbeat
        Return open()
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb _
                            Implements ikeyvalue2(Of store_t.iterator).keycount
        Return sync_async(Function() As Int64
                              Return m.size()
                          End Function,
                          result,
                          +result)
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvalue2(Of store_t.iterator).list
        Return sync_async(Function() As vector(Of Byte())
                              Dim r As vector(Of Byte()) = Nothing
                              r = New vector(Of Byte())()
                              Dim it As store_t.iterator = Nothing
                              it = m.begin()
                              While it <> m.end()
                                  r.push_back(+((+it).first))
                                  it += 1
                              End While
                              Return r
                          End Function,
                          result,
                          +result)
    End Function

    Public Function read_existing(ByVal it As store_t.iterator,
                                  ByVal value As pointer(Of Byte())) As event_comb _
                                 Implements ikeyvalue2(Of store_t.iterator).read_existing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim cid As Int64 = 0
                                  If find_cluster_id(it, Nothing, cid) Then
                                      ec = content.read(cid, value)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue2(Of store_t.iterator).retire
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  m.clear()
                                  ec = index.retire()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = content.retire()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal it As pointer(Of store_t.iterator),
                         ByVal result As pointer(Of Boolean)) As event_comb _
                        Implements ikeyvalue2(Of store_t.iterator).seek
        Return New event_comb(Function() As Boolean
                                  Dim t As store_t.iterator = Nothing
                                  Return eva(result, find_cluster_id(key, t, Nothing, Nothing)) AndAlso
                                         eva(it, t) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sizeof_existing(ByVal it As store_t.iterator,
                                    ByVal result As pointer(Of Int64)) As event_comb _
                                   Implements ikeyvalue2(Of store_t.iterator).sizeof_existing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim cid As Int64 = 0
                                  If find_cluster_id(it, Nothing, cid) Then
                                      ec = content.sizeof(cid, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue2(Of store_t.iterator).stop
        Return sync_async(Sub()
                              index.close()
                              content.close()
                          End Sub)
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb _
                             Implements ikeyvalue2(Of store_t.iterator).valuesize
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of UInt64) = Nothing
        Return New event_comb(Function() As Boolean
                                  _new(p)
                                  ec = content.valuesize(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         +p <= max_int64 AndAlso
                                         eva(result, CLng(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function write_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByVal result As pointer(Of Boolean)) As event_comb _
                             Implements ikeyvalue2(Of store_t.iterator).write_new
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Dim iid As pointer(Of Int64) = Nothing
        Dim cid As pointer(Of Int64) = Nothing
        Dim r1 As pointer(Of Boolean) = Nothing
        Dim r2 As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  If m.size() >= max_key_count Then
                                      Return eva(result, False) AndAlso
                                             goto_end()
                                  Else
                                      iid = New pointer(Of Int64)()
                                      cid = New pointer(Of Int64)()
                                      ec1 = index.alloc(array_size(key) + sizeof_int64, iid)
                                      ec2 = content.alloc(array_size(value), cid)
                                      Return waitfor(ec1, ec2) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim kb() As Byte = Nothing
                                  If ec1.end_result() AndAlso
                                     ec2.end_result() AndAlso
                                     generate_index(key, +cid, kb) Then
                                      r1 = New pointer(Of Boolean)()
                                      r2 = New pointer(Of Boolean)()
                                      ec1 = index.append(+iid, kb, r1)
                                      ec2 = content.append(+cid, value, r2)
                                      Return waitfor(ec1, ec2) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec1.end_result() AndAlso
                                     ec2.end_result() AndAlso
                                     (+r1) AndAlso
                                     (+r2) Then
                                      inject_index(key, +iid, +cid)
                                      Return eva(result, (+r1) AndAlso (+r2)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
