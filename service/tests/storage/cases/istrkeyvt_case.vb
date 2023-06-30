
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.storage

Public Interface iistrkeyvt_case
    Function create(ByVal i As istrkeyvt) As event_comb
    Function reserved_processors() As Int16
End Interface

'MustInherit for utt
Public MustInherit Class istrkeyvt_case
    Inherits event_comb_case

    Private ReadOnly i As iistrkeyvt_case

    Protected Sub New(ByVal i As iistrkeyvt_case)
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Protected Sub New()
        Me.New(New default_istrkeyvt_case())
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return i.reserved_processors()
    End Function

    Protected Overridable Function create_istrkeyvt() As istrkeyvt
        Return Nothing
    End Function

    Protected Overridable Function create_istrkeyvt(ByVal p As ref(Of istrkeyvt)) As event_comb
        Return New event_comb(Function() As Boolean
                                  Return assert(Not p Is Nothing) AndAlso
                                         eva(p, create_istrkeyvt()) AndAlso
                                         assertion.is_not_null(+p) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Overridable Function clean_up() As event_comb
        Return event_comb.succeeded()
    End Function

    Protected Overridable Function clean_up(ByVal i As istrkeyvt) As event_comb
        Return clean_up()
    End Function

    Protected Overridable Function fulfill_precondition() As Boolean
        Return True
    End Function

    Public NotOverridable Overrides Function create() As event_comb
        Dim keyvt As ref(Of istrkeyvt) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If fulfill_precondition() Then
                                      keyvt = New ref(Of istrkeyvt)()
                                      ec = create_istrkeyvt(keyvt)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  raise_error(error_type.warning,
                                              name,
                                              " precondition does not fulfill, ignore")
                                  Return goto_end()
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) AndAlso
                                     assertion.is_not_null(+keyvt) Then
                                      ec = (+keyvt).retire()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) Then
                                      ec = i.create(+keyvt)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) Then
                                      ec = (+keyvt).retire()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) Then
                                      ec = (+keyvt).stop()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) Then
                                      ec = clean_up(+keyvt)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  Return assertion.is_true(ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class

Public Class default_istrkeyvt_case(Of _LOW_RES_TIMESTAMP As _boolean)
    Inherits istrkeyvt_case(Of _2, _2, _8, _1024, _max_int16, _LOW_RES_TIMESTAMP)
End Class

Public NotInheritable Class default_istrkeyvt_case
    Inherits default_istrkeyvt_case(Of _false)
End Class

Public Class istrkeyvt_case(Of _KEY_LENGTH_LOW As _int64,
                               _KEY_LENGTH_UP As _int64,
                               _BYTES_LENGTH_LOW As _int64,
                               _BYTES_LENGTH_UP As _int64,
                               _ROUND_BASE As _int64,
                               _LOW_RES_TIMESTAMP As _boolean)
    Implements iistrkeyvt_case

    Private Shared ReadOnly rand_data As istrkeyvt_random_data(Of _KEY_LENGTH_LOW,
                                                                  _KEY_LENGTH_UP,
                                                                  _BYTES_LENGTH_LOW,
                                                                  _BYTES_LENGTH_UP)
    Private Shared ReadOnly round_count As Int64
    ' Some systems, like vmware, does not support high-res file timestamp. So the timestamp provided by implementations,
    ' say, file_key, is low-res. The assertion needs to be updated to avoid unnecessary failures.
    Private Shared ReadOnly low_res_timestamp As Boolean

    Shared Sub New()
        round_count = +(alloc(Of _ROUND_BASE)()) * If(isdebugbuild(), 1, 8)
        low_res_timestamp = +(alloc(Of _LOW_RES_TIMESTAMP)())
    End Sub

    Private Shared Function rand_key() As String
        Return rand_data.rand_key()
    End Function

    Private Shared Function rand_bytes() As Byte()
        Return rand_data.rand_bytes()
    End Function

    Private Shared Function read_case(ByVal keyvt As istrkeyvt,
                                      ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  If d.find(k) = d.end() Then
                                      assertion.is_null(+v)
                                      assertion.equal(+t, npos)
                                  Else
                                      assertion.equal(memcmp((+v), d(k).first), 0)
                                      If low_res_timestamp Then
                                          assertion.less_or_equal(Math.Abs(((+t) >> 26) - (d(k).second >> 26)), 1)
                                      Else
                                          assertion.equal((+t), d(k).second)
                                      End If
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function append_case(ByVal keyvt As istrkeyvt,
                                        ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  ec = keyvt.append(k, v, t, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.is_true(+r)
                                  If d.find(k) = d.end() Then
                                      d(k) = pair.emplace_of(v, t)
                                  Else
                                      Dim v2() As Byte = Nothing
                                      Dim ov() As Byte = Nothing
                                      ov = d(k).first
                                      ReDim v2(array_size_i(ov) + array_size_i(v) - 1)
                                      arrays.copy(v2, ov)
                                      arrays.copy(v2, array_size(ov), v)
                                      d(k) = pair.emplace_of(v2, t)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function delete_case(ByVal keyvt As istrkeyvt,
                                        ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.equal(+p, d.erase(k))
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function seek_case(ByVal keyvt As istrkeyvt,
                                      ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.equal(+p, d.find(k) <> d.end())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function list_case(ByVal keyvt As istrkeyvt,
                                      ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                      assertion.equal((+r).size(), d.size())
                                      Dim i As UInt32 = 0
                                      While i < (+r).size()
                                          assertion.is_true(d.find((+r)(i)) <> d.end())
                                          i += uint32_1
                                      End While
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function modify_case(ByVal keyvt As istrkeyvt,
                                        ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  d(k) = pair.of(v, t)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function sizeof_case(ByVal keyvt As istrkeyvt,
                                        ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                      assertion.is_true(d.find(k) = d.end())
                                  Else
                                      If assertion.is_true(d.find(k) <> d.end()) Then
                                          assertion.equal(+v, array_size(d(k).first))
                                      End If
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function full_case(ByVal keyvt As istrkeyvt,
                                      ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.is_false(+r)
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function empty_case(ByVal keyvt As istrkeyvt,
                                       ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.equal(+r, d.empty())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function retire_case(ByVal keyvt As istrkeyvt,
                                        ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
        Const p As Double = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If rnd_double(0, 1) >= p Then
                                      Return goto_end()
                                  End If
                                  ec = keyvt.retire()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  d.clear()
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function capacity_case(ByVal keyvt As istrkeyvt,
                                          ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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

    Private Shared Function valuesize_case(ByVal keyvt As istrkeyvt,
                                           ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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

    Private Shared Function keycount_case(ByVal keyvt As istrkeyvt,
                                          ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.equal(+r, d.size())
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function heartbeat_case(ByVal keyvt As istrkeyvt,
                                           ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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

    Private Shared Function stop_case(ByVal keyvt As istrkeyvt,
                                      ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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

    Private Shared Function unique_write_case(ByVal keyvt As istrkeyvt,
                                              ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
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
                                  assertion.equal(+r, d.find(k) = d.end())
                                  If +r Then
                                      d(k) = pair.of(v, t)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function rnd_case(ByVal keyvt As istrkeyvt,
                                     ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
        Dim r As Int32 = 0
        r = rnd_int(0, 16)
        'average_key_rate = average_key_count / all_key_count
        'all_key_count = 16 ^ 3
        Select Case r
            Case 0
                Return read_case(keyvt, d)              '0
            Case 1
                Return append_case(keyvt, d)            '+1 * (1 - average_key_rate)
            Case 2
                Return delete_case(keyvt, d)            '-1 * average_key_rate
            Case 3
                Return seek_case(keyvt, d)              '0
            Case 4
                Return list_case(keyvt, d)              '0
            Case 5
                Return modify_case(keyvt, d)            '+1 * (1 - average_key_rate)
            Case 6
                Return sizeof_case(keyvt, d)            '0
            Case 7
                Return full_case(keyvt, d)              '0
            Case 8
                Return empty_case(keyvt, d)             '0
            Case 9
                Return retire_case(keyvt, d)            '-average_key_count * p
            Case 10
                Return capacity_case(keyvt, d)          '0
            Case 11
                Return valuesize_case(keyvt, d)         '0
            Case 12
                Return keycount_case(keyvt, d)          '0
            Case 13
                Return heartbeat_case(keyvt, d)         '0
            Case 14
                Return stop_case(keyvt, d)              '0
            Case 15
                Return unique_write_case(keyvt, d)      '+1 * (1 - average_key_rate)
            Case Else
                assert(False)
                Return Nothing
        End Select
    End Function

    Private Shared Function cases(ByVal keyvt As istrkeyvt,
                                  ByVal d As map(Of String, pair(Of Byte(), Int64))) As event_comb
        Dim round As Int64 = 0
        Dim failures As Int64 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If round >= round_count Then
                                      Return goto_end()
                                  End If
                                  If round > 0 Then
                                      assert(Not ec Is Nothing)
                                      assertion.is_true(ec.end_result())
                                      If Not ec.end_result() Then
                                          failures += 1
                                          If failures > 20 Then
                                              Return False
                                          End If
                                      End If
                                  End If
                                  round += 1
                                  ec = rnd_case(keyvt, d)
                                  Return waitfor(ec)
                              End Function)
    End Function

    Public Function reserved_processors() As Int16 Implements iistrkeyvt_case.reserved_processors
        Return 1
    End Function

    Public Function create(ByVal keyvt As istrkeyvt) As event_comb Implements iistrkeyvt_case.create
        Dim ec As event_comb = Nothing
        assert(Not keyvt Is Nothing)
        Return New event_comb(Function() As Boolean
                                  ec = cases(keyvt, New map(Of String, pair(Of Byte(), Int64))())
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return assertion.is_true(ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
