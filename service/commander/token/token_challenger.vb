
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class token_challenger
    Public Shared Function [New](Of COLLECTION, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 ByVal p As COLLECTION,
                                 ByVal c As CONNECTION) As token_challenger(Of COLLECTION, CONNECTION)
        Return New token_challenger(Of COLLECTION, CONNECTION)(info, p, c)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class token_challenger(Of COLLECTION, CONNECTION)
    Inherits itoken_challenger(Of COLLECTION, CONNECTION)

    Public Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION), ByVal p As COLLECTION, ByVal c As CONNECTION)
        MyBase.New(info, p, c)
    End Sub

    Protected Overrides Function question(ByVal h As herald, ByVal accepted As ref(Of Boolean)) As event_comb
        assert(Not h Is Nothing)
        assert(Not accepted Is Nothing)
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Byte()) = Nothing
        Dim code() As Byte = Nothing
        Return New event_comb(Function() As Boolean
                                  code = array_concat(constants.token1_prefix, info.token(p))
                                  r = New ref(Of Byte())()
                                  ec = (New questioner(h, info.response_timeout_ms(p)))(code, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not isemptyarray(+r) Then
                                      eva(accepted, memcmp(+r, code) = 0)
                                      If Not +accepted AndAlso info.trace(p) Then
                                          raise_error(info.identity(p),
                                                      " actively reject the token ",
                                                      info.token_as_str(p),
                                                      ", response is ",
                                                      info.token_to_str(+r),
                                                      ", will close the connection ",
                                                      info.identity(c))
                                      End If
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
