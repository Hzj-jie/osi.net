﻿
Imports System.IO
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.storage
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Public Class free_cluster_test
    Inherits event_comb_case

    Private Shared ReadOnly reopen_round_count As Int32 = 4 * If(isdebugmode(), 1, 4)
    Private Shared ReadOnly round_count As Int64 = 4096 * If(isdebugmode(), 1, 8)

    Private Shared Function rand_id() As Int64
        'the id will not exceed round_count * reopen_round_count \ 5
        Return rnd_int(0, round_count * reopen_round_count \ 5)
    End Function

    Private Shared Function rand_buff_size() As Int32
        Return rnd_int(8, 2048 + 1)
    End Function

    Private Shared Function rand_bytes() As Byte()
        Return next_bytes(rand_buff_size())
    End Function

    Private Shared Function read_case(ByVal fc As free_cluster,
                                      ByVal d As map(Of Int64, Byte())) As event_comb
        Dim id As Int64 = 0
        Dim p As pointer(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  id = rand_id()
                                  p = New pointer(Of Byte())()
                                  ec = fc.read(id, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Dim it As map(Of Int64, Byte()).iterator = Nothing
                                  it = d.find(id)
                                  If it = d.end() Then
                                      assert_nothing(+p)
                                  Else
                                      assert_equal(memcmp((+it).second, +p), 0)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function apply_append(ByVal fc As free_cluster,
                                         ByVal d As map(Of Int64, Byte()),
                                         ByVal id As Int64,
                                         ByVal exp As Boolean) As event_comb
        Dim buff() As Byte = Nothing
        Dim ec As event_comb = Nothing
        Dim result As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  buff = rand_bytes()
                                  result = New pointer(Of Boolean)()
                                  ec = fc.append(id, buff, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+result, exp)
                                  If +result Then
                                      If d.find(id) <> d.end() Then
                                          Dim b() As Byte = Nothing
                                          Dim ob() As Byte = Nothing
                                          ob = d(id)
                                          ReDim b(array_size(ob) + array_size(buff) - 1)
                                          memcpy(b, ob)
                                          memcpy(b, array_size(ob), buff)
                                          buff = b
                                      End If
                                      d(id) = buff
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function apply_alloc_append(ByVal fc As free_cluster,
                                               ByVal d As map(Of Int64, Byte())) As event_comb
        Dim ec As event_comb = Nothing
        Dim id As pointer(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  id = New pointer(Of Int64)()
                                  ec = fc.alloc(rand_buff_size(), id)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_true(d.find(+id) = d.end())
                                  ec = apply_append(fc, d, +id, True)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function append_case(ByVal fc As free_cluster,
                                        ByVal d As map(Of Int64, Byte())) As event_comb
        Dim id As Int64 = 0
        id = rand_id()
        If d.find(id) = d.end() Then
            If rnd_bool() Then
                Return apply_alloc_append(fc, d)
            Else
                Return apply_append(fc, d, id, False)
            End If
        Else
            Return apply_append(fc, d, id, True)
        End If
    End Function

    Private Shared Function delete_case(ByVal fc As free_cluster,
                                        ByVal d As map(Of Int64, Byte())) As event_comb
        Dim id As Int64 = 0
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  If rnd_bool() AndAlso Not d.empty() Then
                                      Dim max As Int32 = 0
                                      Dim it As map(Of Int64, Byte()).iterator = Nothing
                                      it = d.begin()
                                      While it <> d.end()
                                          If array_size((+it).second) > max Then
                                              max = array_size((+it).second)
                                              id = (+it).first
                                          End If
                                          it += 1
                                      End While
                                  Else
                                      id = rand_id()
                                  End If
                                  r = New pointer(Of Boolean)()
                                  ec = fc.delete(id, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+r, d.erase(id))
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function seek_case(ByVal fc As free_cluster,
                                      ByVal d As map(Of Int64, Byte())) As event_comb
        Dim id As Int64 = 0
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  id = rand_id()
                                  r = New pointer(Of Boolean)()
                                  ec = fc.seek(id, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(+r, d.find(id) <> d.end())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function sizeof_case(ByVal fc As free_cluster,
                                        ByVal d As map(Of Int64, Byte())) As event_comb
        Dim id As Int64 = 0
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  id = rand_id()
                                  r = New pointer(Of Int64)()
                                  ec = fc.sizeof(id, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  If (+r) = npos Then
                                      assert_true(d.find(id) = d.end())
                                  ElseIf assert_true(d.find(id) <> d.end()) Then
                                      assert_equal(array_size(d(id)), +r)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function rnd_case(ByVal fc As free_cluster,
                                     ByVal d As map(Of Int64, Byte())) As event_comb
        Dim r As Int32 = 0
        r = rnd_int(0, 5)
        Select Case r
            Case 0
                Return read_case(fc, d)
            Case 1
                Return append_case(fc, d)
            Case 2
                Return delete_case(fc, d)
            Case 3
                Return seek_case(fc, d)
            Case 4
                Return sizeof_case(fc, d)
            Case Else
                assert(False)
                Return Nothing
        End Select
    End Function

    Private Shared Function cases(ByVal fc As free_cluster, ByVal d As map(Of Int64, Byte())) As event_comb
        assert(Not fc Is Nothing)
        Dim ec As event_comb = Nothing
        Dim i As Int64 = 0
        Return New event_comb(Function() As Boolean
                                  assert_true(ec Is Nothing OrElse ec.end_result())
                                  If i < round_count Then
                                      i += 1
                                      ec = rnd_case(fc, d)
                                      Return waitfor(ec)
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Overrides Function create() As event_comb
        Dim vd As virtdisk = Nothing
        Dim fc As pointer(Of free_cluster) = Nothing
        Dim ec As event_comb = Nothing
        Dim d As map(Of Int64, Byte()) = Nothing
        Dim round As Int32 = 0
        Return New event_comb(Function() As Boolean
                                  vd = virtdisk.memory_virtdisk()
                                  fc = New pointer(Of free_cluster)()
                                  d = New map(Of Int64, Byte())()
                                  round = reopen_round_count
                                  Return goto_next()
                              End Function,
                              Function() As Boolean
                                  ec = free_cluster.ctor(fc, vd)
                                  round -= 1
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) AndAlso
                                     assert_not_nothing(+fc) Then
                                      ec = cases(+fc, d)
                                      Return waitfor(ec) AndAlso
                                             If(round = 0, goto_end(), goto_prev())
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function
End Class
