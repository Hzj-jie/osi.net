
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports clusters_t = osi.root.formation.hashmap(Of System.Int64, osi.service.storage.cluster, osi.root.template._1023)

Partial Public Class free_cluster
    Private Shared Function head_cluster(ByVal c As cluster) As Boolean
        Return assert(Not c Is Nothing) AndAlso Not c.free() AndAlso Not c.has_prev_id()
    End Function

    Private Shared Function chain(ByVal h As cluster, ByVal cs As vector(Of cluster)) As event_comb
        assert(Not h Is Nothing)
        assert(Not cs Is Nothing)
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If cs.empty() Then
                                      Return goto_end()
                                  Else
                                      ec = storage.cluster.chain(h, +cs)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function cluster(ByVal id As Int64, ByRef c As cluster, ByVal m As clusters_t) As Boolean
        assert(Not m Is Nothing)
        Dim it As clusters_t.iterator = Nothing
        it = m.find(id)
        If it = m.end() Then
            Return False
        Else
            c = (+it).second
            assert(Not c Is Nothing)
            Return True
        End If
    End Function

    Private Function cluster(ByVal id As Int64, ByRef c As cluster) As Boolean
        Return cluster(id, c, cs)
    End Function

    Private Function head_cluster(ByVal id As Int64, ByRef c As cluster) As Boolean
        Return cluster(id, c, hcs) AndAlso
               assert(head_cluster(c))
    End Function

    Private Function next_cluster(ByVal c As cluster, ByRef n As cluster) As Boolean
        assert(Not c Is Nothing)
        If c.has_next_id() Then
            If cluster(c.next_id(), n) Then
                Return True
            Else
                raise_error(error_type.warning, "failed to find next cluster id ", c.next_id())
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Sub chained_clusters(ByVal h As cluster, ByVal cs As vector(Of cluster))
        assert(head_cluster(h))
        assert(Not cs Is Nothing AndAlso cs.empty())
        Dim n As cluster = Nothing
        Do
            If Not n Is Nothing Then
                h = n
            End If
            cs.emplace_back(h)
        Loop While next_cluster(h, n)
    End Sub

    'exp_size is the real size of the data
    Private Function free_cluster(ByVal exp_size As Int64,
                                  ByVal c As pointer(Of cluster)) As event_comb
        assert(exp_size > 0)
        assert(Not c Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not fcs.empty() Then
                                      Return eva(c, fcs.front()) AndAlso
                                             assert(fcs.pop()) AndAlso
                                             goto_end()
                                  End If
                                  ec = storage.cluster.ctor(vd, _inc(max_cluster_id), exp_size, c)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(cs.find((+c).id()) = cs.end())
                                      cs((+c).id()) = (+c)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Function free_clusters(ByVal exp_size As Int64,
                                   ByVal cs As vector(Of cluster),
                                   ByVal offsets As vector(Of Int64)) As event_comb
        assert(exp_size > 0)
        assert(Not cs Is Nothing AndAlso cs.empty())
        assert(Not offsets Is Nothing AndAlso offsets.empty())
        Dim ec As event_comb = Nothing
        Dim c As pointer(Of cluster) = Nothing
        Dim i As Int64 = 0
        Return New event_comb(Function() As Boolean
                                  If ec Is Nothing OrElse
                                     ec.end_result() Then
                                      If ec Is Nothing Then
                                          assert(c Is Nothing)
                                          c = New pointer(Of cluster)()
                                      Else
                                          assert(Not c Is Nothing AndAlso Not (+c) Is Nothing)
                                          cs.emplace_back(+c)
                                          offsets.emplace_back(i)
                                          assert((+c).free())
                                          i += (+c).remain_bytes()
                                          c.clear()
                                      End If
                                      If i < exp_size Then
                                          ec = free_cluster(exp_size - i, c)
                                          Return waitfor(ec)
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
