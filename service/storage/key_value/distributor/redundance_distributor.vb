
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.lock
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Partial Public Class redundance_distributor
    Implements istrkeyvt

    Private Const max_verify_rate As Int32 = 1000
    Private ReadOnly wrap As key_locked_istrkeyvt
    Private ReadOnly container As istrkeyvt_container
    Private ReadOnly verify_rate As Int32
    Private ReadOnly exp As ref(Of singleentry)
    Private ReadOnly sq As syncqueue

    Private Sub New(ByVal c As istrkeyvt_container, ByVal verify_rate As Int32)
        assert(Not c Is Nothing)
        Me.container = c
        Me.verify_rate = verify_rate
        Me.exp = New ref(Of singleentry)()
        Me.wrap = New key_locked_istrkeyvt(Me)
        Me.sq = New syncqueue(Me)
    End Sub

    Public Shared Function ctor(ByVal c As istrkeyvt_container,
                                Optional ByVal verify_rate As Int32 = max_verify_rate) As istrkeyvt
        Return (New redundance_distributor(c, verify_rate)).wrappered()
    End Function

    Public Shared Function ctor(ByVal names() As String,
                                ByVal verify_rate As Int32) As istrkeyvt
        Return ctor(istrkeyvt_container.ctor(names), verify_rate)
    End Function

    Public Shared Function ctor(ByVal ParamArray names() As String) As istrkeyvt
        Return ctor(names, max_verify_rate)
    End Function

    Private Function need_verify() As Boolean
        Return rnd_bool(verify_rate, max_verify_rate)
    End Function

    Private Function amu(ByVal key As String,
                         ByVal d As Func(Of istrkeyvt, ref(Of Boolean), event_comb),
                         ByVal result As ref(Of Boolean)) As event_comb
        Return distribute(container,
                          d,
                          result,
                          Function(ecs() As event_comb,
                                   rs() As ref(Of Boolean),
                                   ByRef r As Boolean) As Boolean
                              For i As Int32 = 1 To container.size() - 1
                                  If ecs(0).end_result() <> ecs(i).end_result() OrElse
                                     (+(rs(0))) <> (+(rs(i))) Then
                                      sq.push_to_prepare_sync_queue(key)
                                      Exit For
                                  End If
                              Next
                              Dim suc As Boolean = False
                              r = False
                              For i As Int32 = 0 To container.size() - 1
                                  If ecs(i).end_result() Then
                                      suc = True
                                      If +(rs(i)) Then
                                          r = +(rs(i))
                                          Return True
                                      End If
                                  End If
                              Next
                              Return suc
                          End Function)
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return amu(key, Function(x, y) x.append(key, value, ts, y), result)
    End Function

    Private Function compare_int64(ByVal d As Func(Of istrkeyvt, ref(Of Int64), event_comb),
                                   ByVal init_value As Int64,
                                   ByVal choose As Func(Of Int64, Int64, Boolean),
                                   ByVal result As ref(Of Int64)) As event_comb
        assert(Not choose Is Nothing)
        Return distribute(container,
                          d,
                          result,
                          Function(ecs() As event_comb, rs() As ref(Of Int64), ByRef r As Int64) As Boolean
                              Dim m As Int64 = 0
                              m = init_value
                              For i As Int32 = 0 To container.size() - 1
                                  If ecs(i).end_result() Then
                                      If m = init_value OrElse
                                         choose(+(rs(i)), m) Then
                                          m = (+(rs(i)))
                                      End If
                                  End If
                              Next
                              Return m <> init_value AndAlso
                                     eva(r, m)
                          End Function)
    End Function

    Private Function min(ByVal d As Func(Of istrkeyvt, ref(Of Int64), event_comb),
                         ByVal result As ref(Of Int64)) As event_comb
        Return compare_int64(d,
                             npos,
                             Function(x, y) x < y,
                             result)
    End Function

    Private Function max(ByVal d As Func(Of istrkeyvt, ref(Of Int64), event_comb),
                         ByVal result As ref(Of Int64)) As event_comb
        Return compare_int64(d,
                             npos,
                             Function(x, y) x > y,
                             result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        If need_verify() Then
            Return min(Function(x, y) x.capacity(y), result)
        Else
            Return container.random_select().capacity(result)
        End If
    End Function

    Private Shared Function delete_result(ByVal ecs() As event_comb,
                                     ByVal r() As ref(Of Boolean)) As Boolean
        assert(array_size(ecs) = array_size(r))
        For i As Int32 = 0 To array_size(ecs) - 1
            If ecs(i).end_result() AndAlso +(r(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return distribute(container,
                          Function(x, y) x.delete(key, y),
                          result,
                          Function(ecs() As event_comb,
                                   i() As ref(Of Boolean),
                                   ByRef o As Boolean) As Boolean
                              Return event_comb_result_suc(ecs) AndAlso
                                     eva(o, delete_result(ecs, i))
                          End Function)
    End Function

    Private Shared Function empty_result(ByVal ecs() As event_comb,
                                         ByVal r() As ref(Of Boolean)) As Boolean
        assert(array_size(ecs) = array_size(r))
        For i As Int32 = 0 To array_size(ecs) - 1
            If ecs(i).end_result() AndAlso Not (+(r(i))) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        If need_verify() Then
            Return distribute(container,
                              Function(x, y) x.empty(y),
                              result,
                              Function(ecs() As event_comb,
                                       i() As ref(Of Boolean),
                                       ByRef o As Boolean) As Boolean
                                  Return event_comb_result_suc(ecs) AndAlso
                                         eva(o, empty_result(ecs, i))
                              End Function)
        Else
            Return container.random_select().empty(result)
        End If
    End Function

    Private Shared Function full_result(ByVal ecs() As event_comb,
                                        ByVal r() As ref(Of Boolean)) As Boolean
        assert(array_size(ecs) = array_size(r))
        For i As Int32 = 0 To array_size(ecs) - 1
            If Not ecs(i).end_result() OrElse +(r(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        If need_verify() Then
            Return distribute(container,
                              Function(x, y) x.full(y),
                              result,
                              Function(ecs() As event_comb,
                                       i() As ref(Of Boolean),
                                       ByRef o As Boolean) As Boolean
                                  Return event_comb_result_suc(ecs) AndAlso
                                         eva(o, full_result(ecs, i))
                              End Function)
        Else
            Return container.random_select().full(result)
        End If
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return all_heartbeat(container)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        If need_verify() Then
            Return max(Function(x, y) x.keycount(y), result)
        Else
            Return container.random_select().keycount(result)
        End If
    End Function

    Private Shared Function list_has_value(ByVal it As stringtrie(Of Boolean).iterator,
                                           ByVal all As stringtrie(Of Boolean)) As Boolean
        If it <> all.end() AndAlso (+it).has_value Then
            assert((+it).value)
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function list_merge(ByVal ss As vector(Of stringtrie(Of Boolean))) As stringtrie(Of Boolean)
        Dim all As stringtrie(Of Boolean) = Nothing
        all = New stringtrie(Of Boolean)()
        For i As Int32 = 0 To ss.size() - 1
            Dim it As stringtrie(Of Boolean).iterator = Nothing
            it = ss(i).begin()
            While it <> ss(i).end()
                If list_has_value(it, ss(i)) Then
                    assert(all.insert(stringtrie(Of Boolean).first(it), True) <> all.end())
                End If
                it += 1
            End While
        Next
        Return all
    End Function

    Private Shared Function list_to_vector(ByVal all As stringtrie(Of Boolean)) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        r = New vector(Of String)()
        Dim it As stringtrie(Of Boolean).iterator = Nothing
        it = all.begin()
        While it <> all.end()
            If list_has_value(it, all) Then
                r.emplace_back(stringtrie(Of Boolean).first(it))
            End If
            it += 1
        End While
        Return r
    End Function

    Private Function list_result(ByVal ecs() As event_comb,
                                 ByVal nr() As ref(Of vector(Of String)),
                                 ByVal result As ref(Of vector(Of String))) As event_comb
        assert(array_size(ecs) = array_size(nr))
        Dim ss As vector(Of stringtrie(Of Boolean)) = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim tecs As vector(Of event_comb) = Nothing
                                  tecs = New vector(Of event_comb)()
                                  ss = New vector(Of stringtrie(Of Boolean))()
                                  For i As Int32 = 0 To array_size(ecs) - 1
                                      If ecs(i).end_result() AndAlso Not (+(nr(i))) Is Nothing Then
                                          Dim j As Int32 = 0
                                          j = i
                                          ss.emplace_back(New stringtrie(Of Boolean)())
                                          tecs.emplace_back(sync_async(
                                                Sub()
                                                    For k As Int32 = 0 To (+(nr(j))).size() - 1
                                                        assert(ss(j).insert((+(nr(j)))(k), True) <> ss(j).end())
                                                    Next
                                                End Sub))
                                      End If
                                  Next
                                  If tecs.empty() Then
                                      Return False
                                  Else
                                      Return waitfor(+tecs) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim all As stringtrie(Of Boolean) = Nothing
                                  all = list_merge(ss)

                                  Dim r As vector(Of String) = Nothing
                                  r = list_to_vector(all)
                                  assert(eva(result, r))

                                  For i As Int32 = 0 To array_size(ecs) - 1
                                      If ecs(i).end_result() AndAlso
                                         ((+(nr(i))) Is Nothing OrElse (+(nr(i))).empty()) Then
                                          For j As Int32 = 0 To r.size() - 1
                                              sq.push_to_prepare_sync_queue(r(j))
                                          Next
                                          Return goto_end()
                                      End If
                                  Next
                                  all.clear()
                                  For i As Int32 = 0 To ss.size() - 1
                                      For j As Int32 = 0 To r.size() - 1
                                          Dim sit As stringtrie(Of Boolean).iterator = Nothing
                                          sit = ss(i).find(r(j))
                                          Dim ait As stringtrie(Of Boolean).iterator = Nothing
                                          ait = all.find(r(j))
                                          If Not list_has_value(sit, ss(i)) AndAlso Not list_has_value(ait, all) Then
                                              sq.push_to_prepare_sync_queue(r(j))
                                              assert(all.insert(r(j), True) <> all.end())
                                          End If
                                      Next
                                  Next
                                  Return goto_end()
                              End Function)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        If need_verify() Then
            Dim nr() As ref(Of vector(Of String)) = Nothing
            Dim ecs() As event_comb = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      ReDim nr(container.size() - 1)
                                      ReDim ecs(container.size() - 1)
                                      For i As Int32 = 0 To container.size() - 1
                                          nr(i) = New ref(Of vector(Of String))()
                                          ecs(i) = container(i).list(nr(i))
                                      Next
                                      Return waitfor(ecs) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Dim has_suc As Boolean = False
                                      Dim has_fail As Boolean = False
                                      event_comb_result(ecs, has_suc, has_fail)
                                      If has_suc Then
                                          ec = list_result(ecs, nr, result)
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
        Else
            Return container.random_select().list(result)
        End If
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return amu(key, Function(x, y) x.modify(key, value, ts, y), result)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        If need_verify() Then
            Dim nr As ref(Of Boolean()) = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      nr = New ref(Of Boolean())()
                                      ec = read(key, result, ts, nr)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() Then
                                          For i As Int32 = 0 To container.size() - 1
                                              If Not ((+nr)(i)) Then
                                                  sq.push_to_prepare_sync_queue(key)
                                                  Exit For
                                              End If
                                          Next
                                          Return goto_end()
                                      Else
                                          'cannot read from any node, do not try to sync
                                          Return False
                                      End If
                                  End Function)
        Else
            Return container.random_select().read(key, result, ts)
        End If
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = all_retire(container)
                                  Return waitfor(sq.clear(), ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Sub event_comb_result(ByVal ecs() As event_comb,
                                         ByRef has_suc As Boolean,
                                         ByRef has_fail As Boolean)
        has_suc = False
        has_fail = False
        For i As Int32 = 0 To array_size(ecs) - 1
            If ecs(i).end_result() Then
                has_suc = True
            ElseIf Not ecs(i).end_result() Then
                has_fail = True
            End If
            If has_suc AndAlso has_fail Then
                Return
            End If
        Next
    End Sub

    Private Shared Function event_comb_result_suc(ByVal ecs() As event_comb) As Boolean
        Dim s As Boolean = False
        event_comb_result(ecs, s, Nothing)
        Return s
    End Function

    Private Shared Sub seek_result(ByVal ecs() As event_comb,
                                   ByVal rs() As ref(Of Boolean),
                                   ByRef has_found As Boolean,
                                   ByRef has_not_found As Boolean)
        assert(array_size(ecs) = array_size(rs))
        has_found = False
        has_not_found = False
        For i As Int32 = 0 To array_size(ecs) - 1
            If ecs(i).end_result() Then
                If +(rs(i)) Then
                    has_found = True
                Else
                    has_not_found = True
                End If
                If has_found AndAlso has_not_found Then
                    Return
                End If
            End If
        Next
    End Sub

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        If need_verify() Then
            Return distribute(container,
                              Function(x, y) x.seek(key, y),
                              result,
                              Function(ecs() As event_comb, i() As ref(Of Boolean), ByRef o As Boolean) As Boolean
                                  Dim has_suc As Boolean = False
                                  Dim has_fail As Boolean = False
                                  event_comb_result(ecs, has_suc, has_fail)
                                  If has_suc Then
                                      Dim has_found As Boolean = False
                                      Dim has_not_found As Boolean = False
                                      seek_result(ecs, i, has_found, has_not_found)
                                      o = has_found
                                      If has_found AndAlso (has_not_found OrElse has_fail) Then
                                          sq.push_to_prepare_sync_queue(key)
                                      End If
                                      Return True
                                  Else
                                      Return False
                                  End If
                              End Function)
        Else
            Return container.random_select().seek(key, result)
        End If
    End Function

    Private Shared Sub sizeof_result(ByVal ecs() As event_comb,
                                     ByVal rs() As ref(Of Int64),
                                     ByRef has_invalid As Boolean,
                                     ByRef min As Int64,
                                     ByRef max As Int64)
        has_invalid = False
        min = max_int64
        max = min_int64
        assert(array_size(ecs) = array_size(rs))
        For i As Int32 = 0 To array_size(ecs) - 1
            If ecs(i).end_result() Then
                If +(rs(i)) >= npos Then
                    If +(rs(i)) > max Then
                        max = +(rs(i))
                    End If
                    If +(rs(i)) < min Then
                        min = +(rs(i))
                    End If
                Else
                    has_invalid = True
                End If
            End If
        Next
    End Sub

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        If need_verify() Then
            Return distribute(container,
                              Function(x, y) x.sizeof(key, y),
                              result,
                              Function(ecs() As event_comb, i() As ref(Of Int64), ByRef o As Int64) As Boolean
                                  Dim has_suc As Boolean = False
                                  Dim has_fail As Boolean = False
                                  event_comb_result(ecs, has_suc, has_fail)
                                  If has_suc Then
                                      Dim min As Int64 = 0
                                      Dim max As Int64 = 0
                                      Dim has_invalid As Boolean = False
                                      sizeof_result(ecs, i, has_invalid, min, max)
                                      If max > npos AndAlso (min <> max OrElse has_invalid OrElse has_fail) Then
                                          sq.push_to_prepare_sync_queue(key)
                                      End If
                                      o = max
                                      Return True
                                  Else
                                      Return False
                                  End If
                              End Function)
        Else
            Return container.random_select().sizeof(key, result)
        End If
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return sync_async(Function() As Boolean
                              Return expired().mark_in_use()
                          End Function)
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return amu(key, Function(x, y) x.unique_write(key, value, ts, y), result)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        If need_verify() Then
            Return max(Function(x, y) x.valuesize(y), result)
        Else
            Return container.random_select().valuesize(result)
        End If
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As redundance_distributor) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Const verify_rate As String = "verify-rate"
            Dim c As istrkeyvt_container = Nothing
            If istrkeyvt_container.create(v, c) Then
                v.bind(verify_rate)
                Dim vr As Int32 = 0
                If v.value(verify_rate, vr) Then
                    o = redundance_distributor.ctor(c, vr)
                Else
                    o = redundance_distributor.ctor(c)
                End If
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of redundance_distributor)(AddressOf create))
        assert(constructor.register(Of istrkeyvt)("redundance-distributor", AddressOf create))
    End Sub
End Class
