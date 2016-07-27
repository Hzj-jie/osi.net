
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public Class file_key
    Implements ikeyvt2(Of String)

    Public Function append_existing(ByVal key As String,
                                    ByVal value() As Byte,
                                    ByVal result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of String).append_existing
        Return write_file(key, value, result, True)
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt2(Of String).capacity
        Return sync_async(Function() As Int64
                              Return ci.capacity()
                          End Function,
                          result,
                          +result)
    End Function

    Public Function delete_existing(ByVal key As String,
                                    ByVal result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of String).delete_existing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = delete_file(key)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return eva(result, ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function delete_existing_timestamp(ByVal key As String,
                                              ByVal result As pointer(Of Boolean)) As event_comb _
                                             Implements ikeyvt2(Of String).delete_existing_timestamp
        Return New event_comb(Function() As Boolean
                                  Return eva(result, True) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt2(Of String).empty
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of String()) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of String())()
                                  ec = list_files(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, isemptyarray(+r)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt2(Of String).full
        Return sync_async(Sub()
                              assert(eva(result, ci.capacity() <= 0))
                          End Sub)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt2(Of String).heartbeat
        Return create_directory(dr)
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt2(Of String).keycount
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of String()) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of String())()
                                  ec = list_files(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, array_size(+r)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvt2(Of String).list
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of String()) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of String())()
                                  ec = list_files(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Dim v As vector(Of Byte()) = Nothing
                                      v = New vector(Of Byte())()
                                      For i As Int32 = 0 To array_size(+r) - 1
                                          Dim k() As Byte = Nothing
                                          If key((+r)(i), k) Then
                                              v.emplace_back(k)
                                          End If
                                      Next
                                      Return eva(result, v) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function read_existing(ByVal key As String,
                                  ByVal result As pointer(Of Byte())) As event_comb _
                                 Implements ikeyvt2(Of String).read_existing
        Return read_file(key, result)
    End Function

    Public Function read_existing_timestamp(ByVal key As String,
                                            ByVal ts As pointer(Of Int64)) As event_comb _
                                           Implements ikeyvt2(Of String).read_existing_timestamp
        Return get_timestamp(key, ts)
    End Function

    Public Function retire() As event_comb Implements ikeyvt2(Of String).retire
        Return clear_directory(dr)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal r As pointer(Of String),
                         ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt2(Of String).seek
        Dim f As String = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If file_full_path(key, f) Then
                                      assert(Not String.IsNullOrEmpty(f))
                                      ec = file_exists(f, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      'should never happen
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return eva(r, f) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sizeof_existing(ByVal key As String,
                                    ByVal result As pointer(Of Int64)) As event_comb _
                                   Implements ikeyvt2(Of String).sizeof_existing
        Return file_size(key, result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt2(Of String).stop
        Return event_comb.succeeded()
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt2(Of String).valuesize
        Return directory_size(dr.base_directory(), result)
    End Function

    Public Function write_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt2(Of String).write_new
        Dim f As String = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If file_full_path(key, f) Then
                                      assert(Not String.IsNullOrEmpty(f))
                                      ec = write_file(f, value, result, False)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      'should never happen
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
                                        ByVal result As pointer(Of Boolean)) As event_comb _
                                       Implements ikeyvt2(Of String).write_new_timestamp
        Dim f As String = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If file_full_path(key, f) Then
                                      assert(Not String.IsNullOrEmpty(f))
                                      ec = set_timestamp(f, ts, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      'should never happen
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
