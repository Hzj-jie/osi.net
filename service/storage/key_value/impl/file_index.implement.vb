
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public Class file_index
    Implements ikeyvt2(Of atom)

    Public Function append_existing(ByVal key As atom,
                                    ByVal value() As Byte,
                                    ByVal result As ref(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of atom).append_existing
        assert(key IsNot Nothing)
        Return write_file(key.full_file_path(), value, result, True)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt2(Of atom).capacity
        Return sync_async(Function() As Int64
                              Return CLng(ci.capacity())
                          End Function,
                          result,
                          +result)
    End Function

    Public Function delete_existing(ByVal key As atom,
                                    ByVal result As ref(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of atom).delete_existing
        assert(key IsNot Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New ref(Of Boolean)()
                                  End If
                                  ec = index.delete(key.fn_key, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+result) Then
                                          ec = delete_file(key.full_file_path())
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
                                  Return eva(result, ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function delete_existing_timestamp(ByVal key As atom,
                                              ByVal result As ref(Of Boolean)) As event_comb _
                                             Implements ikeyvt2(Of atom).delete_existing_timestamp
        assert(key IsNot Nothing)
        Return index.delete(key.ts_key, result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt2(Of atom).empty
        Return index.empty(result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt2(Of atom).full
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If ci.capacity() = 0 Then
                                      Return eva(result, True) AndAlso
                                             goto_end()
                                  Else
                                      ec = index.full(result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt2(Of atom).heartbeat
        Return parallel(AddressOf index.heartbeat, Function() create_directory(dr))
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt2(Of atom).keycount
        Return half_keycount(AddressOf index.keycount, result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvt2(Of atom).list
        Return select_list(AddressOf index.list, AddressOf is_timestamp_key, result)
    End Function

    Public Function read_existing(ByVal key As atom,
                                  ByVal result As ref(Of Byte())) As event_comb _
                                 Implements ikeyvt2(Of atom).read_existing
        assert(key IsNot Nothing)
        Return read_file(key.full_file_path(), result)
    End Function

    Public Function read_existing_timestamp(ByVal key As atom,
                                            ByVal ts As ref(Of Int64)) As event_comb _
                                           Implements ikeyvt2(Of atom).read_existing_timestamp
        Return sync_async(Function() As Boolean
                              Return eva(ts, key.timestamp)
                          End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvt2(Of atom).retire
        Return parallel(AddressOf index.retire,
                        Function() As event_comb
                            'expected to be fail, since the index files could be in the same folder
                            Return New event_comb(Function() As Boolean
                                                      Return waitfor(clear_directory(dr)) AndAlso
                                                             goto_end()
                                                  End Function)
                        End Function)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal r As ref(Of atom),
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt2(Of atom).seek
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = atom.ctor(Me, key, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return eva(result, ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sizeof_existing(ByVal key As atom,
                                    ByVal result As ref(Of Int64)) As event_comb _
                                   Implements ikeyvt2(Of atom).sizeof_existing
        assert(key IsNot Nothing)
        Return file_size(key.full_file_path(), result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt2(Of atom).stop
        Return index.stop()
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt2(Of atom).valuesize
        Return directory_size(dr.base_directory(), result)
    End Function

    Public Function write_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt2(Of atom).write_new
        Dim fn As ref(Of String) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  fn = New ref(Of String)()
                                  ec = new_filename(fn)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If result Is Nothing Then
                                          result = New ref(Of Boolean)()
                                      End If
                                      ec = write_file(dr.fullpath(+fn), value, result, False)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+result) Then
                                          ec = index.modify(filename_key(key), str_bytes(+fn), result)
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

    Public Function write_new_timestamp(ByVal key() As Byte,
                                        ByVal ts As Int64,
                                        ByVal result As ref(Of Boolean)) As event_comb _
                                       Implements ikeyvt2(Of atom).write_new_timestamp
        Return index.modify(timestamp_key(key), ts.to_bytes(), result)
    End Function
End Class
