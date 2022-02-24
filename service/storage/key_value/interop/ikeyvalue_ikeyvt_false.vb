
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.delegates

'the bridge between ikeyvt<_false> and ikeyvalue
Friend Class ikeyvalue_ikeyvt_false
    Implements ikeyvt(Of _false)

    Private Shared ReadOnly value_prefix As Byte
    Private Shared ReadOnly timestamp_prefix As Byte

    Shared Sub New()
        Dim b() As Byte = Nothing
        b = str_bytes("v")
        assert(array_size(b) = 1)
        value_prefix = b(0)
        b = str_bytes("t")
        assert(array_size(b) = 1)
        timestamp_prefix = b(0)
        assert(value_prefix <> timestamp_prefix)
    End Sub

    Private Shared Function value_key(ByVal key() As Byte) As Byte()
        Return merge_key(key, value_prefix)
    End Function

    Private Shared Function timestamp_key(ByVal key() As Byte) As Byte()
        Return merge_key(key, timestamp_prefix)
    End Function

    Private Shared Function is_timestamp_key(ByVal key() As Byte, ByRef original() As Byte) As Boolean
        Return is_merged_key(key, timestamp_prefix, original)
    End Function

    Private ReadOnly s As ikeyvalue = Nothing

    Public Sub New(ByVal s As ikeyvalue)
        assert(Not s Is Nothing)
        Me.s = s
    End Sub

    Private Function append_or_modify(ByVal key() As Byte,
                                      ByVal value() As Byte,
                                      ByVal ts As Int64,
                                      ByVal result As ref(Of Boolean),
                                      ByVal op As Func(Of Byte(),
                                                          Byte(),
                                                          ref(Of Boolean),
                                                          event_comb)) As event_comb
        assert(Not op Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = op(value_key(key), value, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Then
                                          ec = s.modify(timestamp_key(key), ts.to_bytes(), result)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  'the size of timestamp has not changed,
                                  'so there is no possibility for the underline ikeyvalue to
                                  'complain the storage has been full
                                  'but if so, just return false
                                  If ec.end_result() Then
                                      If +result Then
                                          Return goto_end()
                                      Else
                                          Return False
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).append
        Return append_or_modify(key, value, ts, result, Function(a, b, c) s.append(a, b, c))
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _false).capacity
        Return s.capacity(result)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).delete
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = s.delete(timestamp_key(key), result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Then
                                          ec = s.delete(value_key(key), result)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      'if the value_key is not existing, just consider as success
                                      Return eva(result, True) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).empty
        Return s.empty(result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).full
        Return s.full(result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt(Of _false).heartbeat
        Return s.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _false).keycount
        Return half_keycount(AddressOf s.keycount, result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvt(Of _false).list
        Return select_list(AddressOf s.list, AddressOf is_timestamp_key, result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).modify
        Return append_or_modify(key, value, ts, result, AddressOf s.modify)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements ikeyvt(Of _false).read
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = s.read(timestamp_key(key), result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Is Nothing Then
                                          'no found
                                          Return goto_end()
                                      Else
                                          Dim o As Int64 = 0
                                          If to_timestamp(+result, o) Then
                                              eva(ts, o)
                                              ec = s.read(value_key(key), result)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          Else
                                              Return False
                                          End If
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

    Public Function retire() As event_comb Implements ikeyvt(Of _false).retire
        Return s.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _false).seek
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = s.seek(timestamp_key(key), result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Then
                                          ec = s.seek(value_key(key), result)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
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

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _false).sizeof
        Return s.sizeof(value_key(key), result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt(Of _false).stop
        Return s.stop()
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb _
                                Implements ikeyvt(Of _false).unique_write
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = seek(key, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Then
                                          Return eva(result, False) AndAlso
                                                 goto_end()
                                      Else
                                          ec = modify(key, value, ts, result)
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

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _false).valuesize
        Return s.valuesize(result)
    End Function
End Class
