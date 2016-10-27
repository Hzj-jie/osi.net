
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Module _complete_io
    Private Function copy_once(ByVal r As Func(Of Byte(), 
                                                  UInt32, 
                                                  UInt32, 
                                                  pointer(Of UInt32), 
                                                  event_comb),
                               ByVal w As Func(Of Byte(), 
                                                  UInt32, 
                                                  UInt32, 
                                                  event_comb),
                               ByVal buff() As Byte,
                               ByVal max_copy As UInt32,
                               ByVal p As pointer(Of UInt32)) As event_comb
        assert(Not r Is Nothing)
        assert(Not w Is Nothing)
        assert(array_size(buff) >= max_copy)
        assert(Not p Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p.set(uint32_0)
                                  ec = r(buff, uint32_0, max_copy, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+p) = uint32_0 Then
                                          Return goto_end()
                                      Else
                                          ec = w(buff, uint32_0, +p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function prepare_buff(ByRef buff_size As UInt32, Optional ByVal count As UInt32 = max_uint32) As Byte()
        assert(count > 0)
        If buff_size = 0 Then
            buff_size = constants.default_io_buff_size
        End If
        If buff_size > count Then
            buff_size = count
        End If
        Dim r() As Byte = Nothing
        ReDim r(buff_size - 1)
        Return r
    End Function

    Public Function until_pending(ByVal r As Func(Of Byte(), 
                                                     UInt32, 
                                                     UInt32, 
                                                     pointer(Of UInt32), 
                                                     event_comb),
                                  ByVal w As Func(Of Byte(), 
                                                     UInt32, 
                                                     UInt32, 
                                                     event_comb),
                                  Optional ByVal buff_size As UInt32 = 0,
                                  Optional ByVal result As pointer(Of UInt64) = Nothing) As event_comb
        Dim buff() As Byte = Nothing
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of UInt32) = Nothing
        Return New event_comb(Function() As Boolean
                                  buff = prepare_buff(buff_size)
                                  p = New pointer(Of UInt32)()
                                  ec = event_comb.while(Function() As event_comb
                                                            Return copy_once(r, w, buff, buff_size, p)
                                                        End Function,
                                                        Function(last_ec As event_comb,
                                                                 ByRef break_error As Boolean) As Boolean
                                                            assert(Not last_ec Is Nothing)
                                                            break_error = (Not last_ec.end_result())
                                                            If (+p) > 0 Then
                                                                If Not result Is Nothing Then
                                                                    eva(result, (+result) + (+p))
                                                                End If
                                                                Return True
                                                            Else
                                                                Return False
                                                            End If
                                                        End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function count_condition(ByVal last_ec As event_comb,
                                     ByRef break_error As Boolean,
                                     ByRef count As UInt64,
                                     ByVal p As pointer(Of UInt32),
                                     ByVal pending_counter As pending_io_punishment) As Boolean
        assert(Not pending_counter Is Nothing)
        If last_ec Is Nothing Then
            assert(p Is Nothing OrElse (+p) = 0)
            break_error = False
        ElseIf last_ec.end_result() Then
            break_error = False
            Dim pending_time_ms As Int64 = 0
            If pending_counter.record((+p) > 0, pending_time_ms) Then
                waitfor(pending_time_ms)
            Else
                assert((+p) > 0)
                assert(count >= (+p))
                count -= (+p)
            End If
        Else
            break_error = True
        End If
        Return count > 0
    End Function

    Public Function complete_io(ByVal r As Func(Of Byte(), 
                                                   UInt32, 
                                                   UInt32, 
                                                   pointer(Of UInt32), 
                                                   event_comb),
                                ByVal w As Func(Of Byte(), 
                                                   UInt32, 
                                                   UInt32, 
                                                   event_comb),
                                ByVal count As UInt64,
                                Optional ByVal buff_size As UInt32 = 0) As event_comb
        Dim ec As event_comb = Nothing
        Dim buff() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  If count = 0 Then
                                      Return goto_end()
                                  Else
                                      buff = prepare_buff(buff_size, count)
                                      Dim p As pointer(Of UInt32) = Nothing
                                      p = New pointer(Of UInt32)()
                                      Dim pending_counter As pending_io_punishment = Nothing
                                      pending_counter = New pending_io_punishment()
                                      ec = event_comb.while(
                                               Function(last_ec As event_comb,
                                                        ByRef break_error As Boolean) As Boolean
                                                   Return count_condition(last_ec,
                                                                          break_error,
                                                                          count,
                                                                          p,
                                                                          pending_counter)
                                               End Function,
                                               Function() As event_comb
                                                   Return copy_once(r, w, buff, min(buff_size, count), p)
                                               End Function)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function complete_io(ByVal r As Func(Of pointer(Of Byte()), 
                                                   event_comb),
                                ByVal w As Func(Of Byte(), 
                                                   event_comb)) _
                               As event_comb
        assert(Not r Is Nothing)
        assert(Not w Is Nothing)
        Dim p As pointer(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  p.renew()
                                  ec = r(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = w(+p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_begin()
                              End Function)
    End Function

    Public Function complete_io(ByVal buff() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByVal partial_io As Func(Of Byte(), 
                                                            UInt32, 
                                                            UInt32, 
                                                            pointer(Of UInt32), 
                                                            event_comb)) _
                               As event_comb
        assert(Not partial_io Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If array_size(buff) < offset + count Then
                                      Return False
                                  ElseIf count = 0 Then
                                      Return goto_end()
                                  Else
                                      Dim p As pointer(Of UInt32) = Nothing
                                      p = New pointer(Of UInt32)()
                                      Dim pending_counter As pending_io_punishment = Nothing
                                      pending_counter = New pending_io_punishment()
                                      ec = event_comb.while(
                                               Function(last_ec As event_comb,
                                                        ByRef break_error As Boolean) As Boolean
                                                   Return count_condition(last_ec,
                                                                          break_error,
                                                                          count,
                                                                          p,
                                                                          pending_counter)
                                               End Function,
                                               Function() As event_comb
                                                   Return New event_comb(
                                                              Function() As Boolean
                                                                  assert(array_size(buff) >= offset + count)
                                                                  p.renew()
                                                                  ec = partial_io(buff, offset, count, p)
                                                                  Return waitfor(ec) AndAlso
                                                                         goto_next()
                                                              End Function,
                                                              Function() As Boolean
                                                                  Return ec.end_result() AndAlso
                                                                         eva(offset, offset + (+p)) AndAlso
                                                                         goto_end()
                                                              End Function)
                                               End Function)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
