
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft.VisualBasic
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.storage

Public Class fast_istrkeyvt_case3
    Inherits istrkeyvt_case3(Of _2, _2, _8, _1024, _16383)
End Class

Public Class default_istrkeyvt_case3
    Inherits istrkeyvt_case3(Of _2, _2, _8, _1024, _max_int16)
End Class

'make sure the data will not lose, accept if some deleted data are still existing in the system
Public Class istrkeyvt_case3(Of _KEY_LENGTH_LOW As _int64,
                                _KEY_LENGTH_UP As _int64,
                                _BYTES_LENGTH_LOW As _int64,
                                _BYTES_LENGTH_UP As _int64,
                                _ROUND_BASE As _int64)
    Implements iistrkeyvt_case

    Private Shared ReadOnly round_count As UInt32
    Private Shared ReadOnly rnd As istrkeyvt_random_data(Of _KEY_LENGTH_LOW, 
                                                            _KEY_LENGTH_UP, 
                                                            _BYTES_LENGTH_LOW, 
                                                            _BYTES_LENGTH_UP)
    Private ReadOnly m As map(Of String, Int64)

    Shared Sub New()
        Dim rc As Int64 = 0
        rc = +(alloc(Of _ROUND_BASE)()) * If(isdebugbuild(), 1, 8)
        assert(rc >= 0 AndAlso rc <= max_uint32)
        round_count = CUInt(rc)
    End Sub

    Public Sub New()
        m = New map(Of String, Int64)()
    End Sub

    Private Shared Function rand_key() As String
        Return rnd.rand_key()
    End Function

    Private Shared Function rand_bytes() As Byte()
        Return rnd.rand_bytes()
    End Function

    Private Function read_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v As ref(Of Byte()) = Nothing
        Dim t As ref(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = New ref(Of Byte())()
                                  t = New ref(Of Int64)()
                                  ec = keyvt.read(k, v, t)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If m.find(k) <> m.end() Then
                                      assertion.more_or_equal(Of Int64)(array_size(+v), m(k))
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function append_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes()
                                  r = New ref(Of Boolean)()
                                  ec = keyvt.append(k, v, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.is_true(+r)
                                  m(k) += array_size(v)
                                  Return goto_end()
                              End Function)
    End Function

    Private Function delete_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim p As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  p = New ref(Of Boolean)()
                                  ec = keyvt.delete(k, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If m.erase(k) Then
                                      assertion.is_true(+p)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function seek_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim p As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  p = New ref(Of Boolean)()
                                  ec = keyvt.seek(k, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If m.find(k) <> m.end() Then
                                      assertion.is_true(+p)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function list_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim r As ref(Of vector(Of String)) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of vector(Of String))()
                                  ec = keyvt.list(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If assertion.is_not_null(+r) Then
                                      assertion.more_or_equal((+r).size(), m.size())
                                      Dim m2 As [set](Of String) = Nothing
                                      m2 = New [set](Of String)()
                                      Dim i As UInt32 = 0
                                      While i < (+r).size()
                                          m2.insert((+r)(i))
                                          i += uint32_1
                                      End While
                                      Dim it As map(Of String, Int64).iterator = Nothing
                                      it = m.begin()
                                      While it <> m.end()
                                          assertion.is_true(m2.find((+it).first) <> m2.end())
                                          it += 1
                                      End While
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function modify_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim t As Int64 = 0
        Dim r As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes()
                                  t = Now().to_timestamp()
                                  r = New ref(Of Boolean)()
                                  ec = keyvt.modify(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.is_true(+r)
                                  m(k) = array_size(v)
                                  Return goto_end()
                              End Function)
    End Function

    Private Function sizeof_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v As ref(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = New ref(Of Int64)()
                                  ec = keyvt.sizeof(k, v)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If (+v) = npos Then
                                      assertion.is_true(m.find(k) = m.end())
                                  Else
                                      If m.find(k) <> m.end() Then
                                          assertion.more_or_equal(+v, m(k))
                                      End If
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function full_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = keyvt.full(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  'assertion.is_false(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Function empty_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = keyvt.empty(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If (+r) Then
                                      assertion.is_true(m.empty())
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function retire_case(ByVal keyvt As istrkeyvt) As event_comb
        Const p As Double = 0
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
                                  assertion.is_true(ec.end_result())
                                  m.clear()
                                  Return goto_end()
                              End Function)
    End Function

    Private Function capacity_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Int64)()
                                  ec = keyvt.capacity(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.more_or_equal(+r, 0)
                                  Return goto_end()
                              End Function)
    End Function

    Private Function valuesize_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Int64)()
                                  ec = keyvt.valuesize(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.more_or_equal(+r, 0)
                                  Return goto_end()
                              End Function)
    End Function

    Private Function keycount_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Int64) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Int64)()
                                  ec = keyvt.keycount(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.more_or_equal(+r, m.size())
                                  Return goto_end()
                              End Function)
    End Function

    Private Function heartbeat_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = keyvt.heartbeat()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  Return goto_end()
                              End Function)
    End Function

    Private Function stop_case(ByVal keyvt As istrkeyvt) As event_comb
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
                                  assertion.is_true(ec.end_result())
                                  Return goto_end()
                              End Function)
#Else
        Return event_comb.succeeded()
#End If
    End Function

    Private Function unique_write_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        Dim t As Int64 = 0
        Dim r As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  k = rand_key()
                                  v = rand_bytes()
                                  t = Now().to_timestamp()
                                  r = New ref(Of Boolean)()
                                  ec = keyvt.unique_write(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  If (+r) Then
                                      If m.find(k) = m.end() OrElse
                                         m(k) > array_size(v) Then
                                          m(k) = array_size(v)
                                      End If
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function random_case(ByVal keyvt As istrkeyvt) As event_comb
        Dim r As Int32 = 0
        r = rnd_int(0, 16)
        'average_key_rate = average_key_count / all_key_count
        'all_key_count = key_char_sample ^ key_average_length
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

    Public Function create(ByVal i As istrkeyvt) As event_comb Implements iistrkeyvt_case.create
        Return random_case(i).repeat(round_count)
    End Function

    Public Function reserved_processors() As Int16 Implements iistrkeyvt_case.reserved_processors
        Return 1
    End Function
End Class
