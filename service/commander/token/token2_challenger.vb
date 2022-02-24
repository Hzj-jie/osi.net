
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.secure

Public NotInheritable Class token2_challenger
    Public Shared Function [New](Of COLLECTION, CONNECTION) _
                            (ByVal info As token_info(Of COLLECTION, CONNECTION),
                             ByVal p As COLLECTION,
                             ByVal c As CONNECTION,
                             ByVal s As signer) As token2_challenger(Of COLLECTION, CONNECTION)
        Return New token2_challenger(Of COLLECTION, CONNECTION)(info, p, c, s)
    End Function

    Public Shared Function [New](Of COLLECTION, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 ByVal p As COLLECTION,
                                 ByVal c As CONNECTION) As token2_challenger(Of COLLECTION, CONNECTION)
        Return New token2_challenger(Of COLLECTION, CONNECTION)(info, p, c)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class token2_challenger(Of COLLECTION, CONNECTION)
    Inherits itoken_challenger(Of COLLECTION, CONNECTION)

    Private ReadOnly s As signer

    Public Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION), ByVal p As COLLECTION, ByVal c As CONNECTION)
        Me.New(info, p, c, constants.default_signer)
    End Sub

    Public Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION),
                   ByVal p As COLLECTION,
                   ByVal c As CONNECTION,
                   ByVal s As signer)
        MyBase.New(info, p, c)
        assert(Not s Is Nothing)
        Me.s = s
    End Sub

    Protected Overrides Function question(ByVal h As herald, ByVal accepted As ref(Of Boolean)) As event_comb
        assert(Not h Is Nothing)
        assert(Not accepted Is Nothing)
        assert(Not +accepted)
        Dim ec As event_comb = Nothing
        Dim r As ref(Of command) = Nothing
        Dim code As piece = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of command)()
                                  ec = h.wait_and_receive(info.response_timeout_ms(p), r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not r.empty() Then
                                      Dim b As piece = Nothing
                                      b = (+r).action()
                                      If b.start_with(constants.token2_prefix) AndAlso
                                         info.sign(p, b.consume(constants.token2_prefix_len), code) Then
                                          ec = h.send(command.[New]().attach(code.export()))
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          If info.trace(p) Then
                                              raise_error(info.identity(p), " did not send a valid challenge code")
                                          End If
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      r.clear()
                                      ec = h.wait_and_receive(info.response_timeout_ms(p), r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim reply As piece = Nothing
                                  If ec.end_result() AndAlso Not r.empty() Then
                                      assert(Not code Is Nothing AndAlso Not code.empty())
                                      If (+r).has_action() AndAlso
                                         info.sign_match(p, code, (+r).action()) Then
                                          eva(accepted, True)
                                          reply = constants.token2_prefix
                                      Else
                                          If info.trace(p) Then
                                              raise_error(info.identity(p), " did not pass the challenge")
                                          End If
                                          eva(accepted, False)
                                          assert(info.forge_signature(p, code, reply))
                                      End If
                                      ec = h.send(command.[New]().attach(reply.export()))
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
