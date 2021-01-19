
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.procedure

Public Module _synchronize
    Private ReadOnly empty_array() As Byte
    Private Const lock_failure_interval_ms As Int64 = 1 * second_milli

    Sub New()
        empty_array = {byte_0}
    End Sub

    <Extension()> Public Function locked(ByVal i As istrkeyvt,
                                         ByVal key As String,
                                         ByVal d As Func(Of event_comb),
                                         Optional ByVal result As ref(Of Boolean) = Nothing,
                                         Optional ByVal wait_ms As Int64 = npos) As event_comb
        Dim r As Boolean = False
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If d Is Nothing Then
                                      Return False
                                  Else
                                      result.renew()
                                      ec = lock(i, key, result, wait_ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+result) Then
                                          ec = d()
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
                                  r = ec.end_result()
                                  ec = release(i, key, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         r AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function lock(ByVal i As istrkeyvt,
                                       ByVal key As String,
                                       Optional ByVal result As ref(Of Boolean) = Nothing,
                                       Optional ByVal wait_ms As Int64 = npos) As event_comb
        Dim start_ms As Int64 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      If wait_ms >= 0 Then
                                          start_ms = nowadays.milliseconds()
                                      End If
                                      result.renew()
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  ec = i.unique_write(key, empty_array, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If (ec.end_result() AndAlso
                                      (+result)) OrElse
                                     (wait_ms >= 0 AndAlso
                                      nowadays.milliseconds() - start_ms >= wait_ms) Then
                                      Return goto_end()
                                  Else
                                      Return waitfor(lock_failure_interval_ms) AndAlso
                                             goto_prev()
                                  End If
                              End Function)
    End Function

    <Extension()> Public Function release(ByVal i As istrkeyvt,
                                          ByVal key As String,
                                          Optional ByVal result As ref(Of Boolean) = Nothing) As event_comb
        Return event_comb.chain_before(Function() As Boolean
                                           Return Not i Is Nothing
                                       End Function,
                                       Function() As event_comb
                                           Return i.delete(key, result)
                                       End Function)
    End Function
End Module
