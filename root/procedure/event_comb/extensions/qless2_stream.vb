
Imports System.Runtime.CompilerServices
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.envs

Public Module _qless2_stream
    <Extension()> Public Function push(Of T, MAX_COUNT As _int64)(ByVal i As qless2_stream(Of T, MAX_COUNT),
                                                                  ByVal d() As T,
                                                                  ByVal timeout_ms As Int64) As event_comb
        Return push(i, d, timeout_ms, timeslice_length_ms)
    End Function

    <Extension()> Public Function push(Of T, MAX_COUNT As _int64)(ByVal i As qless2_stream(Of T, MAX_COUNT),
                                                                  ByVal d() As T,
                                                                  ByVal timeout_ms As Int64,
                                                                  ByVal interval_ms As Int64) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse
                                     isemptyarray(d) OrElse
                                     array_size(d) > qless2_stream(Of T, MAX_COUNT).max_size Then
                                      Return False
                                  Else
                                      ec = lifetime_event_comb(timeout_ms,
                                                               Function() As Boolean
                                                                   If i.push(d) Then
                                                                       Return goto_end()
                                                                   Else
                                                                       Return waitfor(interval_ms)
                                                                   End If
                                                               End Function)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
