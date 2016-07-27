
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.formation
Imports utils = osi.root.utils

Friend Class dataprovider_collection
    Inherits unique_strong_map(Of String, idataprovider)

    Private auto_cleanup_enabled As singleentry
    Private auto_cleanup_running As singleentry

    Public Sub start_auto_cleanup(ByVal lifetime_ms As Int64)
        If lifetime_ms <= 0 Then
            lifetime_ms = minute_to_milliseconds(60)
        End If
        assert(auto_cleanup_enabled.mark_in_use())
        waitfor_auto_cleanup_stop()
        assert(auto_cleanup_running.mark_in_use())
        Dim ns As vector(Of String) = Nothing
        ns = New vector(Of String)()
        begin_application_lifetime_event_comb(Function() As Boolean
                                                  Return waitfor(lifetime_ms >> 1) AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  ns.clear()
                                                  utils.foreach(AddressOf foreach,
                                                                Sub(ByRef k As String, ByRef v As idataprovider)
                                                                    If out_of_lifetime(lifetime_ms, v) Then
                                                                        v.expire()
                                                                    End If
                                                                    If v.expired() Then
                                                                        assert(Not k Is Nothing)
                                                                        ns.push_back(k)
                                                                    End If
                                                                End Sub)
                                                  assert([erase](ns))
                                                  Return (auto_cleanup_enabled.in_use() AndAlso goto_begin()) OrElse
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  auto_cleanup_running.release()
                                                  Return goto_end()
                                              End Function)
    End Sub

    Public Sub stop_auto_cleanup()
        If auto_cleanup_enabled.in_use() Then
            auto_cleanup_enabled.release()
        End If
    End Sub

    Public Sub waitfor_auto_cleanup_stop()
        timeslice_sleep_wait_when(Function() auto_cleanup_running.in_use())
    End Sub

    Private Shared Function out_of_lifetime(ByVal lifetime_ms As Int64, ByVal v As idataprovider) As Boolean
        assert(Not v Is Nothing)
        Return ticks_to_milliseconds(nowadays.ticks() - v.last_refered_ticks()) > lifetime_ms
    End Function
End Class
