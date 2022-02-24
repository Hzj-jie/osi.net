
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.iosys

Public MustInherit Class status_receiver(Of STATUS_T As istatus)
    Implements ireceiver(Of [case])

    Protected ReadOnly rs As STATUS_T

    Protected Sub New(ByVal rs As STATUS_T)
        assert(Not rs Is Nothing)
        Me.rs = rs
    End Sub

    Protected Overridable Function ignore_status_case() As Boolean
        Return True
    End Function

    Protected Overridable Function check_case(ByVal c As [case]) As Boolean
        Return Not c Is Nothing
    End Function

    Protected MustOverride Function handle(ByVal c As [case], ByRef ec As event_comb) As Boolean

    Public Function receive(ByVal c As [case]) As event_comb Implements iosys.ireceiver(Of [case]).receive
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If check_case(c) Then
                                      If rs.new_status(c) AndAlso ignore_status_case() Then
                                          Return goto_end()
                                      ElseIf handle(c, ec) Then
                                          Return If(ec Is Nothing,
                                                    goto_end(),
                                                    waitfor(ec) AndAlso goto_next())
                                      Else
                                          Return False
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
End Class
