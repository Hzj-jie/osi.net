
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.connector

Public Interface sync_indicator
    Function indicate(ByRef pending As Boolean) As Boolean
End Interface

Public Interface indicator
    Function indicate(ByVal pending As pointer(Of Boolean)) As event_comb
End Interface

Public Interface sensor
    'pending shows whether there are pending data to read
    'if timeout_ms < 0, the procedure will not return until there is data arrived, i.e. pending = true
    Function sense(ByVal pending As pointer(Of Boolean),
                   ByVal timeout_ms As Int64) As event_comb
End Interface

Public Module _sensor
    <Extension()> Public Function sense(ByVal this As sensor,
                                        ByVal timeout_ms As Int64) As event_comb
        assert(Not this Is Nothing)
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of Boolean)()
                                  ec = this.sense(p, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+p) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function sense(ByVal this As sensor) As event_comb
        assert(Not this Is Nothing)
#If DEBUG Then
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of Boolean)()
                                  ec = this.sense(p, npos)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(+p) AndAlso
                                         goto_end()
                              End Function)
#Else
        Return this.sense(Nothing, npos)
#End If
    End Function
End Module
