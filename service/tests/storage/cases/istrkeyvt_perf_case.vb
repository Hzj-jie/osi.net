
Imports System.DateTime
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.lock
Imports osi.service.storage

Public Class default_istrkeyvt_perf_case
    Inherits istrkeyvt_perf_case(Of _4, _2, _2, _8, _1024, _10000, _true)
End Class

Public Class istrkeyvt_perf_case(Of _PARALLEL As _int64,
                                    _KEY_LENGTH_LOW As _int64,
                                    _KEY_LENGTH_UP As _int64,
                                    _BYTES_LENGTH_LOW As _int64,
                                    _BYTES_LENGTH_UP As _int64,
                                    _ROUND_BASE As _int64,
                                    _ALLOW_LIST As _boolean)
    Implements iistrkeyvt_case

    Private Shared ReadOnly parallel As Int32
    Private Shared ReadOnly key_length_low As UInt32
    Private Shared ReadOnly key_length_up As UInt32
    Private Shared ReadOnly bytes_length_low As UInt32
    Private Shared ReadOnly bytes_length_up As UInt32
    Private Shared ReadOnly timestamp_low As Int64
    Private Shared ReadOnly timestamp_up As Int64
    Private Shared ReadOnly round_count As Int64
    Private Shared ReadOnly allow_list As Boolean
    Private ReadOnly count As atomic_int
    Private report_ec As event_comb

    Shared Sub New()
        parallel = +(alloc(Of _PARALLEL)())
        key_length_low = +(alloc(Of _KEY_LENGTH_LOW)())
        key_length_up = +(alloc(Of _KEY_LENGTH_UP)()) + 1
        bytes_length_low = +(alloc(Of _BYTES_LENGTH_LOW)())
        bytes_length_up = +(alloc(Of _BYTES_LENGTH_UP)()) + 1
        timestamp_low = (Now().to_timestamp() + rnd_int64(-2048, -1024 + 1))
        timestamp_up = (Now().to_timestamp() + rnd_int64(1024, 2048 + 1)) + 1
        round_count = +(alloc(Of _ROUND_BASE)()) * If(isdebugbuild(), 1, 8)
        allow_list = +(alloc(Of _ALLOW_LIST)())
    End Sub

    Public Sub New()
        If round_count < 0 Then
            count = New atomic_int()
        End If
    End Sub

    Private Sub start_report()
        If round_count < 0 Then
            Dim start_ms As Int64 = 0
            report_ec = New event_comb(Function() As Boolean
                                           start_ms = Now().milliseconds()
                                           Return goto_next()
                                       End Function,
                                       Function() As Boolean
                                           Return waitfor(seconds_to_milliseconds(1)) AndAlso
                                                  goto_next()
                                       End Function,
                                       Function() As Boolean
                                           Dim sec As Int64 = 0
                                           sec = milliseconds_to_seconds(Now().milliseconds() - start_ms)
                                           If sec = 0 Then
                                               sec = 1
                                           End If
                                           rewrite_console("total requests ",
                                                           +count,
                                                           ", qps ",
                                                           (+count) / sec)
                                           Return goto_prev()
                                       End Function)
            assert_begin(report_ec)
        End If
    End Sub

    Private Sub end_report()
        assert((report_ec Is Nothing) Xor (round_count < 0))
        If round_count < 0 Then
            report_ec.cancel()
        End If
    End Sub

    Private Shared Function rand_key() As String
        Return rndenchars(rnd_uint(key_length_low, key_length_up))
    End Function

    Private Shared Function rand_bytes(ByVal key As String) As Byte()
        Return next_bytes(rnd_uint(bytes_length_low, bytes_length_up))
    End Function

    Private Shared Function rand_timestamp(ByVal key As String) As Int64
        Return rnd_int64(timestamp_low, timestamp_low + timestamp_up)
    End Function

    Private Shared Function read_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v As pointer(Of Byte()) = Nothing
        Dim t As pointer(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = New pointer(Of Byte())()
                                  t = New pointer(Of Int64)()
                                  ec = keyvt.read(k, v, t)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function append_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim t As Int64 = 0
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes(k)
                                  t = rand_timestamp(k)
                                  r = New pointer(Of Boolean)()
                                  ec = keyvt.append(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_true(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function delete_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim p As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  p = New pointer(Of Boolean)()
                                  ec = keyvt.delete(k, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function seek_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim p As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  p = New pointer(Of Boolean)()
                                  ec = keyvt.seek(k, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function list_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim r As pointer(Of vector(Of String)) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If allow_list Then
                                      r = New pointer(Of vector(Of String))()
                                      ec = keyvt.list(r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_not_nothing(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function modify_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim t As Int64 = 0
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes(k)
                                  t = rand_timestamp(k)
                                  r = New pointer(Of Boolean)()
                                  ec = keyvt.modify(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_true(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function sizeof_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v As pointer(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = New pointer(Of Int64)()
                                  ec = keyvt.sizeof(k, v)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function full_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = keyvt.full(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_false(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function empty_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = keyvt.empty(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function retire_case(ByVal keyvt As istrkeyvt) As event_comb
        Const p As Double = 0.0001
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If rnd_double(0, 1) >= p Then
                                      Return goto_end()
                                  Else
                                      ec = keyvt.retire()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function capacity_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Int64)()
                                  ec = keyvt.capacity(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_more_or_equal(+r, 0)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function valuesize_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Int64)()
                                  ec = keyvt.valuesize(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_more_or_equal(+r, 0)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function keycount_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Int64)()
                                  ec = keyvt.keycount(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function heartbeat_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = keyvt.heartbeat()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function stop_case(ByVal keyvt As istrkeyvt) As event_comb
#Const accept_stop = False
#If accept_stop Then
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Return goto_end()
                                  ec = keyvt.stop()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
#Else
        Return event_comb.succeeded()
#End If
    End Function

    Private Shared Function unique_write_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim t As Int64 = 0
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes(k)
                                  t = rand_timestamp(k)
                                  r = New pointer(Of Boolean)()
                                  ec = keyvt.unique_write(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function rnd_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim r As Int32 = 0
        r = rnd_int(0, 16)
        'average_key_rate = average_key_count / all_key_count
        'all_key_count = 16 ^ 3
        Select Case r
            Case 0
                Return read_case(keyvt)              '0
            Case 1
                Return append_case(keyvt)            '+1 * (1 - average_key_rate)
            Case 2
                Return delete_case(keyvt)            '-1 * average_key_rate
            Case 3
                Return seek_case(keyvt)              '0
            Case 4
                Return list_case(keyvt)              '0
            Case 5
                Return modify_case(keyvt)            '+1 * (1 - average_key_rate)
            Case 6
                Return sizeof_case(keyvt)            '0
            Case 7
                Return full_case(keyvt)              '0
            Case 8
                Return empty_case(keyvt)             '0
            Case 9
                Return retire_case(keyvt)            '-average_key_count * p
            Case 10
                Return capacity_case(keyvt)          '0
            Case 11
                Return valuesize_case(keyvt)         '0
            Case 12
                Return keycount_case(keyvt)          '0
            Case 13
                Return heartbeat_case(keyvt)         '0
            Case 14
                Return stop_case(keyvt)              '0
            Case 15
                Return unique_write_case(keyvt)      '+1 * (1 - average_key_rate)
            Case Else
                assert(False)
                Return Nothing
        End Select
    End Function

    Private Function cases(ByVal keyvt As istrkeyvt) As event_comb
        Dim round As Int64 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If round_count < 0 OrElse round < round_count Then
                                      If round > 0 Then
                                          assert(Not ec Is Nothing)
                                          assert_true(ec.end_result())
                                      End If
                                      round += 1
                                      If round_count < 0 Then
                                          assert(Not count Is Nothing)
                                          count.increment()
                                      End If
                                      ec = rnd_case(keyvt)
                                      Return waitfor(ec)
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Function preserved_processors() As Int16 Implements iistrkeyvt_case.preserved_processors
        Return Environment.ProcessorCount()
    End Function

    Public Function create(ByVal keyvt As istrkeyvt) As event_comb Implements iistrkeyvt_case.create
        Dim ecs() As event_comb = Nothing
        assert(Not keyvt Is Nothing)
        Return New event_comb(Function() As Boolean
                                  start_report()
                                  ReDim ecs(parallel - 1)
                                  For i As Int32 = 0 To parallel - 1
                                      ecs(i) = cases(keyvt)
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  end_report()
                                  Return assert_true(ecs.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
