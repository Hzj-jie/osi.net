
Imports System.DateTime
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.template
Imports osi.root.constants
Imports osi.service.storage

Public Class default_istrkeyvt_case2
    Inherits istrkeyvt_case2(Of _8, _4096, _1024, _max_uint16, _131071)
End Class

Public Class short_key_istrkeyvt_case2
    Inherits istrkeyvt_case2(Of _8, _100, _1024, _max_uint16, _131071)
End Class

Public Class fast_istrkeyvt_case2
    Inherits istrkeyvt_case2(Of _4, _8, _128, _1024, _10000)
End Class

Public Class istrkeyvt_case2(Of _KEY_LENGTH_LOW As _int64,
                                _KEY_LENGTH_UP As _int64,
                                _BYTES_LENGTH_LOW As _int64,
                                _BYTES_LENGTH_UP As _int64,
                                _ROUND_BASE As _int64)
    Implements iistrkeyvt_case

    Private Shared ReadOnly rand_data As istrkeyvt_random_data(Of _KEY_LENGTH_LOW, 
                                                                  _KEY_LENGTH_UP, 
                                                                  _BYTES_LENGTH_LOW, 
                                                                  _BYTES_LENGTH_UP)
    Private Shared ReadOnly round_count As Int64

    Shared Sub New()
        round_count = +(alloc(Of _ROUND_BASE)()) * If(isdebugbuild(), 1, 8)
    End Sub

    Private Shared Function rand_key() As String
        Return rand_data.fake_rand_key()
    End Function

    Private Shared Function is_rand_key(ByVal k As String) As Boolean
        Return rand_data.is_fake_rand_key(k)
    End Function

    Private Shared Function rand_bytes(ByVal key As String) As Byte()
        Return rand_data.fake_rand_bytes(key)
    End Function

    Private Shared Function is_rand_bytes(ByVal key As String, ByVal b() As Byte) As Boolean
        Return rand_data.is_fake_rand_bytes(key, b)
    End Function

    Private Shared Function rand_timestamp(ByVal key As String) As Int64
        Return rand_data.fake_rand_timestamp(key)
    End Function

    Private Shared Function is_rand_timestamp(ByVal key As String, ByVal ts As Int64) As Boolean
        Return rand_data.is_fake_rand_timestamp(key, ts)
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
                                  If Not (+v) Is Nothing Then
                                      assert_true(is_rand_bytes(k, +v))
                                      assert_true(is_rand_timestamp(k, +t))
                                  End If
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
                                  r = New pointer(Of vector(Of String))()
                                  ec = keyvt.list(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  If assert_not_nothing(+r) Then
                                      For i As Int32 = 0 To (+r).size() - 1
                                          assert_true(is_rand_key((+r)(i)))
                                      Next
                                  End If
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
                                  'assert_false(+r)
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
        Const p As Double = 0.00002
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

    Private Shared Function cases(ByVal keyvt As istrkeyvt) As event_comb
        Dim round As Int64 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If round < round_count Then
                                      If round > 0 Then
                                          assert(Not ec Is Nothing)
                                          assert_true(ec.end_result())
                                      End If
                                      round += 1
                                      ec = rnd_case(keyvt)
                                      Return waitfor(ec)
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Function preserved_processors() As Int16 Implements iistrkeyvt_case.preserved_processors
        Return 1
    End Function

    Public Function create(ByVal keyvt As istrkeyvt) As event_comb Implements iistrkeyvt_case.create
        Dim ec As event_comb = Nothing
        assert(Not keyvt Is Nothing)
        Return New event_comb(Function() As Boolean
                                  ec = cases(keyvt)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return assert_true(ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
