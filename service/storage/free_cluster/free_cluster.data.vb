
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports clusters_t = osi.root.formation.hashmap(Of System.Int64, osi.service.storage.cluster)

Partial Public Class free_cluster
    Public Function read(ByVal id As Int64, ByVal r As pointer(Of Byte())) As event_comb
        Dim buff() As Byte = Nothing
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim h As cluster = Nothing
                                  If head_cluster(id, h) Then
                                      Dim cs As vector(Of cluster) = Nothing
                                      cs = New vector(Of cluster)()
                                      chained_clusters(h, cs)
                                      assert(Not cs.empty())
                                      Dim offset As UInt64 = 0
                                      For i As UInt32 = 0 To cs.size() - uint32_1
                                          offset += cs(i).used_bytes()
                                          If offset > max_uint32 Then
                                              Return False
                                          End If
                                      Next

                                      ReDim buff(CInt(offset) - 1)
                                      offset = 0
                                      ReDim ecs(CInt(cs.size()) - 1)
                                      For i As UInt32 = 0 To cs.size() - uint32_1
                                          ecs(CInt(i)) = cs(i).read(buff, CUInt(offset))
                                          offset += cs(i).used_bytes()
                                      Next
                                      Return waitfor(ecs) AndAlso
                                             goto_next()
                                  Else
                                      Return eva(r, DirectCast(Nothing, Byte())) AndAlso
                                             goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ecs.end_result() Then
                                      Return eva(r, buff) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function append(ByVal id As Int64,
                           ByVal buff() As Byte,
                           ByVal result As pointer(Of Boolean)) As event_comb
        Dim offsets As vector(Of UInt64) = Nothing
        Dim cs As vector(Of cluster) = Nothing
        Dim hr As UInt64 = 0
        Dim ecs() As event_comb = Nothing
        Dim ec As event_comb = Nothing
        Dim h As cluster = Nothing
        Return New event_comb(Function() As Boolean
                                  If isemptyarray(buff) Then
                                      Return eva(result, True) AndAlso
                                             goto_end()
                                  Else
                                      If head_cluster(id, h) Then
                                          cs = New vector(Of cluster)()
                                          chained_clusters(h, cs)
                                          assert(Not cs.empty())
                                          h = cs(cs.size() - uint32_1)
                                          cs.clear()
                                          offsets = New vector(Of UInt64)()
                                          hr = h.remain_bytes()
                                          If hr < array_size(buff) Then
                                              ec = free_clusters(array_size(buff) - hr, cs, offsets)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          Else
                                              Return goto_next()
                                          End If
                                      Else
                                          Return eva(result, False) AndAlso
                                                 goto_end()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec Is Nothing OrElse ec.end_result() Then
                                      assert(cs.size() = offsets.size())
                                      assert(Not h Is Nothing)
                                      ecs.resize(cs.size() + uint32_1)
                                      Dim bs As UInt32 = 0
                                      bs = array_size(buff)
                                      If cs.empty() Then
                                          assert(hr >= bs)
                                          ecs(0) = h.append(buff, 0, bs)
                                      Else
                                          assert(hr < bs)
                                          ecs(0) = h.append(buff, 0, CUInt(hr))
                                          For i As UInt32 = 0 To cs.size() - uint32_1
                                              Dim current_offset As UInt64 = 0
                                              current_offset = offsets(i) + hr
                                              assert(current_offset <= max_uint32)
                                              If i < cs.size() - 1 Then
                                                  assert(cs(i).remain_bytes() < bs - offsets(i) - hr)
                                                  ecs(CInt(i) + 1) = cs(i).append(buff,
                                                                                  CUInt(current_offset),
                                                                                  CUInt(cs(i).remain_bytes()))
                                              Else
                                                  assert(cs(i).remain_bytes() >= bs - offsets(i) - hr)
                                                  ecs(CInt(i) + 1) = cs(i).append(buff,
                                                                                  CUInt(current_offset),
                                                                                  CUInt(bs - offsets(i) - hr))
                                              End If
                                          Next
                                      End If
                                      Return waitfor(ecs) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ecs.end_result() Then
                                      ec = chain(h, cs)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, True) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function delete(ByVal id As Int64, ByVal result As pointer(Of Boolean)) As event_comb
        Dim cs As vector(Of cluster) = Nothing
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim h As cluster = Nothing
                                  If head_cluster(id, h) Then
                                      assert(hcs.erase(h.id()))
                                      cs = New vector(Of cluster)()
                                      chained_clusters(h, cs)
                                      assert(Not cs.empty())
                                      ReDim ecs(CInt(cs.size()) - 1)
                                      For i As UInt32 = 0 To cs.size() - uint32_1
                                          ecs(CInt(i)) = cs(i).delete()
                                      Next
                                      Return waitfor(ecs) AndAlso
                                             goto_next()
                                  Else
                                      Return eva(result, False) AndAlso
                                             goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not isemptyarray(ecs))
                                  Dim r As Boolean = False
                                  r = True
                                  For i As UInt32 = 0 To array_size(ecs) - uint32_1
                                      If ecs(CInt(i)).end_result() Then
                                          assert(fcs.push(cs(i)))
                                      Else
                                          r = False
                                      End If
                                  Next
                                  Return r AndAlso
                                         eva(result, True) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function seek(ByVal id As Int64) As Boolean
        Return head_cluster(id, Nothing)
    End Function

    Public Function seek(ByVal id As Int64,
                     ByVal result As pointer(Of Boolean)) As event_comb
        Return sync_async(Sub()
                              eva(result, seek(id))
                          End Sub)
    End Function

    Public Function valuesize() As UInt64
        Return vd.size()
    End Function

    Public Function valuesize(ByVal result As pointer(Of UInt64)) As event_comb
        Return sync_async(Sub()
                              eva(result, valuesize())
                          End Sub)
    End Function

    Public Function sizeof(ByVal id As Int64, ByVal result As pointer(Of Int64)) As event_comb
        Dim cs As vector(Of cluster) = Nothing
        Return sync_async(Function() As Boolean
                              Dim h As cluster = Nothing
                              If head_cluster(id, h) Then
                                  cs = New vector(Of cluster)()
                                  chained_clusters(h, cs)
                                  assert(Not cs.empty())
                                  Dim r As UInt64 = 0
                                  For i As UInt32 = 0 To cs.size() - uint32_1
                                      r += cs(i).used_bytes()
                                      If r > max_int64 Then
                                          Return False
                                      End If
                                  Next
                                  Return eva(result, CLng(r))
                              Else
                                  Return eva(result, npos)
                              End If
                          End Function)
    End Function

    Public Function alloc(ByVal exp_size As UInt64, ByVal id As pointer(Of Int64)) As event_comb
        Dim ec As event_comb = Nothing
        Dim c As pointer(Of cluster) = Nothing
        Return New event_comb(Function() As Boolean
                                  c = New pointer(Of cluster)()
                                  ec = free_cluster(exp_size, c)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not (+c) Is Nothing)
                                      assert((+c).mark_free_as_empty())
                                      assert(hcs.find((+c).id()) = hcs.end())
                                      hcs((+c).id()) = (+c)
                                      Return eva(id, (+c).id()) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function retire() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  cs.clear()
                                  hcs.clear()
                                  fcs.clear()
                                  max_cluster_id = 0
                                  ec = vd.drop(0)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function head_clusters() As vector(Of Int64)
        Dim r As vector(Of Int64) = Nothing
        Dim it As clusters_t.iterator = Nothing
        r = New vector(Of Int64)(hcs.size())
        it = hcs.begin()
        While it <> hcs.end()
            r.emplace_back((+it).first)
            it += 1
        End While
        Return r
    End Function
End Class
