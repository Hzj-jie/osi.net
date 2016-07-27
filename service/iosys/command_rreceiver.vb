
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.service.commander

Public MustInherit Class command_rreceiver(Of CASE_T)
    Implements ireceiver(Of CASE_T)

    Protected MustOverride Function receive(ByVal i As command) As event_comb

    Public Function receive(ByVal c As CASE_T) As event_comb Implements ireceiver(Of CASE_T).receive
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim r As command = Nothing
                                  If case_codec(Of CASE_T).serialize(c, r) Then
                                      r.attach(constants.remote.action.push)
                                      ec = receive(r)
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
End Class
