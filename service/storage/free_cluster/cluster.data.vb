
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock

Partial Public Class cluster
    ' TODO: offset should be uint64.
    Public Function read(ByVal buff() As Byte, Optional ByVal offset As Int32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If free() OrElse empty() Then
                                      Return goto_end()
                                  Else
                                      ec = virtdisk().read(data_disk_offset(), used_bytes(), buff, offset)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read(ByVal r As pointer(Of Byte())) As event_comb
        Dim ec As event_comb = Nothing
        Dim buff() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim buff(used_bytes() - 1)
                                  ec = read(buff)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function delete() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  _used = FREE_USED
                                  _next_id = INVALID_ID
                                  _prev_id = INVALID_ID
                                  assert(free())
                                  ec = write_structure()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function mark_free_as_empty() As Boolean
        If free() Then
            _used = 0
            assert(Not free() AndAlso empty())
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function chain(ByVal h As cluster, ByVal others() As cluster) As event_comb
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If h Is Nothing OrElse isemptyarray(others) Then
                                      Return False
                                  Else
                                      For i As Int32 = 0 To array_size(others) - 1
                                          If others(i) Is Nothing Then
                                              Return False
                                          End If
                                      Next
                                  End If
                                  If h.has_next_id() Then
                                      Return False
                                  End If
                                  For i As Int32 = 0 To array_size(others) - 2
                                      If others(i).has_prev_id() OrElse others(i).has_next_id() Then
                                          Return False
                                      End If
                                      If (If(i = 0, h, others(i - 1))).id() = others(i).id() Then
                                          Return False
                                      End If
                                  Next
                                  If others(array_size(others) - 1).has_prev_id() Then
                                      Return False
                                  End If

                                  ReDim ecs(array_size(others))
                                  h.mark_free_as_empty()
                                  For i As Int32 = 0 To array_size(others) - 1
                                      Dim l As cluster = Nothing
                                      l = If(i = 0, h, others(i - 1))
                                      others(i)._prev_id = l.id()
                                      l._next_id = others(i).id()
                                      others(i).mark_free_as_empty()
                                      ecs(i) = l.write_structure()
                                  Next
                                  ecs(array_size(others)) = others(array_size(others) - 1).write_structure()
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ecs.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function append(ByVal buff() As Byte,
                           ByVal offset As Int64,
                           ByVal count As Int64) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If offset < 0 OrElse
                                     count < 0 OrElse
                                     array_size(buff) < offset + count OrElse
                                     remain_bytes() < count Then
                                      Return False
                                  ElseIf count = 0 Then
                                      Return goto_end()
                                  Else
                                      ec = virtdisk().write(remain_data_disk_offset(),
                                                            count,
                                                            buff,
                                                            offset)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      mark_free_as_empty()
                                      _used += count
                                      ec = write_structure()
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

    Public Function append(ByVal buff() As Byte,
                           Optional ByVal offset As Int64 = 0) As event_comb
        Return append(buff, offset, array_size(buff))
    End Function

    Private Function enlarge_virtdisk() As event_comb
        Return virtdisk().fill(end_disk_offset())
    End Function
End Class
