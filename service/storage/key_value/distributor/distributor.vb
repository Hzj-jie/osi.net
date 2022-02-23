
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.lock

Public Module _distributor
    Public Function distribute(Of T, T2)(ByVal container As istrkeyvt_container,
                                         ByVal d As Func(Of istrkeyvt, ref(Of T), event_comb),
                                         ByVal result As ref(Of T2),
                                         ByVal merge As _do_val_val_ref(Of event_comb(),
                                                                           ref(Of T)(),
                                                                           T2,
                                                                           Boolean)) As event_comb
        assert(container IsNot Nothing)
        assert(d IsNot Nothing)
        assert(merge IsNot Nothing)
        assert(container.size() > 1)
        Dim ecs() As event_comb = Nothing
        Dim rs() As ref(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim ecs(container.size() - 1)
                                  ReDim rs(container.size() - 1)
                                  For i As Int32 = 0 To container.size() - 1
                                      rs(i) = New ref(Of T)()
                                      assert(container(i) IsNot Nothing)
                                      ecs(i) = d(container(i), rs(i))
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As T2 = Nothing
                                  Return merge(ecs, rs, r) AndAlso
                                         eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function distribute(Of T, T2)(ByVal container As istrkeyvt_container,
                                         ByVal d As Func(Of istrkeyvt, ref(Of T), event_comb),
                                         ByVal result As ref(Of T2),
                                         ByVal merge As _do_val_ref(Of ref(Of T)(), T2, Boolean)) As event_comb
        assert(merge IsNot Nothing)
        Return distribute(container,
                          d,
                          result,
                          Function(ecs() As event_comb, rs() As ref(Of T), ByRef r As T2) As Boolean
                              Return ecs.end_result() AndAlso
                                     merge(rs, r)
                          End Function)
    End Function

    Public Function distribute(Of T, T2)(ByVal container As istrkeyvt_container,
                                         ByVal d As Func(Of istrkeyvt, ref(Of T), event_comb),
                                         ByVal result As ref(Of T2),
                                         ByVal merge As Func(Of ref(Of T)(), T2)) As event_comb
        assert(merge IsNot Nothing)
        Return distribute(container,
                          d,
                          result,
                          Function(rs() As ref(Of T), ByRef r As T2) As Boolean
                              r = merge(rs)
                              Return True
                          End Function)
    End Function

    Public Function distribute(ByVal container As istrkeyvt_container,
                               ByVal d As Func(Of istrkeyvt, event_comb)) As event_comb
        assert(d IsNot Nothing)
        Return distribute(container,
                          Function(x, y) d(x),
                          DirectCast(Nothing, ref(Of Int32)),
                          Function(rs() As ref(Of Int32), ByRef r As Int32) As Boolean
                              Return True
                          End Function)
    End Function

    Public Function sum(ByVal container As istrkeyvt_container,
                        ByVal d As Func(Of istrkeyvt, ref(Of Int64), event_comb),
                        ByVal result As ref(Of Int64)) As event_comb
        Return distribute(container,
                          d,
                          result,
                          Function(rs() As ref(Of Int64)) As Int64
                              Dim r As Int64 = 0
                              For i As Int32 = 0 To array_size(rs) - 1
                                  If (+rs(i)) < 0 Then
                                      Return npos
                                  Else
                                      r += (+rs(i))
                                  End If
                              Next
                              Return r
                          End Function)
    End Function

    Public Function [and](ByVal container As istrkeyvt_container,
                          ByVal d As Func(Of istrkeyvt, ref(Of Boolean), event_comb),
                          ByVal result As ref(Of Boolean)) As event_comb
        Return distribute(container,
                          d,
                          result,
                          Function(rs() As ref(Of Boolean)) As Boolean
                              For i As Int32 = 0 To array_size(rs) - 1
                                  If Not (+rs(i)) Then
                                      Return False
                                  End If
                              Next
                              Return True
                          End Function)
    End Function

    Public Function [or](ByVal container As istrkeyvt_container,
                         ByVal d As Func(Of istrkeyvt, ref(Of Boolean), event_comb),
                         ByVal result As ref(Of Boolean)) As event_comb
        Return distribute(container,
                          d,
                          result,
                          Function(rs() As ref(Of Boolean)) As Boolean
                              For i As Int32 = 0 To array_size(rs) - 1
                                  If (+rs(i)) Then
                                      Return True
                                  End If
                              Next
                              Return False
                          End Function)
    End Function

    Public Function sum_capacity(ByVal container As istrkeyvt_container,
                                 ByVal result As ref(Of Int64)) As event_comb
        Return sum(container, Function(x, y) x.capacity(y), result)
    End Function

    Public Function and_empty(ByVal container As istrkeyvt_container,
                              ByVal result As ref(Of Boolean)) As event_comb
        Return [and](container, Function(x, y) x.empty(y), result)
    End Function

    Public Function or_full(ByVal container As istrkeyvt_container,
                            ByVal result As ref(Of Boolean)) As event_comb
        Return [or](container, Function(x, y) x.full(y), result)
    End Function

    Public Function all_heartbeat(ByVal container As istrkeyvt_container) As event_comb
        Return distribute(container, Function(x) x.heartbeat())
    End Function

    Public Function sum_keycount(ByVal container As istrkeyvt_container,
                                 ByVal result As ref(Of Int64)) As event_comb
        Return sum(container, Function(x, y) x.keycount(y), result)
    End Function

    Public Function merge_list(ByVal container As istrkeyvt_container,
                               ByVal result As ref(Of vector(Of String))) As event_comb
        Return distribute(container,
                          Function(x, y) x.list(y),
                          result,
                          Function(rs() As ref(Of vector(Of String)),
                                   ByRef r As vector(Of String)) As Boolean
                              r = New vector(Of String)()
                              For i As Int32 = 0 To array_size(rs) - 1
                                  If (+rs(i)) Is Nothing Then
                                      Return False
                                  Else
                                      r.emplace_back(+rs(i))
                                  End If
                              Next
                              Return True
                          End Function)
    End Function

    Public Function all_retire(ByVal container As istrkeyvt_container) As event_comb
        Return distribute(container, Function(x) x.retire())
    End Function

    Public Function sum_valuesize(ByVal container As istrkeyvt_container,
                                  ByVal result As ref(Of Int64)) As event_comb
        Return sum(container, Function(x, y) x.valuesize(y), result)
    End Function

    Public Function all_delete(ByVal container As istrkeyvt_container,
                               ByVal key As String,
                               ByVal result As ref(Of Boolean)) As event_comb
        Return distribute(container,
                          Function(x, y) x.delete(key, y),
                          result,
                          Function(rs() As ref(Of Boolean)) As Boolean
                              For i As Int32 = 0 To array_size(rs) - 1
                                  If +(rs(i)) Then
                                      Return True
                                  End If
                              Next
                              Return False
                          End Function)
    End Function
End Module
