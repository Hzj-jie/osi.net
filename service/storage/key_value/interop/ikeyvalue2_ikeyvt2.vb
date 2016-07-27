
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

Public Class ikeyvalue2_ikeyvt2(Of SEEK_RESULT)
    Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT))

    Private ReadOnly d As ikeyvalue2(Of SEEK_RESULT)
    Private ReadOnly t As ikeyvalue2(Of SEEK_RESULT)

    Public Sub New(ByVal d As ikeyvalue2(Of SEEK_RESULT),
                   ByVal t As ikeyvalue2(Of SEEK_RESULT))
        assert(Not d Is Nothing)
        assert(Not t Is Nothing)
        Me.d = d
        Me.t = t
    End Sub

    Private Shared Function data_seek_result(ByVal key As pair(Of SEEK_RESULT, SEEK_RESULT)) As SEEK_RESULT
        assert(Not key Is Nothing)
        Return key.first
    End Function

    Private Shared Function timestamp_seek_result(ByVal key As pair(Of SEEK_RESULT, SEEK_RESULT)) As SEEK_RESULT
        assert(Not key Is Nothing)
        Return key.second
    End Function

    Private Shared Function merge_seek_result(ByVal dr As SEEK_RESULT,
                                              ByVal tr As SEEK_RESULT) As pair(Of SEEK_RESULT, SEEK_RESULT)
        Return make_pair(dr, tr)
    End Function

    Public Function append_existing(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                    value() As Byte,
                                    result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).append_existing
        Return d.append_existing(data_seek_result(key), value, result)
    End Function

    Public Function capacity(result As pointer(Of Int64)) As event_comb _
                            Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).capacity
        Return d.capacity(result)
    End Function

    Public Function delete_existing(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                    result As pointer(Of Boolean)) As event_comb _
                                   Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).delete_existing
        Return d.delete_existing(data_seek_result(key), result)
    End Function

    Public Function delete_existing_timestamp(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                              result As pointer(Of Boolean)) As event_comb _
                                             Implements ikeyvt2(Of pair(Of SEEK_RESULT, 
                                                                           SEEK_RESULT)).delete_existing_timestamp
        Return t.delete_existing(timestamp_seek_result(key), result)
    End Function

    Public Function empty(result As pointer(Of Boolean)) As event_comb _
                         Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).empty
        Return parallel(AddressOf d.empty, AddressOf t.empty, result)
    End Function

    Public Function full(result As pointer(Of Boolean)) As event_comb _
                        Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).full
        Return _parallel.[or](AddressOf d.full, AddressOf t.full, result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).heartbeat
        Return parallel(AddressOf d.heartbeat, AddressOf t.heartbeat)
    End Function

    Public Function keycount(result As pointer(Of Int64)) As event_comb _
                            Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).keycount
        Return parallel(AddressOf d.keycount,
                        AddressOf t.keycount,
                        result)
    End Function

    Public Function list(result As pointer(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).list
        Return parallel(AddressOf d.list,
                        AddressOf t.list,
                        Function(i As vector(Of Byte()), j As vector(Of Byte())) As Boolean
                            If i Is Nothing AndAlso j Is Nothing Then
                                Return True
                            ElseIf i Is Nothing OrElse j Is Nothing Then
                                Return False
                            Else
                                Return i.size() = j.size()
                            End If
                        End Function,
                        result)
    End Function

    Public Function read_existing(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                  result As pointer(Of Byte())) As event_comb _
                                 Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).read_existing
        Return d.read_existing(data_seek_result(key), result)
    End Function

    Public Function read_existing_timestamp(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                            ts As pointer(Of Int64)) As event_comb _
                                           Implements ikeyvt2(Of pair(Of SEEK_RESULT, 
                                                                         SEEK_RESULT)).read_existing_timestamp
        Dim b As pointer(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  b = New pointer(Of Byte())()
                                  ec = t.read_existing(timestamp_seek_result(key), b)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As Int64 = 0
                                  Return ec.end_result() AndAlso
                                         to_timestamp(+b, r) AndAlso
                                         eva(ts, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).retire
        Return parallel(AddressOf d.retire, AddressOf t.retire)
    End Function

    Public Function seek(key() As Byte,
                         r As pointer(Of pair(Of SEEK_RESULT, SEEK_RESULT)),
                         result As pointer(Of Boolean)) As event_comb _
                        Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).seek
        Dim dr As pointer(Of SEEK_RESULT) = Nothing
        Dim tr As pointer(Of SEEK_RESULT) = Nothing
        Dim drb As pointer(Of Boolean) = Nothing
        Dim trb As pointer(Of Boolean) = Nothing
        Return parallel(Function() As event_comb
                            dr = New pointer(Of SEEK_RESULT)()
                            drb = New pointer(Of Boolean)()
                            Return d.seek(key, dr, drb)
                        End Function,
                        Function() As event_comb
                            tr = New pointer(Of SEEK_RESULT)()
                            trb = New pointer(Of Boolean)()
                            Return t.seek(key, tr, trb)
                        End Function,
                        Function() As Boolean
                            If (+drb) = (+trb) Then
                                If (+drb) Then
                                    Return eva(result, True) AndAlso
                                           eva(r, merge_seek_result(+dr, +tr))
                                Else
                                    Return eva(result, False)
                                End If
                            Else
                                Return False
                            End If
                        End Function)
    End Function

    Public Function sizeof_existing(key As pair(Of SEEK_RESULT, SEEK_RESULT),
                                    result As pointer(Of Int64)) As event_comb _
                                   Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).sizeof_existing
        Return d.sizeof_existing(data_seek_result(key), result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).stop
        Return parallel(AddressOf d.stop, AddressOf t.stop)
    End Function

    Public Function valuesize(result As pointer(Of Int64)) As event_comb _
                             Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).valuesize
        Return d.valuesize(result)
    End Function

    Public Function write_new(key() As Byte,
                              value() As Byte,
                              result As pointer(Of Boolean)) As event_comb _
                             Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).write_new
        Return d.write_new(key, value, result)
    End Function

    Public Function write_new_timestamp(key() As Byte,
                                        ts As Int64,
                                        result As pointer(Of Boolean)) As event_comb _
                                       Implements ikeyvt2(Of pair(Of SEEK_RESULT, SEEK_RESULT)).write_new_timestamp
        Return t.write_new(key, ts.to_bytes(), result)
    End Function
End Class
