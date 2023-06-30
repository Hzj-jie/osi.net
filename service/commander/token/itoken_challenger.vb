
#Const bypass_empty_token = False
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Interface itoken_challenger
    Default ReadOnly Property challenge(ByVal accepted As ref(Of Boolean)) As event_comb
End Interface

Public MustInherit Class itoken_challenger(Of COLLECTION, CONNECTION)
    Implements itoken_challenger

    Protected ReadOnly info As token_info(Of COLLECTION, CONNECTION)
    Protected ReadOnly p As COLLECTION
    Protected ReadOnly c As CONNECTION

    Protected Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION), ByVal p As COLLECTION, ByVal c As CONNECTION)
        assert(Not info Is Nothing)
        assert(Not p Is Nothing)
        assert(Not c Is Nothing)
        Me.info = info
        Me.p = p
        Me.c = c
    End Sub

    Protected MustOverride Function question(ByVal h As herald, ByVal accepted As ref(Of Boolean)) As event_comb

    Private Shared Function bypass_empty_token() As Boolean
#If bypass_empty_token Then
        Return True
#Else
        Return False
#End If
    End Function

    Default Public ReadOnly Property challenge(ByVal accepted As ref(Of Boolean)) As event_comb _
                                              Implements itoken_challenger.challenge
        Get
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If bypass_empty_token() AndAlso isemptyarray(info.token(p)) Then
                                          Return eva(accepted, True) AndAlso
                                                 goto_end()
                                      Else
                                          Dim h As herald = Nothing
                                          If info.create_questioner_herald(p, c, h) Then
                                              accepted.renew()
                                              ec = question(h, accepted)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          Else
                                              info.shutdown(c)
                                              Return False
                                          End If
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() Then
                                          If Not +accepted Then
                                              If info.trace(p) Then
                                                  raise_error("Connection ",
                                                              info.identity(c),
                                                              " has been actively rejected by defender.")
                                              End If
                                              info.shutdown(c)
                                          End If
                                          Return goto_end()
                                      Else
                                          info.shutdown(c)
                                          Return False
                                      End If
                                  End Function)
        End Get
    End Property
End Class
