
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.constants

Public Class ikeyvt2_ikeyvt_false(Of SEEK_RESULT)
    Implements ikeyvt(Of _false)

    Private ReadOnly impl As ikeyvt2(Of SEEK_RESULT)

    Public Sub New(ByVal impl As ikeyvt2(Of SEEK_RESULT))
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Private Function if_existing(ByVal key() As Byte,
                                 ByVal existing_do As Func(Of SEEK_RESULT, event_comb),
                                 ByVal not_existing_do As Func(Of event_comb)) As event_comb
        assert(Not existing_do Is Nothing)
        assert(Not not_existing_do Is Nothing)
        Dim ec As event_comb = Nothing
        Dim sr As pointer(Of SEEK_RESULT) = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  sr = New pointer(Of SEEK_RESULT)()
                                  r = New pointer(Of Boolean)()
                                  ec = impl.seek(key, sr, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+r) Then
                                          ec = existing_do(+sr)
                                      Else
                                          ec = not_existing_do()
                                      End If
                                      Return If(ec Is Nothing,
                                                goto_end(),
                                                waitfor(ec) AndAlso goto_next())
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function delete_then_write(Of VT)(ByVal sr As SEEK_RESULT,
                                                     ByVal key() As Byte,
                                                     ByVal v As VT,
                                                     ByVal result As pointer(Of Boolean),
                                                     ByVal delete_existing As Func(Of SEEK_RESULT, 
                                                                                      pointer(Of Boolean), 
                                                                                      event_comb),
                                                     ByVal write_new As Func(Of Byte(), 
                                                                                VT, 
                                                                                pointer(Of Boolean), 
                                                                                event_comb)) As event_comb
        assert(Not delete_existing Is Nothing)
        assert(Not write_new Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New pointer(Of Boolean)()
                                  End If
                                  ec = delete_existing(sr, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso (+result) Then
                                      ec = write_new(key, v, result)
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

    Private Function delete_then_write(ByVal sr As SEEK_RESULT,
                                       ByVal key() As Byte,
                                       ByVal ts As Int64,
                                       ByVal result As pointer(Of Boolean)) As event_comb
        Return delete_then_write(sr,
                                 key,
                                 ts,
                                 result,
                                 AddressOf impl.delete_existing_timestamp,
                                 AddressOf impl.write_new_timestamp)
    End Function

    Private Function delete_then_write(ByVal sr As SEEK_RESULT,
                                       ByVal key() As Byte,
                                       ByVal value() As Byte,
                                       ByVal result As pointer(Of Boolean)) As event_comb
        Return delete_then_write(sr,
                                 key,
                                 value,
                                 result,
                                 AddressOf impl.delete_existing,
                                 AddressOf impl.write_new)
    End Function

    Private Function amuw_existing(ByVal sr As SEEK_RESULT,
                                   ByVal key() As Byte,
                                   ByVal value() As Byte,
                                   ByVal ts As Int64,
                                   ByVal result As pointer(Of Boolean),
                                   ByVal existing_do As Func(Of SEEK_RESULT,
                                                                Byte(),
                                                                Byte(), 
                                                                pointer(Of Boolean), 
                                                                event_comb)) As event_comb
        assert(Not existing_do Is Nothing)
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = existing_do(sr, key, value, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +r Then
                                          ec = delete_then_write(sr, key, ts, r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return eva(result, False) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, +r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function amuw_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByVal ts As Int64,
                              ByVal result As pointer(Of Boolean)) As event_comb
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = impl.write_new(key, value, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +r Then
                                          ec = impl.write_new_timestamp(key, ts, r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return eva(result, False) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, +r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function amuw(ByVal key() As Byte,
                          ByVal value() As Byte,
                          ByVal ts As Int64,
                          ByVal result As pointer(Of Boolean),
                          ByVal existing_do As Func(Of SEEK_RESULT,
                                                       Byte(),
                                                       Byte(), 
                                                       pointer(Of Boolean), 
                                                       event_comb)) As event_comb
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return amuw_existing(sr, key, value, ts, result, existing_do)
                           End Function,
                           Function() As event_comb
                               Return amuw_new(key, value, ts, result)
                           End Function)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).append
        Return amuw(key, value, ts, result, Function(sr, k, v, r) impl.append_existing(sr, v, r))
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).capacity
        Return impl.capacity(result)
    End Function

    Private Function delete_existing(ByVal sr As SEEK_RESULT,
                                     ByVal result As pointer(Of Boolean)) As event_comb
        Dim r As pointer(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = impl.delete_existing(sr, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +r Then
                                          ec = impl.delete_existing_timestamp(sr, r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return eva(result, False) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, +r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).delete
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return delete_existing(sr, result)
                           End Function,
                           Function() As event_comb
                               assert(eva(result, False))
                               Return Nothing
                           End Function)
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).empty
        Return impl.empty(result)
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).full
        Return impl.full(result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt(Of _false).heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).keycount
        Return impl.keycount(result)
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb Implements ikeyvt(Of _false).list
        Return impl.list(result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).modify
        Return amuw(key,
                    value,
                    ts,
                    result,
                    Function(sr, k, v, r) delete_then_write(sr, k, v, r))
    End Function

    Private Function read_existing(ByVal sr As SEEK_RESULT,
                                   ByVal result As pointer(Of Byte()),
                                   ByVal ts As pointer(Of Int64)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = impl.read_existing(sr, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = impl.read_existing_timestamp(sr, ts)
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

    Public Function read(ByVal key() As Byte,
                         ByVal result As pointer(Of Byte()),
                         ByVal ts As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).read
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return read_existing(sr, result, ts)
                           End Function,
                           Function() As event_comb
                               assert(eva(result, DirectCast(Nothing, Byte())))
                               assert(eva(ts, npos))
                               Return Nothing
                           End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvt(Of _false).retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).seek
        Return impl.seek(key, New pointer(Of SEEK_RESULT)(), result)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).sizeof
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return impl.sizeof_existing(sr, result)
                           End Function,
                           Function() As event_comb
                               assert(eva(result, npos))
                               Return Nothing
                           End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt(Of _false).stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).unique_write
        Return amuw(key,
                    value,
                    ts,
                    result,
                    Function(sr As SEEK_RESULT, k() As Byte, v() As Byte, r As pointer(Of Boolean)) As event_comb
                        Return New event_comb(Function() As Boolean
                                                  Return eva(r, False) AndAlso
                                                         goto_end()
                                              End Function)
                    End Function)
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).valuesize
        Return impl.valuesize(result)
    End Function
End Class
