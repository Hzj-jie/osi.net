
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

Public NotInheritable Class dispenser
    Public Shared Function [New](Of DATA_T, ID_T) _
                                (ByVal d As T_receiver(Of pair(Of DATA_T, ID_T)),
                                 Optional ByVal sense_timeout_ms As Int64 = constants.default_sense_timeout_ms) _
                                As dispenser(Of DATA_T, ID_T)
        Return New dispenser(Of DATA_T, ID_T)(d, sense_timeout_ms)
    End Function

    Private Sub New()
    End Sub
End Class

' Delegates all incoming requests to accepters
Partial Public Class dispenser(Of DATA_T, ID_T)
    Inherits reference_count_event_comb_1

    Public Event unaccepted(ByVal data As DATA_T, ByVal remote As ID_T)
    Private ReadOnly d As T_receiver(Of pair(Of DATA_T, ID_T))
    Private ReadOnly sense_timeout_ms As Int64
    Private ReadOnly accepters As object_unique_set(Of accepter)

    Shared Sub New()
        assert(start_after_trigger())
    End Sub

    Public Sub New(ByVal d As T_receiver(Of pair(Of DATA_T, ID_T)),
                   Optional ByVal sense_timeout_ms As Int64 = constants.default_sense_timeout_ms)
        assert(Not d Is Nothing)
        Me.d = d
        If sense_timeout_ms < 0 Then
            Me.sense_timeout_ms = constants.default_sense_timeout_ms
        Else
            Me.sense_timeout_ms = sense_timeout_ms
        End If
        Me.accepters = New object_unique_set(Of accepter)()
    End Sub

    Public Function attach(ByVal accepter As accepter) As Boolean
        If accepters.insert(accepter) Then
            bind()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function detach(ByVal accepter As accepter) As Boolean
        If accepters.erase(accepter) Then
            release()
            Return True
        Else
            Return False
        End If
    End Function

    Protected NotOverridable Overrides Function work() As event_comb
        Dim ec As event_comb = Nothing
        Dim result As ref(Of pair(Of DATA_T, ID_T)) = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = d.sense(sense_timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      result.renew()
                                      ec = d.receive(result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If Not result.empty() Then
                                          Dim accepted As Boolean = False
                                          If Not accepters.empty() Then
                                              accepters.foreach(
                                                  Sub(ByVal i As accepter)
                                                      assert(Not i Is Nothing)
                                                      If i.accept((+result).second) Then
                                                          i.raise((+result).first, (+result).second)
                                                          accepted = True
                                                          break_lambda.at_here()
                                                      End If
                                                  End Sub)
                                          End If
                                          If Not accepted Then
                                              RaiseEvent unaccepted((+result).first, (+result).second)
                                          End If
                                      End If
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
